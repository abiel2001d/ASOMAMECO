using ApplicationCore.Services;
using Infraestructura.Model;
using Infraestructura.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ApplicationCore
{
    public class ServiceInvitacion : IServiceInvitacion
    {
        public Task ActualizarConfirmacion(int eventoId, int usuarioId, string respuesta)
        {
            IRepositoryInvitacion repository = new RepositoryInvitacion();
            return repository.ActualizarConfirmacion(eventoId,usuarioId,respuesta);
        }

        public Task<bool> EnviarInvitaciones(Evento evento, UrlHelper urlHelper)
        {
            IRepositoryInvitacion repository = new RepositoryInvitacion();
            return repository.EnviarInvitaciones(evento, urlHelper);
        }

        public IEnumerable<Invitacion> GetInvitacionesByEvento(int id_evento)
        {
            IRepositoryInvitacion repository = new RepositoryInvitacion();
            return repository.GetInvitacionesByEvento(id_evento);
        }
    }
}
