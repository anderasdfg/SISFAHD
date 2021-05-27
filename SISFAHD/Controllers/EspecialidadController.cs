using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EspecialidadController : ControllerBase
    {
        private readonly EspecialidadService _especialidadeservice;
        public EspecialidadController(EspecialidadService especialidadeservice)
        {
            _especialidadeservice = especialidadeservice;
        }

        [HttpGet("all")]
        public ActionResult<List<Especialidad>> GetAll()
        {
            return _especialidadeservice.GetAll();
        }
    }
}
