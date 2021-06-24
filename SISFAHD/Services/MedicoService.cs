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
        public async Task<List<MedicoDTO>> GetMedicosByEspecialidad(string idEspecialidad)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("id_especialidad", idEspecialidad));

            var addFields = new BsonDocument("$addFields",
                            new BsonDocument("id_usuario_obj",
                            new BsonDocument("$toObjectId", "$id_usuario")));

            var lookup = new BsonDocument("$lookup",
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
        public async Task<MedicoDTOEspcialidad> GetMedicosAndEspecialidad(string idMedico)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("_id",
                        new ObjectId(idMedico)));
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_especialidad_m",
                            new BsonDocument("$toObjectId", "$id_especialidad")));
            var lookup = new BsonDocument("$lookup",
                         new BsonDocument
                         {
                            { "from", "especialidades" },
                            { "localField", "id_especialidad_m" },
                            { "foreignField", "_id" },
                            { "as", "especialidad" }
                          });
            var unwind = new BsonDocument("$unwind",
                         new BsonDocument
                         {
                             { "path", "$especialidad" },
                             { "preserveNullAndEmptyArrays", true }
                         });
            var project = new BsonDocument("$project",
                            new BsonDocument
                            {
                                { "_id", 1 },
                                { "id_usuario", 1 },
                                { "especialidad.nombre", 1 },
                                { "especialidad.codigo", 1 },
                                { "id_especialidad", 1 }
                            });
            MedicoDTOEspcialidad medico = new MedicoDTOEspcialidad();
            medico = await _medicos.Aggregate()
                     .AppendStage<dynamic>(match)
                     .AppendStage<dynamic>(addfields)
                     .AppendStage<dynamic>(lookup)
                     .AppendStage<dynamic>(unwind)
                     .AppendStage<MedicoDTOEspcialidad>(project).FirstOrDefaultAsync();
            return medico;
        }
        public async Task<MedicoDTO3> GetMedicosAndDatosUsuario(string idMedico)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("_id",
                        new ObjectId(idMedico)));
            var addFields = new BsonDocument("$addFields",
                            new BsonDocument("id_usuario",
                            new BsonDocument("$toObjectId", "$id_usuario")));
            var lookup = new BsonDocument("$lookup",
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
                            { "preserveNullAndEmptyArrays", true }
                        });
            var project = new BsonDocument("$project",
                          new BsonDocument
                          {
                            { "id_usuario", 0 },
                            { "usuario._id", 0 },
                            { "usuario.usuario", 0 },
                            { "usuario.clave", 0 },
                            { "usuario.fecha_creacion", 0 },
                            { "usuario.rol", 0 },
                            { "usuario.estado", 0 },
                            { "turnos", 0 },
                            { "suscripcion", 0 }
                          });

            MedicoDTO3 medico = new MedicoDTO3();
            medico = await _medicos.Aggregate()
                     .AppendStage<dynamic>(match)
                     .AppendStage<dynamic>(addFields)
                     .AppendStage<dynamic>(lookup)
                     .AppendStage<dynamic>(unwind)
                     .AppendStage<MedicoDTO3>(project).FirstOrDefaultAsync();
            return medico;
        }
    }
}
