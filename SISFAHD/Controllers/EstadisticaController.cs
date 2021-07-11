using System;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using SISFAHD.Services;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SISFAHD.Controllers
{
    public class EstadisticaController
    {
        [ApiController]
        [Route("api/[controller]")]
        public class EstadisticasController
        {
            private readonly EstadisticaService _estadistica;
            public EstadisticasController(EstadisticaService estadistica)
            {
                _estadistica = estadistica;
            }
            [HttpGet("xMedico")]
            public async Task<ActionResult<EstadisticaDTO>> CitasxMedico(string idMedico, string estado = null)
            {
                if (String.IsNullOrEmpty(estado))
                {
                    return await _estadistica.CitasxMedico(idMedico);
                }
                else
                {
                    return await _estadistica.CitasxMedico_y_Estado(idMedico, estado);
                }
            }
            [HttpGet("xEspecialidad")]
            public async Task<ActionResult<EstadisticaDTO>> CitasxEspecialidad(string especialidad, string estado = null)
            {
                if (String.IsNullOrEmpty(estado))
                {
                    return await _estadistica.CitasxEspecialidad(especialidad);
                }
                else
                {
                    return await _estadistica.CitasxEspecialidad_yEstado(especialidad, estado);
                }
            }
            [HttpGet("xPaciente")]
            public async Task<ActionResult<EstadisticaDTO>> CitasxPaciente(string idPaciente, string estado = null)
            {
                if (String.IsNullOrEmpty(estado))
                {
                    return await _estadistica.CantidadCitasxPaciente(idPaciente);
                }
                else
                {
                    return await _estadistica.CantidadCitasxPaciente_y_Estado(idPaciente, estado);
                }
            }
        }
    }
}
