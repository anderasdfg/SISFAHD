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
    public class NotificacionesController
    {
        private readonly NotificacionesService _notificacionCita ;
        public NotificacionesController(NotificacionesService notificacionservice)
        {
            _notificacionCita = notificacionservice;
        }
        [HttpPost("")]
        public async Task<ActionResult<Cita>> EnviarCorreo([FromQuery] string idCita)
        {
            return await _notificacionCita.sendNotificacion(idCita);
        }
    }
}
