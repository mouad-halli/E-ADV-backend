using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using Server.common.Exceptions;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly ICurrentUserService _currentUserService;

        public UserService(
            UserManager<User> userManager,
            ITokenService tokenService,
            IUserRepository userRepository,
            ILogger<UserService> logger,
            ICurrentUserService currentUserService
        ) {
            _userManager = userManager;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<User> getCurrentUser()
        {
            var userId = _currentUserService.GetUserId();

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new BadRequestException("User not found");

            return user;
        }

        public async Task<User> findUserById(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null)
                throw new BadRequestException("User not found");
            return user;
        }

        public async Task<(string? errorMsg, IdentityResult? result)> CreateUser(User newUser, string userPassword)
        {
            if (await _userManager.FindByNameAsync(newUser.UserName!) != null)
                return ("username already in use", null);
            if (await _userManager.FindByEmailAsync(newUser.Email!) != null)
                return ("email already in use", null);

            IdentityResult identity = await _userManager.CreateAsync(newUser, userPassword);

            return (null, identity);
        }

        public async Task RegisterUserAsync(string firstname, string lastname, string email, string password)
        {
            if (await _userRepository.IsEmailTakenAsync(email))
                throw new BadRequestException("Email already in use");

            var passwordValidator = new PasswordValidator<User>();
            
            IdentityResult result = await passwordValidator.ValidateAsync(_userManager, null, password);

            if (!result.Succeeded)
                throw new BadRequestException("Password does not meet the required complexity.");

            var user = new User
            {
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                UserName = email,
                PasswordHash = password
            };

            result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new BadRequestException(result.ToString());

        }

        private async Task<User?> CreateMsalUserAsync(JObject graphUser)
        {

            /* 
                *****IF UNCOMENTED REMEMBER TO CHANGE GRAPH API URI INSIDE GRAPH API SERVICE {GetUserFromGraphApiAsync}*****
            */

            // Graph User validation
            var employeeId = graphUser.Value<string>("employeeId");
            // var department = graphUser.Value<string>("department");

            // if (string.IsNullOrEmpty(employeeId))
            //     throw new ArgumentException("Employee number can't be null");

            // if (await _userRepository.EmployeeNumberAlreadyExists(employeeId))
            //     throw new ArgumentException("Employee number already exists");

            // var departmentEntity = await _departmentRepository.GetDepartmentByNameOrFirstOneAsync(department);
            // if (departmentEntity == null)
                // throw new ApplicationException("No department exists in the database");

            // var userStatusEntity = await _userStatusRepository.GetUserStatusByNameAsync("Cadre");
            // if (userStatusEntity == null)
            //     throw new ApplicationException("User status not found");

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = graphUser.Value<string>("mail") ?? graphUser.Value<string>("userPrincipalName"),
                FirstName = graphUser.Value<string>("givenName") ?? string.Empty,
                LastName = graphUser.Value<string>("surname") ?? string.Empty,
                Email = graphUser.Value<string>("mail") ?? graphUser.Value<string>("userPrincipalName"),
                // UserStatusId = userStatusEntity.Id,
                // Title = graphUser.Value<string>("jobTitle"),
                // DepartmentId = departmentEntity.Id,
                EmployeeNumber = int.TryParse(employeeId, out var empNumber) ? empNumber : throw new ArgumentException("Invalid employee number"),
                // RecruitmentDate = ParseRecruitmentDate(graphUser.Value<string>("employeeHireDate")) ?? DateTime.MinValue
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                _logger.LogError("User creation failed: {Errors}", string.Join(", ", createResult.Errors.Select(e => e.Description)));
                return null;
            }

            // var roleResult = await _userManager.AddToRoleAsync(newUser, "Collaborator");
            // if (!roleResult.Succeeded)
            // {
            //     _logger.LogError("Failed to assign role to new user: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            //     return null;
            // }

            return newUser;
        }

        public async Task<(string jwtToken, User user)> MsalLoginAsync(JObject graphUser) {
            
            var email = graphUser.Value<string>("mail") ?? graphUser.Value<string>("userPrincipalName");
                if (string.IsNullOrEmpty(email))
                    throw new BadRequestException("Graph user email not found");

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                user = await CreateMsalUserAsync(graphUser);
                if (user == null)
                    throw new BadRequestException("User creation failed");
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Email, user.Email ?? string.Empty)
            };

            string jwtToken = _tokenService.CreateToken(claims);

            return (jwtToken, user);
        }

        public async Task<(string jwtToken, User user)> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                throw new BadRequestException("invalid email or password");

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Email, user.Email ?? string.Empty)
            };

            string jwtToken = _tokenService.CreateToken(claims);

            return (jwtToken, user);
        }
    }
}