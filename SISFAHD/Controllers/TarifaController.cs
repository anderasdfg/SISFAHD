using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Entities;
using SISFAHD.Services;
using SISFAHD.Helpers;
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

        [HttpPut("tarifasmedico/Modificar")]
        public async Task<ActionResult<Tarifa>> ModificarTarifa(Tarifa id)
        {
            Tarifa tarifa = await _tarifaservice.ModifyTarifa(id);
            return tarifa;

        }

        [HttpPost("tarifasmedico/Registrar")]
        public async Task<ActionResult<Tarifa>> CrearTarifa(Tarifa tarifa)
        {
            return await _tarifaservice.Createtarifas(tarifa);
        }

        //[HttpDelete("tarifasmedico/Delete")]

    }
}
