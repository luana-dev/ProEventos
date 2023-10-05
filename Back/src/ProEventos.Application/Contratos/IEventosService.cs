using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventosService
    {
        Task<EventoDto> AddEventos(int userId, EventoDto model);
        Task<EventoDto> UpdateEventos(int userId, int EventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int EventoId);

        Task<EventoDto[]> GetAllEventosByTemaASync(int userId, string tema, bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosASync(int userId, bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdASync(int userId, int EventoId, bool includePalestrantes = false);
    }
}