﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using SISFAHD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController:Controller
    {
        private readonly VentaService _pagocita;

        public VentaController(VentaService ventaService)
        {
            _pagocita = ventaService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<VentaDTO>>> GetAll()
        {
            return await _pagocita.GetAllVentas();
        }

        [HttpGet("id")]
        public async Task<ActionResult<VentaDTO>> GetById([FromQuery] string id)
        {
            return await _pagocita.GetById(id);
        }

        [HttpPost("")]
        public async Task<ActionResult<Venta>> PostCrearVenta([FromBody] Venta pagorealizado)
        {
            return _pagocita.CrearVenta(pagorealizado);
        }
        //[HttpPost("/responsevisa/{id_cita}")]
        //public async Task<ActionResult<Venta>> PostTransaccion(String id_cita, [FromForm] ResponsePost response)
        //{
        //    return await _pagocita.ConcretandoTransaccion(id_cita, response);
        //}
        [HttpPut("token")]
        public ActionResult<Venta> PutVentaToken(Venta venta)
        {
            return _pagocita.ModifyTokenVenta(venta);
        }

        [HttpPost]
        [Route("[action]/{id_cita}")]
        public async Task<IActionResult> Test(String id_cita, [FromForm] ResponsePost response)
        {
            PagoProcesadoDTO pagoProcesado = new PagoProcesadoDTO();
            pagoProcesado = await _pagocita.ConcretandoTransaccion(id_cita, response);

            return View();
        }
        /*  
          [HttpPost("responsevisa/{id_cita}")]
          public string SuccessfulResponse([FromBody] string body)
          {
              return _pagocita.SuccessfulResponse(body);
          }*/
    }
}
