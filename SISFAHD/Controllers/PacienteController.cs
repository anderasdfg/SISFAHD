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
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService _pacienteService;
        public PacienteController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }
        [HttpGet("all")]
        public ActionResult<List<Paciente>> GetAll()
        {
            return _pacienteService.GetAll();
        }
        [HttpGet("")]
        public ActionResult<Paciente> GetById([FromQuery] string id)
        {
            return _pacienteService.GetById(id);
        }
        [HttpPost("")]
        public async Task<ActionResult<Paciente>> CreatePaciente([FromQuery]bool lleno,Paciente paciente)
        {
            if (lleno)
            {
                return await _pacienteService.CreatePaciente(paciente);
            }
            else
            {
                return await _pacienteService.CreatePaciente2(paciente);
            }
            
        }
        [HttpPut("")]
        public async Task<ActionResult<Paciente>> UpdatePaciente(Paciente paciente)
        {
            return await _pacienteService.ModifyPaciente(paciente);
        }
    }
}
