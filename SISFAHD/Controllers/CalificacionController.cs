using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SISFAHD.Services;
using SISFAHD.Entities;
using SISFAHD.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SISFAHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionController : ControllerBase
    {

        private readonly CalificacionService _atencionservices;
        private readonly CitaService _cita;
        private readonly IFileStorage _fileStorage;

        public CalificacionController(CalificacionService atencionservices, IFileStorage fileStorage) {

            _atencionservices = atencionservices;
            _fileStorage = fileStorage;
        }

        [HttpGet("all")]

        public ActionResult<List<Opiniones>> GetAll() {

            return _atencionservices.GetaAll();
        }

        [HttpGet("pruebaEstado")]

        public int PruebaEstado(string id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    return _atencionservices.GetEstadobyIdCita(id);
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<Opiniones>> Calificar(Opiniones opiniones)
        {
            return await _atencionservices.CrearOpiniones(opiniones);
        }


     

    }
}
