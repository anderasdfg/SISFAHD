using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;


namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly MedicoService _medicoservice;
        public MedicoController(MedicoService medicosservice)
        {
            _medicoservice = medicosservice;
        }

        [HttpGet("all")]
        public ActionResult<List<Medico>> GetAll()
        {
            return _medicoservice.GetAll();
        }
    }
}
