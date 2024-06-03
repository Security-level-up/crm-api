using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Api.Interfaces;

namespace Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Login()
        {
            IActionResult response = Unauthorized("Missing or invalid token");

            var claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? email = claimsIdentity?.FindFirst(ClaimTypes.Email);

            if (email == null)
            {
                return response;
            }

            var user = _userRepository.AddOrGetUser(email.Value);
            return Ok(user);
        }
    }
}
