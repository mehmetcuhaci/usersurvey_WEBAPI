using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SurveyMicroServices.Dtos;
using SurveyMicroServices.Models;
using System.Security.Claims;

namespace SurveyMicroServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public SurveyController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]

        public async Task<ActionResult<Survey>> CreateSurvey([FromBody]Survey survey /*string userId*/)
        {
            //var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userId == null) { 
            
            //return Unauthorized();
            
            //}

            //var user = await userManager.FindByIdAsync(userId);
            //if (user == null)
            //{
            //    return NotFound("Kullanici bulunamadi");

            //}
            survey.CreatedAt = DateTime.UtcNow;
            //survey.CreatedBy = user.Id; 

            foreach (var question in survey.Questions)
            {
                _context.Questions.Add(question);
                foreach (var option in question.Options)
                {
                    _context.Options.Add(option);
                }
            }

            _context.Surveys.Add(survey);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
                
            }

            return StatusCode(200,"Anket basariyla olusturuldu.");
        }


        [HttpGet]
        public async Task<IActionResult> GetSurveyTitle()
        {
            var surveys = await _context.Surveys.Select(s => new { s.SurveyID, s.Title }).ToListAsync();
            return Ok(surveys);
        }


        [HttpGet]
        public async Task<IActionResult> TitleByCode(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest("Başlık ismi girmek zorunlu!");
            }

            var matchingSurveys= await _context.Surveys
                .Where(s=> s.Title.Contains(title))
                .Select(s=> new SurveyDto
                {
                    SurveyId=s.SurveyID,
                    Title=s.Title,
                })
                .ToListAsync();

            if (!matchingSurveys.Any())
            {
                return NotFound("Bu basliga uygun anket bulunamadi");
            }

            return Ok(matchingSurveys);
        }

        [HttpGet]
        public async Task<IActionResult> GetSurvey()
        {
            var surveys=await _context.Surveys
                .Select(s=> new SurveyDto
                {
                    SurveyId=s.SurveyID,
                    Title=s.Title,
                    Questions=s.Questions.Select(q=>new QuestionDto
                    {
                        QuestionId=q.QuestionID,
                        Text=q.Text,
                        Options=q.Options.Select(o=>new OptionDto
                        {
                            OptionId=o.OptionID,
                            Text = o.Text
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            return Ok(surveys);
        }

        [HttpGet("{surveyID}")]
        public async Task<IActionResult> GetSurveyDetails(int surveyID)
        {
            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(s => s.SurveyID == surveyID);

            if (survey == null)
            {
                return NotFound("Anket bulunamadı");
            }

            var surveyDto = new SurveyDto
            {
                SurveyId = survey.SurveyID,
                Title = survey.Title,
                Questions = survey.Questions.Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionID,
                    Text = q.Text,
                    Options = q.Options.Select(o => new OptionDto
                    {
                        OptionId = o.OptionID,
                        Text = o.Text
                    }).ToList()
                }).ToList()
            };
            return Ok(surveyDto);
        }

        [HttpPost("{surveyId}")]
        public async Task<IActionResult> SubmitSurveyResponse(int surveyId, [FromBody] List<QuestionDto> answers, [FromHeader] string userId)
        {
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            foreach (var answer in answers)
            {
                foreach (var option in answer.Options)
                {
                    if (option.OptionId != 0)
                    {
                        var response = new Models.Response
                        {
                            SurveyID = surveyId,
                            QuestionID = answer.QuestionId,
                            OptionID = option.OptionId,
                            UserID = userId,
                            AnsweredAt = DateTime.UtcNow,
                            //AzureIntegration = false
                        };

                        _context.Responses.Add(response);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
            }

            return StatusCode(200, "Responses saved successfully.");
        }




        [HttpGet]
        public async Task<IActionResult> UserSurveys(string userId)
        {
            if (userId is null)
                return Unauthorized();
            
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return NotFound("Kullanici bulunamadi");
            }

            var surveyTitles = await _context.Responses
                  .Where(r => r.UserID == userId)
                  //Responses classında userid'yi doğruladı
                  .Join(_context.Surveys, //Surveys tablosuna girdi
                  response => response.SurveyID, //response içinden SurveyID aldı
                  survey => survey.SurveyID, // Survey içinden surveyid aldı
                  (response, survey) => new { survey.Title }) // ikisin birleştirip title'ı doğrulaadı
                  .Select(s => s.Title)
                  .Distinct()
                  .ToListAsync();

            if (surveyTitles == null || surveyTitles.Count == 0)
            {
                return NotFound("Kullanıcının katıldığı anket bulunamadı");
            }

            return Ok(surveyTitles);


        }



    }
}
