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
using Microsoft.AspNetCore.Http;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiUsuarioController:ControllerBase
    {
        private readonly UsuarioService _usuarioservice;
        private readonly IFileStorage _fileStorage;
        public MiUsuarioController(UsuarioService usuarioservice, IFileStorage fileStorage)
        {
            _usuarioservice = usuarioservice;
            _fileStorage = fileStorage;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<UsuarioDTO>>> GetAllUsuarios()
        {
            return await _usuarioservice.GetAllUsuarios();
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
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
        public async Task< ActionResult<Usuario>> ModificarUsuario(Usuario id)
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
        public async Task<ActionResult<Usuario>> ModificarPerfilMedico(MedicoDTO3 user)
        {
            try
            {
                if (!user.usuario.datos.foto.StartsWith("http"))
                {
                    if (!string.IsNullOrWhiteSpace(user.usuario.datos.foto))
                    {
                        var profileimg = Convert.FromBase64String(user.usuario.datos.foto);
                        user.usuario.datos.foto = await _fileStorage.EditFile(profileimg, "jpg", "user", user.usuario.datos.foto);
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

        [HttpGet("usuarioId")]
        public ActionResult<Usuario> GetUsuarioById([FromQuery] string id)
        {
            return _usuarioservice.GetById(id);
        }

        [HttpGet("usuarioIdMedico")]
        public async Task <ActionResult<UsuarioMedico>> GetUsuarioMedicoById([FromQuery] string id)
        {

            return await _usuarioservice.GetByIDmedico(id);
        }


    }
}
