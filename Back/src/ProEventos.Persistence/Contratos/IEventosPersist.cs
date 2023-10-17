using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventosPersist
    {
         //EVENTos
        Task<PageList<Evento>> GetAllEventosASync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdASync(int userId, int eventoId, bool includePalestrantes = false);
    }
}