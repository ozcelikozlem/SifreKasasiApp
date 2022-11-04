using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SifreKasasi.API.Data;
using SifreKasasi.API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SifreKasasi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;

        public readonly DatabaseContext _context;
        public TokenController(IConfiguration configuration, DatabaseContext context)
        {
            _configuration = configuration;
            _context = context;


        }

        [HttpPost]
        public async Task<IActionResult> Post(Kayit kayit)
        {
            if(kayit !=null && kayit.KullaniciAdi !=null && kayit.Password != null)
            {
               
                var user = await GetKayit(kayit.KullaniciAdi, kayit.Password);

                if(user != null)
                {
                    
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("Id",user.Id.ToString()),
                        new Claim("KullaniciAdi",user.KullaniciAdi),
                        new Claim("Password",user.Password)

                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));


                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
               
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<Kayit> GetKayit(string kullaniciAdi, string password)
        {
            
            return await _context.Kayits.FirstOrDefaultAsync(u=> u.KullaniciAdi==kullaniciAdi && u.Password==password);
        }


    }
}
