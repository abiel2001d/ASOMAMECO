using Infraestructura.Model;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repository
{
    public class RepositoryAsistencia : IRepositoryAsistencia
    {
        
        public IEnumerable<Asistencia> GetAsistenciasByEvento(int idEvento)
        {
            IEnumerable<Asistencia> lista = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.Asistencia.Include("Usuario").Include("Evento").Where(a => a.ID_Evento == idEvento).ToList();

                    var invitaciones = ctx.Invitacion.Include("Usuario").Where(i => i.ID_Evento == idEvento).ToList();

                    foreach (var invitacion in invitaciones)
                    {
                        if (!lista.Any(a => a.ID_Usuario == invitacion.ID_Usuario))
                        {
                            lista = lista.Append(new Asistencia
                            {
                                ID_Usuario = invitacion.ID_Usuario,
                                ID_Evento = invitacion.ID_Evento,
                                Usuario = invitacion.Usuario,
                                Presente = "No"
                            });
                        }
                    }
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

        public Asistencia MarcarAsistencia(int idUsuario, int idEvento)
        {
            Asistencia asistencia = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    asistencia = ctx.Asistencia
                        .Include("Usuario") // Eagerly load the Usuario entity
                        .Where(a => a.ID_Usuario == idUsuario && a.ID_Evento == idEvento)
                        .FirstOrDefault();

                    if (asistencia != null)
                    {
                        asistencia.Presente = "Presente";
                        asistencia.Fecha_Asistencia = DateTime.Now;
                        asistencia.Hora_Asistencia = DateTime.Now.TimeOfDay;
                    }
                    else
                    {
                        asistencia = new Asistencia
                        {
                            ID_Usuario = idUsuario,
                            ID_Evento = idEvento,
                            Fecha_Asistencia = DateTime.Now,
                            Hora_Asistencia = DateTime.Now.TimeOfDay,
                            Presente = "Presente",
                            Usuario = ctx.Usuario.Find(idUsuario) // Include the Usuario entity
                        };
                        ctx.Asistencia.Add(asistencia);
                    }
                    ctx.SaveChanges();
                }
                return asistencia;
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
