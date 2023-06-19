using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantesPersist
    {
        //PALESTRANTES
        Task<Palestrante[]> GetAllPalestrantesByNomeASync(string nome, bool includeEventos);
        Task<Palestrante[]> GetAllPalestrantesASync(bool includeEventos);
        Task<Palestrante> GetPalestranteByIdASync(int palestranteId, bool includeEventos);
    }
}