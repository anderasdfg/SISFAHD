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
                //return await _estadistica.CitasxMedico(idMedico);
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
        public async Task<ActionResult<List<EspecialidadesMPedidas>>> EspecialidadesMasPedidas(DateTime fecha)
        {
                return await _estadistica.EspecialidadesMasPedidas(fecha);
        }
        [HttpGet("medicamentospedidos")]
        public async Task<ActionResult<List<MedicamentosMPedidos>>> MedicamentosMasPedidos(DateTime fecha)
        {
            return await _estadistica.MedicamentosMasPedidos(fecha);
        }
        [HttpGet("laboratoriopedidos")]
        public async Task<ActionResult<List<LaboratorioPedidos>>> LaboratorioMasPedidos(DateTime fecha)
        {
            return await _estadistica.LaboratorioMasPedidos(fecha);
        }
        [HttpGet("Medico")]
        public async Task<ActionResult<List<CitasxMedicos>>> ECitasxMedico()
        {
                return await _estadistica.EstadisticasAllCitasxMedico();           
        }
        [HttpGet("MedicoyEstado")]
        public async Task<ActionResult<List<CitasxMedicosyEstadoAtencion>>> ECitasxMedicoyEstado(string idMedico = null,string estado = null)
        {
            if (String.IsNullOrEmpty(estado)) {
                return await _estadistica.EstadisticasCitasxMedicoyEstadoByMedico(idMedico);
            }
            else if (String.IsNullOrEmpty(idMedico))
            {
                return await _estadistica.EstadisticasCitasxMedicoyEstadoByEstado(estado);
            }
            else
            {
                return await _estadistica.EstadisticasCitasxMedicoyEstado(idMedico,estado);
            }
        }
        [HttpGet("Especialidad")]
        public async Task<ActionResult<List<CitasxEspecialidad>>> ECitasxEspecialidad()
        {
            return await _estadistica.EstadisticasAllCitasxEspecialidad();
        }
        [HttpGet("EspecialidadyEstado")]
        public async Task<ActionResult<List<CitasxEspecialidadyEstadoAtencion>>> ECitasxEspecialidadyEstado(string especialidad=null, string estado = null)
        {
            if (String.IsNullOrEmpty(estado))
            {
                return await _estadistica.EstadisticasCitasxEspecialidadyEstadoAtencionByEspecialidad(especialidad);
            }
            else if (String.IsNullOrEmpty(especialidad))
            {
                return await _estadistica.EstadisticasCitasxEspecialidadyEstadoAtencionByEstado(estado);
            }
            else
            {
                return await _estadistica.EstadisticasCitasxEspecialidadyEstadoAtencion(especialidad, estado);
            }
        }
        [HttpGet("Paciente")]
        public async Task<ActionResult<List<CitasxPaciente>>> ECitasxPaciente()
        {
            return await _estadistica.EstadisticasAllCitasxPaciente();
        }
        [HttpGet("PacienteyEspecialidad")]
        public async Task<ActionResult<List<CitasxPacienteyEstadoAtencion>>> ECitasxPacienteyEspecialidad(string idpaciente=null,string estado=null)
        {
            if (String.IsNullOrEmpty(estado))
            {
                return await _estadistica.EstadisticasCitasxPacienteyEstadoAtencion(idpaciente);
            }
            else if (String.IsNullOrEmpty(idpaciente))
            {
                return await _estadistica.EstadisticasCitasxPacienteyEstadoAtencionByEstado(estado);
            }
            else
            {
                return await _estadistica.EstadisticasCitasxPacienteyEstadoAtencion(idpaciente, estado);
            }
        }
    }
}
