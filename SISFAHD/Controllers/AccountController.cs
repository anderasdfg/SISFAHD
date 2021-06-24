using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SISFAHD.Models;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UsuarioService _usuarioservice;
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration,
            UsuarioService usuarioservice)
        {
            _configuration = configuration;
            _usuarioservice = usuarioservice;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<UserToken> Login(/*[FromBody]*/ UserInfo userInfo)
        {
            var result = _usuarioservice.GetByUserNameAndPass(userInfo.username, userInfo.password);
            if (result != null)
            {
                Usuario usuario = (Usuario)result;
                return BuildToken(userInfo, usuario.rol);
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }

        private UserToken BuildToken(UserInfo userInfo, String rol)//IList<string> roles
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.username),
                new Claim(ClaimTypes.Name, userInfo.username),
                new Claim("myValue", "it's what i want"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.Add(new Claim(ClaimTypes.Role, rol));

            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var creds = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(8);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<Usuario>> GetByUserName()
        {
            Usuario usuario = null;

            try
            {
                String userName = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

                if (String.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Sesión expirada");
                }

                usuario = await _usuarioservice.GetByUserName(userName);

                return usuario;
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

    }
}
