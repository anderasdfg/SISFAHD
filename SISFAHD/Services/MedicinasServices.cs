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
            //var limit = new BsonDocument("$limit", 1000);
            medicina = await _MedicinasCollection.Aggregate().AppendStage<Medicinas>(project).ToListAsync();
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
        public async Task<List<Medicinas>> GetByGenericoFiltrer(string generico)
        {
            List<Medicinas> medicina = new List<Medicinas>();
            var match = new BsonDocument("$match",
                        new BsonDocument("generico",
                        new BsonDocument("$regex", generico)));
            medicina = await _MedicinasCollection.Aggregate().AppendStage<Medicinas>(match).ToListAsync();
            return medicina;
        }
        public Medicinas CreateMedicinas(Medicinas medicinas)
        {
            _MedicinasCollection.InsertOne(medicinas);
            return medicinas;
        }
        public Medicinas UpdateMedicinas(Medicinas medicinas)
        {
            var filter = Builders<Medicinas>.Filter.Eq("id", medicinas.id);
            var update = Builders<Medicinas>.Update
                                 .Set("descripcion", medicinas.descripcion)
                                 .Set("generico", medicinas.generico)
                                 .Set("precio", medicinas.precio);
            medicinas = _MedicinasCollection.FindOneAndUpdate<Medicinas>(filter, update, new FindOneAndUpdateOptions<Medicinas>
            {
                ReturnDocument = ReturnDocument.After
            });
            return medicinas;
        }

        public Medicinas GetByID(string id)
        {
            Medicinas medicinas = new Medicinas();
            medicinas = _MedicinasCollection.Find(medicinas => medicinas.id == id).FirstOrDefault();
            return medicinas;
        }

        // 100 Medicinas _ Comprar Servicios Adicionales
        public async Task<List<Medicinas>> GetLimit()
        {
            List<Medicinas> examen = new List<Medicinas>();
            var limit = new BsonDocument("$limit", 100);
            examen = await _MedicinasCollection.Aggregate().AppendStage<Medicinas>(limit).ToListAsync();
            return examen;
        }

    }
}
