using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EspecialidadController : ControllerBase
    {
        private readonly EspecialidadService _especialidadeservice;
        private readonly IFileStorage _fileStorage;
        public EspecialidadController(EspecialidadService especialidadeservice, IFileStorage fileStorage)
        {
            _especialidadeservice = especialidadeservice;
            _fileStorage = fileStorage;
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

     /*  Modificar Sin jpg
      * [HttpPut("Modificar")]

        public ActionResult<Especialidad> ModificarEspecialidad(Especialidad id)
        {
            Especialidad especialidad = _especialidadeservice.ModifyEspecialidad(id);
            return especialidad;
        }*/

      /* Crear sin jpg
       * [HttpPost("Registrar")]
        public ActionResult<Especialidad> CreateEspecialidad(Especialidad especialidad)
        {
            Especialidad objetoespecialidad = _especialidadeservice.CreateEspecialidad(especialidad);
            return objetoespecialidad;
        }*/

     //Para luego xd
         [HttpPost("Registrar")]
        // Tipo 2
        public async Task<ActionResult<Especialidad>> CrearEspecialidad(Especialidad especialidad)
        {
                if (!string.IsNullOrWhiteSpace(especialidad.url))
                {
                    var profileimg = Convert.FromBase64String(especialidad.url);
                    especialidad.url = await _fileStorage.SaveFile(profileimg, "jpg", "especialidad");
                }
            Especialidad objetoespecialdiad = _especialidadeservice.CrearEspecialdiad2(especialidad);
            return objetoespecialdiad;
        }
        /* Tipo 1 de crear
         * 
         * public async Task<ActionResult<Especialidad>> CrearEspecialidad(Especialidad especialidad)
          {
               try
               {
                   if (!string.IsNullOrWhiteSpace(especialidad.url))
                   {
                       var profileimg = Convert.FromBase64String(especialidad.url);
                       especialidad.url = await _fileStorage.SaveFile(profileimg, "jpg", "especialidad");
                   }

                   return await _especialidadeservice.CreateEspecialidad(especialidad);
               }
               catch(Exception ex)
               {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex);
               }
          }*/

        [HttpPost("Modificar")]
        public async Task<ActionResult<Especialidad>> ModificarEspecialidad(Especialidad id)
        {
            Especialidad especialidadbd = new Especialidad();
            especialidadbd = _especialidadeservice.GetByID(id.id);
            try
            {
                if (!string.IsNullOrWhiteSpace(id.url))
                {
                    var profileimg = Convert.FromBase64String(id.url);
                    id.url = await _fileStorage.EditFile(profileimg, "jpg", "especialidad", especialidadbd.url);
                }

                Especialidad objetoespecialidadbd = _especialidadeservice.ModificarEspecialidad2(especialidadbd);
                return objetoespecialidadbd;
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        /*   
         *   Tipo 1 de Modificar
         *   [HttpPost("Modificar")]
          public async Task<ActionResult<Especialidad>> ModificarEspecialidad(Especialidad id)
          {
              Especialidad especialidadbd = new Especialidad();
              especialidadbd = _especialidadeservice.GetByID(id.id);
             try
               {
                   if (!string.IsNullOrWhiteSpace(id.url))
                   {
                       var profileimg = Convert.FromBase64String(id.url);
                       id.url = await _fileStorage.EditFile(profileimg, "jpg", "especialidad", especialidadbd.url);
                   }

                   Especialidad objetoespecialidadbd = await _especialidadeservice.ModificarEspecialidad(especialidadbd);
                   return objetoespecialidadbd;
               }
               catch (Exception ex)
               {

                   return StatusCode(StatusCodes.Status500InternalServerError, ex);
               }
           }*/


    }
}
