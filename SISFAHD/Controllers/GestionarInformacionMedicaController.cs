using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using SISFAHD.Services;
using System.Web.Http.Cors;
namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GestionarInformacionMedicaController : ControllerBase
    {
        private readonly GestionarInformacionMedicaService _gestionarinformacionmedicaService;
        public GestionarInformacionMedicaController(GestionarInformacionMedicaService gestionarinformacionmedicaService)
        {
            _gestionarinformacionmedicaService = gestionarinformacionmedicaService;
        }
        [HttpGet("all")]
        public ActionResult<List<Paciente>> GetAll()
        {
            return _gestionarinformacionmedicaService.GetAll();
        }
        [HttpGet("")]
        public ActionResult<Paciente> GetById([FromQuery] string id)
        {
            return _gestionarinformacionmedicaService.GetById(id);
        }
        [HttpPost("")]
        public async Task<ActionResult<Paciente>> CreatePaciente(Paciente paciente)
        {
            return await _gestionarinformacionmedicaService.CreatePaciente(paciente);
        }
        [HttpPut("")]
        public async Task<ActionResult<Paciente>> UpdatePaciente(Paciente paciente)
        {
            return await _gestionarinformacionmedicaService.ModifyPaciente(paciente);
        }
    }
}
