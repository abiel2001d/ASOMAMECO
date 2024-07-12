using Infraestructura.Model;
using Infraestructura.Utils;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Infraestructura.Repository
{
    public class RepositoryInvitacion : IRepositoryInvitacion
    {

        public async Task<bool> EnviarInvitaciones(Evento evento, UrlHelper urlHelper)
{
    IEnumerable<Usuario> lista = null;
    try
    {
        using (MyContext ctx = new MyContext())
        {
            ctx.Configuration.LazyLoadingEnabled = false;
            lista = ctx.Usuario.Where(u => u.Estado_usuario == "Activo").ToList();
        }

        if (!lista.Any())
        {
            return false; // No users to send invitations to
        }

        foreach (var usuario in lista)
        {
            // Generate response URLs
            string yesUrl = urlHelper.Action("RespuestaInvitacion", "Evento", new { eventoId = evento.ID_Evento, usuarioId = usuario.Id_Usuario, respuesta = "Sí Asistirá" }, protocol: HttpContext.Current.Request.Url.Scheme);
            string noUrl = urlHelper.Action("RespuestaInvitacion", "Evento", new { eventoId = evento.ID_Evento, usuarioId = usuario.Id_Usuario, respuesta = "No Asistirá" }, protocol: HttpContext.Current.Request.Url.Scheme);

            // Build email subject and body
            string subject = $"Invitación al evento: {evento.Nombre_Evento}";
            string body = $@"
<html>
<head>
    <style>
        .btn {{
           display: inline-block;
    padding: 2px 20px;
    margin: 5px;
    text-decoration: none;
    border-radius: 30px;
    border: 1px solid;
        }}
        .btn-yes {{
            border-color: green;
        }}
        .btn-no {{
            border-color: red;
        }}
    </style>
</head>
<body>
    <div>
        <p>Saludos <strong>{usuario.Nombre}</strong>,<p>
        <p>Nos complace invitarle al evento <strong>{evento.Nombre_Evento}</strong>.</p>
        <p>Detalles del evento:</p>
        <ul>
            <li><strong>Fecha y Hora:</strong> {evento.Fecha_Evento.ToString("dd/MMMM/yyyy hh:mm tt")}</li>
            <li><strong>Lugar:</strong> {evento.Lugar}</li>
            <li><strong>Descripción:</strong> {evento.Descripcion}</li>
        </ul>
        <p>Esperamos contar con su presencia.</p>
        <p>Por favor, confirme su asistencia:</p>
        <p>
            <a href='{yesUrl}' class='btn btn-yes'>Sí Asistiré</a>
            <a href='{noUrl}' class='btn btn-no'>No Asistiré</a>
        </p>
        <p>Cordialmente,</p>
        <p>Eventos ASOMAMECO</p>
    </div>
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

        return true;
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

        public async Task ActualizarConfirmacion(int eventoId, int usuarioId, string respuesta)
        {
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    var invitacion = ctx.Invitacion.FirstOrDefault(i => i.ID_Evento == eventoId && i.ID_Usuario == usuarioId);
                    if (invitacion != null)
                    {
                        invitacion.Confirmado = respuesta;
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
