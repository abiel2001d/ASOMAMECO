using ApplicationCore.Services;
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
using Web.Utils;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        private Proyecto_Calidad_SoftwareEntities db = new Proyecto_Calidad_SoftwareEntities();

        // GET: Usuario
        public ActionResult Index()
        {
            IEnumerable<Usuario> lista = null;
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

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Usuario,Nombre,Cedula,Estado_usuario,Estado_2,Correo,Telefono")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

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
                                Estado_2 = worksheet.Cells[row, 5].Value.ToString(),
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


        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario= db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Usuario,Nombre,Cedula,Estado_usuario,Estado_2,Correo,Telefono")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");  
            }
            return View(usuario);
        }

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

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            db.Usuario.Remove(usuario);
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

    }
}
