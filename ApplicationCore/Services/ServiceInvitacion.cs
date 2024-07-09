using ApplicationCore.Services;
using Infraestructura.Model;
using Infraestructura.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class ServiceInvitacion : IServiceInvitacion
    {

        public Task EnviarInvitaciones(Evento evento)
        {
            IRepositoryInvitacion repository = new RepositoryInvitacion();
            return repository.EnviarInvitaciones(evento);
        }

        public IEnumerable<Invitacion> GetInvitacionesByEvento(int id_evento)
        {
            IRepositoryInvitacion repository = new RepositoryInvitacion();
            return repository.GetInvitacionesByEvento(id_evento);
        }
    }
}
