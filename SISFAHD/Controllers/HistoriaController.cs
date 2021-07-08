using Microsoft.AspNetCore.Mvc;
using SISFAHD.Entities;
using SISFAHD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoriaController : ControllerBase
    {
        private readonly HistoriaService _historiaservice;
        public HistoriaController(HistoriaService historiaservice)
        {
            _historiaservice = historiaservice;
        }
        [HttpGet("id")]
        public ActionResult<Historia> Get([FromQuery] string id)
        {
            return _historiaservice.GetById(id);
        }
        [HttpPut("historia")]
        public ActionResult<Historia> PushHistorial(Historia historia)
        {
            return _historiaservice.ModifyHistoria(historia);
        }
    }
}
