﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;
using System.Threading.Tasks;
using SISFAHD.DTOs;


namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly MedicoService _medicoservice;
        public MedicoController(MedicoService medicosservice)
        {
            _medicoservice = medicosservice;
        }

        [HttpGet("all")]
        public ActionResult<List<Medico>> GetAll()
        {
            return _medicoservice.GetAll();
        }

        [HttpGet("especialidad")]
        public async Task<ActionResult<List<MedicoDTO>>> GetMedicoXEspecialidad(string idEspecialidad)
        {
            return await _medicoservice.GetMedicosByEspecialidad(idEspecialidad);
        }
        [HttpGet("medicoespcialidad/{idMedico}")]
        public async Task<ActionResult<MedicoDTOEspcialidad>> GetByIdMedico(string idMedico)
        {
            return await _medicoservice.GetMedicosAndEspecialidad(idMedico);
        }
        [HttpGet("medicousuario/{idMedico}")]
        public async Task<ActionResult<MedicoDTO3>> GetByIdMedicoUsuario(string idMedico)
        {
            return await _medicoservice.GetMedicosAndDatosUsuario(idMedico);
        }
    }
}
