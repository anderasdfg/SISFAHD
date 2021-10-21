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
    public class ResultadoExamenService
    {
        private readonly IMongoCollection<Paciente> _paciente;
        private readonly IMongoCollection<ResultadoExamen> _resultadosExamen;
        public ResultadoExamenService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _paciente = database.GetCollection<Paciente>("pacientes");
            _resultadosExamen = database.GetCollection<ResultadoExamen>("resultado_examen");
        }
        public async Task<List<ExamenAuxiliar>> GetAllExamenesAuxiliares_By_Paciente(string idPaciente)
        {
            List<ExamenAuxiliar> lstExamenesAuxiliares= new List<ExamenAuxiliar>();

            var match_idPaciente = new BsonDocument("$match",
                                    new BsonDocument("_id",
                                    new ObjectId(idPaciente)));
            var addFields = new BsonDocument("$addFields",
                                    new BsonDocument("id_historia",
                                    new BsonDocument("$toObjectId", "$id_historia")));
            var lookup = new BsonDocument("$lookup",
                                    new BsonDocument
                                        {
                                            { "from", "historias" },
                                            { "localField", "id_historia" },
                                            { "foreignField", "_id" },
                                            { "as", "historia" }
                                        });
            var unwind = new BsonDocument("$unwind",
                                    new BsonDocument
                                        {
                                            { "path", "$historia" },
                                            { "preserveNullAndEmptyArrays", true }
                                        });
            var unwind1 = new BsonDocument("$unwind",
                                    new BsonDocument
                                        {
                                            { "path", "$historia.historial" },
                                            { "preserveNullAndEmptyArrays", true }
                                        });
            var addFields1 = new BsonDocument("$addFields",
                                    new BsonDocument("historia",
                                    new BsonDocument("historial",
                                    new BsonDocument("id_cita",
                                    new BsonDocument("$toObjectId", "$historia.historial.id_cita")))));
            var lookup1 = new BsonDocument("$lookup",
                                    new BsonDocument
                                        {
                                            { "from", "citas" },
                                            { "localField", "historia.historial.id_cita" },
                                            { "foreignField", "_id" },
                                            { "as", "cita" }
                                        });
            var unwind2 = new BsonDocument("$unwind",
                                    new BsonDocument
                                        {
                                            { "path", "$cita" },
                                            { "preserveNullAndEmptyArrays", false }
                                        });
            var addFields2 = new BsonDocument("$addFields",
                                    new BsonDocument("cita",
                                    new BsonDocument("id_acto_medico",
                                    new BsonDocument("$toObjectId", "$cita.id_acto_medico"))));
            var lookup2 = new BsonDocument("$lookup",
                                    new BsonDocument
                                        {
                                            { "from", "acto_medico" },
                                            { "localField", "cita.id_acto_medico" },
                                            { "foreignField", "_id" },
                                            { "as", "acto_medico" }
                                        });
            var unwind3 = new BsonDocument("$unwind",
                                    new BsonDocument
                                        {
                                            { "path", "$acto_medico" },
                                            { "preserveNullAndEmptyArrays", true }
                                        });
            var project = new BsonDocument("$project",
                                    new BsonDocument
                                        {
                                            { "_id", 0 },
                                            { "acto_medico",
                                    new BsonDocument
                                            {
                                                //{ "_id", 1 },
                                                { "diagnostico",
                                    new BsonDocument("examenes_auxiliares", 1) }
                                            } }
                                        });
            var unwind4 = new BsonDocument("$unwind",
                                    new BsonDocument
                                        {
                                            { "path", "$acto_medico.diagnostico" },
                                            { "preserveNullAndEmptyArrays", true }
                                        });
            var unwind5 = new BsonDocument("$unwind",
                                    new BsonDocument
                                        {
                                            { "path", "$acto_medico.diagnostico.examenes_auxiliares" },
                                            { "preserveNullAndEmptyArrays", false }
                                        });
            //Agregación que envía la información que está dentro de varios objetos afuera de dichos objetos 
            var replaceRoot = new BsonDocument("$replaceRoot",
                                    new BsonDocument("newRoot",
                                    new BsonDocument("$mergeObjects",
                                    new BsonArray
                                                {
                                                    new BsonDocument
                                                    {
                                                        { "codigo", 0 },
                                                        { "nombre", 0 },
                                                        { "observaciones", 0 },
                                                        { "tipo", 0 }
                                                    },
                                                    "$acto_medico.diagnostico.examenes_auxiliares"
                                                })));
            lstExamenesAuxiliares = await _paciente.Aggregate()
                                .AppendStage<dynamic>(match_idPaciente)
                                .AppendStage<dynamic>(addFields)
                                .AppendStage<dynamic>(lookup)
                                .AppendStage<dynamic>(unwind)
                                .AppendStage<dynamic>(unwind1)
                                .AppendStage<dynamic>(addFields1)
                                .AppendStage<dynamic>(lookup1)
                                .AppendStage<dynamic>(unwind2)
                                .AppendStage<dynamic>(addFields2)
                                .AppendStage<dynamic>(lookup2)
                                .AppendStage<dynamic>(unwind3)
                                .AppendStage<dynamic>(project)
                                .AppendStage<dynamic>(unwind4)
                                .AppendStage<dynamic>(unwind5)
                                .AppendStage<ExamenAuxiliar>(replaceRoot)
                                .ToListAsync();
            return lstExamenesAuxiliares;
        }

        public async Task<List<ResultadoExamen>> GetAllExamenesSubidos(string idPaciente)
        {
            List<ResultadoExamen> lstResultadoExamen = new List<ResultadoExamen>();

            var match_idPaciente = new BsonDocument("$match",
                                    new BsonDocument("_id",
                                    new ObjectId(idPaciente)));
            var unwind = new BsonDocument("$unwind",
                                    new BsonDocument
                                        {
                                            { "path", "$archivos" },
                                            { "preserveNullAndEmptyArrays", false }
                                        });
            var addFields = new BsonDocument("$addFields",
                                    new BsonDocument("archivos.id_resultado",
                                    new BsonDocument("$toObjectId", "$archivos.id_resultado")));
            var lookup = new BsonDocument("$lookup",
                                    new BsonDocument
                                        {
                                            { "from", "resultado_examen" },
                                            { "localField", "archivos.id_resultado" },
                                            { "foreignField", "_id" },
                                            { "as", "resultado_examen" }
                                        });
            var unwind2 = new BsonDocument("$unwind",
                                    new BsonDocument
                                        {
                                            { "path", "$resultado_examen" },
                                            { "preserveNullAndEmptyArrays", true }
                                        });
            var project = new BsonDocument("$project",
                                    new BsonDocument
                                        {
                                            { "_id", 0 },
                                            { "resultado_examen", 1 }
                                        });
            var replaceRoot = new BsonDocument("$replaceRoot",
                                    new BsonDocument("newRoot",
                                    new BsonDocument("$mergeObjects",
                                    new BsonArray
                                                {
                                                    new BsonDocument
                                                    {
                                                        { "_id", 0 },
                                                        { "nombre", 0 },
                                                        { "tipo", 0 },
                                                        { "observaciones", 0 },
                                                        { "documento_anexo", 0 },
                                                        { "codigo", 0 }
                                                    },
                                                    "$resultado_examen"
                                                })));
            lstResultadoExamen = await _resultadosExamen.Aggregate()
                                .AppendStage<dynamic>(match_idPaciente)
                                .AppendStage<dynamic>(unwind)
                                .AppendStage<dynamic>(addFields)
                                .AppendStage<dynamic>(lookup)
                                .AppendStage<dynamic>(unwind2)
                                .AppendStage<dynamic>(project)
                                .AppendStage<ResultadoExamen>(replaceRoot)
                                .ToListAsync();
            return lstResultadoExamen;
        }
        
        public async Task<ResultadoExamen> GetByIdExamenesSubidos(string id)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("_id", id));

            ResultadoExamen resultadoExamen = new ResultadoExamen();
            resultadoExamen = await _resultadosExamen.Aggregate()
                           .AppendStage<ResultadoExamen>(match).FirstOrDefaultAsync();
            return resultadoExamen;
        }

        public async Task<ResultadoExamen> CrearResultadoExamen(ResultadoExamen resultados)
        {
            await _resultadosExamen.InsertOneAsync(resultados);
            return resultados;
        }

        public ResultadoExamen ModificarResultadoExamen(ResultadoExamen resultado)
        {

            var filter = Builders<ResultadoExamen>.Filter.Eq("id", resultado.id);
            var update = Builders<ResultadoExamen>.Update
                .Set("nombre", resultado.nombre)
                .Set("tipo", resultado.tipo)
                .Set("observaciones", resultado.observaciones)
                .Set("documento_anexo", resultado.documento_anexo)
                .Set("codigo", resultado.codigo);
            resultado = _resultadosExamen.FindOneAndUpdate<ResultadoExamen>(filter, update, new FindOneAndUpdateOptions<ResultadoExamen>
            {
                ReturnDocument = ReturnDocument.After
            });
            return resultado;
        }
        public async Task<ResultadoExamen> EliminarResultadosExamen(string idResultado)
        {
            var filtro = Builders<ResultadoExamen>.Filter.Eq("id", idResultado);
            return await _resultadosExamen.FindOneAndDeleteAsync<ResultadoExamen>(filtro);    
        }
    }
}
