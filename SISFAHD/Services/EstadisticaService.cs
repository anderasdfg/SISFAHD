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
        private readonly IMongoCollection<Medico> _medicos;
        private readonly IMongoCollection<Paciente> _pacientes;
        private readonly IMongoCollection<Pedidos> _pedidos;
        public EstadisticaService(ISisfahdDatabaseSettings settings)
        {
            var paciente = new MongoClient(settings.ConnectionString);
            var database = paciente.GetDatabase(settings.DatabaseName);
            _cita = database.GetCollection<Cita>("citas");
            _acto = database.GetCollection<ActoMedico>("acto_medico");

            /////////-----------Colecciones Usadas en Estadistica---------/////////////
            _medicos = database.GetCollection<Medico>("medicos");
            _pacientes = database.GetCollection<Paciente>("pacientes");
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
                         new BsonDocument("_id", 1));
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
            //var addFields3 = new BsonDocument("$addFields",
            //                new BsonDocument("_id",
            //                new BsonDocument("$toString", "$_id")));
            //var project = new BsonDocument("$project",
            //              new BsonDocument("_id", 1));
            EstadisticaDTO eDTO = new EstadisticaDTO();
            eDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(addFields1)
                   .AppendStage<dynamic>(lookUp)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(addFields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(match)
                   .AppendStage<EstadisticaDTO>(group).FirstAsync();//<------
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
                    { "especialidad.nombre", especialidad },
                    { "estado_atencion", estado }
                });
            var group = new BsonDocument("$group",
            new BsonDocument
                {
                    { "_id", "$especialidad.nombre" },
                    { "cantidad",
            new BsonDocument("$sum", 1) }
                });
            var project = new BsonDocument("$project",
            new BsonDocument("_id", 1));
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
                    { "id_paciente", idpaciente },
                    { "estado_atencion", estado }
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
        public async Task<List<ExamenLaboratorio>> AllExamenesSolicitados()
        {
            List<ExamenLaboratorio> listaLabo = new List<ExamenLaboratorio>();

            var unwind = new BsonDocument("$unwind",
                        new BsonDocument
                            {
                                { "path", "$diagnostico" },
                                { "preserveNullAndEmptyArrays", false }
                            });
            var unwind2 = new BsonDocument("$unwind",
                        new BsonDocument
                            {
                                { "path", "$diagnostico.examenes_auxiliares" },
                                { "preserveNullAndEmptyArrays", false }
                            });
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument
                                {
                                    { "id_examen",
                            new BsonDocument("$toObjectId", "$diagnostico.examenes_auxiliares.codigo") },
                                    { "nombre", "$diagnostico.examenes_auxiliares.nombre" }
                                });
            var group = new BsonDocument("$group",
                    new BsonDocument
                        {
                            { "_id", "$id_examen" },
                            { "cantidad",
                    new BsonDocument("$sum", 1) }
                        });
            var lookup = new BsonDocument("$lookup",
                        new BsonDocument
                            {
                                { "from", "examenes" },
                                { "localField", "_id" },
                                { "foreignField", "_id" },
                                { "as", "datos" }
                            });
            var unwind3 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos" },
                                    { "preserveNullAndEmptyArrays", false }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("nombre", "$datos.descripcion"));
            var project = new BsonDocument("$project",
                        new BsonDocument("datos", 0));
            listaLabo = await _acto.Aggregate()
                .AppendStage<dynamic>(unwind)
                .AppendStage<dynamic>(unwind2)
                .AppendStage<dynamic>(addfields)
                .AppendStage<dynamic>(group)
                .AppendStage<dynamic>(lookup)
                .AppendStage<dynamic>(unwind3)
                .AppendStage<dynamic>(addfields2).
                AppendStage<ExamenLaboratorio>(project).ToListAsync();
            return listaLabo;
        }

        public async Task<List<CitasDeMedicoXIdUsuario_y_EstadoPago>> CitasDeMedicoXIdUsuario_y_EstadoPago(string idUser, string estadoPago)
        {

            var addfields = new BsonDocument("$addFields",
             new BsonDocument("id_med",
             new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
             new BsonDocument
                 {
                        { "from", "citas" },
                        { "localField", "id_med" },
                        { "foreignField", "id_medico" },
                        { "as", "cita" }
                 });
            var unwind = new BsonDocument("$unwind",
             new BsonDocument
                 {
                        { "path", "$cita" },
                        { "preserveNullAndEmptyArrays", false }
                 });
            var group = new BsonDocument("$group",
             new BsonDocument
                 {
                        { "_id",
                new BsonDocument
                        {
                            { "id_medico", "$_id" },
                            { "estado", "$cita.estado_atencion" },
                            { "estadoPago", "$cita.estado_pago" }
                        } },
                        { "cantidad",
                new BsonDocument("$sum", 1) }
                 });

            var lookup2 = new BsonDocument("$lookup",
             new BsonDocument
                 {
                        { "from", "medicos" },
                        { "localField", "_id.id_medico" },
                        { "foreignField", "_id" },
                        { "as", "datos_medico" }
                 });
            var unwind2 = new BsonDocument("$unwind",
             new BsonDocument
                 {
                        { "path", "$datos_medico" },
                        { "preserveNullAndEmptyArrays", true }
                 });
            var addfields2 = new BsonDocument("$addFields",
             new BsonDocument
                 {
                        { "estado_atencion", "$_id.estado" },
                        { "estado_pago", "$_id.estadoPago" }
                 });
            var addfields3 = new BsonDocument("$addFields",
                new BsonDocument("id_usuario",
                new BsonDocument("$toObjectId", "$datos_medico.id_usuario")));
            var lookup3 = new BsonDocument("$lookup",
                new BsonDocument
                    {
                        { "from", "usuarios" },
                        { "localField", "id_usuario" },
                        { "foreignField", "_id" },
                        { "as", "usuario" }
                    });

            var unwind3 = new BsonDocument("$unwind",
             new BsonDocument
                 {
                        { "path", "$usuario" },
                        { "preserveNullAndEmptyArrays", true }
                 });
            var project = new BsonDocument("$project",
             new BsonDocument
                 {
                        { "Nombre_medico",
                new BsonDocument("$concat",
                new BsonArray
                            {
                                "$usuario.datos.nombre",
                                " ",
                                "$usuario.datos.apellido_paterno"
                            }) },
                        { "id_usuario", 1 },
                        { "cantidad", 1 },
                        { "estado_atencion", 1 },
                        { "estado_pago", 1 },
                        { "_id", 0 }
                 });
            var addfields4 = new BsonDocument("$addFields",
                new BsonDocument("id_usuario",
                new BsonDocument("$toString", "$id_usuario")));
            var match = new BsonDocument("$match",
             new BsonDocument
                 {
                        { "id_usuario", idUser },
                        { "estado_pago", estadoPago }
                 });
            List<CitasDeMedicoXIdUsuario_y_EstadoPago> edto = new List<CitasDeMedicoXIdUsuario_y_EstadoPago>();
            edto = await _medicos.Aggregate()
                .AppendStage<dynamic>(addfields)
                .AppendStage<dynamic>(lookup)
                .AppendStage<dynamic>(unwind)
                .AppendStage<dynamic>(group)
                .AppendStage<dynamic>(lookup2)
                .AppendStage<dynamic>(unwind2)
                .AppendStage<dynamic>(addfields2)
                .AppendStage<dynamic>(addfields3).
                AppendStage<dynamic>(lookup3)
                .AppendStage<dynamic>(unwind3)
                .AppendStage<dynamic>(project)
                .AppendStage<dynamic>(addfields4)
                .AppendStage<CitasDeMedicoXIdUsuario_y_EstadoPago>(match).ToListAsync();
            return edto;
        }

        /////------------Citas x Estado Atencion--------------//////
        public async Task<List<CitasxEstadoAtencion>> EstadisticasAllCitasxEstadoAtencion()
        {
            var group = new BsonDocument("$group",
                        new BsonDocument
                            {
                                { "_id", "$estado_atencion" },
                                { "cantidad",
                        new BsonDocument("$sum", 1) }
                            });
            var addfields = new BsonDocument("$addFields",
                        new BsonDocument("estado",
                        new BsonDocument("$toString", "$_id")));
            var project = new BsonDocument("$project",
                        new BsonDocument("_id", 0));
            List<CitasxEstadoAtencion> eDTO = new List<CitasxEstadoAtencion>();
            eDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<CitasxEstadoAtencion>(project).ToListAsync();
            return eDTO;
        }
        public async Task<List<CitasxEstadoAtencion>> EstadisticasCitasxEstadoAtencion(string estado)
        {
            var group = new BsonDocument("$group",
                        new BsonDocument
                            {
                                { "_id", "$estado_atencion" },
                                { "cantidad",
                        new BsonDocument("$sum", 1) }
                            });
            var addfields = new BsonDocument("$addFields",
                        new BsonDocument("estado",
                        new BsonDocument("$toString", "$_id")));
            var project = new BsonDocument("$project",
                        new BsonDocument("_id", 0));
            var match = new BsonDocument("$match",
                        new BsonDocument("estado", estado));
            List<CitasxEstadoAtencion> eDTO = new List<CitasxEstadoAtencion>();
            eDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(project)
                   .AppendStage<CitasxEstadoAtencion>(match).ToListAsync();
            return eDTO;
        }
        /////------------Citas x Médicos--------------//////
        public async Task<List<CitasxMedicos>> EstadisticasAllCitasxMedico()
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_med",
                            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_med" },
                                    { "foreignField", "id_medico" },
                                    { "as", "citas" }
                                });
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuario",
                                new BsonDocument("$toObjectId", "$id_usuario")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuario" },
                                    { "foreignField", "_id" },
                                    { "as", "usuario" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$usuario" },
                                    { "preserveNullAndEmptyArrays", false }
                                });

            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "datos_basicos", 1 },
                                    { "turnos", 1 },
                                    { "suscripcion", 1 },
                                    { "citas", 1 },
                                    { "id_especialidad", 1 },
                                    { "usuario", 1 },
                                    { "cantidad",
                            new BsonDocument("$size", "$citas") }
                                });
            var sort = new BsonDocument("$sort",
                            new BsonDocument("cantidad", -1));
            List<CitasxMedicos> estadisticaDTO = new List<CitasxMedicos>();
            estadisticaDTO = await _medicos.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(project)
                   .AppendStage<CitasxMedicos>(sort).ToListAsync();

            return estadisticaDTO;
        }
        public async Task<List<CitaxMedicoNombre>> AllCistasxMedico_con_nombre()
        {
            var addfields = new BsonDocument("$addFields",
            new BsonDocument("id_med",
            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
            new BsonDocument
                {
            { "from", "citas" },
            { "localField", "id_med" },
            { "foreignField", "id_medico" },
            { "as", "cita" }
                });
            var unwind = new BsonDocument("$unwind",
            new BsonDocument
                {
            { "path", "$cita" },
            { "preserveNullAndEmptyArrays", false }
                });
            var group = new BsonDocument("$group",
            new BsonDocument
                {
            { "_id",
            new BsonDocument
            {
                { "id_medico", "$_id" }
            } },
            { "cantidad",
            new BsonDocument("$sum", 1) }
                });
            var lookup2 = new BsonDocument("$lookup",
            new BsonDocument
                {
            { "from", "medicos" },
            { "localField", "_id.id_medico" },
            { "foreignField", "_id" },
            { "as", "datos_medico" }
                });
            var unwind2 = new BsonDocument("$unwind",
            new BsonDocument
                {
            { "path", "$datos_medico" },
            { "preserveNullAndEmptyArrays", true }
                });
            var addfields2 = new BsonDocument("$addFields",
            new BsonDocument
                {
            { "estado_atencion", "$_id.estado" },
            { "estado_pago", "$_id.estadoPago" }
                });
            var addfields3 = new BsonDocument("$addFields",
            new BsonDocument("id_usuario",
            new BsonDocument("$toObjectId", "$datos_medico.id_usuario")));
            var lookup3=new BsonDocument("$lookup",
            new BsonDocument
                {
                    { "from", "usuarios" },
                    { "localField", "id_usuario" },
                    { "foreignField", "_id" },
                    { "as", "usuario" }
                });
            var unwind3 = new BsonDocument("$unwind",
            new BsonDocument
                {
            { "path", "$usuario" },
            { "preserveNullAndEmptyArrays", true }
                });
            var project = new BsonDocument("$project",
            new BsonDocument
                {
            { "Nombre_medico",
            new BsonDocument("$concat",
            new BsonArray
                {
                    "$usuario.datos.nombre",
                    " ",
                    "$usuario.datos.apellido_paterno"
                }) },
            { "id_usuario", 1 },
            { "cantidad", 1 },
            { "_id", 0 }
                });
            var addfields4 = new BsonDocument("$addFields",
            new BsonDocument("id_usuario",
            new BsonDocument("$toString", "$id_usuario")));
            List<CitaxMedicoNombre> medicos = new List<CitaxMedicoNombre>();
            medicos = await _medicos.Aggregate()
                .AppendStage<dynamic>(addfields)
                .AppendStage<dynamic>(lookup)
                .AppendStage<dynamic>(unwind)
                .AppendStage<dynamic>(group)
                .AppendStage<dynamic>(lookup2)
                .AppendStage<dynamic>(unwind2)
                .AppendStage<dynamic>(addfields2)                
                .AppendStage<dynamic>(addfields3)
                .AppendStage<dynamic>(lookup3)
                .AppendStage<dynamic>(unwind3)
                .AppendStage<dynamic>(project)
                .AppendStage<CitaxMedicoNombre>(addfields4).ToListAsync();
            return medicos;
        }

        /////------------Citas x Médicos y Estado Atencion--------------//////
        public async Task<List<CitasxMedicosyEstadoAtencion>> EstadisticasCitasxMedicoyEstadoByMedico(string medico)
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_med",
                            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_med" },
                                    { "foreignField", "id_medico" },
                                    { "as", "cita" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$cita" },
                                    { "preserveNullAndEmptyArrays", false }
                                });
            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id",
                            new BsonDocument
                                    {
                                        { "id_medico", "$_id" },
                                        { "estado", "$cita.estado_atencion" }
                                    } },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "medicos" },
                                    { "localField", "_id.id_medico" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_medico" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("estado_atencion", "$_id.estado"));
            var project = new BsonDocument("$project",
                            new BsonDocument("_id", 0));
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "datos_medico._id",
                            new ObjectId(medico) }
                                });
            List<CitasxMedicosyEstadoAtencion> estadisticaDTO = new List<CitasxMedicosyEstadoAtencion>();
            estadisticaDTO = await _medicos.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(project)
                   .AppendStage<CitasxMedicosyEstadoAtencion>(match).ToListAsync();

            return estadisticaDTO;
        }
        public async Task<List<CitasxMedicosyEstadoAtencion>> EstadisticasCitasxMedicoyEstadoByEstado(string estado_atencion)
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_med",
                            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_med" },
                                    { "foreignField", "id_medico" },
                                    { "as", "cita" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$cita" },
                                    { "preserveNullAndEmptyArrays", false }
                                });
            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id",
                            new BsonDocument
                                    {
                                        { "id_medico", "$_id" },
                                        { "estado", "$cita.estado_atencion" }
                                    } },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "medicos" },
                                    { "localField", "_id.id_medico" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_medico" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("estado_atencion", "$_id.estado"));
            var project = new BsonDocument("$project",
                            new BsonDocument("_id", 0));
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                   
                                    { "estado_atencion", estado_atencion }
                                });
            List<CitasxMedicosyEstadoAtencion> estadisticaDTO = new List<CitasxMedicosyEstadoAtencion>();
            estadisticaDTO = await _medicos.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(project)
                   .AppendStage<CitasxMedicosyEstadoAtencion>(match).ToListAsync();

            return estadisticaDTO;
        }
        public async Task<List<CitasxMedicosyEstadoAtencion>> EstadisticasCitasxMedicoyEstado(string medico, string estado_atencion)
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_med",
                            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_med" },
                                    { "foreignField", "id_medico" },
                                    { "as", "cita" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$cita" },
                                    { "preserveNullAndEmptyArrays", false }
                                });
            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id",
                            new BsonDocument
                                    {
                                        { "id_medico", "$_id" },
                                        { "estado", "$cita.estado_atencion" }
                                    } },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "medicos" },
                                    { "localField", "_id.id_medico" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_medico" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("estado_atencion", "$_id.estado"));
            var project = new BsonDocument("$project",
                            new BsonDocument("_id", 0));
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "datos_medico._id",
                            new ObjectId(medico) },
                                    { "estado_atencion", estado_atencion }
                                });
            List<CitasxMedicosyEstadoAtencion> estadisticaDTO = new List<CitasxMedicosyEstadoAtencion>();
            estadisticaDTO = await _medicos.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(project)
                   .AppendStage<CitasxMedicosyEstadoAtencion>(match).ToListAsync();

            return estadisticaDTO;
        }

        /////------------Citas x Especialidad--------------//////
        public async Task<List<CitasxEspecialidad>> EstadisticasAllCitasxEspecialidad()
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_med",
                            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
                        new BsonDocument
                            {
                                { "from", "citas" },
                                { "localField", "id_med" },
                                { "foreignField", "id_medico" },
                                { "as", "cita" }
                            });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$cita" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var group = new BsonDocument("$group",
                        new BsonDocument
                            {
                                { "_id", "$id_especialidad" },
                                { "cantidad",
                        new BsonDocument("$sum", 1) }
                            });
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("_id",
                                new BsonDocument("$toObjectId", "$_id")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "especialidades" },
                                    { "localField", "_id" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_especialidad" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_especialidad" },
                                    { "preserveNullAndEmptyArrays", false }
                                });
            var addfields3 = new BsonDocument("$addFields",
                             new BsonDocument("nombre", "$datos_especialidad.nombre"));
            List<CitasxEspecialidad> estadisticaDTO = new List<CitasxEspecialidad>();
            estadisticaDTO = await _medicos.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<CitasxEspecialidad>(addfields3).ToListAsync();

            return estadisticaDTO;

        }
        public async Task<List<CitasxEspecialidad>> EstadisticasCitasxEspecialidad(string especialidad)
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_med",
                            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_med" },
                                    { "foreignField", "id_medico" },
                                    { "as", "cita" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$cita" },
                                    { "preserveNullAndEmptyArrays", false }
                                });
            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id", "$id_especialidad" },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("_id",
                            new BsonDocument("$toObjectId", "$_id")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "especialidades" },
                                    { "localField", "_id" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_especialidad" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_especialidad" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "datos_especialidad.nombre", especialidad }
                                });
            List<CitasxEspecialidad> estadisticaDTO = new List<CitasxEspecialidad>();
            estadisticaDTO = await _medicos.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<CitasxEspecialidad>(match).ToListAsync();

            return estadisticaDTO;

        }
        /////------------Citas x Especialidad y Estado Atencion--------------//////
        public async Task<List<CitasxEspecialidadyEstadoAtencion>> EstadisticasCitasxEspecialidadyEstadoAtencionByEspecialidad(string especialidad)
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_med",
                            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_med" },
                                    { "foreignField", "id_medico" },
                                    { "as", "citas" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$citas" },
                                    { "preserveNullAndEmptyArrays", false }
                                });
            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id",
                            new BsonDocument
                                    {
                                        { "estado", "$citas.estado_atencion" },
                                        { "especialidad", "$id_especialidad" }
                                    } },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("id_esp",
                            new BsonDocument("$toObjectId", "$_id.especialidad")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "especialidades" },
                                    { "localField", "id_esp" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_especialidad" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_especialidad" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields3 = new BsonDocument("$addFields",
                            new BsonDocument("estado_atencion", "$_id.estado"));
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 0 },
                                    { "datos_especialidad", 1 },
                                    { "estado_atencion", 1 },
                                    { "cantidad", 1 }
                                });
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "datos_especialidad.nombre", especialidad }
                                    
                                });
            List<CitasxEspecialidadyEstadoAtencion> estadisticaDTO = new List<CitasxEspecialidadyEstadoAtencion>();
            estadisticaDTO = await _medicos.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(addfields3)
                   .AppendStage<dynamic>(project)
                   .AppendStage<CitasxEspecialidadyEstadoAtencion>(match).ToListAsync();

            return estadisticaDTO;

        }
        public async Task<List<CitasxEspecialidadyEstadoAtencion>> EstadisticasCitasxEspecialidadyEstadoAtencionByEstado( string estado_atencion)
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_med",
                            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_med" },
                                    { "foreignField", "id_medico" },
                                    { "as", "citas" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$citas" },
                                    { "preserveNullAndEmptyArrays", false }
                                });
            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id",
                            new BsonDocument
                                    {
                                        { "estado", "$citas.estado_atencion" },
                                        { "especialidad", "$id_especialidad" }
                                    } },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("id_esp",
                            new BsonDocument("$toObjectId", "$_id.especialidad")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "especialidades" },
                                    { "localField", "id_esp" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_especialidad" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_especialidad" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields3 = new BsonDocument("$addFields",
                            new BsonDocument("estado_atencion", "$_id.estado"));
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 0 },
                                    { "datos_especialidad", 1 },
                                    { "estado_atencion", 1 },
                                    { "cantidad", 1 }
                                });
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    
                                    { "estado_atencion", estado_atencion }
                                });
            List<CitasxEspecialidadyEstadoAtencion> estadisticaDTO = new List<CitasxEspecialidadyEstadoAtencion>();
            estadisticaDTO = await _medicos.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(addfields3)
                   .AppendStage<dynamic>(project)
                   .AppendStage<CitasxEspecialidadyEstadoAtencion>(match).ToListAsync();

            return estadisticaDTO;

        }
        public async Task<List<CitasxEspecialidadyEstadoAtencion>> EstadisticasCitasxEspecialidadyEstadoAtencion(string especialidad, string estado_atencion)
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_med",
                            new BsonDocument("$toString", "$_id")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_med" },
                                    { "foreignField", "id_medico" },
                                    { "as", "citas" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$citas" },
                                    { "preserveNullAndEmptyArrays", false }
                                });
            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id",
                            new BsonDocument
                                    {
                                        { "estado", "$citas.estado_atencion" },
                                        { "especialidad", "$id_especialidad" }
                                    } },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("id_esp",
                            new BsonDocument("$toObjectId", "$_id.especialidad")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "especialidades" },
                                    { "localField", "id_esp" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_especialidad" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_especialidad" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields3 = new BsonDocument("$addFields",
                            new BsonDocument("estado_atencion", "$_id.estado"));
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 0 },
                                    { "datos_especialidad", 1 },
                                    { "estado_atencion", 1 },
                                    { "cantidad", 1 }
                                });
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "datos_especialidad.nombre", especialidad },
                                    { "estado_atencion", estado_atencion }
                                });
            List<CitasxEspecialidadyEstadoAtencion> estadisticaDTO = new List<CitasxEspecialidadyEstadoAtencion>();
            estadisticaDTO = await _medicos.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(addfields3)
                   .AppendStage<dynamic>(project)
                   .AppendStage<CitasxEspecialidadyEstadoAtencion>(match).ToListAsync();

            return estadisticaDTO;

        }

        /////------------Citas x Paciente--------------//////
        public async Task<List<CitasxPaciente>> EstadisticasAllCitasxPaciente()
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_usuario",
                            new BsonDocument("$toObjectId", "$id_usuario")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuario" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("id_paciente",
                            new BsonDocument("$toString", "$_id")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_paciente" },
                                    { "foreignField", "id_paciente" },
                                    { "as", "datos_citas" }
                                });
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "datos", 1 },
                                    { "datos_paciente", 1 },
                                    { "datos_citas", 1 },
                                    { "cantidad",
                            new BsonDocument("$size", "$datos_citas") }
                                });
            List<CitasxPaciente> estadisticaDTO = new List<CitasxPaciente>();
            estadisticaDTO = await _pacientes.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<CitasxPaciente>(project).ToListAsync();

            return estadisticaDTO;
        }
        public async Task<List<CitasxPaciente>> EstadisticasCitasxPaciente(string id_paciente)
        {
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_usuario",
                            new BsonDocument("$toObjectId", "$id_usuario")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuario" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("id_paciente",
                            new BsonDocument("$toString", "$_id")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_paciente" },
                                    { "foreignField", "id_paciente" },
                                    { "as", "datos_citas" }
                                });
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "datos", 1 },
                                    { "datos_paciente", 1 },
                                    { "datos_citas", 1 },
                                    { "cantidad",
                            new BsonDocument("$size", "$datos_citas") }
                                });
            var match = new BsonDocument("$match",
                            new BsonDocument("_id",
                            new ObjectId(id_paciente)));
            List<CitasxPaciente> estadisticaDTO = new List<CitasxPaciente>();
            estadisticaDTO = await _pacientes.Aggregate()
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<CitasxPaciente>(project).ToListAsync();

            return estadisticaDTO;
        }

        /////------------Citas x Paciente y Estado Atencion--------------//////
        public async Task<List<CitasxPacienteyEstadoAtencion>> EstadisticasCitasxPacienteyEstadoAtencion(string id_paciente)
        {
                var project = new BsonDocument("$project",
                                new BsonDocument
                                    {
                                        { "estado_atencion", 1 },
                                        { "estado_pago", 1 },
                                        { "fecha_cita", 1 },
                                        { "id_paciente", 1 },
                                        { "id_medico", 1 },
                                        { "cantidad",
                                        new BsonDocument("$sum", 1) }
                                    });
                var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                            { "_id",
                            new BsonDocument
                            {
                                { "id_paciente", "$id_paciente" },
                                { "estado", "$estado_atencion" }
                            } },
                            { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
                var addfields = new BsonDocument("$addFields",
                                new BsonDocument("estado_cita",
                                new BsonDocument("$toString", "$_id.estado")));

                var addfields2 =new BsonDocument("$addFields",
                                new BsonDocument("_id.id_paciente",
                                new BsonDocument("$toObjectId", "$_id.id_paciente")));

                var lookup =new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "pacientes" },
                                    { "localField", "_id.id_paciente" },
                                    { "foreignField", "_id" },
                                    { "as", "paciente" }
                                });
                var unwind =new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
                var addfields3 = new BsonDocument("$addFields",
                            new BsonDocument("paciente.id_usuario",
                            new BsonDocument("$toObjectId", "$paciente.id_usuario")));

                var lookup2=new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "paciente.id_usuario" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_usuario" }
                                });

                var unwind2 = new BsonDocument("$unwind",
        new BsonDocument
            {
                { "path", "$datos_usuario" },
                { "preserveNullAndEmptyArrays", true }
            });

                var addfields4 = new BsonDocument("$addFields",
        new BsonDocument("id_usuario",
        new BsonDocument("$toString", "$_id.id_paciente")));

                var project2 = new BsonDocument("$project",
                new BsonDocument
                    {
                { "_id", 0 },
                { "paciente", 0 }
                    });
                var match = new BsonDocument("$match",
                new BsonDocument("id_usuario", id_paciente));

                List<CitasxPacienteyEstadoAtencion> estadisticaDTO = new List<CitasxPacienteyEstadoAtencion>();
                estadisticaDTO = await _cita.Aggregate()
                       .AppendStage<dynamic>(project)
                       .AppendStage<dynamic>(group)
                       .AppendStage<dynamic>(addfields)
                       .AppendStage<dynamic>(addfields2)
                       .AppendStage<dynamic>(lookup)
                       .AppendStage<dynamic>(unwind)
                       .AppendStage<dynamic>(addfields3)
                       .AppendStage<dynamic>(lookup2)
                       .AppendStage<dynamic>(unwind2)
                       .AppendStage<dynamic>(addfields4)
                       .AppendStage<dynamic>(project2)
                       .AppendStage<CitasxPacienteyEstadoAtencion>(match).ToListAsync();

            return estadisticaDTO;
        }
        public async Task<List<MedicosFecha>> MedicosHoy()
        {
            var addfields1 = new BsonDocument("$addFields",
                new BsonDocument("id_med",
                new BsonDocument("$toString", "$_id")));
            var lookup1 = new BsonDocument("$lookup",
                new BsonDocument
                    {
                        { "from", "citas" },
                        { "localField", "id_med" },
                        { "foreignField", "id_medico" },
                        { "as", "cita" }
                    });
            var unwind1 = new BsonDocument("$unwind",
                new BsonDocument
                    {
                        { "path", "$cita" },
                        { "preserveNullAndEmptyArrays", false }
                    });
            var addfields2 = new BsonDocument("$addFields",
                new BsonDocument
                    {
                        { "fecha_cita_dia",
                new BsonDocument("$dayOfMonth", "$cita.fecha_cita") },
                        { "fecha_cita_mes",
                new BsonDocument("$month", "$cita.fecha_cita") },
                        { "fecha_cita_anio",
                new BsonDocument("$year", "$cita.fecha_cita") }
                    });
            var group = new BsonDocument("$group",
                new BsonDocument
                    {
                        { "_id",
                new BsonDocument
                        {
                            { "id_Uusario_medico", "$id_usuario" },
                            { "dia", "$fecha_cita_dia" },
                            { "mes", "$fecha_cita_mes" },
                            { "anio", "$fecha_cita_anio" },
                            { "estado_atencion", "$estado_atencion" },
                            { "estado_pago", "$estado_pago" }
                        } },
                        { "cantidad",
                new BsonDocument("$sum", 1) }
                    });
            var addfields3 = new BsonDocument("$addFields",
                new BsonDocument
                    {
                        { "fecha_cita_string",
                new BsonDocument("$concat",
                new BsonArray
                            {
                                new BsonDocument("$toString", "$_id.anio"),
                                "/",
                                new BsonDocument("$toString", "$_id.mes"),
                                "/",
                                new BsonDocument("$toString", "$_id.dia"),
                                "T05:00:00.000+00:00"
                            }) },
                        { "fecha_cita_d_m_y",
                new BsonDocument("$concat",
                new BsonArray
                            {
                                new BsonDocument("$toString", "$_id.dia"),
                                "/",
                                new BsonDocument("$toString", "$_id.mes"),
                                "/",
                                new BsonDocument("$toString", "$_id.anio")
                            }) },
                        { "id_Uusario_medico",
                new BsonDocument("$toObjectId", "$_id.id_Uusario_medico") }
                    });
            var lookup3 = new BsonDocument("$lookup",
                new BsonDocument
                    {
                        { "from", "usuarios" },
                        { "localField", "id_Uusario_medico" },
                        { "foreignField", "_id" },
                        { "as", "usuario" }
                    });
            var unwind2 = new BsonDocument("$unwind",
                new BsonDocument
                    {
                        { "path", "$usuario" },
                        { "preserveNullAndEmptyArrays", true }
                    });
            var addfields4 = new BsonDocument("$addFields",
                new BsonDocument("fecha_cita",
                new BsonDocument("$toDate", "$fecha_cita_string")));
            var project = new BsonDocument("$project",
                new BsonDocument
                    {
                        { "cantidad", 1 },
                        { "fecha_cita", 1 },
                        { "fecha_cita_d_m_y", 1 },
                        { "_id", 0 },
                        { "Nombre_medico",
                new BsonDocument("$concat",
                new BsonArray
                            {
                                "$usuario.datos.nombre",
                                " ",
                                "$usuario.datos.apellido_paterno"
                            }) }
                    });
            var match = new BsonDocument("$match",
                new BsonDocument("fecha_cita",
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0)));
            List<MedicosFecha> estadisticaDTO;
            estadisticaDTO = await _medicos.Aggregate()
                .AppendStage<dynamic>(addfields1)
                .AppendStage<dynamic>(lookup1)
                .AppendStage<dynamic>(unwind1)
                .AppendStage<dynamic>(addfields2)
                .AppendStage<dynamic>(group)
                .AppendStage<dynamic>(addfields3)
                .AppendStage<dynamic>(lookup3)
                .AppendStage<dynamic>(unwind2)
                .AppendStage<dynamic>(addfields4)
                .AppendStage<dynamic>(project)
                .AppendStage<MedicosFecha>(match).ToListAsync();
            return estadisticaDTO;
            //DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0
        }
        public async Task<List<CitasxPacienteyEstadoAtencion>> EstadisticasCitasxPacienteyEstadoAtencionByEstado(string estado_atencion)
        {
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "estado_atencion", 1 },
                                    { "estado_pago", 1 },
                                    { "fecha_cita", 1 },
                                    { "id_paciente", 1 },
                                    { "id_medico", 1 },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id",
                            new BsonDocument
                                    {
                                        { "id_paciente", "$id_paciente" },
                                        { "estado", "$estado_atencion" }
                                    } },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("estado_cita",
                            new BsonDocument("$toString", "$_id.estado")));
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("_id.id_paciente",
                            new BsonDocument("$toObjectId", "$_id.id_paciente")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "pacientes" },
                                    { "localField", "_id.id_paciente" },
                                    { "foreignField", "_id" },
                                    { "as", "paciente" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields3 = new BsonDocument("$addFields",
                            new BsonDocument("paciente.id_usuario",
                            new BsonDocument("$toObjectId", "$paciente.id_usuario")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "paciente.id_usuario" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_usuario" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_usuario" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields4 = new BsonDocument("$addFields",
                            new BsonDocument("id_usuario",
                            new BsonDocument("$toString", "$_id.id_paciente")));
            var project2 = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 0 },
                                    { "paciente", 0 }
                                });
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "estado_cita", estado_atencion }
                                    
                                });
            List<CitasxPacienteyEstadoAtencion> estadisticaDTO = new List<CitasxPacienteyEstadoAtencion>();
            estadisticaDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(project)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(addfields3)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(addfields4)
                   .AppendStage<dynamic>(project2)
                   .AppendStage<CitasxPacienteyEstadoAtencion>(match).ToListAsync();

            return estadisticaDTO;
        }
        public async Task<List<ExamenesFecha>> ExamenesHoy()
        {
            var unwind = new BsonDocument("$unwind",
    new BsonDocument
        {
            { "path", "$diagnostico" },
            { "preserveNullAndEmptyArrays", false }
        });
    var unwind2 = new BsonDocument("$unwind",
    new BsonDocument
        {
            { "path", "$diagnostico.examenes_auxiliares" },
            { "preserveNullAndEmptyArrays", false }
        });
    var addfields = new BsonDocument("$addFields",
    new BsonDocument
        {
            { "id_examen",
    new BsonDocument("$toObjectId", "$diagnostico.examenes_auxiliares.codigo") },
            { "nombre", "$diagnostico.examenes_auxiliares.nombre" },
            { "fecha_atencion_dia",
    new BsonDocument("$dayOfMonth", "$fecha_atencion") },
            { "fecha_atencion_mes",
    new BsonDocument("$month", "$fecha_atencion") },
            { "fecha_atencion_anio",
    new BsonDocument("$year", "$fecha_atencion") }
        });
    var group = new BsonDocument("$group",
    new BsonDocument
        {
            { "_id",
    new BsonDocument
            {
                { "id", "$id_examen" },
                { "dia", "$fecha_atencion_dia" },
                { "mes", "$fecha_atencion_mes" },
                { "anio", "$fecha_atencion_anio" }
            } },
            { "cantidad",
    new BsonDocument("$sum", 1) }
        });
    var lookup = new BsonDocument("$lookup",
    new BsonDocument
        {
            { "from", "examenes" },
            { "localField", "_id.id" },
            { "foreignField", "_id" },
            { "as", "datos" }
        });
    var unwind3 = new BsonDocument("$unwind",
    new BsonDocument
        {
            { "path", "$datos" },
            { "preserveNullAndEmptyArrays", false }
        });
    var addfields2 = new BsonDocument("$addFields",
    new BsonDocument
        {
            { "nombre", "$datos.descripcion" },
            { "fecha_atencion_string",
    new BsonDocument("$concat",
    new BsonArray
                {
                    new BsonDocument("$toString", "$_id.anio"),
                    "/",
                    new BsonDocument("$toString", "$_id.mes"),
                    "/",
                    new BsonDocument("$toString", "$_id.dia"),
                    "T05:00:00.000+00:00"
                }) }
        });
    var addfields3 = new BsonDocument("$addFields",
    new BsonDocument("fecha_atencion",
    new BsonDocument("$toDate", "$fecha_atencion_string")));
    var project = new BsonDocument("$project",
    new BsonDocument
        {
            { "_id", 0 },
            { "datos", 0 }
        });
    var match = new BsonDocument("$match",
    new BsonDocument("fecha_atencion",
    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0)));
            List<ExamenesFecha> estadisticaDTO;
            estadisticaDTO = await _acto.Aggregate()
                .AppendStage<dynamic>(unwind)
                .AppendStage<dynamic>(unwind2)
                .AppendStage<dynamic>(addfields)
                .AppendStage<dynamic>(group)
                .AppendStage<dynamic>(lookup)
                .AppendStage<dynamic>(unwind3)
                .AppendStage<dynamic>(addfields2)
                .AppendStage<dynamic>(addfields3)
                .AppendStage<dynamic>(project)
                .AppendStage<ExamenesFecha>(match).ToListAsync();
            return estadisticaDTO;
        }
        public async Task<List<ExamenesPedidos>> ExamenesNOPagados()
        {
            var unwind = new BsonDocument("$unwind",
 new BsonDocument
     {
            { "path", "$productos" },
            { "preserveNullAndEmptyArrays", false }
     });
            var match = new BsonDocument("$match",
             new BsonDocument("tipo", "Examenes"));
            var group = new BsonDocument("$group",
            new BsonDocument
                {
            { "_id",
    new BsonDocument
            {
                { "codigo_producto", "$productos.codigo" },
                { "nombre_producto", "$productos.nombre" },
                { "estado_pago", "$estado_pago" }
            } },
            { "cantidad",
    new BsonDocument("$sum", 1) }
                });
            var projetc = new BsonDocument("$project",
            new BsonDocument
                {
            { "codigo_producto", "$_id.codigo_producto" },
            { "nombre_producto", "$_id.nombre_producto" },
            { "estado_pago", "$_id.estado_pago" },
            { "cantidad", 1 },
            { "_id", 0 }
                });
            var match2 = new BsonDocument("$match",
    new BsonDocument("estado_pago", "No pagado"));
            List<ExamenesPedidos> estadisticaDTO;
            estadisticaDTO = await _pedidos.Aggregate()
                .AppendStage<dynamic>(unwind)
                .AppendStage<dynamic>(match)
                .AppendStage<dynamic>(group)
                .AppendStage<dynamic>(projetc)
                .AppendStage<ExamenesPedidos>(match2).ToListAsync();
            return estadisticaDTO;
        }
        public async Task<List<ExamenesPedidos>> ExamenesPagados()
        {
            var unwind = new BsonDocument("$unwind",
 new BsonDocument
     {
            { "path", "$productos" },
            { "preserveNullAndEmptyArrays", false }
     });
            var match = new BsonDocument("$match",
             new BsonDocument("tipo", "Examenes"));
            var group = new BsonDocument("$group",
            new BsonDocument
                {
            { "_id",
    new BsonDocument
            {
                { "codigo_producto", "$productos.codigo" },
                { "nombre_producto", "$productos.nombre" },
                { "estado_pago", "$estado_pago" }
            } },
            { "cantidad",
    new BsonDocument("$sum", 1) }
                });
            var projetc = new BsonDocument("$project",
            new BsonDocument
                {
            { "codigo_producto", "$_id.codigo_producto" },
            { "nombre_producto", "$_id.nombre_producto" },
            { "estado_pago", "$_id.estado_pago" },
            { "cantidad", 1 },
            { "_id", 0 }
                });
            var match2 = new BsonDocument("$match",
    new BsonDocument("estado_pago", "Pagado"));
            List<ExamenesPedidos> estadisticaDTO;
            estadisticaDTO = await _pedidos.Aggregate()
                .AppendStage<dynamic>(unwind)
                .AppendStage<dynamic>(match)
                .AppendStage<dynamic>(group)
                .AppendStage<dynamic>(projetc)
                .AppendStage<ExamenesPedidos>(match2).ToListAsync();
            return estadisticaDTO;
        }
        public async Task<List<CitasxEspecialidadFecha>> CitasxEspecialidadHoy()
        {
            var addfields = new BsonDocument("$addFields",
                             new BsonDocument
                                 {
                                    { "fecha_cita_dia",
                            new BsonDocument("$dayOfMonth", "$fecha_cita") },
                                    { "fecha_cita_mes",
                            new BsonDocument("$month", "$fecha_cita") },
                                    { "fecha_cita_anio",
                            new BsonDocument("$year", "$fecha_cita") }
                                 });
            var addfields2 = new BsonDocument("$addFields",
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
            var addfields3 = new BsonDocument("$addFields",
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
            var group = new BsonDocument("$group",
                        new BsonDocument
                            {
                        { "_id",
                        new BsonDocument
                                {
                                    { "dia", "$fecha_cita_dia" },
                                    { "mes", "$fecha_cita_mes" },
                                    { "anio", "$fecha_cita_anio" },
                                    { "especialidad", "$especialidad.nombre" }
                                } },
                                { "cantidad",
                        new BsonDocument("$sum", 1) }
                                    });
            var addfields4 = new BsonDocument("$addFields",
                                new BsonDocument
                                    {
                                { "fecha_cita_string",
                                new BsonDocument("$concat",
                                new BsonArray
                                            {
                                                new BsonDocument("$toString", "$_id.anio"),
                                                "/",
                                                new BsonDocument("$toString", "$_id.mes"),
                                                "/",
                                                new BsonDocument("$toString", "$_id.dia"),
                                                "T05:00:00.000+00:00"
                                            }) },
                                        { "fecha_cita_d_m_y",
                                new BsonDocument("$concat",
                                new BsonArray
                                            {
                                                new BsonDocument("$toString", "$_id.dia"),
                                                "/",
                                                new BsonDocument("$toString", "$_id.mes"),
                                                "/",
                                                new BsonDocument("$toString", "$_id.anio")
                                            }) },
                                        { "estado_atencion", "$_id.estado_atencion" },
                                        { "estado_pago", "$_id.estado_pago" }
                                            });
            var addfields5 = new BsonDocument("$addFields",
                                new BsonDocument
                                    {
                                { "fecha_cita",
                                new BsonDocument("$toDate", "$fecha_cita_string") },
                                        { "especialidad", "$_id.especialidad" }
                                            });
            var project = new BsonDocument("$project",
                            new BsonDocument("_id", 0));
            var match = new BsonDocument("$match",
                        new BsonDocument("fecha_cita", new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0)));
            List<CitasxEspecialidadFecha> estadisticaDTO = new List<CitasxEspecialidadFecha>();
            estadisticaDTO = await _cita.Aggregate()
                .AppendStage<dynamic>(addfields)
                .AppendStage<dynamic>(addfields2)
                .AppendStage<dynamic>(lookup)
                .AppendStage<dynamic>(unwind)
                .AppendStage<dynamic>(addfields3)
                .AppendStage<dynamic>(lookup2)
                .AppendStage<dynamic>(unwind2)
                .AppendStage<dynamic>(group)
                .AppendStage<dynamic>(addfields4)
                .AppendStage<dynamic>(addfields5)
                .AppendStage<dynamic>(project)
                .AppendStage<CitasxEspecialidadFecha>(match).ToListAsync();
            return estadisticaDTO;
        }
        public async Task<List<CitasxPacienteyEstadoAtencion>> EstadisticasCitasxPacienteyEstadoAtencion(string id_paciente, string estado_atencion)
        {
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "estado_atencion", 1 },
                                    { "estado_pago", 1 },
                                    { "fecha_cita", 1 },
                                    { "id_paciente", 1 },
                                    { "id_medico", 1 },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id",
                            new BsonDocument
                                    {
                                        { "id_paciente", "$id_paciente" },
                                        { "estado", "$estado_atencion" }
                                    } },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("estado_cita",
                            new BsonDocument("$toString", "$_id.estado")));
            var addfields2 = new BsonDocument("$addFields",
                            new BsonDocument("_id.id_paciente",
                            new BsonDocument("$toObjectId", "$_id.id_paciente")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "pacientes" },
                                    { "localField", "_id.id_paciente" },
                                    { "foreignField", "_id" },
                                    { "as", "paciente" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields3 = new BsonDocument("$addFields",
                            new BsonDocument("paciente.id_usuario",
                            new BsonDocument("$toObjectId", "$paciente.id_usuario")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "paciente.id_usuario" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_usuario" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_usuario" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields4 = new BsonDocument("$addFields",
                            new BsonDocument("id_usuario",
                            new BsonDocument("$toString", "$_id.id_paciente")));
            var project2 = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 0 },
                                    { "paciente", 0 }
                                });
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "estado_cita", estado_atencion },
                                    { "id_usuario", id_paciente }
                                });
            List<CitasxPacienteyEstadoAtencion> estadisticaDTO = new List<CitasxPacienteyEstadoAtencion>();
            estadisticaDTO = await _cita.Aggregate()
                   .AppendStage<dynamic>(project)
                   .AppendStage<dynamic>(group)
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(addfields2)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<dynamic>(addfields3)
                   .AppendStage<dynamic>(lookup2)
                   .AppendStage<dynamic>(unwind2)
                   .AppendStage<dynamic>(addfields4)
                   .AppendStage<dynamic>(project2)
                   .AppendStage<CitasxPacienteyEstadoAtencion>(match).ToListAsync();

            return estadisticaDTO;
        }
    }
}
