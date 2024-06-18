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

                    oAdministrador = ctx.Administrador.Where(x => x.Usuario == usuario && x.Contraseña == contrasena).FirstOrDefault();

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
    }
}
