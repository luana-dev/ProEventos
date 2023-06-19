using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contratos
{
    public interface IPalestrantesService
    {
        Task<Palestrante> AddPalestrantes(Palestrante model);
        Task<Palestrante> UpdatePalestrantes(int PalestranteId, Palestrante model);
        Task<Palestrante> DeletePalestrantes(int PalestranteId);

        Task<Palestrante[]> GetAllPalestrantesByNomeASync(string Nome, bool includeEventos = false);
        Task<Palestrante[]> GetAllPalestrantesASync(bool includeEventos = false);
        Task<Palestrante> GetPalestranteByIdASync(int PalestranteId, bool includeEventos = false);

    }
}