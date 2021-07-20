using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class ProcedimientoService
    {
        private readonly IMongoCollection<Procedimiento> _ProcedimientoCollection;
        public ProcedimientoService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _ProcedimientoCollection = database.GetCollection<Procedimiento>("procedimientos");
        }
        public async Task<List<Procedimiento>> GetByNombre(string nombre)
        {
            List<Procedimiento> procedimiento = new List<Procedimiento>();
            var match = new BsonDocument("$match",
                        new BsonDocument("codigo_procedimiento",
                        new BsonDocument
                        {
                            { "$regex", nombre },
                            { "$options", "g" }
                        }));
            procedimiento = await _ProcedimientoCollection.Aggregate().AppendStage<Procedimiento>(match).ToListAsync();
            return procedimiento;
        }
    }
}
