using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using SISFAHD.DTOs;
using SISFAHD.Helpers;
using Microsoft.AspNetCore.Http;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioservice;
        private readonly IFileStorage _fileStorage;
        public UsuarioController(UsuarioService usuarioservice, IFileStorage fileStorage)
        {
            _usuarioservice = usuarioservice;
            _fileStorage = fileStorage;
        }

        /*[HttpGet("all")]
        public ActionResult<List<Usuario>> GetAll()
        {
            return _usuarioservice.GetAll();
        }*/
        [HttpGet("all")]
        public async Task<ActionResult<List<UsuarioDTO>>> GetAllUsuarios()
        {
            return await _usuarioservice.GetAllUsuarios();
        }
        [HttpGet("id")]
        public ActionResult<Usuario> GetUsuarioById([FromQuery] string id)
        {
            return _usuarioservice.GetById(id);
        }
        [HttpGet("usuarioId")]
        public ActionResult<Usuario> Get([FromQuery] string id)
        {
            return _usuarioservice.GetById(id);
        }
        [HttpGet("usuarioIdMedico")]
        public async Task<ActionResult<UsuarioMedico>> GetUsuarioMedicoById([FromQuery] string id)
        {

            return await _usuarioservice.GetByIDmedico(id);
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
        [HttpPost("Registrar")]
        public async Task<ActionResult<Usuario>> CreateUsuario2(Usuario usuario)
        {
            if (!string.IsNullOrWhiteSpace(usuario.datos.foto))
            {
                var profileimg = Convert.FromBase64String(usuario.datos.foto);
                usuario.datos.foto = await _fileStorage.SaveFile(profileimg, "jpg", "usuario");
            }
            Usuario objetousuario = _usuarioservice.CreateUsuario(usuario);
            return objetousuario;
        }
        [HttpPost("RegistrarUsuarioMedico")]
        public async Task<ActionResult<Usuario>> CreateUsuarioMedico(UsuarioMedico usuario)
        {
            if (!string.IsNullOrWhiteSpace(usuario.datos.foto))
            {
                var profileimg = Convert.FromBase64String(usuario.datos.foto);
                usuario.datos.foto = await _fileStorage.SaveFile(profileimg, "jpg", "usuario");
            }

            Usuario objetousuario = await _usuarioservice.CreateUsuarioMedico(usuario);
            return objetousuario;
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
        [HttpPut("ModificarPerfilUsuario")]
        public async Task<ActionResult<Usuario>> ModificarPerfilUsuario(Usuario id)
        {

            try
            {
                if (!id.datos.foto.StartsWith("http"))
                {
                    if (!string.IsNullOrWhiteSpace(id.datos.foto))
                    {
                        var profileimg = Convert.FromBase64String(id.datos.foto);
                        id.datos.foto = await _fileStorage.EditFile(profileimg, "jpg", "usuario", id.datos.foto);
                    }
                }

                Usuario usuario = _usuarioservice.ModificarPerfilUsuario(id);
                return usuario;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }


        }

        [HttpPut("ModificarUsuario")]
        public async Task<ActionResult<Usuario>> ModificarUsuario(Usuario id)
        {

            try
            {
                if (!id.datos.foto.StartsWith("http"))
                {
                    if (!string.IsNullOrWhiteSpace(id.datos.foto))
                    {
                        var profileimg = Convert.FromBase64String(id.datos.foto);
                        id.datos.foto = await _fileStorage.EditFile(profileimg, "jpg", "usuario", id.datos.foto);
                    }
                }

                Usuario usuario = _usuarioservice.ModificarUsuario(id);
                return usuario;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }


        }

        [HttpPut("ModificarUsuarioMedico")]
        public async Task<ActionResult<Usuario>> ModificarUsuarioMedico(UsuarioMedico usuario)
        {
            try
            {
                if (!usuario.datos.foto.StartsWith("http"))
                {
                    if (!string.IsNullOrWhiteSpace(usuario.datos.foto))
                    {
                        var profileimg = Convert.FromBase64String(usuario.datos.foto);
                        usuario.datos.foto = await _fileStorage.EditFile(profileimg, "jpg", "usuario", usuario.datos.foto);
                    }
                }

                Usuario usuarioM = await _usuarioservice.UpdateUsuarioMedico(usuario);
                return usuarioM;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }


        }

        [HttpPut("ModificarPerfilMedico")]
        public async Task<ActionResult<Usuario>> ModificarPerfilMedico(UsuarioMedico user)
        {
            try
            {
                if (!user.datos.foto.StartsWith("http"))
                {
                    if (!string.IsNullOrWhiteSpace(user.datos.foto))
                    {
                        var profileimg = Convert.FromBase64String(user.datos.foto);
                        user.datos.foto = await _fileStorage.EditFile(profileimg, "jpg", "user", user.datos.foto);
                    }
                }

                Usuario usuarioM = await _usuarioservice.ModificarPerfilMedico(user);
                return usuarioM;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }


        }
    }
}
