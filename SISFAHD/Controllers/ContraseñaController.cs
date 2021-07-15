using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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
    [ApiController]
    [Route("api/[controller]")]
    public class ContraseñaController : Controller
    {
        private readonly ContraseñaService _contraseña;
        private readonly IMongoCollection<Usuario> _UsuarioCollection;
        public ContraseñaController(ContraseñaService contraseña, ISisfahdDatabaseSettings settings)
        {

            _contraseña = contraseña;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _UsuarioCollection = database.GetCollection<Usuario>("usuarios");

        }

        [HttpGet("Notificacion")]

        public bool SendCode(string correo)
        {
           return _contraseña.SendNotification(correo);
        }
        // Cambiar contraseña

        [HttpPut("Modificar")]
        public void ModificarPass(string code, string pass)
        {
            _contraseña.ModificarPass(code, pass);
        }
        // Verificar Codigo
        [HttpGet("Verify")]
        public bool VerifyCode(string code)
        {

          return  _contraseña.VerifyPass(code);

        }
    }
}
