using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyMicroServices.Models;

namespace SurveyMicroServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class RoleController(RoleManager<AppRole> roleManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> CreateRole(string name, CancellationToken cancellationToken)
        {
            AppRole appRole = new()
            {
                Name = name,
            };

            IdentityResult result=await roleManager.CreateAsync(appRole);
            if (!result.Succeeded)
            {
                return StatusCode(500, "Rol kaydetme işlemi başarısız!");
            }

            return StatusCode(200, "Rol başarıyla kaydedildi1");

        }

        [HttpGet]
        public async Task <IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var roles= await roleManager.Roles.ToListAsync(cancellationToken);

            return Ok(roles);
        }


    }
}
