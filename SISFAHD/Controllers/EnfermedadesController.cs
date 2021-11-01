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
    public class EnfermedadesController
    {
        private readonly EnfermedadesService _enfermedadesService;
        public EnfermedadesController(EnfermedadesService enfermedadesService)
        {
            _enfermedadesService = enfermedadesService;
        }
        [HttpGet("Filter")]
        public async Task<ActionResult<List<Enfermedad>>> GetByCieDescription(string cie = "", string descripcion = "")
        {
            return await _enfermedadesService.GetByCieDescription(cie, descripcion);
        }
        [HttpGet("obtenerporcodigo")]
        public async Task<List<Enfermedad>> GetByCodigo([FromQuery] string codigo)
        {
            return await _enfermedadesService.GetByCodigo(codigo);
        }
        /// 
        [HttpGet("all")]
        public async Task<List<Enfermedad>> GetAll()
        {
            return await _enfermedadesService.GetAll();
        }

        [HttpPost("Registrar")]
        public ActionResult<Enfermedad> Create(Enfermedad enfermedad)
        {
            Enfermedad objetoEnfermedad = _enfermedadesService.RegistrarEnfermedad(enfermedad);
            return objetoEnfermedad;
        }
        [HttpPut("Modificar")]
        public ActionResult<Enfermedad> Update(Enfermedad id)
        {
            Enfermedad objetoEnfermedad = _enfermedadesService.ModificarEnfermedad(id);
            return objetoEnfermedad;
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult<Enfermedad>> Delete(string id)
        {
            return await _enfermedadesService.EliminarEnfermedad(id);
        }

        [HttpGet("obtenerpordescripcion")]
        public async Task<List<Enfermedad>> GetByDesp([FromQuery] string descripcion)
        {
            return await _enfermedadesService.GetAllByDescrip(descripcion);
        }

    }
}
