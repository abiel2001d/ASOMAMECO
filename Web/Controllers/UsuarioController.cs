using ApplicationCore.Services;
using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Utils;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            IEnumerable<Usuarios> lista = null;
            try
            {
                IServiceUsuario _ServiceUsuario = new ServiceUsuario();
                lista = _ServiceUsuario.GetUsuarios();
                ViewBag.title = "Lista Usuarios";
                return View(lista);


            }
            catch (Exception ex)
            {

                Log.Error(ex, MethodBase.GetCurrentMethod());
                //Mensaje de Error personalizado
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                //Botón Regresar
                //Controlador para redirección
                TempData["Redirect"] = "Usuario";
                //Acción para redirección
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");

            }
        }

      
        public ActionResult Login(FormCollection collection)
        {
            string usuario = collection["Usuario"];
            string contraseña = collection["Contraseña"];
            int idAdmin;

            if (ModelState.IsValid)
            {
                Administrador admin = null;

                // Check if input is an ID
                if (int.TryParse(usuario, out idAdmin))
                {
                  
                }

                // If not an ID, check as Usuario
                if (admin == null)
                {
                   
                }

                if (admin != null)
                {
                   
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View();
        }
    

    // GET: Usuario/Details/5
    public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
