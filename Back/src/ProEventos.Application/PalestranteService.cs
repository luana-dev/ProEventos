using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;

namespace ProEventos.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantesPersist _palestrantePersist;
        private readonly IMapper _mapper;

        public PalestranteService( IPalestrantesPersist palestrantePersist,
                             IMapper mapper
                             )
        {
            this._palestrantePersist = palestrantePersist;
            _mapper = mapper;

            
        }
        public async Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestrantePersist.Add<Palestrante>(palestrante);

                if(await _palestrantePersist.SaveChangesASync())
                {
                    var PalestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdASync(userId, false);

                    return _mapper.Map<PalestranteDto>(PalestranteRetorno);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrantes(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdASync(userId, false);
                if(palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante);

                _palestrantePersist.Update<Palestrante>(palestrante);

                if(await _palestrantePersist.SaveChangesASync())
                {
                    var PalestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdASync(userId, false);

                    return _mapper.Map<PalestranteDto>(PalestranteRetorno);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesASync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var palestrantes = await _palestrantePersist.GetAllPalestrantesASync(pageParams, includeEventos);
                if(palestrantes == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes);

                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.TotalPages = palestrantes.TotalPages;
                resultado.PageSize = palestrantes.PageSize;
                resultado.TotalCount = palestrantes.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> GetPalestranteByUserIdASync(int userId, bool includeEventos = false)
        {
             try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdASync(userId, includeEventos);
                if(palestrante == null) return null;

                var resultado = _mapper.Map<PalestranteDto>(palestrante);

                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
    }
}