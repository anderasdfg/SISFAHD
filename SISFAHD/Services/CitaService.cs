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
    public class CitaService
    {
        private readonly IMongoCollection<Cita> _cita;
        private readonly IMongoCollection<Turno> _turnos;        
        private readonly TurnoService _turnoservice;
        private readonly VentaService _ventaservice;



        public CitaService(ISisfahdDatabaseSettings settings, TurnoService turnoService, VentaService ventaService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _cita = database.GetCollection<Cita>("citas");
            _turnos = database.GetCollection<Turno>("turnos");
            _turnoservice = turnoService;
            _ventaservice = ventaService;
        }

        public async Task<Cita> PutSoloidActoMedico(Cita citaobj)
        {
            var filter = Builders<Cita>.Filter.Eq("id", ObjectId.Parse(citaobj.id));
            var update = Builders<Cita>.Update
                .Set("id_acto_medico", citaobj.id_acto_medico);

            await _cita.UpdateOneAsync(filter, update);
            return citaobj;
        }
        public async Task<bool> PutSoloCitaAtendida(string idCita)
        {
            var filter = Builders<Cita>.Filter.Eq("id", ObjectId.Parse(idCita));
            var update = Builders<Cita>.Update
                .Set("estado_atencion", "atendido");

            await _cita.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<List<CitaDTO>> GetAllCitaPagadasNoPagadas()
        {
            List<CitaDTO> PagoDTO = new List<CitaDTO>();

            var addfields1 = new BsonDocument("$addFields",
                                new BsonDocument("id_paciente_pro",
                                new BsonDocument("$toObjectId", "$id_paciente")));
            var lookup1 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "pacientes" },
                                    { "localField", "id_paciente_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_usuario" }
                                });
            var unwind1 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_usuario" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuariopro",
                                new BsonDocument("$toObjectId", "$datos_usuario.id_usuario")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuariopro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields3 = new BsonDocument("$addFields",
                                new BsonDocument("id_rol",
                                new BsonDocument("$toObjectId", "$datos_paciente.rol")));
            var lookup3 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "roles" },
                                    { "localField", "id_rol" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente.nombre_rol" }
                                });
            var unwind3 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente.nombre_rol" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields4 = new BsonDocument("$addFields",
                                new BsonDocument("datos_paciente",
                                new BsonDocument("datos",
                                new BsonDocument("nombre_apellido_paciente",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_paciente.datos.nombre",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_materno"
                                                    })))));
            var addfields5 = new BsonDocument("$addFields",
                                new BsonDocument("id_turno_pro",
                                new BsonDocument("$toObjectId", "$id_turno")));
            var lookup4 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "turnos" },
                                        { "localField", "id_turno_pro" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_turno" }
                                    });
            var unwind4 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields6 = new BsonDocument("$addFields",
                                new BsonDocument("id_medico_pro",
                                new BsonDocument("$toObjectId", "$datos_turno.id_medico")));
            var lookup5 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "medicos" },
                                        { "localField", "id_medico_pro" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_medico" }
                                    });
            var unwind5 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields7 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuario_medico",
                                new BsonDocument("$toObjectId", "$datos_medico.id_usuario")));
            var lookup6 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "usuarios" },
                                        { "localField", "id_usuario_medico" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_turno.datos_medico" }
                                    });
            var unwind6 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno.datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields8 = new BsonDocument("$addFields",
                                new BsonDocument("datos_turno",
                                new BsonDocument("datos_medico",
                                new BsonDocument("nombre_apellido_medico",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_turno.datos_medico.datos.nombre",
                                                        " ",
                                                        "$datos_turno.datos_medico.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_turno.datos_medico.datos.apellido_materno"
                                                    })))));
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 1 },
                                    { "estado_atencion", 1 },
                                    { "estado_pago", 1 },
                                    { "fecha_cita", 1 },
                                    { "fecha_pago", 1 },
                                    { "id_paciente", 1 },
                                    { "precio_neto", 1 },
                                    { "tipo_pago", 1 },
                                    { "datos_paciente",
                            new BsonDocument
                                    {
                                        { "datos",
                            new BsonDocument
                                        {
                                            { "nombre_apellido_paciente", 1 },
                                            { "correo", 1 }
                                        } },
                                        { "usuario", 1 },
                                        { "clave", 1 },
                                        { "nombre_rol",
                            new BsonDocument("nombre", 1) }
                                    } },
                                    { "datos_turno",
                            new BsonDocument
                                    {
                                        { "especialidad", 1 },
                                        { "hora_inicio", 1 },
                                        { "datos_medico",
                            new BsonDocument("nombre_apellido_medico", 1) }
                                    } }
                                });
            PagoDTO = await _cita.Aggregate()
                                .AppendStage<dynamic>(addfields1)
                                .AppendStage<dynamic>(lookup1)
                                .AppendStage<dynamic>(unwind1)
                                .AppendStage<dynamic>(addfields2)
                                .AppendStage<dynamic>(lookup2)
                                .AppendStage<dynamic>(unwind2)
                                .AppendStage<dynamic>(addfields3)
                                .AppendStage<dynamic>(lookup3)
                                .AppendStage<dynamic>(unwind3)
                                .AppendStage<dynamic>(addfields4)
                                .AppendStage<dynamic>(addfields5)
                                .AppendStage<dynamic>(lookup4)
                                .AppendStage<dynamic>(unwind4)
                                .AppendStage<dynamic>(addfields6)
                                .AppendStage<dynamic>(lookup5)
                                .AppendStage<dynamic>(unwind5)
                                .AppendStage<dynamic>(addfields7)
                                .AppendStage<dynamic>(lookup6)
                                .AppendStage<dynamic>(unwind6)
                                .AppendStage<dynamic>(addfields8)
                                .AppendStage<CitaDTO>(project)
                                .ToListAsync();
            return PagoDTO;
        }
        public async Task<CitaDTO> GetByIdCitasPagadasNoPagadas(string id)
        {
            var addfields1 = new BsonDocument("$addFields",
                                new BsonDocument("id_paciente_pro",
                                new BsonDocument("$toObjectId", "$id_paciente")));
            var lookup1 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "pacientes" },
                                    { "localField", "id_paciente_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_usuario" }
                                });
            var unwind1 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_usuario" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuariopro",
                                new BsonDocument("$toObjectId", "$datos_usuario.id_usuario")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuariopro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields3 = new BsonDocument("$addFields",
                                new BsonDocument("id_rol",
                                new BsonDocument("$toObjectId", "$datos_paciente.rol")));
            var lookup3 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "roles" },
                                    { "localField", "id_rol" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente.nombre_rol" }
                                });
            var unwind3 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente.nombre_rol" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields4 = new BsonDocument("$addFields",
                                new BsonDocument("datos_paciente",
                                new BsonDocument("datos",
                                new BsonDocument("nombre_apellido_paciente",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_paciente.datos.nombre",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_materno"
                                                    })))));
            var addfields5 = new BsonDocument("$addFields",
                                new BsonDocument("id_turno_pro",
                                new BsonDocument("$toObjectId", "$id_turno")));
            var lookup4 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "turnos" },
                                        { "localField", "id_turno_pro" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_turno" }
                                    });
            var unwind4 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields6 = new BsonDocument("$addFields",
                                new BsonDocument("id_medico_pro",
                                new BsonDocument("$toObjectId", "$datos_turno.id_medico")));
            var lookup5 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "medicos" },
                                        { "localField", "id_medico_pro" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_medico" }
                                    });
            var unwind5 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields7 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuario_medico",
                                new BsonDocument("$toObjectId", "$datos_medico.id_usuario")));
            var lookup6 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "usuarios" },
                                        { "localField", "id_usuario_medico" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_turno.datos_medico" }
                                    });
            var unwind6 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno.datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields8 = new BsonDocument("$addFields",
                                new BsonDocument("datos_turno",
                                new BsonDocument("datos_medico",
                                new BsonDocument("nombre_apellido_medico",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_turno.datos_medico.datos.nombre",
                                                        " ",
                                                        "$datos_turno.datos_medico.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_turno.datos_medico.datos.apellido_materno"
                                                    })))));
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 1 },
                                    { "estado_atencion", 1 },
                                    { "estado_pago", 1 },
                                    { "fecha_cita", 1 },
                                    { "fecha_pago", 1 },
                                    { "id_paciente", 1 },
                                    { "precio_neto", 1 },
                                    { "tipo_pago", 1 },
                                    { "id_medico", 1 },
                                    { "datos_paciente",
                            new BsonDocument
                                    {
                                        { "datos",
                            new BsonDocument
                                        {
                                            { "nombre_apellido_paciente", 1 },
                                            { "correo", 1 }
                                        } },
                                        { "usuario", 1 },
                                        { "clave", 1 },
                                        { "nombre_rol",
                            new BsonDocument("nombre", 1) }
                                    } },
                                    { "datos_turno",
                            new BsonDocument
                                    {
                                        { "especialidad", 1 },
                                        { "hora_inicio", 1 },
                                        { "datos_medico",
                            new BsonDocument("nombre_apellido_medico", 1)},
                                    } }
                                });
            var match = new BsonDocument("$match",
                        new BsonDocument("_id",
                        new ObjectId(id)));

            CitaDTO pago = new CitaDTO();
            pago = await _cita.Aggregate()
                                .AppendStage<dynamic>(addfields1)
                                .AppendStage<dynamic>(lookup1)
                                .AppendStage<dynamic>(unwind1)
                                .AppendStage<dynamic>(addfields2)
                                .AppendStage<dynamic>(lookup2)
                                .AppendStage<dynamic>(unwind2)
                                .AppendStage<dynamic>(addfields3)
                                .AppendStage<dynamic>(lookup3)
                                .AppendStage<dynamic>(unwind3)
                                .AppendStage<dynamic>(addfields4)
                                .AppendStage<dynamic>(addfields5)
                                .AppendStage<dynamic>(lookup4)
                                .AppendStage<dynamic>(unwind4)
                                .AppendStage<dynamic>(addfields6)
                                .AppendStage<dynamic>(lookup5)
                                .AppendStage<dynamic>(unwind5)
                                .AppendStage<dynamic>(addfields7)
                                .AppendStage<dynamic>(lookup6)
                                .AppendStage<dynamic>(unwind6)
                                .AppendStage<dynamic>(addfields8)
                                .AppendStage<dynamic>(project)
                                .AppendStage<CitaDTO>(match)
                                .FirstAsync();
            return pago;
        }
        public Cita GetById(string id)
        {
            Cita cita = new Cita();
            cita = _cita.Find(cita => cita.id == id).FirstOrDefault();
            return cita;
        }
        public async Task<Cita> ModifyEstadoPagoCita(Cita pagorealizado)
        {
            var filter = Builders<Cita>.Filter.Eq("id", pagorealizado.id);
            var update = Builders<Cita>.Update
                .Set("estado_pago", pagorealizado.estado_pago);
            _cita.UpdateOne(filter, update);
            return pagorealizado;
        } 
        public async Task<List<CitaDTO2>> GetCitasbyMedicoFecha(string medico, int month, int year)
        {
            List<CitaDTO2> citas = new List<CitaDTO2>();
            DateTime firstDate = new DateTime(year, month, 1,0,0,0);
            DateTime lastDate = firstDate.AddMonths(1).AddDays(-1);
            lastDate = lastDate.AddHours(23);
            lastDate = lastDate.AddMinutes(59);
            lastDate = lastDate.AddSeconds(59);

            var match = new BsonDocument("$match",
                                new BsonDocument("$and",
                                new BsonArray
                                        {
                                            new BsonDocument("fecha_cita",
                                            new BsonDocument("$gte",firstDate)),
                                            new BsonDocument("fecha_cita_fin",
                                            new BsonDocument("$lte",lastDate)),
                                            new BsonDocument("id_medico", medico)
                                        }));
            var lookUpTurno = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "turnos" },
                                        { "let",
                                new BsonDocument("turnoID", "$id_turno") },
                                        { "pipeline",
                                new BsonArray
                                        {
                                            new BsonDocument("$match",
                                            new BsonDocument("$expr",
                                            new BsonDocument("$eq",
                                            new BsonArray
                                                        {
                                                            new BsonDocument("$toObjectId", "$$turnoID"),
                                                            "$_id"
                                                        })))
                                        } },
                                        { "as", "turno" }
                                    });

            var lookUpPaciente = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "pacientes" },
                                                { "let",
                                        new BsonDocument("pacienteID", "$id_paciente") },
                                                { "pipeline",
                                        new BsonArray
                                                {
                                                    new BsonDocument("$match",
                                                    new BsonDocument("$expr",
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                                {
                                                                    new BsonDocument("$toObjectId", "$$pacienteID"),
                                                                    "$_id"
                                                                })))
                                                } },
                                                { "as", "paciente" }
                                            });

            var lookUpMedico = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "medicos" },
                                                { "let",
                                        new BsonDocument("medicoID", "$id_medico") },
                                                { "pipeline",
                                        new BsonArray
                                                {
                                                    new BsonDocument("$match",
                                                    new BsonDocument("$expr",
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                                {
                                                                    new BsonDocument("$toObjectId", "$$medicoID"),
                                                                    "$_id"
                                                                }))),
                                                    new BsonDocument("$lookup",
                                                    new BsonDocument
                                                        {
                                                            { "from", "especialidades" },
                                                            { "let",
                                                    new BsonDocument("especialidadID", "$id_especialidad") },
                                                            { "pipeline",
                                                    new BsonArray
                                                            {
                                                                new BsonDocument("$match",
                                                                new BsonDocument("$expr",
                                                                new BsonDocument("$eq",
                                                                new BsonArray
                                                                            {
                                                                                new BsonDocument("$toObjectId", "$$especialidadID"),
                                                                                "$_id"
                                                                            })))
                                                            } },
                                                            { "as", "especialidad" }
                                                        }),
                                                    new BsonDocument("$project",
                                                    new BsonDocument
                                                        {
                                                            { "_id", "$_id" },
                                                            { "turnos", 1 },
                                                            { "subscripcion", 1 },
                                                            { "datos_basicos", 1 },
                                                            { "especialidad",
                                                    new BsonDocument("$arrayElemAt",
                                                    new BsonArray
                                                                {
                                                                    "$especialidad",
                                                                    0
                                                                }) },
                                                            { "id_usuario", 1 }
                                                        })
                                                } },
                                                { "as", "medico" }
                                            });

            var project = new BsonDocument("$project",
                                new BsonDocument
                                    {
                                        { "_id", "$_id" },
                                        { "estado_atencion", "$estado_atencion" },
                                        { "estado_pago", "$estado_pago" },
                                        { "fecha_cita", "$fecha_cita" },
                                        { "fecha_reserva", "$fecha_reserva" },
                                        { "datos_paciente",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$paciente",
                                                0
                                            }) },
                                        { "enlace_cita", "$enlace_cita" },
                                        { "precio_neto", "$precio_neto" },
                                        { "calificacion", "$calificacion" },
                                        { "id_acto_medico", "$id_acto_medico" },
                                        { "observaciones", "$observaciones" },
                                        { "tipo_pago", "$tipo_pago" },
                                        { "id_turno", "$id_turno" },
                                        { "turno",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$turno",
                                                0
                                            }) },
                                        { "medico",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$medico",
                                                0
                                            }) },
                                        { "fecha_cita_fin", "$fecha_cita_fin" }
                                    });

            citas = await _cita.Aggregate()
                .AppendStage<dynamic>(match)
                .AppendStage<dynamic>(lookUpTurno)
                .AppendStage<dynamic>(lookUpPaciente)
                .AppendStage<dynamic>(lookUpMedico)
                .AppendStage<CitaDTO2>(project)
                .ToListAsync();

            return citas;

            //calcula la fecha actual
            //obtener el mes: 29 abril -> abril
            //2 fechas -primer dia del mes 01 abril 00:00, -ultimo dia del mes 31 abril 24:00
        }

        public Cita CreateCita(Cita cita)
        {                        
            //Inserta la cita
            _cita.InsertOne(cita);

            //Ocupa el turno disponible
            Turno turno = _turnoservice.GetById(cita.id_turno);

            for (int i = 0; i < turno.cupos.Count; i++)
            {
                if (turno.cupos[i].hora_inicio == cita.fecha_cita)
                {
                    turno.cupos[i].estado = "ocupado";
                    turno.cupos[i].paciente = cita.id_paciente;
                    turno.cupos[i].id_cita = cita.id;
                }
            }

            var filter = Builders<Turno>.Filter.Eq("id", turno.id);
            var update = Builders<Turno>.Update.Set("cupos", turno.cupos);

            turno = _turnos.FindOneAndUpdate<Turno>(filter, update, new FindOneAndUpdateOptions<Turno>
            {
                ReturnDocument = ReturnDocument.After
            });

            //Crea la venta en pendiente para esa cita
            Venta venta = new Venta();
            venta.codigo_orden = "";
            venta.codigo_referencia = cita.id;
            venta.monto = cita.precio_neto;
            venta.tipo_pago = "Niubiz";
            venta.estado = "";
            venta.detalle_estado = "";
            venta.tipo_operacion = "Reserva de cita";
            venta.titular = "";
            venta.moneda = "";            

            _ventaservice.CrearVenta(venta);



            return cita;
        }
        //Para traer el acto medico correspondiente para el visualizar HCI
        public async Task<CitaActoMedioDTO> GetCitaAndActoMedico(string idCita)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("_id",
                        new ObjectId(idCita)));
            var project = new BsonDocument("$project",
                          new BsonDocument
                          {
                            { "_id", 1 },
                            { "fecha_cita", 1 },
                            { "id_acto_medico", 1 },
                            { "motivo_consulta", 1 }
                          });
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_acto_medico_pro",
                            new BsonDocument("$toObjectId", "$id_acto_medico")));
            var lookup = new BsonDocument("$lookup",
                         new BsonDocument
                         {
                            { "from", "acto_medico" },
                            { "localField", "id_acto_medico_pro" },
                            { "foreignField", "_id" },
                            { "as", "acto_medico" }
                         });
            var unwind = new BsonDocument("$unwind",
                         new BsonDocument
                         {
                            { "path", "$acto_medico" },
                            { "preserveNullAndEmptyArrays", true }
                         });
            var project2 = new BsonDocument("$project",
                           new BsonDocument
                           {
                               { "id_acto_medico_pro", 0 },
                               { "acto_medico._id", 0 }
                           });

            CitaActoMedioDTO cita = new CitaActoMedioDTO();
            cita = await _cita.Aggregate()
                   .AppendStage<dynamic>(match)
                   .AppendStage<dynamic>(project)
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<CitaActoMedioDTO>(project2).FirstOrDefaultAsync();
            return cita;
        }


        public List<Cita> GetAll()
        {
            List<Cita> cita = new List<Cita>();
            cita = _cita.Find(Cita => true).ToList();
            return cita;
        }
        public Cita GetByIdPaciente(string id_paciente)
        {
            Cita cita = new Cita();
            cita = _cita.Find(cita => cita.id_paciente == id_paciente).FirstOrDefault();
            return cita;
        }
        public async Task<List<CitaTurno>> GetCitasANDTurnos(string turnosid)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("id_turno",
                        new ObjectId(turnosid)));
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_turno",
                            new BsonDocument("$toObjectId", "$id_turno")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "turnos" },
                                    { "localField", "id_turno" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_turnos" }
                                });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turnos" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var project = new BsonDocument("$project",
                            new BsonDocument("datos_turnos", 1));

            List<CitaTurno> medico = new List<CitaTurno>();
            medico = await _cita.Aggregate()
                     .AppendStage<dynamic>(match)
                     .AppendStage<dynamic>(addfields)
                     .AppendStage<dynamic>(lookup)
                     .AppendStage<dynamic>(unwind)
                     .AppendStage<CitaTurno>(project).ToListAsync();
            return medico;
        }

        public async Task<List<CitaDTO>> GetAllCitasByIdPaciente(string idPaciente)
        {
            List<CitaDTO> PagoDTO = new List<CitaDTO>();

            var match = new BsonDocument("$match",
                       new BsonDocument("id_paciente", idPaciente));

            var addfields1 = new BsonDocument("$addFields",
                                new BsonDocument("id_paciente_pro",
                                new BsonDocument("$toObjectId", "$id_paciente")));
            var lookup1 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "pacientes" },
                                    { "localField", "id_paciente_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_usuario" }
                                });
            var unwind1 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_usuario" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuariopro",
                                new BsonDocument("$toObjectId", "$datos_usuario.id_usuario")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuariopro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields3 = new BsonDocument("$addFields",
                                new BsonDocument("id_rol",
                                new BsonDocument("$toObjectId", "$datos_paciente.rol")));
            var lookup3 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "roles" },
                                    { "localField", "id_rol" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente.nombre_rol" }
                                });
            var unwind3 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente.nombre_rol" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields4 = new BsonDocument("$addFields",
                                new BsonDocument("datos_paciente",
                                new BsonDocument("datos",
                                new BsonDocument("nombre_apellido_paciente",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_paciente.datos.nombre",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_materno"
                                                    })))));
            var addfields5 = new BsonDocument("$addFields",
                                new BsonDocument("id_turno_pro",
                                new BsonDocument("$toObjectId", "$id_turno")));
            var lookup4 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "turnos" },
                                        { "localField", "id_turno_pro" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_turno" }
                                    });
            var unwind4 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields6 = new BsonDocument("$addFields",
                                new BsonDocument("id_medico_pro",
                                new BsonDocument("$toObjectId", "$datos_turno.id_medico")));
            var lookup5 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "medicos" },
                                        { "localField", "id_medico_pro" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_medico" }
                                    });
            var unwind5 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields7 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuario_medico",
                                new BsonDocument("$toObjectId", "$datos_medico.id_usuario")));
            var lookup6 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "usuarios" },
                                        { "localField", "id_usuario_medico" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_turno.datos_medico" }
                                    });
            var unwind6 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno.datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields8 = new BsonDocument("$addFields",
                                new BsonDocument("datos_turno",
                                new BsonDocument("datos_medico",
                                new BsonDocument("nombre_apellido_medico",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_turno.datos_medico.datos.nombre",
                                                        " ",
                                                        "$datos_turno.datos_medico.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_turno.datos_medico.datos.apellido_materno"
                                                    })))));
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 1 },
                                    { "estado_atencion", 1 },
                                    { "estado_pago", 1 },
                                    { "fecha_cita", 1 },
                                    { "fecha_pago", 1 },
                                    { "id_paciente", 1 },
                                    { "precio_neto", 1 },
                                    { "tipo_pago", 1 },
                                    { "datos_paciente",
                            new BsonDocument
                                    {
                                        { "datos",
                            new BsonDocument
                                        {
                                            { "nombre_apellido_paciente", 1 },
                                            { "correo", 1 }
                                        } },
                                        { "usuario", 1 },
                                        { "clave", 1 },
                                        { "nombre_rol",
                            new BsonDocument("nombre", 1) }
                                    } },
                                    { "datos_turno",
                            new BsonDocument
                                    {
                                        { "especialidad", 1 },
                                        { "hora_inicio", 1 },
                                        { "datos_medico",
                            new BsonDocument("nombre_apellido_medico", 1) }
                                    } }
                                });
            PagoDTO = await _cita.Aggregate()
                                .AppendStage<dynamic>(match)
                                .AppendStage<dynamic>(addfields1)
                                .AppendStage<dynamic>(lookup1)
                                .AppendStage<dynamic>(unwind1)
                                .AppendStage<dynamic>(addfields2)
                                .AppendStage<dynamic>(lookup2)
                                .AppendStage<dynamic>(unwind2)
                                .AppendStage<dynamic>(addfields3)
                                .AppendStage<dynamic>(lookup3)
                                .AppendStage<dynamic>(unwind3)
                                .AppendStage<dynamic>(addfields4)
                                .AppendStage<dynamic>(addfields5)
                                .AppendStage<dynamic>(lookup4)
                                .AppendStage<dynamic>(unwind4)
                                .AppendStage<dynamic>(addfields6)
                                .AppendStage<dynamic>(lookup5)
                                .AppendStage<dynamic>(unwind5)
                                .AppendStage<dynamic>(addfields7)
                                .AppendStage<dynamic>(lookup6)
                                .AppendStage<dynamic>(unwind6)
                                .AppendStage<dynamic>(addfields8)
                                .AppendStage<CitaDTO>(project)
                                .ToListAsync();
            return PagoDTO;
        }

        public async Task<List<CitaActoMedioDTO2>> GetAllCitasByIdPacienteActoMedico(string idUsuario)
        {
            List<CitaActoMedioDTO2> PagoDTO = new List<CitaActoMedioDTO2>();

            

            var addfields1 = new BsonDocument("$addFields",
                                new BsonDocument("id_paciente_pro",
                                new BsonDocument("$toObjectId", "$id_paciente")));
            var lookup1 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "pacientes" },
                                    { "localField", "id_paciente_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_usuario" }
                                });
            var unwind1 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_usuario" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuariopro",
                                new BsonDocument("$toObjectId", "$datos_usuario.id_usuario")));
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuariopro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente" }
                                });
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields3 = new BsonDocument("$addFields",
                                new BsonDocument("id_rol",
                                new BsonDocument("$toObjectId", "$datos_paciente.rol")));
            var lookup3 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "roles" },
                                    { "localField", "id_rol" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente.nombre_rol" }
                                });
            var unwind3 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente.nombre_rol" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields4 = new BsonDocument("$addFields",
                                new BsonDocument("datos_paciente",
                                new BsonDocument("datos",
                                new BsonDocument("nombre_apellido_paciente",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_paciente.datos.nombre",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_materno"
                                                    })))));
            var addfields5 = new BsonDocument("$addFields",
                                new BsonDocument("id_turno_pro",
                                new BsonDocument("$toObjectId", "$id_turno")));
            var lookup4 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "turnos" },
                                        { "localField", "id_turno_pro" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_turno" }
                                    });
            var unwind4 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields6 = new BsonDocument("$addFields",
                                new BsonDocument("id_medico_pro",
                                new BsonDocument("$toObjectId", "$datos_turno.id_medico")));
            var lookup5 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "medicos" },
                                        { "localField", "id_medico_pro" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_medico" }
                                    });
            var unwind5 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields7 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuario_medico",
                                new BsonDocument("$toObjectId", "$datos_medico.id_usuario")));
            var lookup6 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "usuarios" },
                                        { "localField", "id_usuario_medico" },
                                        { "foreignField", "_id" },
                                        { "as", "datos_turno.datos_medico" }
                                    });
            var unwind6 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno.datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });
            var addfields8 = new BsonDocument("$addFields",
                                new BsonDocument("datos_turno",
                                new BsonDocument("datos_medico",
                                new BsonDocument("nombre_apellido_medico",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_turno.datos_medico.datos.nombre",
                                                        " ",
                                                        "$datos_turno.datos_medico.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_turno.datos_medico.datos.apellido_materno"
                                                    })))));
            var addfields9 = new BsonDocument("$addFields",
                                new BsonDocument("id_acto_medico",
                                new BsonDocument("$toObjectId", "$id_acto_medico")));
            var lookup7 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                { "from", "acto_medico" },
                                { "localField", "id_acto_medico" },
                                { "foreignField", "_id" },
                                { "as", "datos_acto_medico" }
                                    });
            var unwind7 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                    { "path", "$datos_acto_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                    });
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 1 },
                                    { "estado_atencion", 1 },
                                    { "estado_pago", 1 },
                                    { "fecha_cita", 1 },
                                    { "fecha_pago", 1 },
                                    { "id_paciente", 1 },
                                    { "precio_neto", 1 },
                                    { "tipo_pago", 1 },
                                    { "datos_usuario.id_usuario", 1 },
                                    { "datos_paciente",
                            new BsonDocument
                                    {
                                        { "datos",
                            new BsonDocument
                                        {
                                            { "nombre_apellido_paciente", 1 },
                                            { "correo", 1 }
                                        } },
                                        { "usuario", 1 },
                                        { "clave", 1 },
                                        { "nombre_rol",
                            new BsonDocument("nombre", 1) }
                                    } },
                                    { "datos_acto_medico", 1 },
                                    { "datos_turno",
                            new BsonDocument
                                    {
                                        { "especialidad", 1 },
                                        { "hora_inicio", 1 },
                                        { "datos_medico",
                            new BsonDocument("nombre_apellido_medico", 1) }
                                    } }
                                });
            var match = new BsonDocument("$match",
                       new BsonDocument("datos_usuario.id_usuario", idUsuario));
            PagoDTO = await _cita.Aggregate()
                                .AppendStage<dynamic>(addfields1)
                                .AppendStage<dynamic>(lookup1)
                                .AppendStage<dynamic>(unwind1)
                                .AppendStage<dynamic>(addfields2)
                                .AppendStage<dynamic>(lookup2)
                                .AppendStage<dynamic>(unwind2)
                                .AppendStage<dynamic>(addfields3)
                                .AppendStage<dynamic>(lookup3)
                                .AppendStage<dynamic>(unwind3)
                                .AppendStage<dynamic>(addfields4)
                                .AppendStage<dynamic>(addfields5)
                                .AppendStage<dynamic>(lookup4)
                                .AppendStage<dynamic>(unwind4)
                                .AppendStage<dynamic>(addfields6)
                                .AppendStage<dynamic>(lookup5)
                                .AppendStage<dynamic>(unwind5)
                                .AppendStage<dynamic>(addfields7)
                                .AppendStage<dynamic>(lookup6)
                                .AppendStage<dynamic>(unwind6)
                                .AppendStage<dynamic>(addfields8)
                                .AppendStage<dynamic>(addfields9)
                                .AppendStage<dynamic>(lookup7)
                                .AppendStage<dynamic>(unwind7)
                                .AppendStage<dynamic>(project)
                                .AppendStage<CitaActoMedioDTO2>(match)
                                .ToListAsync();
            return PagoDTO;
        }

        public async Task<List<CitaDTO2>> GetCitasbyFecha(DateTime fecha)
        {
            int year = fecha.Year;
            int day = fecha.Day;
            int month = fecha.Month;

            List<CitaDTO2> citas = new List<CitaDTO2>();
            DateTime firstDate = new DateTime(year, month, 1, 0, 0, 0);
            DateTime lastDate = firstDate.AddMonths(1).AddDays(-1);
            lastDate = lastDate.AddHours(23);
            lastDate = lastDate.AddMinutes(59);
            lastDate = lastDate.AddSeconds(59);

            var match = new BsonDocument("$match",
                                new BsonDocument("$and",
                                new BsonArray
                                        {
                                            new BsonDocument("fecha_cita",
                                            new BsonDocument("$gte",new DateTime(year, month, day, 0, 0, 0))),
                                            new BsonDocument("fecha_cita_fin",
                                            new BsonDocument("$lte",new DateTime(year, month, day, 23, 59, 59)))                                            
                                        }));
            var lookUpTurno = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "turnos" },
                                        { "let",
                                new BsonDocument("turnoID", "$id_turno") },
                                        { "pipeline",
                                new BsonArray
                                        {
                                            new BsonDocument("$match",
                                            new BsonDocument("$expr",
                                            new BsonDocument("$eq",
                                            new BsonArray
                                                        {
                                                            new BsonDocument("$toObjectId", "$$turnoID"),
                                                            "$_id"
                                                        })))
                                        } },
                                        { "as", "turno" }
                                    });

            var lookUpPaciente = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "pacientes" },
                                                { "let",
                                        new BsonDocument("pacienteID", "$id_paciente") },
                                                { "pipeline",
                                        new BsonArray
                                                {
                                                    new BsonDocument("$match",
                                                    new BsonDocument("$expr",
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                                {
                                                                    new BsonDocument("$toObjectId", "$$pacienteID"),
                                                                    "$_id"
                                                                })))
                                                } },
                                                { "as", "paciente" }
                                            });

            var lookUpMedico = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "medicos" },
                                                { "let",
                                        new BsonDocument("medicoID", "$id_medico") },
                                                { "pipeline",
                                        new BsonArray
                                                {
                                                    new BsonDocument("$match",
                                                    new BsonDocument("$expr",
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                                {
                                                                    new BsonDocument("$toObjectId", "$$medicoID"),
                                                                    "$_id"
                                                                }))),
                                                    new BsonDocument("$lookup",
                                                    new BsonDocument
                                                        {
                                                            { "from", "especialidades" },
                                                            { "let",
                                                    new BsonDocument("especialidadID", "$id_especialidad") },
                                                            { "pipeline",
                                                    new BsonArray
                                                            {
                                                                new BsonDocument("$match",
                                                                new BsonDocument("$expr",
                                                                new BsonDocument("$eq",
                                                                new BsonArray
                                                                            {
                                                                                new BsonDocument("$toObjectId", "$$especialidadID"),
                                                                                "$_id"
                                                                            })))
                                                            } },
                                                            { "as", "especialidad" }
                                                        }),
                                                    new BsonDocument("$project",
                                                    new BsonDocument
                                                        {
                                                            { "_id", "$_id" },
                                                            { "turnos", 1 },
                                                            { "subscripcion", 1 },
                                                            { "datos_basicos", 1 },
                                                            { "especialidad",
                                                    new BsonDocument("$arrayElemAt",
                                                    new BsonArray
                                                                {
                                                                    "$especialidad",
                                                                    0
                                                                }) },
                                                            { "id_usuario", 1 }
                                                        })
                                                } },
                                                { "as", "medico" }
                                            });

            var project = new BsonDocument("$project",
                                new BsonDocument
                                    {
                                        { "_id", "$_id" },
                                        { "estado_atencion", "$estado_atencion" },
                                        { "estado_pago", "$estado_pago" },
                                        { "fecha_cita", "$fecha_cita" },
                                        { "fecha_reserva", "$fecha_reserva" },
                                        { "datos_paciente",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$paciente",
                                                0
                                            }) },
                                        { "enlace_cita", "$enlace_cita" },
                                        { "precio_neto", "$precio_neto" },
                                        { "calificacion", "$calificacion" },
                                        { "id_acto_medico", "$id_acto_medico" },
                                        { "observaciones", "$observaciones" },
                                        { "tipo_pago", "$tipo_pago" },
                                        { "id_turno", "$id_turno" },
                                        { "turno",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$turno",
                                                0
                                            }) },
                                        { "medico",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$medico",
                                                0
                                            }) },
                                        { "fecha_cita_fin", "$fecha_cita_fin" }
                                    });

            citas = await _cita.Aggregate()
                .AppendStage<dynamic>(match)
                .AppendStage<dynamic>(lookUpTurno)
                .AppendStage<dynamic>(lookUpPaciente)
                .AppendStage<dynamic>(lookUpMedico)
                .AppendStage<CitaDTO2>(project)
                .ToListAsync();

            return citas;

            //calcula la fecha actual
            //obtener el mes: 29 abril -> abril
            //2 fechas -primer dia del mes 01 abril 00:00, -ultimo dia del mes 31 abril 24:00
        }

        public async Task<List<CitaDTO2>> GetCitasbyFechaPaciente(DateTime fecha, string idPaciente)
        {
            int year = fecha.Year;
            int day = fecha.Day;
            int month = fecha.Month;

            List<CitaDTO2> citas = new List<CitaDTO2>();
            DateTime firstDate = new DateTime(year, month, 1, 0, 0, 0);
            DateTime lastDate = firstDate.AddMonths(1).AddDays(-1);
            lastDate = lastDate.AddHours(23);
            lastDate = lastDate.AddMinutes(59);
            lastDate = lastDate.AddSeconds(59);

            var match = new BsonDocument("$match",
                                new BsonDocument("$and",
                                new BsonArray
                                        {
                                            new BsonDocument("id_paciente", idPaciente),
                                            new BsonDocument("fecha_cita",
                                            new BsonDocument("$gte",new DateTime(year, month, day, 0, 0, 0))),
                                            new BsonDocument("fecha_cita_fin",
                                            new BsonDocument("$lte",new DateTime(year, month, day, 23, 59, 59)))
                                        }));
            var lookUpTurno = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "turnos" },
                                        { "let",
                                new BsonDocument("turnoID", "$id_turno") },
                                        { "pipeline",
                                new BsonArray
                                        {
                                            new BsonDocument("$match",
                                            new BsonDocument("$expr",
                                            new BsonDocument("$eq",
                                            new BsonArray
                                                        {
                                                            new BsonDocument("$toObjectId", "$$turnoID"),
                                                            "$_id"
                                                        })))
                                        } },
                                        { "as", "turno" }
                                    });

            var lookUpPaciente = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "pacientes" },
                                                { "let",
                                        new BsonDocument("pacienteID", "$id_paciente") },
                                                { "pipeline",
                                        new BsonArray
                                                {
                                                    new BsonDocument("$match",
                                                    new BsonDocument("$expr",
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                                {
                                                                    new BsonDocument("$toObjectId", "$$pacienteID"),
                                                                    "$_id"
                                                                })))
                                                } },
                                                { "as", "paciente" }
                                            });

            var lookUpMedico = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "medicos" },
                                                { "let",
                                        new BsonDocument("medicoID", "$id_medico") },
                                                { "pipeline",
                                        new BsonArray
                                                {
                                                    new BsonDocument("$match",
                                                    new BsonDocument("$expr",
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                                {
                                                                    new BsonDocument("$toObjectId", "$$medicoID"),
                                                                    "$_id"
                                                                }))),
                                                    new BsonDocument("$lookup",
                                                    new BsonDocument
                                                        {
                                                            { "from", "especialidades" },
                                                            { "let",
                                                    new BsonDocument("especialidadID", "$id_especialidad") },
                                                            { "pipeline",
                                                    new BsonArray
                                                            {
                                                                new BsonDocument("$match",
                                                                new BsonDocument("$expr",
                                                                new BsonDocument("$eq",
                                                                new BsonArray
                                                                            {
                                                                                new BsonDocument("$toObjectId", "$$especialidadID"),
                                                                                "$_id"
                                                                            })))
                                                            } },
                                                            { "as", "especialidad" }
                                                        }),
                                                    new BsonDocument("$project",
                                                    new BsonDocument
                                                        {
                                                            { "_id", "$_id" },
                                                            { "turnos", 1 },
                                                            { "subscripcion", 1 },
                                                            { "datos_basicos", 1 },
                                                            { "especialidad",
                                                    new BsonDocument("$arrayElemAt",
                                                    new BsonArray
                                                                {
                                                                    "$especialidad",
                                                                    0
                                                                }) },
                                                            { "id_usuario", 1 }
                                                        })
                                                } },
                                                { "as", "medico" }
                                            });

            var project = new BsonDocument("$project",
                                new BsonDocument
                                    {
                                        { "_id", "$_id" },
                                        { "estado_atencion", "$estado_atencion" },
                                        { "estado_pago", "$estado_pago" },
                                        { "fecha_cita", "$fecha_cita" },
                                        { "fecha_reserva", "$fecha_reserva" },
                                        { "datos_paciente",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$paciente",
                                                0
                                            }) },
                                        { "enlace_cita", "$enlace_cita" },
                                        { "precio_neto", "$precio_neto" },
                                        { "calificacion", "$calificacion" },
                                        { "id_acto_medico", "$id_acto_medico" },
                                        { "observaciones", "$observaciones" },
                                        { "tipo_pago", "$tipo_pago" },
                                        { "id_turno", "$id_turno" },
                                        { "turno",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$turno",
                                                0
                                            }) },
                                        { "medico",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$medico",
                                                0
                                            }) },
                                        { "fecha_cita_fin", "$fecha_cita_fin" }
                                    });

            citas = await _cita.Aggregate()
                .AppendStage<dynamic>(match)
                .AppendStage<dynamic>(lookUpTurno)
                .AppendStage<dynamic>(lookUpPaciente)
                .AppendStage<dynamic>(lookUpMedico)
                .AppendStage<CitaDTO2>(project)
                .ToListAsync();

            return citas;

            //calcula la fecha actual
            //obtener el mes: 29 abril -> abril
            //2 fechas -primer dia del mes 01 abril 00:00, -ultimo dia del mes 31 abril 24:00
        }
        public async Task<List<CitaActoMedioDTO>> GetCitaAndActoMedicoByPaciente(string idPaciente)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("id_paciente", idPaciente));
            var project = new BsonDocument("$project",
                          new BsonDocument
                          {
                            { "_id", 1 },
                            { "fecha_cita", 1 },
                            { "id_acto_medico", 1 },
                            { "motivo_consulta", 1 }
                          });
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_acto_medico_pro",
                            new BsonDocument("$toObjectId", "$id_acto_medico")));
            var lookup = new BsonDocument("$lookup",
                         new BsonDocument
                         {
                            { "from", "acto_medico" },
                            { "localField", "id_acto_medico_pro" },
                            { "foreignField", "_id" },
                            { "as", "acto_medico" }
                         });
            var unwind = new BsonDocument("$unwind",
                         new BsonDocument
                         {
                            { "path", "$acto_medico" },
                            { "preserveNullAndEmptyArrays", true }
                         });
            var project2 = new BsonDocument("$project",
                           new BsonDocument
                           {
                               { "id_acto_medico_pro", 0 },
                               { "acto_medico._id", 0 }
                           });

            List<CitaActoMedioDTO> citas = new List<CitaActoMedioDTO>();
            citas = await _cita.Aggregate()
                   .AppendStage<dynamic>(match)
                   .AppendStage<dynamic>(project)
                   .AppendStage<dynamic>(addfields)
                   .AppendStage<dynamic>(lookup)
                   .AppendStage<dynamic>(unwind)
                   .AppendStage<CitaActoMedioDTO>(project2).ToListAsync();
            return citas;
        }

        public async Task<List<CitaDTO2>> GetCitasbyMedicoAndFecha(string medico, DateTime fecha)
        {
            List<CitaDTO2> citas = new List<CitaDTO2>();
            int year = fecha.Year;
            int month = fecha.Month;
            int day = fecha.Day;

            var match = new BsonDocument("$match",
                                new BsonDocument("$and",
                                new BsonArray
                                        {
                                            new BsonDocument("fecha_cita",
                                            new BsonDocument("$gte",new DateTime(year, month, day, 0, 0, 0))),
                                            new BsonDocument("fecha_cita_fin",
                                            new BsonDocument("$lte",new DateTime(year, month, day, 23, 59, 59))),
                                            new BsonDocument("id_medico", medico)
                                        }));
            var lookUpTurno = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "turnos" },
                                        { "let",
                                new BsonDocument("turnoID", "$id_turno") },
                                        { "pipeline",
                                new BsonArray
                                        {
                                            new BsonDocument("$match",
                                            new BsonDocument("$expr",
                                            new BsonDocument("$eq",
                                            new BsonArray
                                                        {
                                                            new BsonDocument("$toObjectId", "$$turnoID"),
                                                            "$_id"
                                                        })))
                                        } },
                                        { "as", "turno" }
                                    });

            var lookUpPaciente = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "pacientes" },
                                                { "let",
                                        new BsonDocument("pacienteID", "$id_paciente") },
                                                { "pipeline",
                                        new BsonArray
                                                {
                                                    new BsonDocument("$match",
                                                    new BsonDocument("$expr",
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                                {
                                                                    new BsonDocument("$toObjectId", "$$pacienteID"),
                                                                    "$_id"
                                                                })))
                                                } },
                                                { "as", "paciente" }
                                            });

            var lookUpMedico = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "medicos" },
                                                { "let",
                                        new BsonDocument("medicoID", "$id_medico") },
                                                { "pipeline",
                                        new BsonArray
                                                {
                                                    new BsonDocument("$match",
                                                    new BsonDocument("$expr",
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                                {
                                                                    new BsonDocument("$toObjectId", "$$medicoID"),
                                                                    "$_id"
                                                                }))),
                                                    new BsonDocument("$lookup",
                                                    new BsonDocument
                                                        {
                                                            { "from", "especialidades" },
                                                            { "let",
                                                    new BsonDocument("especialidadID", "$id_especialidad") },
                                                            { "pipeline",
                                                    new BsonArray
                                                            {
                                                                new BsonDocument("$match",
                                                                new BsonDocument("$expr",
                                                                new BsonDocument("$eq",
                                                                new BsonArray
                                                                            {
                                                                                new BsonDocument("$toObjectId", "$$especialidadID"),
                                                                                "$_id"
                                                                            })))
                                                            } },
                                                            { "as", "especialidad" }
                                                        }),
                                                    new BsonDocument("$project",
                                                    new BsonDocument
                                                        {
                                                            { "_id", "$_id" },
                                                            { "turnos", 1 },
                                                            { "subscripcion", 1 },
                                                            { "datos_basicos", 1 },
                                                            { "especialidad",
                                                    new BsonDocument("$arrayElemAt",
                                                    new BsonArray
                                                                {
                                                                    "$especialidad",
                                                                    0
                                                                }) },
                                                            { "id_usuario", 1 }
                                                        })
                                                } },
                                                { "as", "medico" }
                                            });

            var project = new BsonDocument("$project",
                                new BsonDocument
                                    {
                                        { "_id", "$_id" },
                                        { "estado_atencion", "$estado_atencion" },
                                        { "estado_pago", "$estado_pago" },
                                        { "fecha_cita", "$fecha_cita" },
                                        { "fecha_reserva", "$fecha_reserva" },
                                        { "datos_paciente",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$paciente",
                                                0
                                            }) },
                                        { "enlace_cita", "$enlace_cita" },
                                        { "precio_neto", "$precio_neto" },
                                        { "calificacion", "$calificacion" },
                                        { "id_acto_medico", "$id_acto_medico" },
                                        { "observaciones", "$observaciones" },
                                        { "tipo_pago", "$tipo_pago" },
                                        { "id_turno", "$id_turno" },
                                        { "turno",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$turno",
                                                0
                                            }) },
                                        { "medico",
                                new BsonDocument("$arrayElemAt",
                                new BsonArray
                                            {
                                                "$medico",
                                                0
                                            }) },
                                        { "fecha_cita_fin", "$fecha_cita_fin" }
                                    });

            citas = await _cita.Aggregate()
                .AppendStage<dynamic>(match)
                .AppendStage<dynamic>(lookUpTurno)
                .AppendStage<dynamic>(lookUpPaciente)
                .AppendStage<dynamic>(lookUpMedico)
                .AppendStage<CitaDTO2>(project)
                .ToListAsync();

            return citas;

            //calcula la fecha actual
            //obtener el mes: 29 abril -> abril
            //2 fechas -primer dia del mes 01 abril 00:00, -ultimo dia del mes 31 abril 24:00
        }
    }      
}

    

