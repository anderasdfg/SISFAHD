using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.Entities;


namespace SISFAHD.Services
{
    public class MedicoService
    {
        private readonly IMongoCollection<Medico> _medicos;
        public MedicoService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _medicos = database.GetCollection<Medico>("medicos");
        }

        public List<Medico> GetAll()
        {
            List<Medico> medicos = new List<Medico>();
            medicos = _medicos.Find(Medico => true).ToList();
            return medicos;
        }        
    }
}
