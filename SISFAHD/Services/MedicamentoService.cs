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
    public class MedicamentoService
    {
        private readonly IMongoCollection<Medicamento> _MedicamentoCollection;
        public MedicamentoService(ISisfahdDatabaseSettings settings)
        {
            var medicamento = new MongoClient(settings.ConnectionString);
            var database = medicamento.GetDatabase(settings.DatabaseName);
            _MedicamentoCollection = database.GetCollection<Medicamento>("medicamentos");
        }
        public async Task<List<Medicamento>> GetAll()
        {
            List<Medicamento> medicamento = new List<Medicamento>();
            //medicamento = _MedicamentoCollection.Find(medicamento => true).ToList();
            var project = new BsonDocument("$project", new BsonDocument { { "codigo", 1 }, { "nombre", 1 }, { "formula_farmaceutica_simplificada", 1 } });
            var limit = new BsonDocument("$limit", 1000);
            medicamento = await _MedicamentoCollection.Aggregate().AppendStage<dynamic>(project).AppendStage<Medicamento>(limit).ToListAsync();
            return medicamento;
        }
        public async Task<List<Medicamento>> GetByName(string nombre)
        {
            List<Medicamento> medicamento = new List<Medicamento>();
            var primermatch = new BsonDocument("$match",
                new BsonDocument("nombre",
                new BsonDocument
                        {
                            { "$regex", nombre },
                            { "$options", "g" }
                        }));
            var segundofiltro = new BsonDocument("$group",
                new BsonDocument
                    {
                        { "_id",
                new BsonDocument
                        {
                            { "nombre", "$nombre" },
                            { "concentracion", "$concentracion" },
                            { "formula_farmaceutica", "$formula_farmaceutica" }
                        } },
                        { "doc",
                new BsonDocument("$first", "$$ROOT") }
                    });
            var tercerfiltro = new BsonDocument("$project",
                new BsonDocument
                    {
                        { "_id", "$doc._id" },
                        { "codigo", "$doc.codigo" },
                        { "nombre", "$_id.nombre" },
                        { "concentracion", "$_id.concentracion" },
                        { "formula_farmaceutica", "$_id.formula_farmaceutica" }
                    });

            medicamento = await _MedicamentoCollection.Aggregate().AppendStage<dynamic>(primermatch).AppendStage<dynamic>(segundofiltro).AppendStage<Medicamento>(tercerfiltro).ToListAsync();
            return medicamento;
        }
        public async Task<List<Medicamento>> GetByNameConcentrationForm(string nombre, string concentracion, string forma)
        {
            List<Medicamento> medicamentos = new List<Medicamento>();
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "nombre", new BsonDocument("$regex", nombre)  },
                                    { "concentracion", new BsonDocument("$regex", concentracion)  },
                                    { "formula_farmaceutica_simplificada", new BsonDocument("$regex", forma) }
                                });
            medicamentos = await _MedicamentoCollection.Aggregate().AppendStage<Medicamento>(match).ToListAsync();
            return medicamentos;
        }
    }
}
