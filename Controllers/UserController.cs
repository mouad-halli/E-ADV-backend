using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Interfaces.Services;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            UserManager<User> userManager,
            ICurrentUserService currentUserService,
            IUserService userService
        )
        {
            _logger = logger;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUserProfile()
        {
            // // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var userId = _currentUserService.GetUserId();

            // if (userId == null)
            //     return Unauthorized();

            // // var user = await _userManager.FindByIdAsync(userId);
            // var user = await _userService.FindUserByIdAsync(userId);

            // if (user == null)
            //     return NotFound();

            var user = await _userService.getCurrentUser();

            return Ok(new
            {
                // user.Id,
                user.FirstName,
                user.LastName,
                user.Email
            });
        }
    }
}