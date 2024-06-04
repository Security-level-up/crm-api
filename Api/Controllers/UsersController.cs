using Microsoft.AspNetCore.Mvc;
using Models;
using Api.Interfaces;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }
    }
}
