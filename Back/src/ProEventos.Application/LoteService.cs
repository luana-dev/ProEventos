using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class LoteService : ILotesService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly ILotesPersist _lotesPersist;
        private readonly IMapper _mapper;

        public LoteService(IGeralPersist geralPersist, 
                             ILotesPersist lotesPersist,
                             IMapper mapper
                             )
        {
            this._geralPersist = geralPersist;
            this._lotesPersist = lotesPersist;
            _mapper = mapper;

            
        }
        public async Task AddLotes(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _geralPersist.Add<Lote>(lote);

                await _geralPersist.SaveChangesASync();
                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotesPersist.GetLotesByEventoIdAsync(eventoId);
                if(lotes == null) return null;

                foreach(var model in models){
                    if(model.Id == 0){

                        await AddLotes(eventoId, model);

                    } else{
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);

                        model.EventoId = eventoId;

                        _mapper.Map(model, lote);

                         _geralPersist.Update<Lote>(lote);

                        await _geralPersist.SaveChangesASync();
                    }

                }
                
                var loteRetorno = await _lotesPersist.GetLotesByEventoIdAsync(eventoId);

                return _mapper.Map<LoteDto[]>(loteRetorno);
                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotesPersist.GetLoteByIdsAsync(eventoId, loteId);
                if(lote == null) throw new Exception("Evento para delete não encontrado");

                _geralPersist.Delete<Lote>(lote);
                return await _geralPersist.SaveChangesASync();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotesPersist.GetLotesByEventoIdAsync(eventoId);
                if(lotes == null) return null;

                var resultado = _mapper.Map<LoteDto[]>(lotes);

                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotesPersist.GetLoteByIdsAsync(eventoId, loteId);
                if(lote == null) return null;

                var resultado = _mapper.Map<LoteDto>(lote);

                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
    }
}