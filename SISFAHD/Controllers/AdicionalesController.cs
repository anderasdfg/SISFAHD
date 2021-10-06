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
    public class AdicionalesController : ControllerBase
    {
        private readonly AdicionalesServices _adicionalservices;
        private readonly IFileStorage _fileStorage;

        public AdicionalesController(AdicionalesServices adicionalservice, IFileStorage fileStorage)
        {
            _adicionalservices = adicionalservice;
            _fileStorage = fileStorage;
        }

        [HttpGet("all")]
        public ActionResult<List<Adicionales>> GetAll()
        {
            return _adicionalservices.GetAll();
        }

        [HttpGet("Titulo")]
        public ActionResult<Adicionales> Get([FromQuery] string titulo)
        {
            return _adicionalservices.GetByTitulo(titulo);
        }

        [HttpGet("Id")]
        public ActionResult<Adicionales> GetActionResult([FromQuery] string id)
        {
            return _adicionalservices.GetByID(id);
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<Adicionales>> CrearComplementario(Adicionales complementario)
        {
            if (!string.IsNullOrWhiteSpace(complementario.url))
            {
                var profileimg = Convert.FromBase64String(complementario.url);
                complementario.url = await _fileStorage.SaveFile(profileimg, "jpg", "especialidad");
            }
            Adicionales objetocomplemtario = _adicionalservices.CrearAdicionales(complementario);
            return objetocomplemtario;
        }
        [HttpPut("Modificar")]
        public async Task<ActionResult<Adicionales>> ModificarComplementario(Adicionales id)
        {
            try
            {
                if (!id.url.StartsWith("http"))
                {
                    if (!string.IsNullOrWhiteSpace(id.url))
                    {
                        var profileimg = Convert.FromBase64String(id.url);
                        id.url = await _fileStorage.EditFile(profileimg, "jpg", "especialidad", id.url);
                    }
                }
                Adicionales objetoadicional = _adicionalservices.ModificarAdicionaless(id);
                return objetoadicional;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

    }
}
