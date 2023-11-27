using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Contratos
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model);
        Task<PalestranteDto> UpdatePalestrantes(int userId, PalestranteUpdateDto model);
        Task<PageList<PalestranteDto>> GetAllPalestrantesASync(PageParams pageParams, bool includeEventos = false);
        Task<PalestranteDto> GetPalestranteByUserIdASync(int userId, bool includeEventos = false);
    }
}