﻿using Microsoft.AspNetCore.Mvc;
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
    public class MedicinasController:ControllerBase
    {
        private readonly MedicinasServices _medicinasService;
        public MedicinasController(MedicinasServices medicinasService)
        {
            _medicinasService = medicinasService;
        }
        [HttpGet("all")]
        public async Task<List<Medicinas>> GetAll()
        {
            return await _medicinasService.GetAll();
        }
        [HttpGet("filter")]
        public async Task<ActionResult<Medicinas>> GetByDescription(string descripcion)
        {
            return await _medicinasService.GetByDescripcionFiltrer(descripcion);
        }
    }
}