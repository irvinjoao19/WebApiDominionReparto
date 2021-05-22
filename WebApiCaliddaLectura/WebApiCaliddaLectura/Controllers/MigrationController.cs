using Entidades;
using Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApiFenosa.Controllers
{
    [RoutePrefix("api/Migration")]
    public class MigrationController : ApiController
    {
        private static string path = ConfigurationManager.AppSettings["uploadFile"];


        [HttpGet]
        [Route("GetLogin")]
        public IHttpActionResult GetLogin(string user, string password, string version, string imei, string token)
        {
            Login login = MigrationDA.GetOne(user, password, version, imei, token);

            if (login != null)
            {
                return Ok(login);
            }
            else return NotFound();

        }

        [HttpGet]
        [Route("MigracionAll")]
        public IHttpActionResult MigracionAll(int operarioId, string version)
        {
            Migracion migracion = MigrationDA.GetMigracion(operarioId, version);
            if (migracion != null)
            {
                return Ok(migracion);
            }
            else
                return BadRequest("Actualizar Versión del App");

        }

        [HttpPost]
        [Route("SaveOperarioGps")]
        public IHttpActionResult SaveOperarioGps(EstadoOperario estadoOperario)
        {
            Mensaje mensaje = MigrationDA.saveOperarioGps(estadoOperario);
            return Ok(mensaje);
        }

        [HttpPost]
        [Route("SaveGpsOperario")]
        public IHttpActionResult SaveGpsOperario(EstadoOperario estadoOperario)
        {
            Mensaje m = MigrationDA.SaveGpsOperario(estadoOperario);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error de envio");
        }

        [HttpPost]
        [Route("SaveEstadoMovil")]
        public IHttpActionResult SaveEstadoMovil(EstadoMovil estadoMovil)
        {
            Mensaje mensaje = MigrationDA.saveEstadoMovil(estadoMovil);
            return Ok(mensaje);
        }

        [HttpPost]
        [Route("SaveNew")]
        public IHttpActionResult SaveRegistroMasivoNew()
        {
            try
            {
                //string path = HttpContext.Current.Server.MapPath("~/Imagen/");
                //string path = "C:/HostingSpaces/admincobraperu/www.cobraperu.com/wwwroot/Calidda/Content/foto/foto/";
                var fotos = HttpContext.Current.Request.Files;
                var json = HttpContext.Current.Request.Form["model"];
                Registro p = JsonConvert.DeserializeObject<Registro>(json);

                Mensaje mensaje = MigrationDA.SaveRegistroRxNew(p);

                if (mensaje != null)
                {
                    for (int i = 0; i < fotos.Count; i++)
                    {
                        string fileName = Path.GetFileName(fotos[i].FileName);
                        fotos[i].SaveAs(path + fileName);
                    }
                }
                else
                {
                    mensaje = new Mensaje();
                    mensaje.mensaje = "Registro repetido";
                }

                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveRegistro")]
        public IHttpActionResult SaveRegistro(Registro p)
        {
            Mensaje m = MigrationDA.SaveRegistroRxNew(p);

            if (m != null)
            {
                return Ok(m);
            }
            else return BadRequest("Error");
        }


        [HttpPost]
        [Route("SavePhoto")]
        public IHttpActionResult Photos()
        {
            try
            {
                var fotos = HttpContext.Current.Request.Files;

                for (int i = 0; i < fotos.Count; i++)
                {
                    string fileName = Path.GetFileName(fotos[i].FileName);
                    fotos[i].SaveAs(path + fileName);
                }

                return Ok("ok");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public async Task Email(int id, Inspeccion i, List<Email> emails)
        {
            try
            {
                //string path;
                //string pathExist;
                //Mensaje mensaje = new Mensaje();
                //pathExist = string.Format("{0}/{1}", System.Web.Hosting.HostingEnvironment.MapPath("~/Pdf"), pdf);
                //if (File.Exists(pathExist))
                //{
                //    File.Delete(pathExist);
                //} 

                var body = "Implemento en mal estado :";

                if (i.detalles != null)
                {
                    foreach (var d in i.detalles)
                    {
                        if (d.estadoCheckList == 2 || d.estadoCheckList == 4)
                        {
                            body += "<p>" + d.descripcion + "</p>";
                        }
                    }
                }

                var message = new MailMessage();

                foreach (var e in emails)
                {
                    message.To.Add(new MailAddress(e.email));
                }

                //var to = "irvin.dsige@gmail.com;david.dsige@gmail.com";                
                //foreach (var curr_address in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                //{
                //    message.To.Add(new MailAddress(curr_address));
                //}

                message.From = new MailAddress("cobrainspecciones@gmail.com");
                message.Subject = "Nro Inspeccion: " + id + " , " + i.nombreOperario + ", " + i.nombreArea + ", " + i.fecha;
                message.Body = body;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.Normal;
                // path = string.Format("{0}/{1}", System.Web.Hosting.HostingEnvironment.MapPath("~/Pdf"), pdf);
                //Attachment attachment = new Attachment(path, MediaTypeNames.Application.Octet);
                //attachment.Name = "Inspeccion.pdf";
                //ContentDisposition disposition = attachment.ContentDisposition;
                //disposition.CreationDate = File.GetCreationTime(path);
                //disposition.ModificationDate = File.GetLastWriteTime(path);
                //disposition.ReadDate = File.GetLastAccessTime(path);
                //message.Attachments.Add(attachment);

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential("cobrainspecciones@gmail.com", "A.123456");
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("SaveReparto")]
        public IHttpActionResult SaveReparto()
        {
            try
            {
                //string path = HttpContext.Current.Server.MapPath("~/Imagen/");
                //string path = "C:/HostingSpaces/admincobraperu/www.cobraperu.com/wwwroot/Calidda/Content/foto/foto/";
                var fotos = HttpContext.Current.Request.Files;
                var json = HttpContext.Current.Request.Form["model"];
                RepartoCargo p = JsonConvert.DeserializeObject<RepartoCargo>(json);

                Mensaje mensaje = MigrationDA.SaveReparto(p);

                if (mensaje != null)
                {
                    for (int i = 0; i < fotos.Count; i++)
                    {
                        string fileName = Path.GetFileName(fotos[i].FileName);
                        fotos[i].SaveAs(path + fileName);
                    }
                }
                else
                {
                    mensaje = new Mensaje();
                    mensaje.mensaje = "Registro repetido";
                }

                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
