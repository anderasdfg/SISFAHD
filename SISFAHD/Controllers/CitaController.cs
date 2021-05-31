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
    [Route("api/[controller]")]
    public class CitaController : Controller
    {
        private readonly CitaService _pagocita;
        private readonly ActoMedicoService _actoMedico;
        public CitaController(CitaService pagoservicio, ActoMedicoService actoMedico)
        {
            _pagocita = pagoservicio;
            _actoMedico = actoMedico;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<CitaDTO>>> GetAll()
        {
            return await _pagocita.GetAll();
        }

        [HttpGet("id")]
        public async Task<ActionResult<CitaDTO>> GetById([FromQuery] string id)
        {
            return await _pagocita.GetById(id);
        }

        [HttpPut("")]
        public async Task<ActionResult<Cita>> PutEstadoPago([FromBody] Cita cita)
        {            
            return await _pagocita.ModifyEstadoPagoCita(cita);
        }

        [HttpPost("")]
        public async Task<ActionResult<Venta>> PostSesionesEducativas([FromBody] Venta pagorealizado)
        {
            return await _pagocita.CreateUnNuevoPagoRealizado(pagorealizado);
        }

        [HttpGet("listacitas/{turno}/{month}/{year}")]
        public async Task<ActionResult<List<CitaDTO2>>> GetListaFechas(string turno, int month, int year)
        {
            return await _pagocita.GetCitasbyMedicoFecha(turno, month, year);
        }

        [HttpGet("actomedico")]
        public async Task<ActionResult<List<ActoMedico>>> GetAllActoMedico()
        {
            return await _actoMedico.GetAll();
        }
    }
}