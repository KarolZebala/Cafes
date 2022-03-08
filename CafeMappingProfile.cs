using AutoMapper;
using CafeApi.Entities;
using CafeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi
{
    public class CafeMappingProfile : Profile
    {
        public CafeMappingProfile()
        {

            CreateMap<Cafe, CafeDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            
            CreateMap<CreateCafeDto, Cafe>()
                .ForMember(r => r.Address, c => c.MapFrom(dto => new Address()
                    { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));

            CreateMap<Drink, DrinkDto>();

            CreateMap<CreateDrinkDto, Drink>();
        }
    }
}
