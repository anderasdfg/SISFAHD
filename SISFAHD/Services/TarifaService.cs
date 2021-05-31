using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class TarifaService
    {
        private readonly IMongoCollection<Tarifa> _tarifas;
        public TarifaService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _tarifas = database.GetCollection<Tarifa>("tarifas");
        }
        public async Task<List<Tarifa>> GetTarifasByIdMedico(string idMedico)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("id_medico", idMedico));
            List<Tarifa> tarifa = new List<Tarifa>();
            tarifa = await _tarifas.Aggregate()
                            .AppendStage<Tarifa>(match)
                            .ToListAsync();
            return tarifa;
        }
    }
}
