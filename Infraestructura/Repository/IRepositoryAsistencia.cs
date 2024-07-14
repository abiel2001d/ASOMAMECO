using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repository
{
    public interface IRepositoryAsistencia
    {
        IEnumerable<Asistencia> GetAsistenciasByEvento(int idEvento);

        Asistencia MarcarAsistencia(int idUsuario, int idEvento);
    }
}
