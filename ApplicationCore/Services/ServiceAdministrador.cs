using ApplicationCore.Utils;
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
        public Administrador GetAdministrador(string usuario, string contrasena)
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();
            string cryptPassword = Cryptography.EncrypthAES(contrasena);

            return repository.GetAdministrador(usuario, cryptPassword);
        }
    }
}
