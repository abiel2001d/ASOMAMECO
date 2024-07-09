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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre_Evento,Fecha_Evento,Descripcion,Lugar")] Evento evento)
        {
            if (ModelState.IsValid)
            {

                ServiceEvento serviceEvento = new ServiceEvento();
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
        public ActionResult Edit([Bind(Include = "ID_Evento,Nombre_Evento,Fecha_Evento,Descripcion,Lugar")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(evento);
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

        public async Task<ActionResult> EnviarInvitaciones(int id)
        {
            ServiceEvento serviceEvento = new ServiceEvento();
            ServiceInvitacion serviceInvitacion = new ServiceInvitacion();

            Evento evento = serviceEvento.GetEventoByID(id);
            if (evento == null)
            {
                return HttpNotFound();
            }

            await serviceInvitacion.EnviarInvitaciones(evento);

            var invitaciones = serviceInvitacion.GetInvitacionesByEvento(id)
                .Select(i => new
                {
                    Cedula = i.Usuario.Cedula,
                    Nombre = i.Usuario.Nombre,
                    Confirmado = i.Confirmado
                });

            return Json(new { success = true, invitaciones = invitaciones.ToList() });
        }


    }
}
