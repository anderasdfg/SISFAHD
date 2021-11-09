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
        private readonly IFileStorage _fileStorage;
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

        [HttpPost("Registrar")]
        public async Task<ActionResult<Examenes>> CrearExamenes(Examenes examen)
        {
            //if (!string.IsNullOrWhiteSpace(examenes.url))
            //{
            //    var profileimg = Convert.FromBase64String(examenes.url);
            //    examenes.url = await _fileStorage.SaveFile(profileimg, "jpg", "examenes");
            //}
            Examenes objetoexamenes = _examenesservice.CrearExamenesAux(examen);
            return objetoexamenes;
        }

        [HttpPut("Modificar")]
        public async Task<ActionResult<Examenes>> ModificarExamenes(Examenes examen)
        {
            try
            {
                //if (!id.url.StartsWith("http"))
                //{
                //    if (!string.IsNullOrWhiteSpace(id.url))
                //    {
                //        var profileimg = Convert.FromBase64String(id.url);
                //        id.url = await _fileStorage.EditFile(profileimg, "jpg", "examenes", id.url);
                //    }
                //}

                Examenes objetoexamenesbd = _examenesservice.ModificarExamenesAux(examen);
                return objetoexamenesbd;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<Examenes>> Delete(string id)
        {
            return await _examenesservice.RemoveExamenes(id);
        }


    }
}
