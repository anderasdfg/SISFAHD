using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
