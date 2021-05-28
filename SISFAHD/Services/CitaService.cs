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
        private readonly IMongoCollection<Venta> _venta;

        public CitaService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _cita = database.GetCollection<Cita>("citas");
            _venta = database.GetCollection<Venta>("ventas");
        }
        public async Task<List<CitaDTO>> GetAll()
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
                                    { "datos_turno",
                            new BsonDocument
                                    {
                                        { "especialidad", 1 },
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
        public async Task<CitaDTO> GetById(string id)
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
                                    { "datos_turno",
                            new BsonDocument
                                    {
                                        { "especialidad", 1 },
                                        { "datos_medico",
                            new BsonDocument("nombre_apellido_medico", 1) }
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
        public async Task<Cita> ModifyEstadoPagoCita(Cita pagorealizado)
        {
            var filter = Builders<Cita>.Filter.Eq("id", pagorealizado.id);
            var update = Builders<Cita>.Update
                .Set("estado_pago", pagorealizado.estado_pago);
            _cita.UpdateOne(filter, update);
            return pagorealizado;
        }
        public async Task<Venta> CreateUnNuevoPagoRealizado(Venta pagorealizado)
        {
            _venta.InsertOne(pagorealizado);
            return pagorealizado;
        }
    }
}
