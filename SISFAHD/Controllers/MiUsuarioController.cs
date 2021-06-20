using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using SISFAHD.Services;
using System.Web.Http.Cors;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiUsuarioController:ControllerBase
    {
        private readonly UsuarioService _usuarioservice;
        public MiUsuarioController(UsuarioService usuarioservice)
        {
            _usuarioservice = usuarioservice;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<UsuarioDTO>>> GetAllUsuarios()
        {
            return await _usuarioservice.GetAllUsuarios();
        }

        [HttpPost("Registrar")]
        public ActionResult<Usuario> CreateUsuario(Usuario usuario)
        {
            Usuario objetousuario = _usuarioservice.CreateUsuario(usuario);
            return objetousuario;
        }
        [HttpPost("RegistrarUsuarioMedico")]
        public async Task<ActionResult<Usuario>> CreateUsuarioMedico(UsuarioMedico usuario)
        {
            Usuario objetousuario = await _usuarioservice.CreateUsuarioMedico(usuario);
            return objetousuario;
        }

        [HttpPut("ModificarUsuario")]
        public ActionResult<Usuario> ModificarUsuario(Usuario id)
        {
            Usuario usuario = _usuarioservice.ModificarUsuario(id);
            return usuario;

        }

        [HttpPut("ModificarUsuarioMedico")]
        public async Task<ActionResult<Usuario>> ModificarUsuarioMedico(UsuarioMedico usuario)
        {
            Usuario usuarioM = await _usuarioservice.UpdateUsuarioMedico(usuario);
            return usuarioM;

        }

        [HttpGet("usuarioId")]
        public ActionResult<Usuario> GetUsuarioById([FromQuery] string id)
        {
            return _usuarioservice.GetByID(id);
        }

        [HttpGet("usuarioIdMedico")]
        public async Task <ActionResult<UsuarioMedico>> GetUsuarioMedicoById([FromQuery] string id)
        {
            return await _usuarioservice.GetByIDmedico(id);
        }


    }
}
