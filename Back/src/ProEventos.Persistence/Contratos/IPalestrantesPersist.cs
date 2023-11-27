using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantesPersist: IGeralPersist
    {
        //PALESTRANTES
        Task<PageList<Palestrante>> GetAllPalestrantesASync(PageParams pageParams, bool includeEventos = false);
        Task<Palestrante> GetPalestranteByUserIdASync(int userId, bool includeEventos = false);
    }
}