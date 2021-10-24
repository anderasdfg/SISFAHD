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
                //if (resultadoExamen.documento_anexo.Count() != 0)
                //{
                //    for (int i = 0; i < resultadoExamen.documento_anexo.Count(); i++)
                //    {
                //        if (!string.IsNullOrWhiteSpace(resultadoExamen.documento_anexo[i]))
                //        {
                //            var solicitudBytes2 = Convert.FromBase64String(resultadoExamen.documento_anexo[i]);
                //            resultadoExamen.documento_anexo[i] = await _fileStorage.SaveDoc(solicitudBytes2, "pdf", "archivos");
                //        }
                //    }
                //}
                return await _resultadoExamenService.CrearResultadoExamen(resultadoExamen, idUsuario);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        [HttpPut("Modificar")]
        public async Task<ActionResult<ResultadoExamen>> ModificarResultadoExamen(ResultadoExamen id)
        {
            try
            {
                //for (int i = 0; i < id.documento_anexo.Count(); i++)
                //{
                //    if (!id.documento_anexo[i].StartsWith("http"))
                //    {
                //        if (!string.IsNullOrWhiteSpace(id.documento_anexo[i]))
                //        {
                //            var profileimg = Convert.FromBase64String(id.documento_anexo[i]);
                //            id.documento_anexo[i] = await _fileStorage.EditFile(profileimg, "jpg", "especialidad", id.documento_anexo[i]);
                //        }
                //    }
                //} 
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
