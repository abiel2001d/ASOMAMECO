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
        IEnumerable<Rol> GetRoles();
        bool RestablecerContrasena(Administrador administrador, int codigoIngresado, string nuevaContrasena);

        IEnumerable<Administrador> GetOperarios();
        Administrador GetOperarioByID(int id_Operario);
        Administrador Save(Administrador operario);
        bool Delete(int id_Operario);

    }
}
