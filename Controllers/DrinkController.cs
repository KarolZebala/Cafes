using CafeApi.Models;
using CafeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi.Controllers
{
    [Route("api/cafe/{cafeId}/drink")]
    [ApiController]
    public class DrinkController : ControllerBase
    {
        private IDrinkService _drinkService;

        public DrinkController(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        [HttpPost]
        public ActionResult CreateDrink([FromBody]CreateDrinkDto dto, [FromRoute]int cafeId)
        {
            var newDrinkId = _drinkService.Create(cafeId, dto);

            return Created($"api/cafe/{cafeId}/drink/{newDrinkId}", null);
        }

        [HttpGet("{drinkId}")]
        public ActionResult<DrinkDto> GetById([FromRoute]int drinkId , [FromRoute] int cafeId)
        {
            var drink = _drinkService.GetById(cafeId, drinkId);

            return Ok(drink);
        }

        [HttpGet]
        public ActionResult<List<DrinkDto>> GetById([FromRoute] int cafeId)
        {
            var drinks = _drinkService.GetAll(cafeId);

            return Ok(drinks);
        }

        [HttpDelete]
        public ActionResult DeleteAllDrinks([FromRoute] int cafeId)
        {
            _drinkService.DeleteAllDrinks(cafeId);

            return NoContent();
        }
    }
}
