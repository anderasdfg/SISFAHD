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
    public class RealizarPagoController : Controller
    {
        private readonly RealizarPagoService _realizarpagoservice;
        public RealizarPagoController(RealizarPagoService residenteservice, IFileStorage fileStorage)
        {
            _realizarpagoservice = residenteservice;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<RealizarPagoDTO>>> GetAll()
        {
            return await _realizarpagoservice.GetAll();
        }
    }
}