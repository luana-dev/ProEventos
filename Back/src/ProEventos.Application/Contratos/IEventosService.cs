using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Contratos
{
    public interface IEventosService
    {
        Task<EventoDto> AddEventos(int userId, EventoDto model);
        Task<EventoDto> UpdateEventos(int userId, int EventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int EventoId);
        Task<PageList<EventoDto>> GetAllEventosASync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdASync(int userId, int EventoId, bool includePalestrantes = false);
    }
}