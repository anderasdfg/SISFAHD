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
    public class EstadisticaService
    {
        private readonly IMongoCollection<Cita> _cita;
        public EstadisticaService(ISisfahdDatabaseSettings settings)
        {
            var paciente = new MongoClient(settings.ConnectionString);
            var database = paciente.GetDatabase(settings.DatabaseName);
            _cita = database.GetCollection<Cita>("citas");
        }
        public async Task<EstadisticaDTO> CitasxEstadoAtencion(string estado)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("estado_atencion", estado));
            var group = new BsonDocument("$group",
                      new BsonDocument
                        {
                            { "_id", "$estado_atencion" },
                            { "cantidad",
                            new BsonDocument("$sum", 1) }
                        });
            var project = new BsonDocument("$project",
                         new BsonDocument("_id", 0));
            EstadisticaDTO eDTO = new EstadisticaDTO();
            eDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(match)
                   .AppendStage<dynamic>(group)
                   .AppendStage<EstadisticaDTO>(project).FirstAsync();
            return eDTO;
        }
        public async Task<EstadisticaDTO> CitasxEspecialidad(string especialidad)
        {
            var addFields1 = new BsonDocument("$addFields",
                            new BsonDocument("id_medico",
                            new BsonDocument("$toObjectId", "$id_medico")));

            var lookUp = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "medicos" },
                                    { "localField", "id_medico" },
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
                                new BsonDocument("datos_medico",
                                new BsonDocument("id_especialidad",
                                new BsonDocument("$toObjectId", "$datos_medico.id_especialidad"))));

            var lookup2 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "especialidades" },
                                        { "localField", "datos_medico.id_especialidad" },
                                        { "foreignField", "_id" },
                                        { "as", "especialidad" }
                                    });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$especialidad" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var match = new BsonDocument("$match",
                        new BsonDocument("especialidad.nombre", especialidad));
            var group = new BsonDocument("$group",
    new BsonDocument
        {
                                { "_id", "$especialidad.nombre" },
                                { "cantidad",
                        new BsonDocument("$sum", 1) }
        });
            var project = new BsonDocument("$project",
                          new BsonDocument("_id", 0));
            EstadisticaDTO eDTO = new EstadisticaDTO();
            eDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(addFields1)
                   .AppendStage<dynamic>(lookUp)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(addFields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(match)
                   .AppendStage<EstadisticaDTO>(project).FirstAsync();
            return eDTO;
        }
        public async Task<EstadisticaDTO> CitasxEspecialidad_yEstado(string especialidad, string estado)
        {
            var addfields = new BsonDocument("$addFields",
            new BsonDocument("id_medico",
            new BsonDocument("$toObjectId", "$id_medico")));
            var lookup = new BsonDocument("$lookup",
            new BsonDocument
                {
                    { "from", "medicos" },
                    { "localField", "id_medico" },
                    { "foreignField", "_id" },
                    { "as", "datos_medico" }
                });
            var unwind = new BsonDocument("$unwind",
            new BsonDocument
                {
                    { "path", "$datos_medico" },
                    { "preserveNullAndEmptyArrays", true }
                });
            var addfields2 = new BsonDocument("$addFields",
            new BsonDocument("datos_medico",
            new BsonDocument("id_especialidad",
            new BsonDocument("$toObjectId", "$datos_medico.id_especialidad"))));
            var lookup2 = new BsonDocument("$lookup",
            new BsonDocument
                {
                    { "from", "especialidades" },
                    { "localField", "datos_medico.id_especialidad" },
                    { "foreignField", "_id" },
                    { "as", "especialidad" }
                });
            var unwind2 = new BsonDocument("$unwind",
            new BsonDocument
                {
                    { "path", "$especialidad" },
                    { "preserveNullAndEmptyArrays", true }
                });
            var match = new BsonDocument("$match",
            new BsonDocument
                {
                    { "especialidad.nombre", "Cardiología" },
                    { "estado_atencion", "no atendido" }
                });
            var group = new BsonDocument("$group",
            new BsonDocument
                {
                    { "_id", "$especialidad.nombre" },
                    { "cantidad",
            new BsonDocument("$sum", 1) }
                });
            var project = new BsonDocument("$project",
            new BsonDocument("_id", 0));
            EstadisticaDTO eDTO = new EstadisticaDTO();
            eDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(match)
                   .AppendStage<dynamic>(group)
                   .AppendStage<EstadisticaDTO>(project).FirstAsync();
            return eDTO;

        }
        public async Task<EstadisticaDTO> CitasxMedico(string idMedico)
        {
            EstadisticaDTO eDTO = new EstadisticaDTO();
            var match = new BsonDocument("$match",
                   new BsonDocument("id_medico", idMedico));

            var group = new BsonDocument("$group",
                        new BsonDocument
                            {
                                { "_id", "$id_medico" },
                                { "cantidad",
                        new BsonDocument("$sum", 1) }
                            });
            var project = new BsonDocument("$project",
                          new BsonDocument("_id", 0));

            eDTO = await _cita.Aggregate()
                  .AppendStage<dynamic>(match)
                  .AppendStage<dynamic>(group)
                  .AppendStage<EstadisticaDTO>(project).FirstAsync();
            return eDTO;
        }
        public async Task<EstadisticaDTO> CitasxMedico_y_Estado(string idMedico, string estado)
        {
            EstadisticaDTO eDTO = new EstadisticaDTO();
            var addfields = new BsonDocument("$addFields",
                                new BsonDocument("id_medico_pro",
                                new BsonDocument("$toObjectId", "$id_medico")));
            var lookup = new BsonDocument("$lookup",
                        new BsonDocument
                            {
                                    { "from", "medicos" },
                                    { "localField", "id_medico_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_medico" }
                            });
            var unwind = new BsonDocument("$unwind",
                        new BsonDocument
                            {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                            });
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("datos_medico",
                                new BsonDocument("id_especialidad",
                                new BsonDocument("$toObjectId", "$datos_medico.id_especialidad"))));

            var lookup2 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                            { "from", "especialidades" },
                                            { "localField", "datos_medico.id_especialidad" },
                                            { "foreignField", "_id" },
                                            { "as", "especialidad" }
                                    });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                        { "path", "$especialidad" },
                                        { "preserveNullAndEmptyArrays", true }
                                });
            var match2 = new BsonDocument("$match",
                            new BsonDocument
                                {
                                        { "id_medico", idMedico },
                                        { "estado_atencion", estado }
                                });
            var group2 = new BsonDocument("$group",
                            new BsonDocument
                                {
                                        { "_id", "$id_medico" },
                                        { "cantidad",
                                new BsonDocument("$sum", 1) }
                                });
            var project2 = new BsonDocument("$project",
                            new BsonDocument("_id", 0));
            eDTO = await _cita.Aggregate()
                  .AppendStage<dynamic>(addfields)
                  .AppendStage<dynamic>(lookup)
                  .AppendStage<dynamic>(unwind)
                  .AppendStage<dynamic>(addfields2)
                  .AppendStage<dynamic>(lookup2)
                  .AppendStage<dynamic>(unwind2)
                  .AppendStage<dynamic>(match2)
                  .AppendStage<dynamic>(group2)
                  .AppendStage<EstadisticaDTO>(project2).FirstAsync();
            return eDTO;
        }
        public async Task<EstadisticaDTO> CantidadCitasxPaciente(string idpaciente)
        {
            var match = new BsonDocument("$match",
            new BsonDocument("id_paciente", idpaciente));
            var group = new BsonDocument("$group",
            new BsonDocument
                {
                    { "_id", "$id_paciente" },
                    { "cantidad",
            new BsonDocument("$sum", 1) }
                });
            var project = new BsonDocument("$project",
            new BsonDocument("_id", 0));
            EstadisticaDTO eDTO = new EstadisticaDTO();
            eDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(match)
                   .AppendStage<dynamic>(group)
                   .AppendStage<EstadisticaDTO>(project).FirstAsync();
            return eDTO;
        }
        public async Task<EstadisticaDTO> CantidadCitasxPaciente_y_Estado(string idpaciente, string estado)
        {
            var addfields = new BsonDocument("$addFields",
            new BsonDocument("id_medico_pro",
            new BsonDocument("$toObjectId", "$id_medico")));
            var lookup = new BsonDocument("$lookup",
            new BsonDocument
                {
                    { "from", "medicos" },
                    { "localField", "id_medico_pro" },
                    { "foreignField", "_id" },
                    { "as", "datos_medico" }
                });
            var unwind = new BsonDocument("$unwind",
            new BsonDocument
                {
                    { "path", "$datos_medico" },
                    { "preserveNullAndEmptyArrays", true }
                });
            var addfields2 = new BsonDocument("$addFields",
            new BsonDocument("datos_medico",
            new BsonDocument("id_especialidad",
            new BsonDocument("$toObjectId", "$datos_medico.id_especialidad"))));
            var lookup2 = new BsonDocument("$lookup",
            new BsonDocument
                {
                    { "from", "especialidades" },
                    { "localField", "datos_medico.id_especialidad" },
                    { "foreignField", "_id" },
                    { "as", "especialidad" }
                });
            var unwind2 = new BsonDocument("$unwind",
            new BsonDocument
                {
                    { "path", "$especialidad" },
                    { "preserveNullAndEmptyArrays", true }
                });
            var match = new BsonDocument("$match",
            new BsonDocument
                {
                    { "id_paciente", "608f70f2a47f0a6734f6db18" },
                    { "estado_atencion", "no atendido" }
                });
            var group = new BsonDocument("$group",
            new BsonDocument
                {
                    { "_id", "$id_paciente" },
                    { "cantidad",
            new BsonDocument("$sum", 1) }
                });
            var project = new BsonDocument("$project",
            new BsonDocument("_id", 0));
            EstadisticaDTO eDTO = new EstadisticaDTO();
            eDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(match)
                   .AppendStage<dynamic>(group)
                   .AppendStage<EstadisticaDTO>(project).FirstAsync();
            return eDTO;
        }

      

        /*
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
         */
    }
}
