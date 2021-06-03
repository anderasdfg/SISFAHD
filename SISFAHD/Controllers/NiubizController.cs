using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using SISFAHD.DTOs;

namespace SISFAHD.Controllers
{
    [Route("responsevisa")]
    public class NiubizController : Controller
    {
        static ISisfahdDatabaseSettings settings;
        readonly VentaService _ventaService = new VentaService(settings);

        //[HttpGet("{id}")]
        //public IActionResult Index()
        //{            
        //    return View();
        //}
        
        [HttpGet("{id_cita}")]
        [HttpPost("{id_cita}")]

        public async Task<ActionResult> Index(String id_cita, [FromForm] ResponsePost response)
        {
            PagoProcesadoDTO pagoProcesado = new PagoProcesadoDTO();
            pagoProcesado = await _ventaService.ConcretandoTransaccion(id_cita, response);
            return View();
        }
    }
}
