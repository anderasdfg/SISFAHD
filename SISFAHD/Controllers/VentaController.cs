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
        public async Task<ActionResult<Venta>> PostSesionesEducativas([FromBody] Venta pagorealizado)
        {
            return await _pagocita.CrearVenta(pagorealizado);
        }
        [HttpPost("/responsevisa/:purchase")]
        public async Task<ActionResult<Venta>> PostTransaccion([FromBody] Venta pagorealizado,string id_cita)
        {
            return await _pagocita.ConcretandoTransaccion(pagorealizado,id_cita);
        }
        /*
        [HttpPost("responsevisa/{id_cita}")]
        public string  PostHtml([FromBody] string html)
        {
            return _pagocita.PostHtml(html);
        }*/
    }
}
