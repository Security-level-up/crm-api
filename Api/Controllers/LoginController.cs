using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Data;
using System.Security.Claims;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Login()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? email = claimsIdentity?.FindFirst(ClaimTypes.Email);

            if (email == null)
            {
                return BadRequest("Email not found in claims");
            }

            User? userObj = null;
            bool userExists = _context.Users.Any(user => user.Username == email.Value);

            if (!userExists)
            {
                _context.Users.Add(new User
                {
                    Username = email.Value,
                    RoleID = 1
                });
                _context.SaveChanges();
            }

            userObj = _context.Users.FirstOrDefault(user => user.Username == email.Value);
            return Ok(userObj);
        }
    }
}
