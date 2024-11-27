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
                RestaurantDTO createdRestaurant = new RestaurantDTO(_restaurantFacade.CreateRestaurant(restaurantDto));
                return Ok(createdRestaurant);
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
                RestaurantDTO updateRestaurant = new RestaurantDTO(_restaurantFacade.UpdateRestaurant(restaurantDto));
                return Ok(updateRestaurant);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // POST: api/Restaurant/MenuItem    
        [HttpPost("MenuItem")]
        public IActionResult CreateMenuItem([FromBody] MenuItemDTO menuItemDto)
        {
            try
            {
                MenuItemDTO createdMenuItem = new MenuItemDTO(_restaurantFacade.CreateMenuItem(menuItemDto));
                return Ok(createdMenuItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // PUT: api/Restaurant/MenuItem
        [HttpPut("MenuItem")]
        public IActionResult UpdateMenuItem([FromBody] MenuItemDTO menuItemDto)
        {
            try
            {
                MenuItemDTO updatedMenuItem = new MenuItemDTO(_restaurantFacade.UpdateMenuItem(menuItemDto));
                return Ok(updatedMenuItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // GET: api/Restaurant
        [HttpGet]
        public IActionResult GetAllRestaurants()
        {
            try
            {
                return Ok(_restaurantFacade.GetAllRestaurants().Select(restaurant => new RestaurantDTO(restaurant)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // GET: api/Restaurant/{id}
        [HttpGet("{id}")]
        public IActionResult GetRestaurant(int id)
        {
            try
            {
                return Ok(new RestaurantDTO(_restaurantFacade.GetRestaurant(id)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // GET: api/Restaurant/MenuItems/{id}
        [HttpGet("MenuItems/{id}")]
        public IActionResult GetMenuItems(int id)
        {
            try
            {
                return Ok(_restaurantFacade.GetMenuItems(id).Select(menuItem => new MenuItemDTO(menuItem)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //Update the rating and number of ratings of a restaurant
        // PUT: api/Restaurant/Rating
        [HttpPut("Rating")]
        public IActionResult UpdateRestaurantWithRating([FromBody] UpdateRatingDTO updateRatingDto)
        {
            try
            {
                RestaurantDTO updatedRestaurant = new RestaurantDTO(_restaurantFacade.UpdateRestaurantWithRating(updateRatingDto));
                return Ok(updatedRestaurant);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
}