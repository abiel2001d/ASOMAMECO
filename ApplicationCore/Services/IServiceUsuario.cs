using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceUsuario
    {
        IEnumerable<Usuario> GetUsuarios();
        Usuario GetUsuarioByID(int id);
        Usuario Save(Usuario usuario);

        bool Delete(int id);

        IEnumerable<Usuario> GetUsuariosPorEstado(string estado);
    }
}
