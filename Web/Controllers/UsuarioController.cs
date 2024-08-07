﻿using ApplicationCore.Services;
using Infraestructura.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        private Proyecto_Calidad_SoftwareEntities db = new Proyecto_Calidad_SoftwareEntities();
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Operario)]
        // GET: Usuario
        public ActionResult Index()
        {
            IEnumerable<Usuario> lista = null;
            try
            {
                IServiceUsuario _ServiceUsuario = new ServiceUsuario();
                lista = _ServiceUsuario.GetUsuarios();

                ViewBag.title = "Lista Usuarios";
                ViewBag.Estados = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Text = "Activo", Value = "Activo" },
            new SelectListItem { Text = "Inactivo", Value = "Inactivo" }
        }, "Value", "Text");
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

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Operario)]
        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Operario)]
        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Operario)]
        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Cedula,Estado_usuario,Correo,Telefono")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Operario)]
        [HttpPost]
        public ActionResult ImportarMiembros(HttpPostedFileBase file)
        {

            IServiceUsuario _ServiceUsuario = new ServiceUsuario();


            // Verificar si el archivo es válido
            if (file != null && file.ContentLength > 0)
            {
                //Codigo de la licencia del EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Leer el archivo
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    int rowCount = worksheet.Dimension.Rows;

                    // Iterar sobre las filas del archivo Excel
                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Verificar si todas las celdas necesarias son distintas de null
                        if (worksheet.Cells[row, 1].Value != null &&
                            worksheet.Cells[row, 2].Value != null &&
                            worksheet.Cells[row, 3].Value != null &&
                            worksheet.Cells[row, 4].Value != null &&
                            worksheet.Cells[row, 5].Value != null &&
                            worksheet.Cells[row, 6].Value != null &&
                            worksheet.Cells[row, 7].Value != null)
                        {
                            // Crear un nuevo miembro y llenar sus propiedades con los datos del archivo
                            Usuario miembro = new Usuario
                            {
                                Id_Usuario = int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                                Nombre = worksheet.Cells[row, 2].Value.ToString(),
                                Cedula = worksheet.Cells[row, 3].Value.ToString(),
                                Estado_usuario = worksheet.Cells[row, 4].Value.ToString(),
                                Correo = worksheet.Cells[row, 6].Value.ToString(),
                                Telefono = worksheet.Cells[row, 7].Value.ToString()
                            };

                            // Añadir el miembro a la BD
                            _ServiceUsuario.Save(miembro);
                        }
                        else
                        {
                            // Si alguna celda es null, terminar el bucle
                            break;
                        }
                    }
                }

                // Redirigir a la vista deseada
                return RedirectToAction("Index");
            }

            // Si el archivo no es válido, redirigir a la vista de error
            return RedirectToAction("Error");
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Operario)]

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Operario)]
        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Usuario,Nombre,Cedula,Estado_usuario,Correo,Telefono")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Operario)]
        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Operario)]
        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                IServiceUsuario _ServiceUsuario = new ServiceUsuario();
                bool result = _ServiceUsuario.Delete(id);
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
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Usuario";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
