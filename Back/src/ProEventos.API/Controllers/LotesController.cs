using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILotesService _lotesService;

        public LotesController(ILotesService lotesService){
            this._lotesService = lotesService;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId){
            try
            {
                var lotes = await _lotesService.GetLotesByEventoIdAsync(eventoId);
                if(lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotesService.SaveLote(eventoId, models);
                if(lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotesService.GetLoteByIdsAsync(eventoId, loteId);
                if(lote == null) return NoContent();

                return await _lotesService.DeleteLote(eventoId, loteId) ?
                    Ok(new {message = "Deletado"}) :
                    throw new Exception("Ocorreu um problema ao deletar o lote.");
            }
            catch (Exception ex)
            {
             return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar lote. Erro: {ex.Message}");   
            }
        }
    }
}
