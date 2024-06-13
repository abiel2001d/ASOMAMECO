using Infraestructura.Model;
using Infraestructura.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceUsuario : IServiceUsuario
    {
        public IEnumerable<Usuario> GetUsuarios()
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            return repository.GetUsuarios();
        }

    }
}
