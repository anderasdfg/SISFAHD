using Microsoft.AspNetCore.Mvc;
using SISFAHD.Entities;
using SISFAHD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicinasController:ControllerBase
    {
        private readonly MedicinasServices _medicinasService;
        public MedicinasController(MedicinasServices medicinasService)
        {
            _medicinasService = medicinasService;
        }
        [HttpGet("all")]
        public async Task<List<Medicinas>> GetAll()
        {
            return await _medicinasService.GetAll();
        }
        [HttpGet("filter")]
        public async Task<ActionResult<List<Medicinas>>> GetByDescription(string descripcion)
        {
            return await _medicinasService.GetByDescripcionFiltrer(descripcion);
        }
        [HttpGet("generico")]
        public async Task<ActionResult<List<Medicinas>>> GetByGenerico(string generico)
        {
            return await _medicinasService.GetByGenericoFiltrer(generico);
        }
        [HttpPost("Registrar")]
        public ActionResult<Medicinas> Create(Medicinas medicinas)
        {
           
            Medicinas objmedicina = _medicinasService.CreateMedicinas(medicinas);
            return objmedicina;
        }
        
        [HttpPut("Modificar")]
        public ActionResult<Medicinas> Update(Medicinas id)
        {
            Medicinas objmedicina = _medicinasService.UpdateMedicinas(id);
            return objmedicina;
        }
    }
}
