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

        public IEnumerable<Administrador> GetOperarios()
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();

            return repository.GetOperarios();
        }

        public Administrador GetOperarioByID(int id_Operario)
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();
            Administrador administrador = repository.GetOperarioByID(id_Operario);
            administrador.Contraseña = Cryptography.DecrypthAES(administrador.Contraseña);
            return administrador;
        }

        public Administrador Save(Administrador operario)
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();
            string cryptPassword = Cryptography.EncrypthAES(operario.Contraseña);
            operario.Contraseña = cryptPassword;
            return repository.Save(operario);
        }

        public bool Delete(int id_Operario)
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();

            return repository.Delete(id_Operario);
        }

        public IEnumerable<Rol> GetRoles()
        {
            IRepositoryAdministrador repository = new RepositoryAdministrador();

            return repository.GetRoles();
        }
    }
}
