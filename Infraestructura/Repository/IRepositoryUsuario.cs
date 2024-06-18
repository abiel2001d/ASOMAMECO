using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repository
{
    public interface IRepositoryUsuario
    {
        IEnumerable<Usuario> GetUsuarios();
        Usuario GetUsuarioByID(int id);
        Usuario Save(Usuario usuario);
    }
}
