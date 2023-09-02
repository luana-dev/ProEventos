using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class LotesPersist : ILotesPersist
    {
        private readonly ProEventosContext _context;
        public LotesPersist(ProEventosContext context)
        {
            this._context = context;
            
        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                    .Where(lote => lote.EventoId == eventoId
                                   && lote.Id == loteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                    .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();
        }
    }
}