using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.Entities;


namespace SISFAHD.Services
{
    public class EspecialidadService
    {
        private readonly IMongoCollection<Especialidad> _especialidades;
        public EspecialidadService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _especialidades = database.GetCollection<Especialidad>("especialidades");
        }

        public List<Especialidad> GetAll()
        {
            List<Especialidad> especialidades = new List<Especialidad>();
            especialidades = _especialidades.Find(Especialidad => true).ToList();
            return especialidades;
        }
        public Especialidad GetByNombre(string nombre)
        {
            Especialidad especialidad = new Especialidad();
            especialidad = _especialidades.Find(especialidad => especialidad.nombre == nombre).FirstOrDefault();
            return especialidad;
        }
    }
}
