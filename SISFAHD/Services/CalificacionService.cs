using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

using SISFAHD.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace SISFAHD.Services
{
    public class CalificacionService
    {
        private readonly IMongoCollection<Opiniones> _opiniones;
        private readonly IMongoCollection<Cita> _cita;


        public CalificacionService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _opiniones = database.GetCollection<Opiniones>("opiniones");
            _cita = database.GetCollection<Cita>("citas");
        }

        public List<Opiniones> GetaAll()
        {
            List<Opiniones> opiniones = new List<Opiniones>();
            opiniones = _opiniones.Find(Opiniones => true).ToList();
            return opiniones;
        }

        public int GetEstadobyIdCita(string idCita)
        {
            Cita estado = new Cita();
            estado = _cita.Find(idestado => idestado.id == idCita).FirstOrDefault();
            if (estado.estado_atencion == "atendido") { return 1; }
            else if (estado.estado_atencion == "evaluado") { return 2; }
            else return 0; //estado null o no atendido 0 -- // atendido 1 //evaluado 2
        }

        public async Task<ActionResult<Opiniones>> CrearOpiniones(Opiniones opiniones)
        {
            await _opiniones.InsertOneAsync(opiniones);
            return opiniones;

        }

        
        
    }
}
