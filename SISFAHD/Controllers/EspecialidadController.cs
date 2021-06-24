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

        [HttpPost("Registrar")]
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
        [HttpPut("Modificar")]
        public async Task<ActionResult<Especialidad>> ModificarEspecialidad(Especialidad id)
        {
            try
            {
                if (!id.url.StartsWith("http"))
                {
                    if (!string.IsNullOrWhiteSpace(id.url))
                    {
                        var profileimg = Convert.FromBase64String(id.url);
                        id.url = await _fileStorage.EditFile(profileimg, "jpg", "especialidad", id.url);
                    }
                }
                Especialidad objetoespecialidadbd = _especialidadeservice.ModificarEspecialidad2(id);
                return objetoespecialidadbd;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

    }
}
