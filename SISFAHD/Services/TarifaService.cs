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

        // Prueba de get all
        public async Task<List<Tarifa>> GetAllTarifas()
        {
            List<Tarifa> listTarifas;
               var lookupMedico = new BsonDocument("$lookup", new BsonDocument
               {
                   {"from","medicos"},
                   {"let", new BsonDocument("idmedico","$id_medico")},
                   { "pipeline", new BsonArray
                                   {
                                       new BsonDocument("$match",
                                       new BsonDocument("$expr",
                                       new BsonDocument("$eq",
                                       new BsonArray
                                       {
                                           "$_id",
                                           new BsonDocument("$toObjectId","$$idmedico")
                                       })))
                                   }
                   },
                   {"as","medico" }
               });
            var projectTarifa = new BsonDocument("$project", new BsonDocument
            {
                {"descripcion",1 },
                {"impuesto",1 },
                {"subtotal",1 },
                {"precio_final",1 },
                {"id_medico", 1 }
            });
            listTarifas = await _tarifas.Aggregate()
                               .AppendStage<dynamic>(lookupMedico)
                               .AppendStage<Tarifa>(projectTarifa).ToListAsync();

            return listTarifas;
        }
    }
}
