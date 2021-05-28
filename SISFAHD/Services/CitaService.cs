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

        public CitaService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _cita = database.GetCollection<Cita>("citas");
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

            var lookup2=    new BsonDocument("$lookup",
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
            var addFields4 = new BsonDocument("$addFields",
                                new BsonDocument("datos_paciente",
                                new BsonDocument("datos",
                                new BsonDocument("apellido",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_paciente.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_materno"
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
                                            { "nombre", 1 },
                                            { "apellido", 1 },
                                            { "correo", 1 }
                                        } },
                                        { "usuario", 1 },
                                        { "clave", 1 },
                                        { "nombre_rol",
                            new BsonDocument("nombre", 1) }
                                    } },
                                    { "apellido", 1 }
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
                                .AppendStage<dynamic>(addFields4)
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
            var addFields4 = new BsonDocument("$addFields",
                                new BsonDocument("datos_paciente",
                                new BsonDocument("datos",
                                new BsonDocument("apellido",
                                new BsonDocument("$concat",
                                new BsonArray
                                                    {
                                                        "$datos_paciente.datos.apellido_paterno",
                                                        " ",
                                                        "$datos_paciente.datos.apellido_materno"
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
                                            { "nombre", 1 },
                                            { "apellido", 1 },
                                            { "correo", 1 }
                                        } },
                                        { "usuario", 1 },
                                        { "clave", 1 },
                                        { "nombre_rol",
                            new BsonDocument("nombre", 1) }
                                    } },
                                    { "apellido", 1 }
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
                                .AppendStage<dynamic>(addFields4)
                                .AppendStage<dynamic>(project)
                                .AppendStage<CitaDTO>(match)
                                .FirstAsync();
            return pago;
        }
    }
}
