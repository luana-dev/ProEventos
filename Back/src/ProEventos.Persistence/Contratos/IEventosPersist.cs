using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventosPersist
    {
         //EVENTOS
        Task<Evento[]> GetAllEventosByTemaASync(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosASync(bool includePalestrantes = false);
        Task<Evento> GetEventoByIdASync(int eventoId, bool includePalestrantes = false);
    }
}