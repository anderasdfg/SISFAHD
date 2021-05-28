using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Route("api/[controller]")]
    public class CitaController : Controller
    {
        private readonly CitaService _realizarpagoservice;
        public CitaController(CitaService residenteservice)
        {
            _realizarpagoservice = residenteservice;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<CitaDTO>>> GetAll()
        {
            return await _realizarpagoservice.GetAll();
        }

        [HttpGet("id")]
        public async Task<ActionResult<CitaDTO>> GetById([FromQuery] string id)
        {
            return await _realizarpagoservice.GetById(id);
        }
    }
}