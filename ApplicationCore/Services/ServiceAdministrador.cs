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
        public Administrador GetAdministrador(string usuario)
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();

            return repository.GetAdministrador(usuario);
        }
        public Task EnviarCodigo(Administrador administrador)
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();
            return repository.EnviarCodigo(administrador);
        }

        public bool RestablecerContrasena(Administrador administrador, int codigoIngresado, string nuevaContrasena)
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();
            string cryptPassword = Cryptography.EncrypthAES(nuevaContrasena);
            return repository.RestablecerContrasena(administrador, codigoIngresado, cryptPassword);
        }
    }
}
