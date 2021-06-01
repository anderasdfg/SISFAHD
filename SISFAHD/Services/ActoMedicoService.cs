using MongoDB.Driver;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class ActoMedicoService
    {
        private readonly IMongoCollection<ActoMedico> _actoMedico;
        public ActoMedicoService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _actoMedico = database.GetCollection<ActoMedico>("acto_medico");
        }

        public async Task<List<ActoMedico>> GetAll()
        {
           return await _actoMedico.Find(x => true).ToListAsync();
        }
    }
}
