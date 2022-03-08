using AutoMapper;
using CafeApi.Entities;
using CafeApi.Exceptions;
using CafeApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CafeApi.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly CafeDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;



        public AccountService(CafeDbContext dbContext, IPasswordHasher<User> passwordHasher
                ,AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }


        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext
                .Users
                .Include(r =>r.Role)
                .FirstOrDefault(r => r.Email == dto.Email);

            if(user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var restult=_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (restult == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd"))
               
            };

            if(!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(
                    new Claim("Nationality", user.Nationality)
                    );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expired = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expired,
                signingCredentials: cred);

            var tokenHandeler = new JwtSecurityTokenHandler();
            return tokenHandeler.WriteToken(token);
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId,

            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;

            _dbContext.Add(newUser);
            _dbContext.SaveChanges();
        }
    }
}
