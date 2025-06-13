using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Interfaces.Services;
using Server.Models;
using Server.Models.DTOS;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        private readonly ILogger<AuthenticationController> _logger;

        private readonly IUserService _userService;

        private readonly IGraphApiService _graphApiService;

        public AuthenticationController(
            UserManager<User> userManager,
            ITokenService tokenService,
            SignInManager<User> signInManager,
            IUserService userService,
            ILogger<AuthenticationController> logger,
            IGraphApiService graphApiService
        ) {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userService = userService;
            _logger = logger;
            _graphApiService = graphApiService;
        }

        [HttpPost("msal-login")]
        public async Task<IActionResult> MsalLogin()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(accessToken))
                return BadRequest("Authorization token is missing");

            try
            {
                var graphUser = await _graphApiService.GetUserFromGraphApiAsync(accessToken);
                if (graphUser == null)
                    return Unauthorized();

                var (jwtToken, user) = await _userService.MsalLoginAsync(graphUser);
                // var (jwtToken, user) = await _userService.temporaryLocalUserCreation(graphUser);


                Response.Cookies.Append("access_token", jwtToken, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddHours(8)
                });

                return Ok(new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Application error: {Message}", ex.Message);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto registerData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.RegisterUserAsync(registerData.FirstName, registerData.LastName, registerData.Email, registerData.Password);

            return Ok("registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (jwtToken, user) = await _userService.LoginAsync(loginData.Email, loginData.Password);

            Response.Cookies.Append("access_token", jwtToken, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(8)
            });
            
            return Ok(new
            {
                // user.Id,
                user.FirstName,
                user.LastName,
                user.Email
            });
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            // making our cookie with the name acess_token expires so the browser remove it automatically
            Response.Cookies.Append("access_token", string.Empty, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            return Ok("logged out successfully");
        }

    }
}