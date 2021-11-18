using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using SISFAHD.DTOs;

namespace SISFAHD.Services
{
    public class ActoMedicoService
    {
        private readonly IMongoCollection<ActoMedico> _actoMedico;
        private readonly IMongoCollection<Usuario> _usuario;
        private readonly IMongoCollection<Cita> _cita;
        private readonly IMongoCollection<Paciente> _paciente;
        private MedicoService medicoService;
        private OrdenesService ordenService;

        public ActoMedicoService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _actoMedico = database.GetCollection<ActoMedico>("acto_medico");
            _usuario = database.GetCollection<Usuario>("usuarios");
            _paciente = database.GetCollection<Paciente>("pacientes");
            _cita = database.GetCollection<Cita>("citas");
            medicoService = new MedicoService(settings);
            ordenService = new OrdenesService(settings);

        }

        public async Task<List<ActoMedico>> GetAll()
        {
           return await _actoMedico.Find(x => true).ToListAsync();
        }

        public async Task<ActoMedico> GetById(string id)
        {
            return await _actoMedico.Find(x => x.id == id).FirstOrDefaultAsync();
        }

        public async Task<ActoMedico> CrearActoMedico(ActoMedico actomedico)
        {
            actomedico.fecha_creacion = DateTime.Now;
            actomedico.fecha_atencion = DateTime.Now;
            await _actoMedico.InsertOneAsync(actomedico);
            return actomedico;
        }

        public async Task<ActoMedico> ModificarActoMedico(ActoMedicoDTO2 actomedico)
        {
            var filter = Builders<ActoMedico>.Filter.Eq("id", ObjectId.Parse(actomedico.acto_medico.id));
            var update = Builders<ActoMedico>.Update
                .Set("medicacion", actomedico.acto_medico.medicacion)
                .Set("diagnostico", actomedico.acto_medico.diagnostico)
                .Set("signos_vitales", actomedico.acto_medico.signos_vitales)
                .Set("anamnesis", actomedico.acto_medico.anamnesis)
                .Set("indicaciones", actomedico.acto_medico.indicaciones);
            await _actoMedico.UpdateOneAsync(filter, update);
            //Agregado por mi persona :)
            List<Ordenes> ordenes = await ordenService.VerifyOrdenesByActoMedicoAsync(actomedico.acto_medico.id);
            if (actomedico.acto_medico.diagnostico.Count > 0)
            {
                if (actomedico.acto_medico.diagnostico[0].examenes_auxiliares.Count > 0)
                {
                    List<Procedimientos> listPro = new List<Procedimientos>();
                    for (int i = 0; i < actomedico.acto_medico.diagnostico.Count; i++)
                    {
                        for (int j = 0; j < actomedico.acto_medico.diagnostico[i].examenes_auxiliares.Count; j++)
                        {
                            Procedimientos pro = new Procedimientos();
                            pro.id_examen = actomedico.acto_medico.diagnostico[i].examenes_auxiliares[j].codigo;
                            pro.estado = "no pagado";
                            pro.id_resultado_examen = "";
                            pro.id_turno_orden = "";
                            listPro.Add(pro);
                        }
                    }
                    Ordenes orden = new Ordenes();
                    orden.estado_atencion = "no atendido";
                    orden.estado_pago = "no reservado";
                    orden.fecha_orden = DateTime.Now;
                    orden.fecha_pago = null;
                    orden.fecha_reserva = null;
                    orden.id_paciente = actomedico.datos_orden.id_paciente;
                    orden.precio_neto = actomedico.datos_orden.precio_neto;
                    orden.tipo_pago = "";
                    orden.id_acto_medico = actomedico.acto_medico.id;
                    orden.id_medico_orden = actomedico.datos_orden.id_medico;
                    orden.procedimientos = listPro;

                    if (ordenes.Count > 0)
                    {
                        orden.id = ordenes[0].id;
                        await ordenService.ModificarOrdenes(orden);
                    }
                    else
                    {
                        ordenService.CreateOrdenes(orden);
                    }
                }
            }
            // hasta aca llega lo agregado por mi :)
            return actomedico.acto_medico;
        }

        public void sendNotificationDiagnostico(string idCita)
        {
            Cita c = new Cita();
            c = _cita.Find(cit => cit.id == idCita).FirstOrDefault();           
            Paciente p = new Paciente();
            p = _paciente.Find(pacient => pacient.id == c.id_paciente).FirstOrDefault();            
            Usuario objUsuario = new Usuario();
            objUsuario = _usuario.Find(user => user.id == p.id_usuario).FirstOrDefault();
            ActoMedico actoMedico= new ActoMedico();
            actoMedico= _actoMedico.Find(am => am.id == c.id_acto_medico).FirstOrDefault();

            Task<MedicoDTO3> medico;
            medico = medicoService.GetMedicosAndDatosUsuario(c.id_medico);
            Task<MedicoDTOEspcialidad> medicoEsp;
            medicoEsp = medicoService.GetMedicosAndEspecialidad(c.id_medico);

            List<Diagnostico> diagnosticos = new List<Diagnostico>();
            diagnosticos = actoMedico.diagnostico;
            List<Prescripcion> prescripciones = new List<Prescripcion>();
            List<ExamenAuxiliar> examenes = new List<ExamenAuxiliar>();

            string diagnostico = "";
            string datosAtencion = "";
            datosAtencion += "<p><b>Médico:</b> " + medico.Result.usuario.datos.nombre + " " + medico.Result.usuario.datos.apellido_paterno + "</p>";
            datosAtencion += "<p><b>Especialidad:</b> " + medicoEsp.Result.especialidad.nombre + "</p>";
            datosAtencion += "<p><b>Fecha de la atención:</b> " + c.fecha_cita.ToString() + "</p>";

            foreach (Diagnostico d in diagnosticos)
            {
                diagnostico += "<p><b>DIAGNÓSTICO</b></p>";
                diagnostico += "<p>" + d.codigo_enfermedad + "-" + d.nombre_enfermedad + "</p>";
                prescripciones = d.prescripcion;
                examenes = d.examenes_auxiliares;
                diagnostico += "<p><b>Prescripción</b></p>";
                for (int i = 0; i < prescripciones.Count; i++)
                {                                        
                    diagnostico += "<p>- " + prescripciones[i].nombre + " " + prescripciones[i].concentracion + " (" + prescripciones[i].formula + ") " +
                                   " - Tomar " + prescripciones[i].dosis.cantidad + " " + prescripciones[i].dosis.via_administracion +                                   
                                   " cada " + prescripciones[i].dosis.frecuencia.valor + " " + prescripciones[i].dosis.frecuencia.medida +
                                   " durante " + prescripciones[i].dosis.tiempo.valor + " " + prescripciones[i].dosis.tiempo.medida + "</p>";
                    
                };
                diagnostico += "<p><b>Exámenes auxiliares</b></p>";
                for (int i = 0; i < examenes.Count; i++)
                {
                    diagnostico += "<p>- " + examenes[i].nombre + " (" + examenes[i].tipo + ")" + "</p>";

                };
                diagnostico += "</br>";

            };

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            string Emisor = "sisfahdq@gmail.com";
            string EmisorPass = "sisf@hd12";
            string displayName = "SISFAHD";
            string Receptor = objUsuario.usuario;

            string htmlbody = "<body class='body' style='padding:0 !important; margin:0 !important; display:block !important; min-width:100% !important; width:100% !important; background:#001f51; -webkit-text-size-adjust:none;'>" +
                                "<table width = '100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#001f51'>" +
                                    "<tr>" +
                                        "<td align = 'center' valign='top'>" +
                                            "<table width = '650' border='0' cellspacing='0' cellpadding='0' class='mobile-shell'>" +
                                                "<tr>" +
                                                    "<td class='td container' style='width:650px; min-width:650px; font-size:0pt; line-height:0pt; margin:0; font-weight:normal; padding:55px 0px;'>" +
                                                        "<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
                                                            "<tr>" +
                                                                "<td class='p30-15 tbrr' style='padding: 30px; border-radius:12px 12px 0px 0px;' bgcolor='#ffffff'>" +
                                                                    "<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
                                                                        "<tr>" +
                                                                            "<th class='column-top' width='145' style='font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal; vertical-align:top;'>" +
                                                                                "<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
                                                                                    "<tr>" +
                                                                                        "<td class='img m-center' style='font-size:0pt; line-height:0pt; text-align:left;'><img src = 'https://i.ibb.co/C1DWyrk/logo-s.png' width='150' height='40' border='0' alt='' /></td>" +
                                                                                    "</tr>" +
                                                                                "</table>" +
                                                                            "</th>" +
                                                                            "<th class='column-empty2' width='1' style='font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal; vertical-align:top;'></th>" +
                                                                        "</tr>" +
                                                                    "</table>" +
                                                                "</td>" +
                                                            "</tr>" +
                                                        "</table>" +
                                                        //"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
                                                        //    "<tr>" +
                                                        //        "<td class='fluid-img' style='font-size:0pt; line-height:0pt; text-align:left;'><img src = 'https://i.ibb.co/k3L5pTX/undraw-doctor-kw5l.png' border='0' width='650' height='370' alt='' /></td>" +
                                                        //    "</tr>" +
                                                        //"</table>" +
                                                        "<table width = '100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#ffffff'>" +
                                                            "<tr>" +
                                                                "<td style = 'padding-bottom: 10px;' >" +
                                                                    "< table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                                                        "<tr>" +
                                                                            "<td class='p30-15' style='padding: 10px 30px;'>" +
                                                                                "<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
                                                                                    "<tr>" +
                                                                                        "<td class='h1 pb25' style='color:#444444; font-size:30px; line-height:42px; text-align:left; padding-bottom:25px;'>Atención realizada satisfactoriamente</td>" +                                                                                        
                                                                                    "</tr>" +
                                                                                    "<tr>" +
                                                                                        "<td class='text-center pb25' style='color:#666666; font-family:Arial,sans-serif; font-size:16px; line-height:30px; text-align:left; padding-bottom:25px;'>" +
                                                                                            datosAtencion + 
                                                                                            diagnostico + 
                                                                                        "</td>" +
                                                                                    "</tr>" +
                                                                                    //"<tr>" +
                                                                                    //    "<td align = 'center' >" +
                                                                                    //        "<table class='center' border='0' cellspacing='0' cellpadding='0' style='text-align:center;'>" +
                                                                                    //            "<tr>" +
                                                                                    //                "<td class='text-button' style='background:#ffda5c; color:#444444; font-size:14px; line-height:18px; padding:12px 15px; text-align:center; border-radius:10px; text-transform:uppercase;'><a href = '" + c.enlace_cita + "' target='_blank' class='link' style='color:#000001; text-decoration:none;'><span class='link' style='color:#000001; text-decoration:none;'>Ir a la consulta</span></a></td>" +
                                                                                    //            "</tr>" +
                                                                                    //        "</table>" +
                                                                                    //    "</td>" +
                                                                                    //"</tr>" +
                                                                                "</table>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                    "</table>" +
                                                                "</td>" +
                                                            "</tr>" +
                                                        "</table>" +
                                                        "<table width = '100%' border= '0' cellspacing= '0' cellpadding= '0' >" +
                                                              "<tr>" +
                                                                  "<td class='p30-15 bbrr' style='padding: 20px 30px; border-radius:0px 0px 12px 12px;' bgcolor='#ffffff'>" +
                                                                    "<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
                                                                        "<tr>" +
                                                                            "<td class='text-footer2' style='color:#999999; font-family:Arial,sans-serif; font-size:12px; line-height:26px; text-align:center;'>Este es un correo enviado automáticamente. No responder. Para consultas o dudas escribir a sisfahd@sisfahd.com</td>" +
                                                                        "</tr>" +
                                                                    "</table>" +
                                                                "</td>" +
                                                            "</tr>" +
                                                        "</table>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                            "</body>";
            MailMessage mail = new MailMessage();
            mail.Subject = "Atención de cita satisfactoria - SISFAHD";
            mail.From = new MailAddress(Emisor.Trim(), displayName);
            mail.Body = htmlbody;
            mail.To.Add(new MailAddress(Receptor));
            mail.IsBodyHtml = true;
            NetworkCredential nc = new NetworkCredential(Emisor, EmisorPass);
            smtp.Credentials = nc;
            smtp.Send(mail);
        }
    }
}
