using RestaurantService.DTOs;
using RestaurantService.Models;

namespace RestaurantService.Facades;

public class RestaurantFacade
{
    private readonly ApplicationDbContext _context;
    
    public RestaurantFacade(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Restaurant CreateRestaurant(RestaurantDTO restaurantDto)
    {
        Address address = _context.Addresses.Find(restaurantDto.Address.Id) ?? new Address(restaurantDto.Address);
        Restaurant restaurant = new Restaurant(restaurantDto.Name, address, restaurantDto.Rating, restaurantDto.CuisineType);
        _context.Restaurants.Add(restaurant);
        _context.SaveChanges();
        return new Restaurant();
    }

}