using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace SISFAHD.Services
{
    public class ResultadoExamenService
    {
        private readonly IMongoCollection<Paciente> _PacienteCollection;
        public ResultadoExamenService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
        }
        public List<PacienteDTO> getExamenesSolicitados(Paciente paciente)
        {
            //--------Tu código---------
            return null; //Borrar esta linea
        }
    }
}
