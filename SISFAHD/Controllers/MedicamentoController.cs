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
    public class MedicamentoController: ControllerBase
    {
        private readonly MedicamentoService _medicamentoService;
        public MedicamentoController(MedicamentoService medicamentoService)
        {
            _medicamentoService = medicamentoService;
        }

        [HttpGet("all")]
        public async Task<List<Medicamento>> GetAll()
        {
            return await _medicamentoService.GetAll();
        }
        //[HttpGet("")]
        //public ActionResult<List<Medicamento>> GetByName([FromQuery] string name)
        //{
        //    return _medicamentoService.GetByName(name);
        //}
        [HttpGet("Filter")]
        public async Task<ActionResult<List<Medicamento>>> GetByNameConcentrationForma(string nombre="", string concentracion="", string forma="")
        {
            return await _medicamentoService.GetByNameConcentrationForm(nombre,concentracion,forma);
        }
    }
}
