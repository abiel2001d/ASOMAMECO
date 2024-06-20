using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repository
{
    public interface IRepositoryAdministrador
    {
            Administrador GetAdministrador(string usuario, string contrasena);
            Task EnviarCodigo(Administrador administrador);

            Administrador GetAdministrador(string usuario);

        bool RestablecerContrasena(Administrador administrador, int codigoIngresado, string nuevaContrasena);
    }
}
