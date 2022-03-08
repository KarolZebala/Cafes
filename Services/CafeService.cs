using AutoMapper;
using CafeApi.Custom_Authorization;
using CafeApi.Entities;
using CafeApi.Exceptions;
using CafeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CafeApi.Services
{
    public interface ICafeService
    {
        CafeDto GetById(int id);
        IEnumerable<CafeDto> GetAll();
        int Create(CreateCafeDto dto, int userId);
        void Delete(int id, ClaimsPrincipal user);
        void Update(UpdateCafeDto dto, int id, ClaimsPrincipal user);
    }
    public class CafeService : ICafeService
    {
        private readonly CafeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CafeService> _logger;
        private readonly IAuthorizationService _authorizationService;
        public CafeService(CafeDbContext dbContext, IMapper mapper, ILogger<CafeService> logger,
            IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
        }

        public void Update(UpdateCafeDto dto, int id, ClaimsPrincipal user)
        {
            

            var cafe = _dbContext
                .Cafes
                .FirstOrDefault(x => x.Id == id);
            if(cafe is null)
            {
                throw new NotFoundEcxeption("Cafe not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync
                (user, cafe, new ResourceOperationRequirment(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidExeption();
            }

            cafe.Name = dto.Name;
            cafe.Description = dto.Description;
            cafe.HasDelivery = dto.HasDelivery;

            _dbContext.SaveChanges();
            

        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            _logger.LogError($"Cafe with id: {id} DELETE action invoked");
            var cafe = _dbContext
               .Cafes
               .FirstOrDefault(x => x.Id == id);

            if(cafe is null)
            {
                throw new NotFoundEcxeption("Cafe not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync
                (user, cafe, new ResourceOperationRequirment(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidExeption();
            }

            _dbContext.Cafes.Remove(cafe);
            _dbContext.SaveChanges();
            

        }

        public CafeDto GetById(int id)
        {
            var cafe = _dbContext
                .Cafes
                .Include(r => r.Address)
                .Include(r => r.Drinks)
                .FirstOrDefault(x => x.Id == id);

            if (cafe is null)
            {
                throw new NotFoundEcxeption("Cafe not found");
            }

            var cafeDto = _mapper.Map<CafeDto>(cafe);
            return cafeDto;
        }

        public IEnumerable<CafeDto> GetAll()
        {
            var cafes = _dbContext
               .Cafes
               .Include(r => r.Address)
               .Include(r => r.Drinks)
               .ToList();

            var cafeDtos = _mapper.Map<List<CafeDto>>(cafes);

            return cafeDtos;
        }

        public int Create(CreateCafeDto dto, int userId)
        {
            var cafe = _mapper.Map<Cafe>(dto);
            cafe.CreatedById = userId;
            
            _dbContext.Cafes.Add(cafe);
            _dbContext.SaveChanges();

            return cafe.Id;
        }
    }
}
