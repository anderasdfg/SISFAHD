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
    public class ProcedimientoController
    {
        private readonly ProcedimientoService _procedimientoService;
        public ProcedimientoController(ProcedimientoService procedimientoService)
        {
            _procedimientoService = procedimientoService;
        }
        [HttpGet("obtenerpornombre")]
        public async Task<List<Procedimiento>> GetByName([FromQuery] string nombre)
        {
            return await _procedimientoService.GetByNombre(nombre);
        }
    }
}
