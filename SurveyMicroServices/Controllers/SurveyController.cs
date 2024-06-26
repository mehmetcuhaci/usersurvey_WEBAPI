using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyMicroServices.Dtos;
using SurveyMicroServices.Models;
using System.Security.Claims;

namespace SurveyMicroServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SurveyController(ApplicationDbContext applicationDbContext, UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpPost]

        public async Task<ActionResult<Survey>> CreateSurvey([FromBody]Survey survey,string userId)
        {
            //var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { 
            
            return Unauthorized();
            
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Kullanici bulunamadi");

            }
            survey.CreatedAt = DateTime.UtcNow;
            survey.CreatedBy = user.Id; 

            foreach (var question in survey.Questions)
            {
                applicationDbContext.Questions.Add(question);
                foreach (var option in question.Options)
                {
                    applicationDbContext.Options.Add(option);
                }
            }

            applicationDbContext.Surveys.Add(survey);
            try
            {
                await applicationDbContext.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
                
            }

            return Ok();
        }





    }
}
