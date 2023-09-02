using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotesPersist
    {
         //LOTES

        /// <summary>
        /// Método que retornará uma Lista de Lotes.
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <returns>Array de Lotes.</returns>

        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        /// <summary>
        /// Método que retornará apenas 1 lote.
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <param name="loteId">Código chave da tabela Lote</param>
        /// <returns>retorna apenas 1 lote.</returns>
        Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}