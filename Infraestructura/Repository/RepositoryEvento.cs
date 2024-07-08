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
    public class RepositoryEvento : IRepositoryEvento
    {
        public IEnumerable<Evento> GetEventos()
        {
             IEnumerable<Evento> lista = null;
        try{
                using(MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.Evento.ToList();
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

        public Evento Save(Evento evento)
        {
            int retorno = 0;
            Evento oEvento = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oEvento = GetEventoByID((int)evento.ID_Evento); 


                if (oEvento == null)
                {

                    //Insertar Evento
                    ctx.Evento.Add(evento);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                    //retorna número de filas afectadas
                }
                else
                {
                    //Actualizar Evento
                    ctx.Evento.Add(evento);
                   
                    retorno = ctx.SaveChanges();
                }
            }

            if (retorno >= 0)
                oEvento = GetEventoByID((int)evento.ID_Evento);

            return oEvento;
        }


        public Evento GetEventoByID(int id)
        {
            Evento oEvento = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oEvento = ctx.Evento.
                        Where(x => x.ID_Evento == id).
                        
                        FirstOrDefault();

                }
                return oEvento;
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
