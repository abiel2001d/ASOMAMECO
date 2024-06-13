using Infraestructura.Model;
using Infraestructura.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceAdministrador : IServiceAdministrador
    {
        public Administrador GetAdministrador(string usuario)
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();
            return repository.GetAdministrador(usuario);
        }
    }
}
