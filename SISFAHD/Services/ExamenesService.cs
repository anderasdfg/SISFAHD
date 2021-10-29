using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.Entities;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class ExamenesService
    {

        private readonly IMongoCollection<Examenes> _examenes;
        private readonly IMongoCollection<Paciente> _paciente;
        public ExamenesService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _examenes = database.GetCollection<Examenes>("examenes");
            _paciente = database.GetCollection<Paciente>("pacientes");
        }
        public List<Examenes> GetAll()
        {
            List<Examenes> examenes = new List<Examenes>();
            examenes = _examenes.Find(examenes => true).ToList();
            return examenes;
        }

        public async Task<List<Examenes>> GetByNombre(string nombre)
        {
            List<Examenes> examenes = new List<Examenes>();
            var match = new BsonDocument("$match",
                        new BsonDocument("descripcion",
                        new BsonDocument
                        {
                            { "$regex", nombre.ToUpper() },
                            { "$options", "g" }
                        }));
            examenes = await _examenes.Aggregate().AppendStage<Examenes>(match).ToListAsync();
            return examenes;
        }

        public Examenes CrearExamenesAux(Examenes examenes)
        {
            _examenes.InsertOne(examenes);
            return examenes;
        }


        public Examenes ModificarExamenesAux(Examenes examenes)
        {

            var filter = Builders<Examenes>.Filter.Eq("id", examenes.id);
            var update = Builders<Examenes>.Update
                .Set("descripcion", examenes.descripcion)
                .Set("precio", examenes.precio);


            examenes = _examenes.FindOneAndUpdate<Examenes>(filter, update, new FindOneAndUpdateOptions<Examenes>
            {
                ReturnDocument = ReturnDocument.After
            });
            

            return examenes;
        }

        public async Task<Examenes> RemoveExamenes(string id)
        {
            var filter = Builders<Examenes>.Filter.Eq("id", id);
            return await _examenes.FindOneAndDeleteAsync<Examenes>(filter, new FindOneAndDeleteOptions<Examenes> { });
        }





        public Examenes GetByID(string id)
        {
            Examenes examen = new Examenes();
            examen = _examenes.Find(examen => examen.id == id).FirstOrDefault();
            return examen;

        }

        public async Task<List<Examenes>> GetAllExamenes_By_Paciente(string idUsuario)
        {
            List<Examenes> lstExamenes = new List<Examenes>();

            var match_idUsuario = new BsonDocument("$match",
                                    new BsonDocument("id_usuario",
                                    idUsuario));
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
            var addFields2 = new BsonDocument("$addFields",
                                    new BsonDocument("resultado_examen",
                                    new BsonDocument("codigo",
                                    new BsonDocument("$toObjectId", "$resultado_examen.codigo"))));
            var lookup2 = new BsonDocument("$lookup",
                                    new BsonDocument
                                        {
                                            { "from", "examenes" },
                                            { "localField", "resultado_examen.codigo" },
                                            { "foreignField", "_id" },
                                            { "as", "examenes" }
                                        });
            var unwind3 = new BsonDocument("$unwind",
                                    new BsonDocument
                                        {
                                            { "path", "$examenes" },
                                            { "preserveNullAndEmptyArrays", false }
                                        });
            var project = new BsonDocument("$project",
                                    new BsonDocument
                                        {
                                            { "_id", 0 },
                                            { "examenes", 1 }
                                        });
            var replaceRoot = new BsonDocument("$replaceRoot",
                                    new BsonDocument("newRoot",
                                    new BsonDocument("$mergeObjects",
                                    new BsonArray
                                                {
                                                    new BsonDocument
                                                    {
                                                        { "_id", 0 },
                                                        { "descripcion", 0 },
                                                        { "precio", 0 }
                                                    },
                                                    "$examenes"
                                                })));
            lstExamenes = await _paciente.Aggregate()
                                .AppendStage<dynamic>(match_idUsuario)
                                .AppendStage<dynamic>(unwind)
                                .AppendStage<dynamic>(addFields)
                                .AppendStage<dynamic>(lookup)
                                .AppendStage<dynamic>(unwind2)
                                .AppendStage<dynamic>(addFields2)
                                .AppendStage<dynamic>(lookup2)
                                .AppendStage<dynamic>(unwind3)
                                .AppendStage<dynamic>(project)
                                .AppendStage<Examenes>(replaceRoot)
                                .ToListAsync();
            return lstExamenes;
        }
    }
}
