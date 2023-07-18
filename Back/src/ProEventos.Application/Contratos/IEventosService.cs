using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventosService
    {
        Task<EventoDto> AddEventos(EventoDto model);
        Task<EventoDto> UpdateEventos(int EventoId, EventoDto model);
        Task<bool> DeleteEvento(int EventoId);

        Task<EventoDto[]> GetAllEventosByTemaASync(string tema, bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosASync(bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdASync(int EventoId, bool includePalestrantes = false);
    }
}