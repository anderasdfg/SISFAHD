﻿using Microsoft.AspNetCore.Mvc;
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
    public class PedidosController : ControllerBase
    {
        private readonly PedidosService _pedidosService;
        public PedidosController(PedidosService pedidosService)
        {
            _pedidosService = pedidosService;
        }
        [HttpGet("all")]
        public async Task<List<Pedidos>> GetAll()
        {
            return _pedidosService.GetAll();
        }
        [HttpPost("")]
        public ActionResult<Pedidos> CreatePedido(Pedidos pedido)
        {
            Pedidos Pedido = _pedidosService.CreatePedido(pedido);
            return Pedido;
        }
        [HttpPut("")]
        public ActionResult<Pedidos> ModifyPedido(Pedidos pedido)
        {
            return _pedidosService.UpdatePedido(pedido);
        }
        [HttpGet("byidpaciente")]
        public async Task<ActionResult<List<Pedidos>>> GetByIdPaciente(string id_paciente)
        {
            return await _pedidosService.GetByIdPaciente(id_paciente);
        }
        [HttpGet("pacientepedidopendiente")]
        public async Task<ActionResult<List<PedidoDTO>>> GetPacientesPedidosPendientes()
        {
            return await _pedidosService.GetPacientesPedidosPendientes();
        }
        [HttpGet("byidpacientepedidospendiente")]
        public async Task<ActionResult<List<Pedidos>>> GetByPacientePendiente(string id_paciente)
        {
            return await _pedidosService.GetByPacientePendiente(id_paciente);
        }
    }
}
