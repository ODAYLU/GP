using GP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GP.Controllers.DataApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EstateController : ControllerBase
    {
        private readonly IEstate _estate;
        private readonly IConfiguration _configuration;
        private readonly IlikedEstates _like;

        public EstateController(IEstate estate, 
            IlikedEstates like,
            IConfiguration configuration)
        {
            _estate = estate;
            _like = like;
            _configuration = configuration;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetFav()
        {
            var data = await _estate.GetAllQuiers().Where(z => z.is_active && z.publish && !z.IsBlock).OrderByDescending(x => x.Likes).Take(4).ToListAsync();
            var JsonData = new { data};
            return Ok(JsonData);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetModern()
        {
            var data = await _estate.GetAllQuiers().Where(z => z.is_active && z.publish && !z.IsBlock).OrderByDescending(x => x.OnDate).Take(4).ToListAsync();
            var JsonData = new { data };
            return Ok(JsonData);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBySearch(string search)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(search))
            {

            var data = await _estate.GetAllQuiers()
                .Where(z => ( z.is_active && z.publish && !z.IsBlock)&& 
                (string.IsNullOrEmpty(search)?true:
                   z.Type.type == search ||
                   z.Category.category == search
                ))
                .OrderByDescending(x => x.OnDate)
                .Take(4).ToListAsync();
                status = true;
            var JsonData = new { status, data };
            return Ok(JsonData);
            }
            string des = "The Text Search can not be Null";
            var result = new { status = false, des };
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddLike(long id)
        {
            
            if (id != 0)
            {
                var estate = await _estate.GetOne(id);
                var data = new likedEstates
                {
                    IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    IdEstate = estate.Id
                };
                var like = _like.GetAll().FirstOrDefault(x => (x.IdUser == data.IdUser) && (x.IdEstate == data.IdEstate));
                if (like == null)
                {
                    await _like.InsertObj(data);
                    estate.Likes++;
                    await _estate.UpdateEstate(estate);
                    int count = estate.Likes;
                    string status = "dislike";
                    var JsonData = new { status, count };
                    return Ok(JsonData);
                }
                else
                {
                    await _like.DeleteObj(like.Id);
                    estate.Likes--;
                    await _estate.UpdateEstate(estate);
                    int count = estate.Likes;
                    string status = "dislike";
                    var JsonData = new { status, count };
                    return Ok(JsonData);
                }


            }
            return BadRequest("id can be null");
           
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
