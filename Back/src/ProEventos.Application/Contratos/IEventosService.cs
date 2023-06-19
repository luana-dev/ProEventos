using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contratos
{
    public interface IEventosService
    {
        Task<Evento> AddEventos(Evento model);
        Task<Evento> UpdateEventos(int EventoId, Evento model);
        Task<bool> DeleteEvento(int EventoId);

        Task<Evento[]> GetAllEventosByTemaASync(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosASync(bool includePalestrantes = false);
        Task<Evento> GetEventoByIdASync(int EventoId, bool includePalestrantes = false);
    }
}