using Microsoft.AspNetCore.Mvc;
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
    public class TurnoController
    {
        private readonly TurnoService _turnoservice;
        public TurnoController(TurnoService turnoService)
        {
            _turnoservice = turnoService;
        }
        [HttpGet("all")]
        public ActionResult<List<Turno>> GetAll()
        {
            return _turnoservice.GetAll();
        }
        [HttpGet("idmedico")]
        public async Task<ActionResult<List<Turno>>> GetByIdMedico([FromQuery] string idMedico)
        {
            return await _turnoservice.GetByMedico(idMedico);
        }
        [HttpPost("")]
        public ActionResult<Turno> CreateTurno(Turno turno)
        {
            return _turnoservice.CreateTurno(turno);
        }
        [HttpPut("")]
        public ActionResult<Turno> ModifyTurno(Turno turno)
        {
            return _turnoservice.ModifyTurno(turno);
        }
        [HttpDelete("")]
        public async Task<ActionResult<String>> DeleteTurno([FromQuery] string id)
        {
            await _turnoservice.DeleteTurno(id);
            return $"se elimin el turno {id}";
        }
    }
}
