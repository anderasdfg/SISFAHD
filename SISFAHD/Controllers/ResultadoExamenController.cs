using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
    public class ResultadoExamenController : ControllerBase
    {
        private readonly ResultadoExamenService _resultadoExamenService;
        private readonly IFileStorage _fileStorage;

        public ResultadoExamenController(IFileStorage fileStorage, ResultadoExamenService resultadoExamenService)
        {
            _fileStorage = fileStorage;
            _resultadoExamenService = resultadoExamenService;
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<ResultadoExamen>>> GetAllExamenesSubidos([FromQuery] string idUsuario)
        {
            return await _resultadoExamenService.GetAllExamenesSubidos(idUsuario);
        }
        [HttpGet("id")]
        public async Task<ActionResult<ResultadoExamen>> GetByIdExamenesSubidos([FromQuery] string id)
        {
            return await _resultadoExamenService.GetByIdExamenesSubidos(id);
        }

        [HttpGet("TraerExamenesSolicitados")]
        public async Task<ActionResult<List<ExamenAuxiliar>>> getAllExamenesSolicitados([FromQuery] string idUsuario)
        {
            return await _resultadoExamenService.GetAllExamenesAuxiliares_By_Paciente(idUsuario);
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<ResultadoExamen>> CrearResultadoExamen([FromBody] ResultadoExamen resultadoExamen, string idUsuario)
        {
            try
            {
               
                return await _resultadoExamenService.CrearResultadoExamen(resultadoExamen, idUsuario);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("RegistrarDTO")]
        public async Task<ActionResult<ResultadoExamen>> CrearResultadoExamen2([FromBody] ResultadoExamenDTO resultadoExamen)
        {
            try
            {
                return await _resultadoExamenService.CrearResultadoExamen2(resultadoExamen);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


        [HttpPut("Modificar")]
        public ActionResult<ResultadoExamen> ModificarResultadoExamen(ResultadoExamen id)
        {
            try
            {
                
                ResultadoExamen objetoResultado = _resultadoExamenService.ModificarResultadoExamen(id);
                return objetoResultado;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult<ResultadoExamen>> EliminarResultadosExamen([FromQuery] string id, string idusuario)
        {
            try
            {
                return await _resultadoExamenService.EliminarResultadosExamen(id, idusuario);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            
        }
    }
}
