using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Infraestructura.Repository
{
    public interface IRepositoryInvitacion
    {
        IEnumerable<Invitacion> GetInvitacionesByEvento(int id_evento);

        Task<bool> EnviarInvitaciones(Evento evento, UrlHelper urlHelper);

        Task ActualizarConfirmacion(int eventoId, int usuarioId, string respuesta);

    }
}
