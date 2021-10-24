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
    public class ExamenesController : ControllerBase
    {
        private readonly ExamenesService _examenesservice;        
        public ExamenesController(ExamenesService examenesService)
        {
            _examenesservice = examenesService;            
        }

        [HttpGet("all")]
        public ActionResult<List<Examenes>> GetAll()
        {
            return _examenesservice.GetAll();
        }

        [HttpGet("id")]
        public ActionResult<Examenes> GetActionResult([FromQuery] string id)
        {
            return _examenesservice.GetByID(id);
        }

        [HttpGet("nombre")]
        public async Task<List<Examenes>> GetByName([FromQuery] string nombre)
        {
            return await _examenesservice.GetByNombre(nombre);
        }
        [HttpGet("allByUsuario")]
        public async Task<ActionResult<List<Examenes>>> GetAllExamenes_By_Paciente([FromQuery] string idUsuario)
        {
            return await _examenesservice.GetAllExamenes_By_Paciente(idUsuario);
        }

    }
}
