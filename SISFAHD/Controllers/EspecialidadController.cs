using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EspecialidadController : ControllerBase
    {
        private readonly EspecialidadService _especialidadeservice;
     //   private readonly IFileStorage fileStorage;
        public EspecialidadController(EspecialidadService especialidadeservice)
        {
            _especialidadeservice = especialidadeservice;
        }

        [HttpGet("all")]
        public ActionResult<List<Especialidad>> GetAll()
        {
            return _especialidadeservice.GetAll();
        }

        [HttpGet("Nombre")]
        public ActionResult<Especialidad> Get([FromQuery] string nombre)
        {
            return _especialidadeservice.GetByNombre(nombre);
        }

        [HttpGet("Id")]
        public ActionResult<Especialidad> GetActionResult([FromQuery] string id)
        {
            return _especialidadeservice.GetByID(id);
        }

        [HttpPut("Modificar")]

        public ActionResult<Especialidad> ModificarEspecialidad(Especialidad id)
        {
            Especialidad especialidad = _especialidadeservice.ModifyEspecialidad(id);
            return especialidad;
        }

        [HttpPost("Registrar")]
        public ActionResult<Especialidad> CreateEspecialidad(Especialidad especialidad)
        {
            Especialidad objetoespecialidad = _especialidadeservice.CreateEspecialidad(especialidad);
            return objetoespecialidad;
        }

        /* Para luego xd
         [HttpPost("Registrar")]
       public async Task<ActionResult<Especialidad>> CreateEspecialidad(Especialidad especialidad)
       {
          if (!string.IsNullOrWhiteSpace(especialidad.url))
           {
               var profileimg = Convert.FromBase64String(especialidad.url);
               especialidad.url = await _fileStorage.SaveFile(profileimg, "jpg", "usuarios");
           }
           Especialidad objetoespecialidad = _especialidadeservice.CreateEspecialidad(especialidad);
           return objetoespecialidad;
       }

          [HttpPost("Modificar")]
       public async Task<ActionResult<Especialidad>> ModificarEspecialidad(Especialidad id)
       {
           Especialidad especialidadbd = new Especialidad();
           especialidadbd = _especialidadeservice.GetByID(id.id);

           if (!string.IsNullOrWhiteSpace(especialidad.url))
           {
               var profileimg = Convert.FromBase64String(especialidad.url);
               especialidad.url = await _fileStorage.EditFile(profileimg, "jpg", "usuarios", especialidadbd.url);
           }
           Especialidad objetoespecialidad = _especialidadeservice.CreateEspecialidad(especialidad);
           return objetoespecialidad;
       }

        */
    }
}
