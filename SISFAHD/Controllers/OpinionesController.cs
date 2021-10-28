using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using SISFAHD.Services;
using System.Web.Http.Cors;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpinionesController : ControllerBase
    {
        private readonly OpinionesService _opinionesService;
        public OpinionesController(OpinionesService opinionesService)
        {
            _opinionesService = opinionesService;
        }

        [HttpGet("all")]
        public ActionResult<Tuple<List<Opiniones>,Double>> GetAll([FromQuery] string idMedico)
        {
            return _opinionesService.GetAll_By_Medico(idMedico);
        }
    }
}
