using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SISFAHD.Services
{
    public class Turno_OrdenService
    {
        private readonly IMongoCollection<Turno_Ordenes> _turnosOr;
        public Turno_OrdenService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _turnosOr = database.GetCollection<Turno_Ordenes>("turnos_ordenes");
        }
        public List<Turno_Ordenes> GetAll()
        {
            List<Turno_Ordenes> turnos = new List<Turno_Ordenes>();
            turnos = _turnosOr.Find(Turnos => true).ToList();
            return turnos;
        }
        public Turno_Ordenes CreateTurno(Turno_Ordenes turno)
        {
            _turnosOr.InsertOne(turno);
            return turno;
        }
        public async Task<List<Turno_Ordenes>> GetByMedico(string idMedico, int month, int year)
        {
            List<Turno_Ordenes> turnos = new List<Turno_Ordenes>();
            DateTime firstDate = new DateTime(year, month, 1, 0, 0, 0);
            DateTime lastDate = firstDate.AddMonths(2).AddDays(-1);
            lastDate.AddHours(23);
            lastDate.AddMinutes(59);
            lastDate.AddSeconds(59);

            var match = new BsonDocument("$match",
                                new BsonDocument("$and",
                                new BsonArray
                                        {
                                            new BsonDocument("fecha_inicio",
                                            new BsonDocument("$gte",firstDate)),
                                            new BsonDocument("fecha_fin",
                                            new BsonDocument("$lte",lastDate)),
                                            new BsonDocument("id_medico", idMedico)
                                        }));

            turnos = await _turnosOr.Aggregate()
                .AppendStage<Turno_Ordenes>(match)
                .ToListAsync();

            return turnos;
        }
        public Turno_Ordenes ModifyTurno(Turno_Ordenes turno)
        {
            var filter = Builders<Turno_Ordenes>.Filter.Eq("id", turno.id);
            var update = Builders<Turno_Ordenes>.Update
                .Set("especialidad", turno.especialidad)
                .Set("estado", turno.estado)
                .Set("fecha_fin", turno.fecha_fin)
                .Set("fecha_inicio", turno.fecha_inicio)
                .Set("hora_fin", turno.hora_fin)
                .Set("hora_inicio", turno.hora_inicio)
                .Set("id_medico", turno.id_medico)
                .Set("cupos", turno.cupos);

            _turnosOr.UpdateOne(filter, update);
            return turno;
        }
        public async Task<Turno_Ordenes> DeleteTurno(string id)
        {
            var filter = Builders<Turno_Ordenes>.Filter.Eq("id", id);
            return await _turnosOr.FindOneAndDeleteAsync<Turno_Ordenes>
                        (filter, new FindOneAndDeleteOptions<Turno_Ordenes>{});
        }
        public Turno_Ordenes GetById(string id)
        {
            Turno_Ordenes turno = new Turno_Ordenes();
            turno = _turnosOr.Find(turno => turno.id == id).FirstOrDefault();
            return turno;
        }
        public async Task<List<Turno_Ordenes>> GetBy_Especialidad_Fecha(Turno_OrdenDTO_By_Especialidad_Fecha consultas)
        {
            List<Turno_Ordenes> turnos = new List<Turno_Ordenes>();
            DateTime firstDate = new DateTime(consultas.año, consultas.mes, consultas.dia, 0, 0, 0);
            DateTime lastDate = firstDate;
            lastDate.AddHours(23);
            lastDate.AddMinutes(59);
            lastDate.AddSeconds(59);

            var match = new BsonDocument("$match",
                                new BsonDocument("$and",
                                new BsonArray
                                        {
                                            new BsonDocument("fecha_inicio",
                                            new BsonDocument("$gte",firstDate)),
                                            new BsonDocument("fecha_fin",
                                            new BsonDocument("$lte",lastDate)),
                                            new BsonDocument("especialidad.codigo", consultas.idespecialidad)
                                        }));

            turnos = await _turnosOr.Aggregate()
                .AppendStage<Turno_Ordenes>(match)
                .ToListAsync();

            return turnos;
        }
    }
}
