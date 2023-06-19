using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class PalestrantesPersist : IPalestrantesPersist
    {
        private readonly ProEventosContext _context;
        public PalestrantesPersist(ProEventosContext context)
        {
            this._context = context;
            
        }
         public async Task<Palestrante[]> GetAllPalestrantesByNomeASync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais);

            if(includeEventos)
            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);

            }

            query = query.OrderBy(p => p.Id)
                         .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesASync(bool includeEventos = false)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais);

            if(includeEventos)
            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);

            }

            query = query.OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdASync(int palestranteId, bool includeEventos)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais);

            if(includeEventos)
            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);

            }

            query = query.OrderBy(p => p.Id)
                         .Where(p => p.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }

    }
}