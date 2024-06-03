using Microsoft.AspNetCore.Mvc;
using Models;
using Api.Interfaces;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
