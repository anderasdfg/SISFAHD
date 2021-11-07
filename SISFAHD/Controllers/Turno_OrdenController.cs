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

    }
}
