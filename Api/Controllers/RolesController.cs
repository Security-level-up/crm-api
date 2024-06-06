using Microsoft.AspNetCore.Mvc;
using Models;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Role>> GetRoles()
        {
            return Ok(_rolesRepository.GetRoles());
        }
    }
}
