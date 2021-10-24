using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class MedicinasServices
    {
        private readonly IMongoCollection<Medicinas> _MedicinasCollection;
        public MedicinasServices(ISisfahdDatabaseSettings settings)
        {
            var medicinas = new MongoClient(settings.ConnectionString);
            var database = medicinas.GetDatabase(settings.DatabaseName);
            _MedicinasCollection = database.GetCollection<Medicinas>("medicinas");
        }
        public async Task<List<Medicinas>> GetAll()
        {
            List<Medicinas> medicina = new List<Medicinas>();
            var project = new BsonDocument("$project", new BsonDocument { { "descripcion", 1 }, { "generico", 1 }, { "precio", 1 } });
            var limit = new BsonDocument("$limit", 1000);
            medicina = await _MedicinasCollection.Aggregate().AppendStage<dynamic>(project).AppendStage<Medicinas>(limit).ToListAsync();
            return medicina;
        }
        public async Task<List<Medicinas>> GetByDescripcionFiltrer(string descripcion)
        {
            List<Medicinas> medicina = new List<Medicinas>();
            var match = new BsonDocument("$match",
                        new BsonDocument("descripcion",
                        new BsonDocument("$regex", descripcion)));
            medicina = await _MedicinasCollection.Aggregate().AppendStage<Medicinas>(match).ToListAsync();
            return medicina;
        }
    }
}
