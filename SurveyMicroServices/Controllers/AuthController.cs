using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyMicroServices.Dtos;
using SurveyMicroServices.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace SurveyMicroServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class AuthController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager) : ControllerBase
    {

        [HttpPost]
        public async Task <IActionResult> Register(RegisterDto request,CancellationToken cancellationToken)
        {
            //Db kayıt işlemi

            AppUser appUser = new()
            {
                Email = request.Email,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            IdentityResult result=await userManager.CreateAsync(appUser,request.Password);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(appUser);


            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s=> s.Description));
            }
            return StatusCode(200, $"Kayıt Başarılı \n" + token);
        }

        [HttpPost]
        public async Task <IActionResult> ChangePassword(ChangePasswordDto request,CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByEmailAsync(request.email);
            if (appUser == null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı!" });
            }
            IdentityResult result =await userManager.ChangePasswordAsync(appUser, request.currentPassword, request.newPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }

            return StatusCode(200,"sifre basariyla degisti");

        }

        [HttpGet]
        public async Task <IActionResult> ForgotPassword(string email,CancellationToken cancellation)
        {
            AppUser? appUser = await userManager.FindByEmailAsync(email);

            if (appUser == null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı!" });
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(appUser);

            return Ok(new { Token = token });


        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordWToken(ChangePasswordUsingTokenDto request, CancellationToken cancellationToken)
        {

            AppUser? appUser = await userManager.FindByEmailAsync(request.Email);

            if (appUser == null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı!" });
            }

            IdentityResult result = await userManager.ResetPasswordAsync(appUser, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }

            return NoContent();


        }

        [HttpPost]
        public async Task <IActionResult> LogIn(LoginDto request,CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(p =>

            p.Email==request.UserNameorOrEmail || p.UserName==request.UserNameorOrEmail,cancellationToken);


            if (appUser is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı!" });
            }


            SignInResult result = await signInManager.CheckPasswordSignInAsync(appUser, request.Password, true);

            if (result.IsNotAllowed)
            {
                return StatusCode(500, "Mail adresi onaylı değil!");
            }

            if (!result.Succeeded)
            {
                return StatusCode(500, "Şifre Yanlış!");
            }


            

            var loginRole =await userManager.GetRolesAsync(appUser);
            string roleString = string.Join(",", loginRole);

            if (loginRole.Contains("Admin"))
            {
                var response = new
                {
                    role=roleString,
                    userId=appUser.Id,
                    Message=$"{roleString} giris yapti"

                };

                return Ok(response);
                //return Ok($"{roleString} giris yapti");
            }

            return StatusCode(200,"Succesful Login");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string token,string email)
        {
            var user=await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return StatusCode(500, "Hatalı Mail");
            }
            
            IdentityResult result=await userManager.ConfirmEmailAsync(user,token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }
            
            return StatusCode(200, "Mail Doğrulama Başarılı");



        }
    }
}
