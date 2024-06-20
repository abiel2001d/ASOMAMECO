using Infraestructura.Model;
using Infraestructura.Utils;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repository
{
    public class RepositoryAdministrador : IRepositoryAdministrador
    {
        public Administrador GetAdministrador(string usuario, string contrasena)
        {
            Administrador oAdministrador = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    oAdministrador = ctx.Administrador.Where(x => x.Usuario == usuario && x.Contraseña == contrasena).Include("Rol").FirstOrDefault();

                }

                return oAdministrador;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public Administrador GetAdministrador(string usuario)
        {
            Administrador oAdministrador = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    oAdministrador = ctx.Administrador.Where(x => x.Usuario == usuario).Include("Rol").FirstOrDefault();

                }

                return oAdministrador;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public async Task EnviarCodigo(Administrador administrador)
        {
            try
            {
                // Generate a 6-digit verification code
                Random random = new Random();
                int verificationCode = random.Next(100000, 999999);

                // Update the Administrador entity with the verification code
                using (MyContext ctx = new MyContext())
                {
                    administrador.CodigoVerificacion = verificationCode;
                    ctx.Entry(administrador).State = EntityState.Modified;
                    ctx.SaveChanges();
                }

                // Create email content
                string subject = "Recuperación Contraseña - Código Verificación";
                string body = $@"
            <html>
            <body>
                <p>Hola {administrador.Usuario},</p>
                <p>Tu código de verificación es: <strong>{verificationCode}</strong>.</p>
                <p>Saludos,<br/>Equipo de Soporte ASOMAMECO</p>
            </body>
            </html>";

                // Send email
                Correo correo = new Correo(administrador.Correo, subject, body);
                await correo.EnviarCorreo();

                Console.WriteLine("Correo enviado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
            }
        }

        public bool RestablecerContrasena(Administrador administrador, int codigoIngresado, string nuevaContrasena)
        {
            try
            {
                using (MyContext ctx = new MyContext())
                {
                   
                   
                    if (administrador.CodigoVerificacion == codigoIngresado)
                    {
                        administrador.Contraseña = nuevaContrasena;
                        administrador.CodigoVerificacion = null; // Clear the verification code after successful update
                        ctx.Entry(administrador).State = EntityState.Modified;
                        ctx.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                   
                }
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        
        }
    }
}
