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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(Administrador administrador)
        {
            if (administrador == null || string.IsNullOrWhiteSpace(administrador.Usuario) || string.IsNullOrWhiteSpace(administrador.Contraseña))
            {
                Log.Warn($"Intento de inicio: {administrador?.Usuario}");
                TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Iniciar Sesión", "Datos incompletos", Util.SweetAlertMessageType.error);
                return View("Index");
            }

            IServiceAdministrador _serviceAdministrador = new ServiceAdministrador();
            try
            {
                // Clear the model state for these properties as they are not part of the form submission.
                ModelState.Remove("ID_Administrador");
                ModelState.Remove("Usuario");
                ModelState.Remove("Contraseña");

                if (ModelState.IsValid)
                {
                    var oAdministrador = _serviceAdministrador.GetAdministrador(administrador.Usuario);
                    if (oAdministrador != null)
                    {
                        if (oAdministrador.Contraseña == administrador.Contraseña)
                        {
                            Session["User"] = oAdministrador;
                            Log.Info($"Inicio sesion: {administrador.Usuario}");
                            TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Iniciar Sesión", "Bienvenido a ASOMAMECO", Util.SweetAlertMessageType.success);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            Log.Warn($"Intento de inicio: {administrador.Usuario}");
                            TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Iniciar Sesión", "Contraseña no válida", Util.SweetAlertMessageType.error);
                        }
                    }
                    else
                    {
                        Log.Warn($"Intento de inicio: {administrador.Usuario}");
                        TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Iniciar Sesión", "Usuario no válido", Util.SweetAlertMessageType.error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                return RedirectToAction("Default", "Error");
            }

            return View("Index");
        }
        public ActionResult Logout()
        {
            try
            {
                //Eliminar variable de sesion
                Log.Info($"Inicio cerrada: {((Administrador)Session["User"]).Usuario}");
                Session["User"] = null;
                Session.Remove("User");

                TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Sesión Cerrada",
                    "Gracias por visitar ASOMAMECO", Util.SweetAlertMessageType.success
                    );
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

    }
}


