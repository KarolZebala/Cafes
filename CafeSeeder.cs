using CafeApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi
{
    public class CafeSeeder
    {
        private readonly CafeDbContext _dbContext;
        public CafeSeeder(CafeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {

            //sprawdzenie czy można się połączyć z baza danych
            
            if (_dbContext.Database.CanConnect())
            {
                //sprawdzenie czy tabela Cafes jest pusta
                if (!_dbContext.Cafes.Any())
                {
                    //jeśli tak to dodanie domyślnych wartości
                    var cafes = GetCafes();
                    _dbContext.Cafes.AddRange(cafes);
                    _dbContext.SaveChanges();
                }

                //sprawdzenie czy tabela z rolami jest pusta
                if (!_dbContext.Roles.Any())
                {
                    //jeśli tak to dodanie domyślnych wartości
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                Name = "Manager"
            },
                new Role()
                {
                    Name = "Admin"
                },
            };

            return roles;
        }

        private IEnumerable<Cafe> GetCafes()
        {
            var cafes = new List<Cafe>()
            {
                new Cafe()
                {
                    Name = "Costa",
                    Category = "Coffehouse",
                    Description =
                        "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Drinks = new List<Drink>()
                    {
                        new Drink()
                        {
                            Name = "Tea",
                            Price = 10.30M,
                        },

                        new Drink()
                        {
                            Name = "Caffe Late",
                            Price = 5.30M,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Długa 5",
                        PostalCode = "30-001"
                    }
                },
                new Cafe()
                {
                    Name = "Starbucks",
                    Category = "Fast",
                    Description =
                        "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                    ContactEmail = "contact@mcdonald.com",
                    HasDelivery = true,
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Szewska 2",
                        PostalCode = "30-001"
                    }
                }
            };

            return cafes;
        }
    }
}
