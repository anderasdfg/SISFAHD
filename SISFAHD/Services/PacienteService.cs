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
        private readonly IMongoCollection<Historia> _HistoriaCollection;
        public PacienteService(ISisfahdDatabaseSettings settings)
        {
            var paciente = new MongoClient(settings.ConnectionString);
            var database = paciente.GetDatabase(settings.DatabaseName);
            _PacienteCollection = database.GetCollection<Paciente>("pacientes");
            _UsuarioCollection = database.GetCollection<Usuario>("usuarios");
            _HistoriaCollection = database.GetCollection<Historia>("historias");
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
            Usuario u = new Usuario();
            u = _UsuarioCollection.Find(user => user.id == p.id_usuario).FirstOrDefault();
            var filter = Builders<Usuario>.Filter.Eq("id", u.id);
            var update = Builders<Usuario>.Update
                .Set("rol", "607f37c1cb41a8de70be1df3");
            Historia h = new Historia();
            h.id = ObjectId.GenerateNewId().ToString();
            h.fecha_creacion = DateTime.Today;
            h.historial = new List<Historial>();
            h.numero_historia = u.datos.numero_Documento;
            p.id_historia = h.id;
            _HistoriaCollection.InsertOne(h);            
            _PacienteCollection.InsertOne(p);
            _UsuarioCollection.UpdateOne(filter, update);
            return p;
        }
        public async Task<Paciente> CreatePaciente2(Paciente p)
        {
            Usuario u = new Usuario();
            u = _UsuarioCollection.Find(user => user.id == p.id_usuario).FirstOrDefault();
            Historia h = new Historia();
            h.id = ObjectId.GenerateNewId().ToString();
            h.fecha_creacion = DateTime.Today;
            h.historial = new List<Historial>();
            h.numero_historia = u.datos.numero_Documento;
            p.id_historia = h.id;
            _HistoriaCollection.InsertOne(h);
            _PacienteCollection.InsertOne(p);
            return p;
        }
        public async Task<Paciente> ModifyPaciente(Paciente p)
        {
            var filter = Builders<Paciente>.Filter.Eq("id",p.id);
            var update = Builders<Paciente>.Update
                .Set("datos", p.datos)
                .Set("antecedentes", p.antecedentes)
                .Set("id_historia", p.id_historia)
                .Set("archivos", p.archivos);
            _PacienteCollection.UpdateOne(filter,update);
            return p;
        }
    }
}
