using AutoMapper;
using CafeApi.Entities;
using CafeApi.Models;
using CafeApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CafeApi.Controllers
{
    [Route("api/cafe")]
    [ApiController]
    [Authorize]
    public class CafeController : ControllerBase
    {
        private readonly ICafeService _cafeServiece;
        public CafeController(ICafeService cafeServiece)
        {
            _cafeServiece = cafeServiece;
        }


        [HttpPut("{id}")]
        public ActionResult Update([FromBody]UpdateCafeDto dto, [FromRoute]int id)
        {
            /*tego nie trzeba bo jest [ApiController] - to robi za nas walidacje modelu
             * if (!ModelState.IsValid)
            {
                
                return BadRequest(ModelState);
            }*/

             _cafeServiece.Update(dto, id, User);


            return Ok();
        }


        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _cafeServiece.Delete(id, User);

            

            return NoContent();
        }

        [HttpPost]
        
        public ActionResult CreateCafe([FromBody] CreateCafeDto dto)
        {
            //jeżeli mamy nałożone wymagania na kolumny w tabeli
            //za pomocą nawiasów kwadratowych nad nazwami poszczególnych warości
            //w klasie CreateCafeDto to możemy sprawdzić czy dto jest poprawe 
            //w taki sposób
            if (!ModelState.IsValid)
            {
                //przesyłamy też modelState bo on zawiera informację o błędach walidacji
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id=_cafeServiece.Create(dto, userId);

            return Created($"/api/cafe/{id}",null);
        }

        [HttpGet]
       //ten nagłówek podwoduje że tylko użytkonicy w roli
       ////managera mogą dostać sie do tej metody
       //[Authorize(Roles ="Manager")] - to nie działało :(

       //mozemy też zdefiniować swoją własną politkę autoryzacji
       //którą określamy w klasie startup
       //i wykorzystać do tego nagłówek, ten działa:
       //[Authorize(Policy = "HasNationality")]

        //możemy też dodać własną customową politykę autoryzacji
        [Authorize(Policy = "AtLeast20")]
        public ActionResult<IEnumerable<CafeDto>> GetAll()
        {
            var cafes = _cafeServiece.GetAll();

            return Ok(cafes);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<CafeDto> Get([FromRoute]int id)
        {
            var cafe = _cafeServiece.GetById(id);

            

            return Ok(cafe);
        }

    }
}
