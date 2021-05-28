using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.Entities;
using System.Threading.Tasks;
using SISFAHD.DTOs;

namespace SISFAHD.Services
{
    public class MedicoService
    {
        private readonly IMongoCollection<Medico> _medicos;
        public MedicoService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _medicos = database.GetCollection<Medico>("medicos");
        }
        public List<Medico> GetAll()
        {
            List<Medico> medicos = new List<Medico>();
            medicos = _medicos.Find(Medico => true).ToList();
            return medicos;
        }
        public List<Medico> GetByEspecialidad(string idEspecialidad)
        {
            List<Medico> medicos = new List<Medico>();
            medicos = _medicos.Find(medico => medico.id_especialidad == idEspecialidad).ToList();
            return medicos;
        }
        public async Task <List<MedicoDTO>> GetMedicosByEspecialidad(string idEspecialidad)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("id_especialidad", idEspecialidad));

            var addFields = new BsonDocument("$addFields",
                            new BsonDocument("id_usuario_obj",
                            new BsonDocument("$toObjectId", "$id_usuario")));

            var lookup =      new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuario_obj" },
                                    { "foreignField", "_id" },
                                    { "as", "datosUsuario" }
                                });
            var unwind = new BsonDocument("$unwind",
                         new BsonDocument
                            {
                                { "path", "$datosUsuario" },
                                { "preserveNullAndEmptyArrays", true }
                            });
            var project = new BsonDocument("$project",
                          new BsonDocument
                              {
                                { "_id", 1 },
                                { "turnos", 1 },
                                { "suscripcion", 1 },
                                { "datos_basicos", 1 },
                                { "id_especialidad", 1 },
                                { "id_usuario", 1 },
                                { "nombrecompleto",
                        new BsonDocument("$concat",
                        new BsonArray
                                    {
                                        "$datosUsuario.datos.nombre",
                                        " ",
                                        "$datosUsuario.datos.apellido_paterno",
                                        "$datosUsuario.datos.apellido_materno"
                                    }) }
                                });

            List<MedicoDTO> medicos = new List<MedicoDTO>();

            medicos = await _medicos.Aggregate()
                       .AppendStage<dynamic>(match)
                       .AppendStage<dynamic>(addFields)
                       .AppendStage<dynamic>(lookup)
                       .AppendStage<dynamic>(unwind)
                       .AppendStage<MedicoDTO>(project).ToListAsync();

            return medicos;

        }       

    }
}
