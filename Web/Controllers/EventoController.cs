using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationCore;
using ApplicationCore.Services;
using Infraestructura.Model;

namespace Web.Controllers
{
    public class EventoController : Controller
    {
        private Proyecto_Calidad_SoftwareEntities db = new Proyecto_Calidad_SoftwareEntities();

        // GET: Evento
        public ActionResult Index()
        {

            ServiceEvento serviceEvento = new ServiceEvento();
      
            return View(serviceEvento.GetEventos());
        }

        // GET: Evento/Details/5
        public ActionResult Details(int id)
        {
          

            ServiceEvento serviceEvento = new ServiceEvento();

            Evento evento = serviceEvento.GetEventoByID(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // GET: Evento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Evento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] // se agrego el timeperiod y horaevento para poder sacar el calculo am/pm
        public ActionResult Create([Bind(Include = "Nombre_Evento,Fecha_Evento,Descripcion,Lugar,timePeriod,Hora_Evento")] Evento evento)
        {
            if (ModelState.IsValid)
            {

                ServiceEvento serviceEvento = new ServiceEvento();
                //aqui empieza el nuevo codigo para la fecha y la hora
                DateTime fechaEvento = evento.Fecha_Evento;
                string horaEvento = Request.Form["Hora_Evento"];
                string timePeriod = Request.Form["timePeriod"];

                // Combina la fecha y la hora en un DateTime completo
                DateTime fechaHoraEvento = DateTime.ParseExact(fechaEvento.ToString("yyyy-MM-dd") + " " + horaEvento, "yyyy-MM-dd HH:mm", null);

                // Ajusta según AM/PM
                if (timePeriod == "PM" && fechaHoraEvento.Hour < 12)
                {
                    fechaHoraEvento = fechaHoraEvento.AddHours(12);
                }
                else if (timePeriod == "AM" && fechaHoraEvento.Hour == 12)
                {
                    fechaHoraEvento = fechaHoraEvento.AddHours(-12);
                }

                evento.Fecha_Evento = fechaHoraEvento;
                //aqui termina el codigo de la fecha y hora
                serviceEvento.Save(evento);
                return RedirectToAction("Index");
            }

            return View(evento);
        }

        // GET: Evento/Edit/5
        public ActionResult Edit(int id)
        {

            ServiceEvento serviceEvento = new ServiceEvento();

            Evento evento = serviceEvento.GetEventoByID(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Evento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Evento,Nombre_Evento,Fecha_Evento,Descripcion,Lugar,timePeriod,Estado")] Evento evento, string timePeriod, string Hora_Evento)
        {
            if (ModelState.IsValid)
            {
                // Combina la fecha y la hora
                DateTime fechaEvento = evento.Fecha_Evento.Date;
                TimeSpan horaEvento = TimeSpan.Parse(Hora_Evento);
                fechaEvento = fechaEvento.Add(horaEvento);

                // Ajusta la hora según AM/PM
                if (timePeriod == "PM" && fechaEvento.Hour < 12)
                {
                    fechaEvento = fechaEvento.AddHours(12);
                }
                else if (timePeriod == "AM" && fechaEvento.Hour == 12)
                {
                    fechaEvento = fechaEvento.AddHours(-12);
                }

                // Asigna el valor ajustado al modelo
                evento.Fecha_Evento = fechaEvento;
                evento.Estado = true;
                // Guardar los cambios en la base de datos
                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(evento);
        }




        public ActionResult Asistencias(int id, string nombre = "", string cedula = "", string estado = "")
        {
            ServiceAsistencia serviceAsistencia = new ServiceAsistencia();
            ServiceInvitacion serviceInvitacion = new ServiceInvitacion();

            var asistencias = serviceAsistencia.GetAsistenciasByEvento(id);
            var invitaciones = serviceInvitacion.GetInvitacionesByEvento(id);

            if (!string.IsNullOrEmpty(nombre))
            {
                invitaciones = invitaciones.Where(i => i.Usuario.Nombre.ToLower().Contains(nombre.ToLower()));
            }

            if (!string.IsNullOrEmpty(cedula))
            {
                invitaciones = invitaciones.Where(i => i.Usuario.Cedula.ToLower().Contains(cedula.ToLower()));
            }

            if (!string.IsNullOrEmpty(estado))
            {
                if (estado == "Presente")
                {
                    invitaciones = invitaciones.Where(i => asistencias.Any(a => a.ID_Usuario == i.ID_Usuario && a.Presente == "Presente"));
                }
                else if (estado == "No Presente")
                {
                    invitaciones = invitaciones.Where(i => !asistencias.Any(a => a.ID_Usuario == i.ID_Usuario && a.Presente == "Presente"));
                }
            }

            ViewBag.Asistencias = asistencias;
            ViewBag.Invitaciones = invitaciones;

            var evento = invitaciones.FirstOrDefault()?.Evento ?? new Evento();
            return View(evento);
        }



        [HttpPost]
        public JsonResult MarcarAsistencia(int idUsuario, int idEvento)
        {
            ServiceAsistencia serviceAsistencia = new ServiceAsistencia();
            var result = serviceAsistencia.MarcarAsistencia(idUsuario, idEvento);
            if (result != null)
            {
                var asistenciaDto = new
                {
                    Usuario = new
                    {
                        Cedula = result.Usuario.Cedula,
                        Nombre = result.Usuario.Nombre,
                        Correo = result.Usuario.Correo
                    },
                    Presente = result.Presente,
                    Hora_Asistencia = DateTime.Today.Add(result.Hora_Asistencia).ToString("hh:mm tt")
                };

                return Json(new { success = true, asistencia = asistenciaDto });
            }
            return Json(new { success = false, message = "Error al marcar asistencia" });
        }



        // GET: Evento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Evento.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Evento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evento evento = db.Evento.Find(id);
            db.Evento.Remove(evento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // GET: Evento/Invitaciones/5
        public ActionResult Invitaciones(int id)
        {
            ServiceEvento serviceEvento = new ServiceEvento();
            ServiceInvitacion serviceInvitacion = new ServiceInvitacion();

            Evento evento = serviceEvento.GetEventoByID(id);
            if (evento == null)
            {
                return HttpNotFound();
            }

            var invitaciones = serviceInvitacion.GetInvitacionesByEvento(id);
            ViewBag.Invitaciones = invitaciones;

            return View(evento);
        }

        [HttpGet]
        public async Task<ActionResult> RespuestaInvitacion(int eventoId, int usuarioId, string respuesta)
        {
            try
            {
                ServiceInvitacion serviceInvitacion = new ServiceInvitacion();
                await serviceInvitacion.ActualizarConfirmacion(eventoId, usuarioId, respuesta);
                return View("RespuestaInvitacion");
            }
            catch (Exception ex)
            {
                // Optionally handle the error in a user-friendly way
                ViewBag.ErrorMessage = "Hubo un error al registrar tu respuesta. Por favor, intenta nuevamente.";
                return View("Error");
            }
        }

   
        [HttpPost]
        public async Task<ActionResult> EnviarInvitaciones(int id)
        {
            ServiceEvento serviceEvento = new ServiceEvento();
            ServiceInvitacion serviceInvitacion = new ServiceInvitacion();

            Evento evento = serviceEvento.GetEventoByID(id);
            if (evento == null)
            {
                return HttpNotFound();
            }

            UrlHelper urlHelper = new UrlHelper(Request.RequestContext);
            bool invitationsSent =  await serviceInvitacion.EnviarInvitaciones(evento, urlHelper);

            if (!invitationsSent)
            {
                return Json(new { success = false, message = "No hay usuarios activos para enviar invitaciones." });
            }

            var invitaciones = serviceInvitacion.GetInvitacionesByEvento(id)
                .Select(i => new
                {
                    Cedula = i.Usuario.Cedula,
                    Nombre = i.Usuario.Nombre,
                    Correo = i.Usuario.Correo,
                    Confirmado = i.Confirmado
                });

            return Json(new { success = true, invitaciones = invitaciones.ToList() });
        }




    }
}
