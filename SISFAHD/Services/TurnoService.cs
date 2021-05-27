using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class TurnoService
    {
        private readonly IMongoCollection<Turno> _turnos;
        public TurnoService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _turnos = database.GetCollection<Turno>("turnos");
        }
        public List<Turno> GetAll()
        {
            List<Turno> turnos = new List<Turno>();
            turnos = _turnos.Find(Turnos => true).ToList();
            return turnos;
        }
        public async Task<List<Turno>> GetByMedico(string idMedico)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("id_medico", idMedico));
            List<Turno> turno = new List<Turno>();
            turno = await _turnos.Aggregate()
                            .AppendStage<Turno>(match)
                            .ToListAsync();
            return turno;
        }
        public Turno CreateTurno(Turno turno)
        {
            _turnos.InsertOne(turno);
            return turno;
        }
        public Turno ModifyTurno(Turno turno)
        {
            var filter = Builders<Turno>.Filter.Eq("id", turno.id);
            var update = Builders<Turno>.Update
                .Set("especialidad", turno.especialidad)
                .Set("estado", turno.estado)
                .Set("fecha_fin", turno.fecha_fin)
                .Set("fecha_inicio", turno.fecha_inicio)
                .Set("hora_fin", turno.hora_fin)
                .Set("hora_inicio", turno.hora_inicio)
                .Set("id_medico", turno.id_medico)
                .Set("id_tarifa", turno.id_tarifa)
                .Set("cupos", turno.cupos);

            _turnos.UpdateOne(filter, update);
            return turno;
        }
        public async Task DeleteTurno(string id)
        {
            FilterDefinition<Turno> filtro = Builders<Turno>.Filter.Eq("id", id);
            await _turnos.DeleteOneAsync(filtro);
        }
    }
}
