using GP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GP.Controllers.DataApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginData model)
        {
            bool status = false;
            string des = "";
            if (!ModelState.IsValid)
            {
                des = "Login Is Not Valid";
                status = false;
                return BadRequest(new {status,des  });
            }
            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user is null) {
                status = false;
                des = "the user not found";
                return BadRequest(new { status ,des});
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                status = true;
                string Token = CreateJWT(user);
                return Ok(new {status,Token });
            }
            else
            {
                status = false;
                des = "The Login Attemp Is Not Valid";
                return BadRequest(new { status,des});
            }
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            bool status = false;
            string des = "";
            if (!ModelState.IsValid)
            {
                des = "Login Is Not Valid";
                status = false;
                return BadRequest(new { status, des });
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                status = false;
                des = "the user is already exist";
                return BadRequest(new { status, des });
            }
            var userModel = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = new MailAddress(model.Email).User
            };
            var result = await _userManager.CreateAsync(userModel, model.Password);

            if (result.Succeeded)
            {
                if (model.IsOwner)
                {
                   await _userManager.AddToRoleAsync(userModel,"Owner");
                }
                else
                {
                    await _userManager.AddToRoleAsync(userModel,"User");
                }
                status = true;
                string Token = CreateJWT(userModel);

                return Ok(new { status, Token });
            }
            else
            {
                status = false;
                des = "The Register Attemp Is Not Valid";
                return BadRequest(new { status, des });
            }
        }
        private string CreateJWT(AppUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var Expir = DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
               issuer: _configuration["JWT:Issuer"],
               audience: _configuration["JWT:Audience"],
               claims: claims,
               expires: Expir,
               signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
