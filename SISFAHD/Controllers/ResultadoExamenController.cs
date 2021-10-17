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

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultadoExamenController : ControllerBase
    {
        private readonly ResultadoExamenService _resultadoExamenService;
        public ResultadoExamenController(ResultadoExamenService resultadoExamenService)
        {
            _resultadoExamenService = resultadoExamenService;
        }
        [HttpGet]
        public ActionResult<List<PacienteDTO>> getExamenesSolicitados([FromQuery] Paciente p)
        {
            return _resultadoExamenService.getExamenesSolicitados(p);
        }
    }
}
