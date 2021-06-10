using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using SISFAHD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitaController : Controller
    {
        private readonly CitaService _cita;
        private readonly ActoMedicoService _actoMedico;        
        public CitaController(CitaService citaService, ActoMedicoService actoMedico)
        {
            _cita = citaService;
            _actoMedico = actoMedico;            
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<CitaDTO>>> GetAll()
        {
            return await _cita.GetAllCitaPagadasNoPagadas();
        }

        [HttpGet("id")]
        public async Task<ActionResult<CitaDTO>> GetById([FromQuery] string id)
        {
            return await _cita.GetByIdCitasPagadasNoPagadas(id);
        }

        [HttpGet("listacitas/{medico}/{month}/{year}")]
        public async Task<ActionResult<List<CitaDTO2>>> GetListaCitasPorFechaMedico(string medico, int month, int year)
        {
            return await _cita.GetCitasbyMedicoFecha(medico, month, year);
        }

        [HttpGet("actomedico")]
        public async Task<ActionResult<List<ActoMedico>>> GetAllActoMedico()
        {
            return await _actoMedico.GetAll();
        }
        [HttpPost("cita")]
        public Cita CreateCita([FromBody] Cita cita)
        {
            return _cita.CreateCita(cita);
        }
        [HttpGet("citaactomedico")]
        public async Task<CitaActoMedioDTO> GetCitaAndActoMedico([FromQuery] string idCita)
        {
            return await _cita.GetCitaAndActoMedico(idCita);
        }
    }
}