using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.DTOs;
using SISFAHD.Entities;

namespace SISFAHD.Services
{
    public class OpinionesService
    {
        private readonly IMongoCollection<Opiniones> _opiniones;

        public OpinionesService(ISisfahdDatabaseSettings settings)
        {
            var paciente = new MongoClient(settings.ConnectionString);
            var database = paciente.GetDatabase(settings.DatabaseName);
            _opiniones = database.GetCollection<Opiniones>("opiniones");
        }

        public Tuple<List<Opiniones>,Double> GetAll_By_Medico(string idMedico)
        {
            List<Opiniones> opiniones = new List<Opiniones>();
            opiniones = _opiniones.Find(opiniones => opiniones.datos_medico.id_medico == idMedico).ToList();
            int contador = 0;
            double sumatoria = 0;
            double promedio;
            if(opiniones.Count > 0)
            {
                foreach (Opiniones opinion in opiniones)
                {
                    sumatoria += opinion.calificacion;
                    contador++;
                }
                promedio = sumatoria / contador;
            }
            else { promedio = 0; }
            return Tuple.Create(opiniones, promedio);
        }
    }
}
