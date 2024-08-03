using System.Web.Mvc;
using Infraestructura.Model;
using Infraestructura.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using Web.Utils;
using ApplicationCore.Services;

namespace Web.Controllers
{
    public class AdministradorController : Controller
    {
        private ServiceAdministrador _repository = new ServiceAdministrador();

        // Method to populate roles dropdown
        private void PopulateRolesDropDownList(object selectedRole = null)
        {
            IEnumerable<Rol> rolesQuery = _repository.GetRoles();
            ViewBag.ID_Rol = new SelectList(rolesQuery, "ID_Rol", "Descripcion", selectedRole);
        }

        // GET: Administrador
        public ActionResult Index()
        {
            IEnumerable<Administrador> lista = null;
            try
            {
                lista = _repository.GetOperarios();

                ViewBag.title = "Lista Administradores";
                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Administrador";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: Administrador/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = _repository.GetOperarioByID(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // GET: Administrador/Create
        public ActionResult Create()
        {
            PopulateRolesDropDownList();
            return View();
        }

        // POST: Administrador/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Usuario,Contraseña,Correo,ID_Rol,Nombre")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Administrador oAdministrador = _repository.GetAdministrador(administrador.Usuario);
                    if (oAdministrador == null)
                    {
                        _repository.Save(administrador);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Log.Warn($"Intento de inicio: {administrador.Usuario}");
                        TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("", "El nombre de usuario "+ administrador.Usuario +" ya está en uso", Util.SweetAlertMessageType.error);
                    }


                     
                }
                catch (Exception ex)
                {
                    Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod());
                    TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                    TempData["Redirect"] = "Administrador";
                    TempData["Redirect-Action"] = "Create";
                    return RedirectToAction("Default", "Error");
                }
            }
            PopulateRolesDropDownList(administrador.ID_Rol);
            return View(administrador);
        }

        // GET: Administrador/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = _repository.GetOperarioByID(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            PopulateRolesDropDownList(administrador.ID_Rol);
            return View(administrador);
        }

        // POST: Administrador/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Administrador,Usuario,Contraseña,Correo,CodigoVerificacion,ID_Rol,Nombre")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Save(administrador);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod());
                    TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                    TempData["Redirect"] = "Administrador";
                    TempData["Redirect-Action"] = "Edit";
                    return RedirectToAction("Default", "Error");
                }
            }
            PopulateRolesDropDownList(administrador.ID_Rol);
            return View(administrador);
        }

        // GET: Administrador/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = _repository.GetOperarioByID(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // POST: Administrador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool result = _repository.Delete(id);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Administrador";
                TempData["Redirect-Action"] = "Delete";
                return RedirectToAction("Default", "Error");
            }
        }
    }
}
