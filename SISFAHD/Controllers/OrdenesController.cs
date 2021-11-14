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
        [HttpPost("")]
        public ActionResult<Ordenes> CreateOrdenes(Ordenes ordenes)
        {
            Ordenes objeOrden = _ordenesService.CreateOrdenes(ordenes);
            return objeOrden;
        }
    }
}
