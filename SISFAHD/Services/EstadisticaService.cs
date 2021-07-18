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
        private readonly IMongoCollection<ActoMedico> _acto;
        public EstadisticaService(ISisfahdDatabaseSettings settings)
        {
            var paciente = new MongoClient(settings.ConnectionString);
            var database = paciente.GetDatabase(settings.DatabaseName);
            _cita = database.GetCollection<Cita>("citas");
            _acto = database.GetCollection<ActoMedico>("acto_medico");
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
        public async Task<List<EspecialidadesMPedidas>> EspecialidadesMasPedidas(DateTime fecha)
        {


            DateTime fechaComparacion = new DateTime();
            var match = new BsonDocument("$match",
                        new BsonDocument("estado_pago", "pagado"));

            if (fecha != fechaComparacion)
            {
                DateTime fechasiguiente = fecha;
                fechasiguiente = fechasiguiente.AddDays(1);

                match = new BsonDocument("$match",
                            new BsonDocument
                            {
                                { "estado_pago", "pagado" },
                                { "fecha_cita",
                            new BsonDocument
                            {
                                { "$gte", new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0) },
                                { "$lt", new DateTime(fechasiguiente.Year, fechasiguiente.Month, fechasiguiente.Day, 0, 0, 0) }
                            } }
                            });
            }
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_turno",
                            new BsonDocument("$toObjectId", "$id_turno")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                            {
                                { "from", "turnos" },
                                { "localField", "id_turno" },
                                { "foreignField", "_id" },
                                { "as", "turno" }
                            });
            var unwind = new BsonDocument("$unwind",
                         new BsonDocument
                         {
                            { "path", "$turno" },
                            { "preserveNullAndEmptyArrays", true }
                         });
            var addfields1 = new BsonDocument("$addFields",
                            new BsonDocument("especialidad", "$turno.especialidad"));
            var group = new BsonDocument("$group",
                        new BsonDocument
                        {
                            { "_id", "$especialidad.codigo" },
                            { "cantidad", new BsonDocument("$sum", 1) }
                        });
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("id",
                                new BsonDocument("$toObjectId", "$_id")));
            var lookup1 = new BsonDocument("$lookup",
                            new BsonDocument
                            {
                                { "from", "especialidades" },
                                { "localField", "id" },
                                { "foreignField", "_id" },
                                { "as", "datos" }
                            });
            var unwind1 = new BsonDocument("$unwind",
                            new BsonDocument
                            {
                                { "path", "$datos" },
                                { "preserveNullAndEmptyArrays", false }
                            });
            var project = new BsonDocument("$project",
                            new BsonDocument
                            {
                                { "_id", 1 },
                                { "cantidad", 1 },
                                { "datos.nombre", 1 },
                                { "datos.url", 1 }
                            });

            List<EspecialidadesMPedidas> listaEsp = new List<EspecialidadesMPedidas>();
            listaEsp = await _cita.Aggregate()
                        .AppendStage<dynamic>(match)
                        .AppendStage<dynamic>(addfields)
                        .AppendStage<dynamic>(lookup)
                        .AppendStage<dynamic>(unwind)
                        .AppendStage<dynamic>(addfields1)
                        .AppendStage<dynamic>(group)
                        .AppendStage<dynamic>(addfields2)
                        .AppendStage<dynamic>(lookup1)
                        .AppendStage<dynamic>(unwind1)
                        .AppendStage<EspecialidadesMPedidas>(project).ToListAsync();

            listaEsp = listaEsp.OrderByDescending(x => x.cantidad).ToList();
            return listaEsp;
        }
        public async Task<List<MedicamentosMPedidos>> MedicamentosMasPedidos(DateTime fecha)
        {
            DateTime fechaComparacion = new DateTime();
            BsonDocument match = new BsonDocument();
            if (fecha != fechaComparacion)
            {
                DateTime fechasiguiente = fecha;
                fechasiguiente = fechasiguiente.AddDays(1);
                match = new BsonDocument("$match",
                                    new BsonDocument("fecha_atencion",
                                    new BsonDocument
                                    {
                                        { "$gte", new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0) },
                                        { "$lt", new DateTime(fechasiguiente.Year, fechasiguiente.Month, fechasiguiente.Day, 0, 0, 0) }
                                    }));
            }
            var unwind = new BsonDocument("$unwind",
                        new BsonDocument
                        {
                            { "path", "$diagnostico" },
                            { "preserveNullAndEmptyArrays", false }
                        });
            var unwind1 = new BsonDocument("$unwind",
                        new BsonDocument
                        {
                            { "path", "$diagnostico.prescripcion" },
                            { "preserveNullAndEmptyArrays", false }
                        });
            var group = new BsonDocument("$group",
                        new BsonDocument
                        {
                            { "_id", "$diagnostico.prescripcion.codigo" },
                            { "cantidad", new BsonDocument("$sum", 1) }
                        });
            var lookup = new BsonDocument("$lookup",
                        new BsonDocument
                        {
                            { "from", "medicamentos" },
                            { "localField", "_id" },
                            { "foreignField", "codigo" },
                            { "as", "datos" }
                        });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                            {
                                { "path", "$datos" },
                                { "preserveNullAndEmptyArrays", false }
                            });
            var project = new BsonDocument("$project",
                            new BsonDocument
                            {
                                { "_id", 1 },
                                { "cantidad", 1 },
                                { "datos.nombre", 1 },
                                { "datos.concentracion", 1 },
                                { "datos.formula_farmaceutica", 1 }
                            });
            List<MedicamentosMPedidos> listaMedi = new List<MedicamentosMPedidos>();
            if (fecha != fechaComparacion)
            {
                listaMedi = await _acto.Aggregate()
                        .AppendStage<dynamic>(match)
                        .AppendStage<dynamic>(unwind)
                        .AppendStage<dynamic>(unwind1)
                        .AppendStage<dynamic>(group)
                        .AppendStage<dynamic>(lookup)
                        .AppendStage<dynamic>(unwind2)
                        .AppendStage<MedicamentosMPedidos>(project).ToListAsync();
            }
            else
            {
                listaMedi = await _acto.Aggregate()
                        .AppendStage<dynamic>(unwind)
                        .AppendStage<dynamic>(unwind1)
                        .AppendStage<dynamic>(group)
                        .AppendStage<dynamic>(lookup)
                        .AppendStage<dynamic>(unwind2)
                        .AppendStage<MedicamentosMPedidos>(project).ToListAsync();
            }
            listaMedi = listaMedi.OrderByDescending(x => x.cantidad).ToList();
            return listaMedi;
        }
        public async Task<List<LaboratorioPedidos>> LaboratorioMasPedidos(DateTime fecha)
        {
            DateTime fechaComparacion = new DateTime();
            BsonDocument match = new BsonDocument();
            if (fecha != fechaComparacion)
            {
                DateTime fechasiguiente = fecha;
                fechasiguiente = fechasiguiente.AddDays(1);
                match = new BsonDocument("$match",
                                    new BsonDocument("fecha_atencion",
                                    new BsonDocument
                                    {
                                        { "$gte", new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0) },
                                        { "$lt", new DateTime(fechasiguiente.Year, fechasiguiente.Month, fechasiguiente.Day, 0, 0, 0) }
                                    }));
            }
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                            {
                                { "path", "$diagnostico" },
                                { "preserveNullAndEmptyArrays", false }
                            });
            var unwind1 = new BsonDocument("$unwind",
                            new BsonDocument
                            {
                                { "path", "$diagnostico.examenes_auxiliares" },
                                { "preserveNullAndEmptyArrays", false }
                            });
            var group = new BsonDocument("$group",
                        new BsonDocument
                        {
                            { "_id", "$diagnostico.examenes_auxiliares.nombre" },
                            { "cantidad", new BsonDocument("$sum", 1) }
                        });

            List<LaboratorioPedidos> listaLabo = new List<LaboratorioPedidos>();
            if (fecha != fechaComparacion)
            {
                listaLabo = await _acto.Aggregate()
                        .AppendStage<dynamic>(match)
                        .AppendStage<dynamic>(unwind)
                        .AppendStage<dynamic>(unwind1)
                        .AppendStage<LaboratorioPedidos>(group).ToListAsync();
            }
            else
            {
                listaLabo = await _acto.Aggregate()
                       .AppendStage<dynamic>(unwind)
                       .AppendStage<dynamic>(unwind1)
                       .AppendStage<LaboratorioPedidos>(group).ToListAsync();
            }
            listaLabo = listaLabo.OrderByDescending(x => x.cantidad).ToList();
            return listaLabo;
        }
    }
}
