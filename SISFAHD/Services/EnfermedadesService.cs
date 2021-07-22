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
    public class EnfermedadesService
    {
        private readonly IMongoCollection<Enfermedad> _EnfermdedadCollection;
        public EnfermedadesService(ISisfahdDatabaseSettings settings)
        {
            var medicamento = new MongoClient(settings.ConnectionString);
            var database = medicamento.GetDatabase(settings.DatabaseName);
            _EnfermdedadCollection = database.GetCollection<Enfermedad>("enfermedades");
        }
        public async Task<List<Enfermedad>> GetByCieDescription(string cie, string descripcion)
        {
            List<Enfermedad> enfermedades = new List<Enfermedad>();
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "codigo_cie", new BsonDocument("$regex", cie)  },
                                    { "descripcion", new BsonDocument("$regex", descripcion)  }
                                });
            enfermedades = await _EnfermdedadCollection.Aggregate().AppendStage<Enfermedad>(match).ToListAsync();
            return enfermedades;
        }
        public async Task<List<Enfermedad>> GetByCodigo(string codigo)
        {
            List<Enfermedad> enfermedad = new List<Enfermedad>();
            var match = new BsonDocument("$match",
                        new BsonDocument("codigo_cie",
                        new BsonDocument
                        {
                            { "$regex", codigo },
                            { "$options", "g" }
                        }));

            enfermedad = await _EnfermdedadCollection.Aggregate().AppendStage<Enfermedad>(match).ToListAsync();
            return enfermedad;
        }
    }
}
