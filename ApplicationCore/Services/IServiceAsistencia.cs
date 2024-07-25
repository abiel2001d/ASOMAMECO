using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceAsistencia
    {
        IEnumerable<Asistencia> GetAsistenciasByEvento(int idEvento);

        Asistencia MarcarAsistencia(int idUsuario, int idEvento);

        bool ConcluirAsistencia(int idEvento);
    }
}
