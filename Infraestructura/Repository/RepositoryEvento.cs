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

                    lista = ctx.Evento.Include("Invitacion").Where(x => x.Estado == true).ToList();
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
                    evento.Estado = true;
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


        
    }
}
