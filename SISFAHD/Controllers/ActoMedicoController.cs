using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Services;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActoMedicoController : ControllerBase
    {
        private readonly ActoMedicoService _actomedicoservice;
        public ActoMedicoController(ActoMedicoService actomedicoservice)
        {
            _actomedicoservice = actomedicoservice;
        }

        [HttpGet("id")]
        public async Task<ActionResult<ActoMedico>> GetActoMedicoById([FromQuery] string id)
        {
            return await _actomedicoservice.GetById(id);
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<ActoMedico>> PostActoMedico(ActoMedico actomedico)
        {
            return await _actomedicoservice.CrearActoMedico(actomedico);
        }
        [HttpPut("Actualizar")]
        public async Task<ActionResult<ActoMedico>> PutActoMedico(ActoMedicoDTO2 actomedico)
        {
            return await _actomedicoservice.ModificarActoMedico(actomedico);
        }
        [HttpPost("enviarDiagnostico")]
        public string SendDiagnostico(string idCita)
        {
            _actomedicoservice.sendNotificationDiagnostico(idCita);
            return "Correo enviado";
        }
    }
}