using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repository
{
    public interface IRepositoryInvitacion
    {
        IEnumerable<Invitacion> GetInvitacionesByEvento(int id_evento);

        Task EnviarInvitaciones(Evento evento);
    }
}
