using AutoMapper;
using Movies.Data.Dto;
using MoviesDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Mapper
{
    class AppMappingProfile: Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Actor, ActorDto>()
                .ForMember(destination => destination.MoviesActors,
                options => options.MapFrom(src => src.MoviesActors.Select(actors => actors.Actor)));

            //CreateMap<ActorDto, Actor>()
            //    .ForMember(destination => destination.
            //    options => options.MapFrom(src => src.MoviesActors));


            //CreateMap<ADTO, A>()
            //.ForMember(dest => dest.AName, opt => opt.MapFrom(src => src.AName))
            //.ForMember(dest => dest.ABs, opt => opt.MapFrom(src => src.Bs))
            //.AfterMap((src, dest) => {
            //    foreach (var b in dest.ABs)
            //    {
            //        b.AId = src.Id;
            //    }
            //});
            //CreateMap<B, AB>()
            //          .ForMember(dest => dest.BId, opt => opt.MapFrom(src => src.Id))
            //          .ForMember(dest => dest.B, opt => opt.MapFrom(src => src));
        }
    }
}
