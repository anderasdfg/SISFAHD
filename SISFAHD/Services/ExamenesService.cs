using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.Entities;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class ExamenesService
    {

        private readonly IMongoCollection<Examenes> _examenes;
        public ExamenesService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _examenes = database.GetCollection<Examenes>("examenes");
        }
        public List<Examenes> GetAll()
        {
            List<Examenes> examenes = new List<Examenes>();
            examenes = _examenes.Find(examenes => true).ToList();
            return examenes;
        }

        public async Task<List<Examenes>> GetByNombre(string nombre)
        {
            List<Examenes> examenes = new List<Examenes>();
            var match = new BsonDocument("$match",
                        new BsonDocument("descripcion",
                        new BsonDocument
                        {
                            { "$regex", nombre.ToUpper() },
                            { "$options", "g" }
                        }));
            examenes = await _examenes.Aggregate().AppendStage<Examenes>(match).ToListAsync();
            return examenes;
        }

        public Examenes GetByID(string id)
        {
            Examenes examen = new Examenes();
            examen = _examenes.Find(examen => examen.id == id).FirstOrDefault();
            return examen;

        }
    }
}
