using Infraestructura.Model;
using Infraestructura.Utils;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repository
{
    public class RepositoryInvitacion : IRepositoryInvitacion
    {

        public async Task EnviarInvitaciones(Evento evento)
        {
            IEnumerable<Usuario> lista = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.Usuario.Where(u => u.Estado_usuario == "Activo").ToList();
                }

                foreach (var usuario in lista)
                {
                    // Build email subject and body
                    string subject = $"Invitación al evento: {evento.Nombre_Evento}";
                    string body = $@"
            <html>
            <body>
                <h2>Estimado/a {usuario.Nombre},</h2>
                <p>Nos complace invitarle al evento <strong>{evento.Nombre_Evento}</strong>.</p>
                <p>Detalles del evento:</p>
                <ul>
                    <li><strong>Fecha:</strong> {evento.Fecha_Evento}</li>
                    <li><strong>Lugar:</strong> {evento.Lugar}</li>
                    <li><strong>Descripción:</strong> {evento.Descripcion}</li>
                </ul>
                <p>Esperamos contar con su presencia.</p>
                <p>Saludos,</p>
                <p>El equipo de ASOMAMECO</p>
            </body>
            </html>";

                    // Send email
                    var correo = new Correo(usuario.Correo, subject, body);
                    await correo.EnviarCorreo();

                    // Insert invitation into the database
                    using (MyContext ctx = new MyContext())
                    {
                        ctx.Invitacion.Add(new Invitacion
                        {
                            ID_Usuario = usuario.Id_Usuario,
                            ID_Evento = evento.ID_Evento,
                            Confirmado = "Enviado sin respuesta aún"
                        });

                        await ctx.SaveChangesAsync();
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




        public IEnumerable<Invitacion> GetInvitacionesByEvento(int id_evento)
        {
            IEnumerable<Invitacion> lista = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.Invitacion.Include("Usuario").Where(x => x.ID_Evento == id_evento).ToList();
                }
                return lista;
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
