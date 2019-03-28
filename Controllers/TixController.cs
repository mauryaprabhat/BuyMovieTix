using System.Collections.Generic;
 using System.Linq;
  using System.Net;
   using System.Net.Http;
    using Microsoft.AspNetCore.Mvc;
     
     using BuyTixApi.Business;

      using BuyTixApi.Models.Movies; 
      using BuyTixApi.Models.Purchase;
       using BuyTixApi.Models.Seats;

namespace BuyTix.Api.Controllers
{
    [Route("api/[Controller]")]
    public class TixController:Controller
    {
        [HttpGet]
        [Route("GetMovies")]
        [ProducesResponseType(typeof(List<ActiveMovie>), 200)]
        public IActionResult GetAllActiveMovies()
        {
                return Ok(Movies.GetAllActiveMovies());
        }

        // [HttpGet]
        // [Route("GetSeatingPlan/{ActiveMovieId}")]
        // [ProducesResponseType(typeof(SeatingPlan), 200)]
        // public IActionResult GetActiveMovieSeatingPlan(int ActiveMovieId)
        // {
        //     return Ok(Business.Movies.GetActiveMovieSeatingPlan(ActiveMovieId));
        // }

        [HttpPost]
        [Route("PurchaseTickets")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult PurchaseTickets([FromBody]Order order) 
        {
           return Ok(Movies.PurchaseTickets(order));
        }

    }
}