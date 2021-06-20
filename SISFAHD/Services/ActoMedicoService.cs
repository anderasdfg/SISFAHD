using MongoDB.Bson;
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

        public async Task<ActoMedico> GetById(string id)
        {
            return await _actoMedico.Find(x => x.id == id).FirstOrDefaultAsync();
        }

        public async Task<ActoMedico> CrearActoMedico(ActoMedico actomedico)
        {
            actomedico.fecha_creacion = DateTime.Now;
            actomedico.fecha_atencion = DateTime.Now;
            await _actoMedico.InsertOneAsync(actomedico);
            return actomedico;
        }

        public async Task<ActoMedico> ModificarActoMedico(ActoMedico actomedico)
        {
            var filter = Builders<ActoMedico>.Filter.Eq("id", ObjectId.Parse(actomedico.id));
            var update = Builders<ActoMedico>.Update
                .Set("medicacion", actomedico.medicacion)
                .Set("diagnostico", actomedico.diagnostico)
                .Set("signos_vitales", actomedico.signos_vitales)
                .Set("anamnesis", actomedico.anamnesis)
                .Set("indicaciones", actomedico.indicaciones);
            await _actoMedico.UpdateOneAsync(filter, update);
            return actomedico;
        }
    }
}
