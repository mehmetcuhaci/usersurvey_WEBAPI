﻿using Microsoft.AspNetCore.Http;
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
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s=> s.Description));
            }
            return NoContent();

        }

        [HttpPost]
        public async Task <IActionResult> ChangePassword(ChangePasswordDto request,CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByIdAsync(request.Id.ToString());
            if (appUser == null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı!" });
            }
            IdentityResult result =await userManager.ChangePasswordAsync(appUser, request.CurrentPassowrd, request.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }

            return NoContent();

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


            if (!result.Succeeded)
            {
                return StatusCode(500, "Şifre Yanlış!");
            }


            if (result.IsNotAllowed)
            {
                return StatusCode(500, "Mail adresi onaylı değil!"); 
            }

            return Ok(new { Token = "Token" });
        }


    }
}