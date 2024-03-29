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
    public class EventoService : IEventosService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventosPersist _eventosPersist;
        private readonly IMapper _mapper;

        public EventoService(IGeralPersist geralPersist, 
                             IEventosPersist eventosPersist,
                             IMapper mapper
                             )
        {
            this._geralPersist = geralPersist;
            this._eventosPersist = eventosPersist;
            _mapper = mapper;

            
        }
        public async Task<EventoDto> AddEventos(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralPersist.Add<Evento>(evento);

                if(await _geralPersist.SaveChangesASync())
                {
                    var eventoRetorno = await _eventosPersist.GetEventoByIdASync(userId, evento.Id, false);

                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEventos(int userId, int EventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventosPersist.GetEventoByIdASync(userId, EventoId, false);
                if(evento == null) return null;

                model.Id = evento.Id;
                model.UserId = userId;

                _mapper.Map(model, evento);

                _geralPersist.Update<Evento>(evento);

                if(await _geralPersist.SaveChangesASync())
                {
                    var eventoRetorno = await _eventosPersist.GetEventoByIdASync(userId, evento.Id, false);

                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userId, int EventoId)
        {
            try
            {
                var evento = await _eventosPersist.GetEventoByIdASync(userId, EventoId, false);
                if(evento == null) throw new Exception("Evento para delete não encontrado");

                _geralPersist.Delete<Evento>(evento);
                return await _geralPersist.SaveChangesASync();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<EventoDto>> GetAllEventosASync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersist.GetAllEventosASync(userId, pageParams, includePalestrantes);
                if(eventos == null) return null;

                var resultado = _mapper.Map<PageList<EventoDto>>(eventos);

                resultado.CurrentPage = eventos.CurrentPage;
                resultado.TotalPages = eventos.TotalPages;
                resultado.PageSize = eventos.PageSize;
                resultado.TotalCount = eventos.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdASync(int userId, int EventoId, bool includePalestrantes = false)
        {
             try
            {
                var evento = await _eventosPersist.GetEventoByIdASync(userId, EventoId, includePalestrantes);
                if(evento == null) return null;

                var resultado = _mapper.Map<EventoDto>(evento);

                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
    }
}