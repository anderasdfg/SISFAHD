using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace SISFAHD.Services
{
    public class OrdenesService
    {
        private readonly IMongoCollection<Ordenes> _ordenes;
        public OrdenesService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _ordenes = database.GetCollection<Ordenes>("ordenes");
        }
        public Ordenes CreateOrdenes(Ordenes medicinas)
        {
            _ordenes.InsertOne(medicinas);
            return medicinas;
        }
        public async Task<bool> VerifyOrdenesByActoMedicoAsync(string id_acto_medico)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("id_acto_medico", id_acto_medico));
            List<Ordenes> ordenes = new List<Ordenes>();
            ordenes = await _ordenes.Aggregate()
                                .AppendStage<Ordenes>(match)
                                .ToListAsync();
            if(ordenes.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<Ordenes> ModificarOrdenes(Ordenes orden)
        {
            var filter = Builders<Ordenes>.Filter.Eq("id", ObjectId.Parse(orden.id));
            var update = Builders<Ordenes>.Update
                .Set("estado_atencion", orden.estado_atencion)
                .Set("estado_pago", orden.estado_pago)
                .Set("fecha_orden", orden.fecha_orden)
                .Set("fecha_pago", orden.fecha_pago)
                .Set("fecha_reserva", orden.fecha_reserva)
                .Set("id_paciente", orden.id_paciente)
                .Set("precio_neto", orden.precio_neto)
                .Set("tipo_pago",orden.tipo_pago)
                .Set("id_acto_medico", orden.id_acto_medico)
                .Set("id_medico_orden", orden.id_medico_orden)
                .Set("procedimientos", orden.procedimientos);
            await _ordenes.UpdateOneAsync(filter, update);
            return orden;
        }
        public async Task<List<OrdenesDTO>> GetAllExamenesAuxiliares_By_Paciente(string idUsuario)
        {
            List<OrdenesDTO> lstordenes_Examenes = new List<OrdenesDTO>();

            var addFields1 = new BsonDocument("$addFields",
                                new BsonDocument
                                    {
                                        { "id_paciente",
                                new BsonDocument("$toObjectId", "$id_paciente") },
                                        { "id_acto_medico",
                                new BsonDocument("$toObjectId", "$id_acto_medico") },
                                        { "id_medico_orden",
                                new BsonDocument("$toObjectId", "$id_medico_orden") }
                                    });
            var lookup1 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "pacientes" },
                                        { "localField", "id_paciente" },
                                        { "foreignField", "_id" },
                                        { "as", "paciente" }
                                    });
            var unwind1 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                        { "path", "$paciente" },
                                        { "preserveNullAndEmptyArrays", true }
                                    });
            var lookup2 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "acto_medico" },
                                        { "localField", "id_acto_medico" },
                                        { "foreignField", "_id" },
                                        { "as", "acto_medico" }
                                    });
            var unwind2 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                        { "path", "$id_acto_medico" },
                                        { "preserveNullAndEmptyArrays", true }
                                    });
            var lookup3 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "medicos" },
                                        { "localField", "id_medico_orden" },
                                        { "foreignField", "_id" },
                                        { "as", "medico" }
                                    });
            var unwind3 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                        { "path", "$medico" },
                                        { "preserveNullAndEmptyArrays", false }
                                    });
            var addFields2 = new BsonDocument("$addFields",
                                new BsonDocument
                                    {
                                        { "medico.id_usuario",
                                new BsonDocument("$toObjectId", "$medico.id_usuario") },
                                        { "medico.id_especialidad",
                                new BsonDocument("$toObjectId", "$medico.id_especialidad") }
                                    });
            var lookup4 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "usuarios" },
                                        { "localField", "medico.id_usuario" },
                                        { "foreignField", "_id" },
                                        { "as", "usuario" }
                                    });
            var unwind4 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                        { "path", "$usuario" },
                                        { "preserveNullAndEmptyArrays", false }
                                    });
            var lookup5 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "especialidades" },
                                        { "localField", "medico.id_especialidad" },
                                        { "foreignField", "_id" },
                                        { "as", "especialidad" }
                                    });
            var unwind5 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                        { "path", "$especialidad" },
                                        { "preserveNullAndEmptyArrays", false }
                                    });
            var project1 = new BsonDocument("$project",
                                new BsonDocument
                                    {
                                        { "fecha_orden", 1 },
                                        { "id_usuario", "$paciente.id_usuario" },
                                        { "datos_medico",
                                new BsonDocument
                                        {
                                            { "nombre", "$usuario.datos.nombre" },
                                            { "apellido",
                                new BsonDocument("$concat",
                                new BsonArray
                                                {
                                                    "$usuario.datos.apellido_paterno",
                                                    " ",
                                                    "$usuario.datos.apellido_materno"
                                                }) },
                                            { "especialidad", "$especialidad.nombre" }
                                        } },
                                        { "examenes_auxiliares",
                                new BsonDocument("$concatArrays", "$acto_medico.diagnostico.examenes_auxiliares") },
                                        { "procedimientos", 1 }
                                    });
            var unwind6 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                        { "path", "$examenes_auxiliares" },
                                        { "preserveNullAndEmptyArrays", false }
                                    });
            var unwind7 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                        { "path", "$examenes_auxiliares" },
                                        { "preserveNullAndEmptyArrays", false }
                                    });
            var project2 = new BsonDocument("$project",
                                new BsonDocument
                                    {
                                        { "examenes",
                                new BsonDocument("$map",
                                new BsonDocument
                                            {
                                                { "input", "$procedimientos" },
                                                { "as", "primero" },
                                                { "in",
                                new BsonDocument("$mergeObjects",
                                new BsonArray
                                                    {
                                                        "$$primero",
                                                        new BsonDocument("$arrayElemAt",
                                                        new BsonArray
                                                            {
                                                                new BsonDocument("$filter",
                                                                new BsonDocument
                                                                    {
                                                                        { "input", "$examenes_auxiliares" },
                                                                        { "as", "segundo" },
                                                                        { "cond",
                                                                new BsonDocument("$eq",
                                                                new BsonArray
                                                                            {
                                                                                "$$primero.id_examen",
                                                                                "$$segundo.codigo"
                                                                            }) }
                                                                    }),
                                                                0
                                                            })
                                                    }) }
                                            }) },
                                        { "_id", 1 },
                                        { "fecha_orden", 1 },
                                        { "datos_medico", 1 },
                                        { "id_usuario", 1 }
                                    });
            var unwind8 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                        { "path", "$examenes" },
                                        { "preserveNullAndEmptyArrays", true }
                                    });
            var addFields3 = new BsonDocument("$addFields",
                                new BsonDocument("examenes.id_resultado_examen",
                                new BsonDocument("$toObjectId", "$examenes.id_resultado_examen")));
            var lookup6 = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "resultado_examen" },
                                        { "localField", "examenes.id_resultado_examen" },
                                        { "foreignField", "_id" },
                                        { "as", "examenes.resultado" }
                                    });
            var unwind9 = new BsonDocument("$unwind",
                                new BsonDocument
                                    {
                                        { "path", "$examenes.resultado" },
                                        { "preserveNullAndEmptyArrays", true }
                                    });
            var addFields4 = new BsonDocument("$addFields",
                                new BsonDocument("examenes.resultado",
                                new BsonDocument("$reverseArray", "$examenes.resultado.documento_anexo")));
            var project3 = new BsonDocument("$project",
                                new BsonDocument("examenes",
                                new BsonDocument
                                        {
                                            { "id_examen", 0 },
                                            { "id_resultado_examen", 0 },
                                            { "id_turno_orden", 0 }
                                        }));
            var group = new BsonDocument("$group",
                                new BsonDocument
                                    {
                                        { "_id", "$_id" },
                                        { "fecha_orden",
                                new BsonDocument("$first", "$fecha_orden") },
                                        { "id_usuario",
                                new BsonDocument("$first", "$id_usuario") },
                                        { "datos_medico",
                                new BsonDocument("$first", "$datos_medico") },
                                        { "examenes",
                                new BsonDocument("$addToSet", "$examenes") }
                                    });
            var match = new BsonDocument("$match",
                                new BsonDocument("id_usuario", idUsuario));

            lstordenes_Examenes = await _ordenes.Aggregate()
                                .AppendStage<dynamic>(addFields1)
                                .AppendStage<dynamic>(lookup1)
                                .AppendStage<dynamic>(unwind1)
                                .AppendStage<dynamic>(lookup2)
                                .AppendStage<dynamic>(unwind2)
                                .AppendStage<dynamic>(lookup3)
                                .AppendStage<dynamic>(unwind3)
                                .AppendStage<dynamic>(addFields2)
                                .AppendStage<dynamic>(lookup4)
                                .AppendStage<dynamic>(unwind4)
                                .AppendStage<dynamic>(lookup5)
                                .AppendStage<dynamic>(unwind5)
                                .AppendStage<dynamic>(project1)
                                .AppendStage<dynamic>(unwind6)
                                .AppendStage<dynamic>(unwind7)
                                .AppendStage<dynamic>(project2)
                                .AppendStage<dynamic>(unwind8)
                                .AppendStage<dynamic>(addFields3)
                                .AppendStage<dynamic>(lookup6)
                                .AppendStage<dynamic>(unwind9)
                                .AppendStage<dynamic>(addFields4)
                                .AppendStage<dynamic>(project3)
                                .AppendStage<dynamic>(group)
                                .AppendStage<OrdenesDTO>(match)
                                .ToListAsync();
            return lstordenes_Examenes;
        }
    }
}
