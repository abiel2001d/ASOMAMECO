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
            if (Request.Cookies["UserLogin"] != null)
            {
                ViewBag.Usuario = Request.Cookies["UserLogin"]["Usuario"];
                ViewBag.Contraseña = Request.Cookies["UserLogin"]["Contraseña"];
                ViewBag.RememberMe = true;
            }
            else
            {
                ViewBag.Usuario = string.Empty;
                ViewBag.Contraseña = string.Empty;
                ViewBag.RememberMe = false;
            }

            return View();
        }


        public ActionResult Login(Administrador administrador, bool RememberMe = false)
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
                    var oAdministrador = _serviceAdministrador.GetAdministrador(administrador.Usuario, administrador.Contraseña);
                    if (oAdministrador != null)
                    {
                        Session["User"] = oAdministrador;
                        Log.Info($"Inicio sesion: {administrador.Usuario}");

                        if (RememberMe)
                        {
                            HttpCookie cookie = new HttpCookie("UserLogin");
                            cookie.Values["Usuario"] = administrador.Usuario;
                            cookie.Values["Contraseña"] = administrador.Contraseña;
                            cookie.Values["RememberMe"] = "true";
                            cookie.Expires = DateTime.Now.AddDays(2);
                            Response.Cookies.Add(cookie);
                        }
                        else
                        {
                            // Remove the cookie if RememberMe is not checked
                            if (Request.Cookies["UserLogin"] != null)
                            {
                                HttpCookie cookie = new HttpCookie("UserLogin");
                                cookie.Expires = DateTime.Now.AddDays(-1);
                                Response.Cookies.Add(cookie);
                            }
                        }

                        TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Iniciar Sesión", "Bienvenido a ASOMAMECO", Util.SweetAlertMessageType.success);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Log.Warn($"Intento de inicio: {administrador.Usuario}");
                        TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Iniciar Sesión", "Usuario o contraseña incorrecta.", Util.SweetAlertMessageType.error);
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


