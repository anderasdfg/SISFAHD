using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class TarifaController
    {
        private readonly TarifaService _tarifaservice;
        public TarifaController(TarifaService tarifaService)
        {
            _tarifaservice = tarifaService;
        }
        [HttpGet("tarifasmedico/{idMedico}")]
        public async Task<ActionResult<List<Tarifa>>> GetByIdMedico(string idMedico)
        {
            return await _tarifaservice.GetTarifasByIdMedico(idMedico);
        }

        [HttpGet("tarifasmedico/all")]
        public async Task<ActionResult<List<Tarifa>>> GetAll() 
        {
                return await _tarifaservice.GetAllTarifas();
        }
    }
}
