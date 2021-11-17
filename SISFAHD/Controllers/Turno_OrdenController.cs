using Microsoft.AspNetCore.Mvc;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Turno_OrdenController
    {
        private readonly Turno_OrdenService _turnoservice;
        public Turno_OrdenController(Turno_OrdenService turnoService)
        {
            _turnoservice = turnoService;
        }
        [HttpGet("GetAll")]
        public ActionResult<List<Turno_Ordenes>> GetAll()
        {
            return _turnoservice.GetAll();
        }
        [HttpGet("GetByID")]
        public ActionResult<Turno_Ordenes> GetID(string id)
        {
            return _turnoservice.GetById(id);
        }
        [HttpPost("Create")]
        public ActionResult<Turno_Ordenes> CreateTurno(Turno_Ordenes turno)
        {
            return _turnoservice.CreateTurno(turno);
        }
        [HttpPut("Modify")]
        public ActionResult<Turno_Ordenes> ModifyTurno(Turno_Ordenes turno)
        {
            return _turnoservice.ModifyTurno(turno);
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult<Turno_Ordenes>> DeleteTurno(string id)
        {
            
            return await _turnoservice.DeleteTurno(id);
        }
        [HttpGet("listaturnosO/{idMedico}/{month}/{year}")]
        public async Task<ActionResult<List<Turno_Ordenes>>> GetByIdMedico(string idMedico, int month, int year)
        {
            return await _turnoservice.GetByMedico(idMedico, month, year);
        }
        [HttpPost("listaturnosO")]
        public async Task<ActionResult<List<Turno_Ordenes>>> GetBy_Especialidad_Fecha(Turno_OrdenDTO_By_Especialidad_Fecha consultas)
        {
            return await _turnoservice.GetBy_Especialidad_Fecha(consultas);
        }
    }
}
