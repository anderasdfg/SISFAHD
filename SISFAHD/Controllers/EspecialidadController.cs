using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EspecialidadController : ControllerBase
    {
        private readonly EspecialidadService _especialidadeservice;
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

        public ActionResult<Especialidad> ModificarUsuario([FromQuery] Especialidad id)
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

    }
}
