using ApplicationCore.Services;
using Infraestructura.Model;
using OfficeOpenXml;
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

        [HttpPost]
        public ActionResult ImportarMiembros(HttpPostedFileBase file)
        {

            IServiceUsuario _ServiceUsuario = new ServiceUsuario();


            // Verificar si el archivo es válido
            if (file != null && file.ContentLength > 0)
            {

                // Leer el archivo
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    int rowCount = worksheet.Dimension.Rows;

                    // Iterar sobre las filas del archivo Excel
                    for (int row = 2; row <= rowCount; row++)
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
                }

                // Redirigir a la vista deseada
                return RedirectToAction("Index");
            }

            // Si el archivo no es válido, redirigir a la vista de error
            return RedirectToAction("Error");
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
