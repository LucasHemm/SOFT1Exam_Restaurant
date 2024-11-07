using Microsoft.AspNetCore.Mvc;
using RestaurantService.DTOs;
using RestaurantService.Facades;
using RestaurantService.Models;

namespace RestaurantService.API;
[ApiController]
[Route("api/[controller]")]
public class RestaurantApi : ControllerBase
{
        private readonly RestaurantFacade _restaurantFacade;

        public RestaurantApi(RestaurantFacade restaurantFacade)
        {
            _restaurantFacade = restaurantFacade;
        }

        // POST: api/Restaurant
        [HttpPost]
        public IActionResult CreateRestaurant([FromBody] RestaurantDTO restaurantDto)
        {
            try
            {
                _restaurantFacade.CreateRestaurant(restaurantDto);
                return Ok("Restaurant created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // PUT: api/Restaurant
        [HttpPut]
        public IActionResult UpdateRestaurant([FromBody] RestaurantDTO restaurantDto)
        {
            try
            {
                _restaurantFacade.UpdateRestaurant(restaurantDto);
                return Ok("Restaurant updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
}