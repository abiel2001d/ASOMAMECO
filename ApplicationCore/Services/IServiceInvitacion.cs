using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceInvitacion
    {

         IEnumerable<Invitacion> GetInvitacionesByEvento(int id_evento);

        Task EnviarInvitaciones(Evento evento);
    }
}
