using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceAdministrador
    {
        Administrador GetAdministrador(string usuario, string contrasena);
        Administrador GetAdministrador(string usuario);
        Task EnviarCodigo(Administrador administrador);

        bool RestablecerContrasena(Administrador administrador, int codigoIngresado, string nuevaContrasena);

    }
}
