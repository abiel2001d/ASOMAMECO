using Infraestructura.Model;
using Infraestructura.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceAsistencia : IServiceAsistencia
    {
        public bool ConcluirAsistencia(int idEvento)
        {
            IRepositoryAsistencia repository = new RepositoryAsistencia();
            return repository.ConcluirAsistencia(idEvento);
        }

        public IEnumerable<Asistencia> GetAsistenciasByEvento(int idEvento)
        {
            IRepositoryAsistencia repository = new RepositoryAsistencia();
            return repository.GetAsistenciasByEvento(idEvento);
        }

        public Asistencia MarcarAsistencia(int idUsuario, int idEvento)
        {
            IRepositoryAsistencia repository = new RepositoryAsistencia();
            return repository.MarcarAsistencia(idUsuario,idEvento);
        }
    }
}
