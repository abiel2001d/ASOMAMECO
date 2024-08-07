using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Web.Security
{
    public class AutorizeView
    {
        public static bool IsUserInRole(string[] nombreRoles)
        {
            IEnumerable<Roles> allowedroles = nombreRoles.
                Select(a => (Roles)Enum.Parse(typeof(Roles), a));
            bool authorize = false;
            var oUsuario = (Administrador)HttpContext.Current.Session["User"];
            if (oUsuario != null)
            {
                foreach (var rol in allowedroles)
                {
                    if ((int)rol == oUsuario.ID_Rol)
                        return true;
                }
            }
            return authorize;
        }
    }
}