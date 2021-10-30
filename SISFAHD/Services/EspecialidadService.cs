using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        public Especialidad GetByID(string id)
        {
            Especialidad especialidad = new Especialidad();
            especialidad = _especialidades.Find(especialidad => especialidad.id == id).FirstOrDefault();
            return especialidad;

        }
        public Especialidad ModificarEspecialidad2(Especialidad especialidad)
        {

            var filter = Builders<Especialidad>.Filter.Eq("id", especialidad.id);
            var update = Builders<Especialidad>.Update
                .Set("nombre", especialidad.nombre)
                .Set("codigo", especialidad.codigo)
                .Set("descripcion", especialidad.descripcion)
                .Set("url", especialidad.url)
                .Set("estado", especialidad.estado);
            especialidad = _especialidades.FindOneAndUpdate<Especialidad>(filter, update, new FindOneAndUpdateOptions<Especialidad>
            {
                ReturnDocument = ReturnDocument.After
            });
            return especialidad;
        }
        public Especialidad CrearEspecialdiad2(Especialidad especialidad) 
        {
            
            _especialidades.InsertOne(especialidad);
            return especialidad;
        }
        
    }
}
