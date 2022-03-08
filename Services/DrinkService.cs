using AutoMapper;
using CafeApi.Entities;
using CafeApi.Exceptions;
using CafeApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi.Services
{
    public interface IDrinkService
    {
        int Create(int cafeId, CreateDrinkDto dto);
        DrinkDto GetById(int cafeId, int drinkId);
        IEnumerable<DrinkDto> GetAll(int cafeId);
        void DeleteAllDrinks(int cafeId);
    }
    public class DrinkService : IDrinkService 
    {
        private readonly CafeDbContext _context;
        private readonly IMapper _mapper;


        public DrinkService(CafeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void DeleteAllDrinks(int cafeId)
        {
            var cafe = GetCafeById(cafeId);

            _context.RemoveRange(cafe.Drinks);
            _context.SaveChanges();
        }

        public IEnumerable<DrinkDto> GetAll(int cafeId)
        {
            var cafe = GetCafeById(cafeId);

            var drinkDtos = _mapper.Map <List<DrinkDto>>(cafe.Drinks);

            return drinkDtos;
        }

        public DrinkDto GetById(int cafeId, int drinkId)
        {
            var cafe = GetCafeById(cafeId);

            var drink = _context
                .Drinks
                .FirstOrDefault(r => r.Id == drinkId);
            if(drink is null || drink.CafeId!=cafeId)
            {
                throw new NotFoundEcxeption("Drink not found");
            }

            var drinkDto = _mapper.Map<DrinkDto>(drink);
            return drinkDto;
        }
        public int Create(int cafeId, CreateDrinkDto dto)
        {
            var cafe = GetCafeById(cafeId);

            var drinkEntity = _mapper.Map<Drink>(dto);

            drinkEntity.CafeId = cafeId;

            _context.Drinks.Add(drinkEntity);
            _context.SaveChanges();

            return drinkEntity.Id;

        }

        private Cafe GetCafeById(int cafeId)
        {
            var cafe = _context
                .Cafes
                .Include(r =>r.Drinks)
                .FirstOrDefault(r => r.Id == cafeId);

            if (cafe is null)
            {
                throw new NotFoundEcxeption("Cafe not found");
            }

            return cafe;
        }
    }
}
