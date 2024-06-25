using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SurveyMicroServices.Models;

namespace SurveyMicroServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserRolesController(ApplicationDbContext context,UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Create(string mail, string roleName, CancellationToken cancellationToken)
        {
            //AppUserRole appUserRole = new()
            //{
            //    UserId = userId,
            //    RoleId = roleId,
            //};

            //await context.UserRoles.AddAsync(appUserRole);

            //await context.SaveChangesAsync(cancellationToken);

            AppUser? appUser = await userManager.FindByEmailAsync(mail);
            if (appUser == null)
            {
                return StatusCode(500, "Kullanıcı bulunamadı!");
            }

            IdentityResult result = await userManager.AddToRoleAsync(appUser, roleName);

            if (!result.Succeeded) {

                return BadRequest(result.Errors.Select(s => s.Description));
            }


            return StatusCode(200,"Kullanıcı rolü başarıyla güncellendi!");
        }



    }
}
