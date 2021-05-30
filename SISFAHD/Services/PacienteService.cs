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
        private readonly IMongoCollection<Usuario> _UsuarioCollection;
        public PacienteService(ISisfahdDatabaseSettings settings)
        {
            var paciente = new MongoClient(settings.ConnectionString);
            var database = paciente.GetDatabase(settings.DatabaseName);
            _PacienteCollection = database.GetCollection<Paciente>("pacientes");
            _UsuarioCollection = database.GetCollection<Usuario>("usuarios");
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
            Usuario u = new Usuario();
            u = _UsuarioCollection.Find(user => user.id == p.idUsuario).FirstOrDefault();
            var filter = Builders<Usuario>.Filter.Eq("id", u.id);
            var update = Builders<Usuario>.Update
                .Set("rol", "607f37c1cb41a8de70be1df3");
            _UsuarioCollection.UpdateOne(filter, update);
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
