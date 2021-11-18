using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using SISFAHD.Services;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenesController : ControllerBase
    {
        private readonly OrdenesService _ordenesService;

        public OrdenesController(OrdenesService ordenesService)
        {
            _ordenesService = ordenesService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<OrdenesDTO>>> GetAllOrdenes([FromQuery] string idUsuario)
        {
            return await _ordenesService.GetAllExamenesAuxiliares_By_Paciente(idUsuario);
        }
        [HttpGet("allOrdenes")]
        public async Task<ActionResult<List<OrdenesDTO_GetAll>>> GetAll_By_Paciente([FromQuery] string idUsuario)
        {
            return await _ordenesService.GetAll_By_Paciente(idUsuario);
        }
        [HttpPost("")]
        public ActionResult<Ordenes> CreateOrdenes(Ordenes ordenes)
        {
            Ordenes objeOrden = _ordenesService.CreateOrdenes(ordenes);
            return objeOrden;
        }
        [HttpGet("ordenesbypaciente")]
        public async Task<ActionResult<List<OrdenDTO2>>> GetOrdenesByIdPaciente(string id_paciente)
        {
            return await _ordenesService.GetOrdenesByPaciente(id_paciente);
        }
        [HttpPut("modificarestado/{id_orden}/{id_examen}/{id_turno_orden}/{estado}/{id_resultados}")]
        public async Task<ActionResult<Ordenes>> ModifyState (string id_orden, string id_examen, string id_turno_orden, string estado, string id_resultados)
        {
            return await _ordenesService.ModificarOrdenesEstado(id_orden, id_examen, id_turno_orden, estado, id_resultados);
        }
    }
}
