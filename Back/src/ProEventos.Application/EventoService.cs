using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventosService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventosPersist _eventosPersist;
        
        public EventoService(IGeralPersist geralPersist, IEventosPersist eventosPersist)
        {
            this._geralPersist = geralPersist;
            this._eventosPersist = eventosPersist;

            
        }
        public async Task<Evento> AddEventos(Evento model)
        {
            try
            {
                _geralPersist.Add<Evento>(model);

                if(await _geralPersist.SaveChangesASync())
                {
                    return await _eventosPersist.GetEventoByIdASync(model.Id, false);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEventos(int EventoId, Evento model)
        {
            try
            {
                var evento = await _eventosPersist.GetEventoByIdASync(EventoId, false);
                if(evento == null) return null;

                model.Id = evento.Id;

                _geralPersist.Update(model);
                if(await _geralPersist.SaveChangesASync())
                {
                    return await _eventosPersist.GetEventoByIdASync(model.Id, false);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int EventoId)
        {
            try
            {
                var evento = await _eventosPersist.GetEventoByIdASync(EventoId, false);
                if(evento == null) throw new Exception("Evento para delete não encontrado");

                _geralPersist.Delete<Evento>(evento);
                return await _geralPersist.SaveChangesASync();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosASync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersist.GetAllEventosASync(includePalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaASync(string tema, bool includePalestrantes = false)
        {
             try
            {
                var eventos = await _eventosPersist.GetAllEventosByTemaASync(tema, includePalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventoByIdASync(int EventoId, bool includePalestrantes = false)
        {
             try
            {
                var eventos = await _eventosPersist.GetEventoByIdASync(EventoId, includePalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
    }
}