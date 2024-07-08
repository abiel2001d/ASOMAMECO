using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repository
{
   public interface IRepositoryEvento
    {
        IEnumerable<Evento> GetEventos();

        Evento Save(Evento evento);

        Evento GetEventoByID(int id);

        Task EnviarInvitaciones(Evento evento);
    }
}
