using Infraestructura.Model;
using Infraestructura.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceEvento : IServiceEvento
    {

        public IEnumerable<Evento> GetEventos()
        {
            IRepositoryEvento repository = new RepositoryEvento();
            return repository.GetEventos();
        }

        public Evento Save(Evento evento)
        {
            IRepositoryEvento repository = new RepositoryEvento();
            return repository.Save(evento);
        }

        public Evento GetEventoByID(int id)
        {
            IRepositoryEvento repository = new RepositoryEvento();
            return repository.GetEventoByID(id);
        }

        public Task EnviarInvitaciones(Evento evento)
        {
            IRepositoryEvento repository = new RepositoryEvento();
            return repository.EnviarInvitaciones(evento);
        }
    }
}
