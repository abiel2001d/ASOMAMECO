using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceEvento
    {
        IEnumerable<Evento> GetEventos();

        Evento GetEventoByID(int id);

        Evento Save(Evento evento);
    }
}
