﻿using Infraestructura.Model;
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
    }
}
