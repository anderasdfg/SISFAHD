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
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticaController : ControllerBase
    {                       
        private readonly EstadisticaService _estadistica;
        public EstadisticaController(EstadisticaService estadistica)
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

        [HttpGet("especialidadespedidas")]
        public async Task<ActionResult<List<EspecialidadesMPedidas>>> EspecialidadesMasPedidas()
        {
                return await _estadistica.EspecialidadesMasPedidas();
        }
        [HttpGet("medicamentospedidos")]
        public async Task<ActionResult<List<MedicamentosMPedidos>>> MedicamentosMasPedidos()
        {
            return await _estadistica.MedicamentosMasPedidos();
        }
        [HttpGet("laboratoriopedidos")]
        public async Task<ActionResult<List<LaboratorioPedidos>>> LaboratorioMasPedidos()
        {
            return await _estadistica.LaboratorioMasPedidos();
        }
    }
}
