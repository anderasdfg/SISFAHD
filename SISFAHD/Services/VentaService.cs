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
    public class VentaService
    {
        private readonly IMongoCollection<Venta> _venta;
        private readonly IMongoCollection<Cita> _cita;
        private readonly CitaService _citaservice;

        public VentaService(ISisfahdDatabaseSettings settings, CitaService citaservice)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _venta = database.GetCollection<Venta>("ventas");
            _cita = database.GetCollection<Cita>("citas");
            _citaservice = citaservice;
        }

        public async Task<List<VentaDTO>> GetAllVentas()
        {
            List<VentaDTO> PagoDTO = new List<VentaDTO>();

            //Transformar codigo_referencia en un toObjectId
            var addfields1 = new BsonDocument("$addFields",
                                new BsonDocument("id_cita",
                                new BsonDocument("$toObjectId", "$codigo_referencia")));

            //Traer los datos de la entidad citas a la de ventas
            var lookup1 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_cita" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_cita" }
                                });

            //Transformar array datos_cita en un object
            var unwind1 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_cita" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_cita.id_paciente en toObjectId
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("id_paciente_pro",
                                new BsonDocument("$toObjectId", "$datos_cita.id_paciente")));

            //Traer los datos de la entidad pacientes a traves del toObjectId id_paciente_pro
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "pacientes" },
                                    { "localField", "id_paciente_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_usuario" }
                                });

            //Transformar array datos_usuario en un object
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_usuario" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_usuario.id_usuario en toObjectId
            var addfields3 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuariopro",
                                new BsonDocument("$toObjectId", "$datos_usuario.id_usuario")));

            //Traer los datos de la entidad usuarios a traves del toObjectId id_usuariopro
            var lookup3 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuariopro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente" }
                                });

            //Transformar array datos_paciente en un object
            var unwind3 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_paciente.rol en toObjectId
            var addfields4 = new BsonDocument("$addFields",
                                new BsonDocument("id_rol_pro",
                                new BsonDocument("$toObjectId", "$datos_paciente.rol")));

            //Traer los datos de la entidad roles a traves del toObjectId id_rol_pro
            var lookup4 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "roles" },
                                    { "localField", "id_rol_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente.nombre_rol" }
                                });

            //Transformar array datos_paciente.nombre_rol en un object
            var unwind4 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente.nombre_rol" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Juntar nombre-apellido paterno-apellido materno del paciente
            var addfields5 = new BsonDocument("$addFields",
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

            //Transformar datos_cita.id_turno en toObjectId
            var addfields6 = new BsonDocument("$addFields",
                                new BsonDocument("id_turno_pro",
                                new BsonDocument("$toObjectId", "$datos_cita.id_turno")));

            //Traer los datos de la entidad turnos a traves del toObjectId id_turno_pro
            var lookup5 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "turnos" },
                                    { "localField", "id_turno_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_turno" }
                                });

            //Transformar array datos_turno en un object
            var unwind5 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_turno.id_medico en toObjectId
            var addfields7 = new BsonDocument("$addFields",
                                new BsonDocument("id_medico_pro",
                                new BsonDocument("$toObjectId", "$datos_turno.id_medico")));

            //Traer los datos de la entidad medicos a traves del toObjectId id_medico_pro
            var lookup6 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "medicos" },
                                    { "localField", "id_medico_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_medico" }
                                });

            //Transformar array datos_medico en un object
            var unwind6 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_medico.id_usuario en toObjectId
            var addfields8 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuario_medico",
                                new BsonDocument("$toObjectId", "$datos_medico.id_usuario")));

            //Traer los datos de la entidad usuarios a traves del toObjectId id_usuario_medico
            var lookup7 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuario_medico" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_turno.datos_medico" }
                                });

            //Transformar array datos_turno.datos_medico en un object
            var unwind7 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno.datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Juntar nombre-apellido paterno-apellido materno del médico
            var addfields9 = new BsonDocument("$addFields",
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

            //Mapeo final para DTO
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 1 },
                                    { "codigo_orden", 1 },
                                    { "estado", 1 },
                                    { "detalle_estado", 1 },
                                    { "tipo_operacion", 1 },
                                    { "tipo_pago", 1 },
                                    { "monto", 1 },
                                    { "titular", 1 },
                                    { "fecha_pago", 1 },
                                    { "moneda", 1 },
                                    { "codigo_referencia", 1 },
                                    { "datos_cita",
                            new BsonDocument
                                    {
                                        { "estado_atencion", 1 },
                                        { "estado_pago", 1 },
                                        { "fecha_cita", 1 },
                                        { "fecha_pago", 1 },
                                        { "id_paciente", 1 },
                                        { "precio_neto", 1 },
                                        { "tipo_pago", 1 }
                                    } },
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
            PagoDTO = await _venta.Aggregate()
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
                                .AppendStage<dynamic>(lookup4)
                                .AppendStage<dynamic>(unwind4)
                                .AppendStage<dynamic>(addfields5)
                                .AppendStage<dynamic>(addfields6)
                                .AppendStage<dynamic>(lookup5)
                                .AppendStage<dynamic>(unwind5)
                                .AppendStage<dynamic>(addfields7)
                                .AppendStage<dynamic>(lookup6)
                                .AppendStage<dynamic>(unwind6)
                                .AppendStage<dynamic>(addfields8)
                                .AppendStage<dynamic>(lookup7)
                                .AppendStage<dynamic>(unwind7)
                                .AppendStage<dynamic>(addfields9)
                                .AppendStage<VentaDTO>(project)
                                .ToListAsync();
            return PagoDTO;
        }
        //BUSQUEDA POR ID
        public async Task<VentaDTO> GetById(string id)
        {
            VentaDTO PagoDTO = new VentaDTO();

            //Transformar codigo_referencia en un toObjectId
            var addfields1 = new BsonDocument("$addFields",
                                new BsonDocument("id_cita",
                                new BsonDocument("$toObjectId", "$codigo_referencia")));

            //Traer los datos de la entidad citas a la de ventas
            var lookup1 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "citas" },
                                    { "localField", "id_cita" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_cita" }
                                });

            //Transformar array datos_cita en un object
            var unwind1 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_cita" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_cita.id_paciente en toObjectId
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument("id_paciente_pro",
                                new BsonDocument("$toObjectId", "$datos_cita.id_paciente")));

            //Traer los datos de la entidad pacientes a traves del toObjectId id_paciente_pro
            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "pacientes" },
                                    { "localField", "id_paciente_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_usuario" }
                                });

            //Transformar array datos_usuario en un object
            var unwind2 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_usuario" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_usuario.id_usuario en toObjectId
            var addfields3 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuariopro",
                                new BsonDocument("$toObjectId", "$datos_usuario.id_usuario")));

            //Traer los datos de la entidad usuarios a traves del toObjectId id_usuariopro
            var lookup3 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuariopro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente" }
                                });

            //Transformar array datos_paciente en un object
            var unwind3 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_paciente.rol en toObjectId
            var addfields4 = new BsonDocument("$addFields",
                                new BsonDocument("id_rol_pro",
                                new BsonDocument("$toObjectId", "$datos_paciente.rol")));

            //Traer los datos de la entidad roles a traves del toObjectId id_rol_pro
            var lookup4 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "roles" },
                                    { "localField", "id_rol_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_paciente.nombre_rol" }
                                });

            //Transformar array datos_paciente.nombre_rol en un object
            var unwind4 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_paciente.nombre_rol" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Juntar nombre-apellido paterno-apellido materno del paciente
            var addfields5 = new BsonDocument("$addFields",
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

            //Transformar datos_cita.id_turno en toObjectId
            var addfields6 = new BsonDocument("$addFields",
                                new BsonDocument("id_turno_pro",
                                new BsonDocument("$toObjectId", "$datos_cita.id_turno")));

            //Traer los datos de la entidad turnos a traves del toObjectId id_turno_pro
            var lookup5 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "turnos" },
                                    { "localField", "id_turno_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_turno" }
                                });

            //Transformar array datos_turno en un object
            var unwind5 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_turno.id_medico en toObjectId
            var addfields7 = new BsonDocument("$addFields",
                                new BsonDocument("id_medico_pro",
                                new BsonDocument("$toObjectId", "$datos_turno.id_medico")));

            //Traer los datos de la entidad medicos a traves del toObjectId id_medico_pro
            var lookup6 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "medicos" },
                                    { "localField", "id_medico_pro" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_medico" }
                                });

            //Transformar array datos_medico en un object
            var unwind6 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Transformar datos_medico.id_usuario en toObjectId
            var addfields8 = new BsonDocument("$addFields",
                                new BsonDocument("id_usuario_medico",
                                new BsonDocument("$toObjectId", "$datos_medico.id_usuario")));

            //Traer los datos de la entidad usuarios a traves del toObjectId id_usuario_medico
            var lookup7 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "usuarios" },
                                    { "localField", "id_usuario_medico" },
                                    { "foreignField", "_id" },
                                    { "as", "datos_turno.datos_medico" }
                                });

            //Transformar array datos_turno.datos_medico en un object
            var unwind7 = new BsonDocument("$unwind",
                            new BsonDocument
                                {
                                    { "path", "$datos_turno.datos_medico" },
                                    { "preserveNullAndEmptyArrays", true }
                                });

            //Juntar nombre-apellido paterno-apellido materno del médico
            var addfields9 = new BsonDocument("$addFields",
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

            //Mapeo final para DTO
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 1 },
                                    { "codigo_orden", 1 },
                                    { "estado", 1 },
                                    { "detalle_estado", 1 },
                                    { "tipo_operacion", 1 },
                                    { "tipo_pago", 1 },
                                    { "monto", 1 },
                                    { "titular", 1 },
                                    { "fecha_pago", 1 },
                                    { "moneda", 1 },
                                    { "codigo_referencia", 1 },
                                    { "datos_cita",
                            new BsonDocument
                                    {
                                        { "estado_atencion", 1 },
                                        { "estado_pago", 1 },
                                        { "fecha_cita", 1 },
                                        { "fecha_pago", 1 },
                                        { "id_paciente", 1 },
                                        { "precio_neto", 1 },
                                        { "tipo_pago", 1 }
                                    } },
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
            var match = new BsonDocument("$match",
                        new BsonDocument("_id",
                        new ObjectId(id)));
            PagoDTO = await _venta.Aggregate()
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
                                .AppendStage<dynamic>(lookup4)
                                .AppendStage<dynamic>(unwind4)
                                .AppendStage<dynamic>(addfields5)
                                .AppendStage<dynamic>(addfields6)
                                .AppendStage<dynamic>(lookup5)
                                .AppendStage<dynamic>(unwind5)
                                .AppendStage<dynamic>(addfields7)
                                .AppendStage<dynamic>(lookup6)
                                .AppendStage<dynamic>(unwind6)
                                .AppendStage<dynamic>(addfields8)
                                .AppendStage<dynamic>(lookup7)
                                .AppendStage<dynamic>(unwind7)
                                .AppendStage<dynamic>(addfields9)
                                .AppendStage<dynamic>(project)
                                .AppendStage<VentaDTO>(match)
                                .FirstAsync();
            return PagoDTO;
        }
        public async Task<Venta> CreateUnNuevoPagoRealizado(Venta pagorealizado)
        {
            _venta.InsertOne(pagorealizado);
            Cita cita = _citaservice.GetById(pagorealizado.codigo_referencia);

            var filter = Builders<Cita>.Filter.Eq("id", cita.id);
            var update = Builders<Cita>.Update.Set("estado_pago", cita.estado_pago);

            cita = _cita.FindOneAndUpdate<Cita>(filter, update, new FindOneAndUpdateOptions<Cita>
            {
                ReturnDocument = ReturnDocument.After
            });
            return pagorealizado;
        }
    }
}
