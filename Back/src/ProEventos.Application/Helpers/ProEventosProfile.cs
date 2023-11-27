using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteAddDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteUpdateDto>().ReverseMap();

            //CreateMap<PageList<Evento>, PageList<EventoDto>>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, User>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
        }
    }
}