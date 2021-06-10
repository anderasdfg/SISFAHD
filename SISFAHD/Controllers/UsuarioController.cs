using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using SISFAHD.DTOs;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioservice;
        public UsuarioController(UsuarioService usuarioservice)
        {
            _usuarioservice = usuarioservice;            
        }

        [HttpGet("all")]
        public ActionResult<List<Usuario>> GetAll()
        {
            return _usuarioservice.GetAll();
        }
        [HttpGet("id")]
        public ActionResult<Usuario> Get([FromQuery] string id)
        {
            return _usuarioservice.GetById(id);
        }

        
    }
}
