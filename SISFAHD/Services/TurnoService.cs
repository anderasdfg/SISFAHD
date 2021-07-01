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
        public async Task<List<Turno>> GetByMedico(string idMedico, int month, int year)
        {
            List<Turno> turnos = new List<Turno>();
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

            turnos = await _turnos.Aggregate()
                .AppendStage<Turno>(match)
                .ToListAsync();

            return turnos;
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

        public async Task<List<TurnoDTO>> GetCuposByEspecialidadAndFecha(string idEspecialidad, DateTime fecha)
        {            
            int year = fecha.Year;
            int day = fecha.Day;
            int month = fecha.Month;


            var match = new BsonDocument("$match",
                        new BsonDocument("$and",
                        new BsonArray
                                {
                                    new BsonDocument("especialidad.codigo", idEspecialidad),
                                    new BsonDocument("cupos.hora_inicio",
                                    new BsonDocument("$lte",
                                    new DateTime(year, month, day, 23, 59, 59))),
                                    new BsonDocument("cupos.hora_inicio",
                                    new BsonDocument("$gte",
                                    new DateTime(year, month, day, 0, 0, 0)))
                                }));

            var addFields = new BsonDocument("$addFields",
                            new BsonDocument("id_medico_obj",
                            new BsonDocument("$toObjectId", "$id_medico")));

            var lookup = new BsonDocument("$lookup",
                         new BsonDocument
                            {
                                { "from", "medicos" },
                                { "localField", "id_medico_obj" },
                                { "foreignField", "_id" },
                                { "as", "datos_medico" }
                            });
            var unwind = new BsonDocument("$unwind",
                         new BsonDocument
                            {
                                { "path", "$datos_medico" },
                                { "preserveNullAndEmptyArrays", true }
                            });

            var addFields2 = new BsonDocument("$addFields",
                             new BsonDocument("id_usuario_obj",
                             new BsonDocument("$toObjectId", "$datos_medico.id_usuario")));
            var lookup2 = new BsonDocument("$lookup",
                          new BsonDocument
                            {
                                { "from", "usuarios" },
                                { "localField", "id_usuario_obj" },
                                { "foreignField", "_id" },
                                { "as", "datosUsuario" }
                            });

            var unwind2 = new BsonDocument("$unwind",
                          new BsonDocument
                            {
                                { "path", "$datosUsuario" },
                                { "preserveNullAndEmptyArrays", true }
                            });

            var addFieldsTarifa = new BsonDocument("$addFields",
                                  new BsonDocument("id_tarifa_obj",
                                  new BsonDocument("$toObjectId", "$id_tarifa")));

            var lookupTarifa = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "tarifas" },
                                        { "localField", "id_tarifa_obj" },
                                        { "foreignField", "_id" },
                                        { "as", "datosTarifa" }
                                    });

            var unwindTarifa = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datosTarifa" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            var project =   new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 1 },
                                    { "especialidad", 1 },
                                    { "estado", 1 },
                                    { "fecha_fin", 1 },
                                    { "fecha_inicio", 1 },
                                    { "hora_fin", 1 },
                                    { "id_medico", 1 },
                                    { "nombre_medico",
                            new BsonDocument("$concat",
                            new BsonArray
                                        {
                                            "$datosUsuario.datos.nombre",
                                            " ",
                                            "$datosUsuario.datos.apellido_paterno",
                                            " ",
                                            "$datosUsuario.datos.apellido_materno"
                                        }) },
                                    { "id_tarifa", 1 },
                                    { "cupos", 1 },
                                    { "precio", "$datosTarifa.precio_final" }
                                });

            List<TurnoDTO> turnos = new List<TurnoDTO>();

            turnos = await _turnos.Aggregate()
                           .AppendStage<dynamic>(match)
                           .AppendStage<dynamic>(addFields)
                           .AppendStage<dynamic>(lookup)
                           .AppendStage<dynamic>(unwind)
                           .AppendStage<dynamic>(addFields2)
                           .AppendStage<dynamic>(lookup2)
                           .AppendStage<dynamic>(unwind2)
                           .AppendStage<dynamic>(addFieldsTarifa)
                           .AppendStage<dynamic>(lookupTarifa)
                           .AppendStage<dynamic>(unwindTarifa)
                           .AppendStage<TurnoDTO>(project).ToListAsync();

            return turnos;

        }
        public Turno GetById(string id)
        {
            Turno turno = new Turno();
            turno = _turnos.Find(turno => turno.id == id).FirstOrDefault();
            return turno;
        }
    }
}
