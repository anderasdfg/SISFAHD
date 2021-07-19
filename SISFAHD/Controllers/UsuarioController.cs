﻿using System.Collections.Generic;
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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [HttpGet("")]
        public ActionResult<Usuario> GetbyUsernameAndPassword([FromQuery] string username, string pass)
        {
            return _usuarioservice.GetByUserNameAndPass(username,pass);
        }
        [HttpPost("")]
        public ActionResult<Usuario> CreateUsuario(Usuario usuario)
        {
            Usuario objUsuario = _usuarioservice.CreateUsuario(usuario);
            return objUsuario;
        }
        [HttpGet("correo")]
        public ActionResult<Usuario> GetByCorreo([FromQuery] string correo)
        {
            return _usuarioservice.GetByCorreo(correo);
        }
        [HttpGet("docidentidad")]
        public ActionResult<Usuario> GetByDocIdentidad([FromQuery] string docIdentidad)
        {
            return _usuarioservice.GetByDocIdentidad(docIdentidad);
        }
        [HttpGet("fechacreacion")]
        public async Task<ActionResult<List<Usuario>>> GetbyRolFechaCreacion([FromQuery] string rol, DateTime fecha)
        {
            return await _usuarioservice.GetByFechaCreacion(rol, fecha);
        }
    }
}
