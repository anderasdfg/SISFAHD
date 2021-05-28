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
    public class PacienteService
    {
        private readonly IMongoCollection<Paciente> _PacienteCollection;
        public PacienteService(ISisfahdDatabaseSettings settings)
        {
            var paciente = new MongoClient(settings.ConnectionString);
            var database = paciente.GetDatabase(settings.DatabaseName);
            _PacienteCollection = database.GetCollection<Paciente>("pacientes");
        }
        public List<Paciente> GetAll()
        {
            List<Paciente> paciente = new List<Paciente>();
            paciente = _PacienteCollection.Find(paciente => true).ToList();
            return paciente;
        }
        public Paciente GetById(string id)
        {
            Paciente paciente = new Paciente();
            paciente = _PacienteCollection.Find(paciente => paciente.id == id).FirstOrDefault();
            return paciente;
        }
        public async Task<Paciente> CreatePaciente(Paciente p)
        {
            _PacienteCollection.InsertOne(p);
            return p;
        }
        public async Task<Paciente> ModifyPaciente(Paciente p)
        {
            var filter = Builders<Paciente>.Filter.Eq("id",p.id);
            var update = Builders<Paciente>.Update
                .Set("datos", p.datos)
                .Set("antecedentes", p.antecedentes)
                .Set("id_historia", p.idHistoria)
                .Set("archivos", p.archivos);
            _PacienteCollection.UpdateOne(filter,update);
            return p;
        }
    }
}
