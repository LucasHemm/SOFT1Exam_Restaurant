using Microsoft.EntityFrameworkCore;
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
        Restaurant restaurant = new Restaurant(restaurantDto, address);
        _context.Restaurants.Add(restaurant);
        _context.SaveChanges();
        return restaurant;
    }
    
    public Restaurant GetRestaurant(int id)
    {
       Restaurant restaurant = _context.Restaurants
            .Include(restaurant => restaurant.Address)
            .Include(restaurant => restaurant.MenuItems)
            .FirstOrDefault(restaurant => restaurant.Id == id);
        if (restaurant == null)
        {
            throw new Exception("Restaurant not found");
        }
        return restaurant;
    }
    
    public Restaurant UpdateRestaurant(RestaurantDTO restaurantDto)
    {
        Restaurant restaurant = GetRestaurant(restaurantDto.Id);
        restaurant.Name = restaurantDto.Name;
        restaurant.Address = UpdateAddress(restaurantDto.Address);
        restaurant.Rating = restaurantDto.Rating;
        restaurant.CuisineType = restaurantDto.CuisineType;
        restaurant.NumberOfRatings = restaurantDto.NumberOfRatings;
        _context.SaveChanges();
        return restaurant;
    }
    
    private Address UpdateAddress(AddressDTO addressDto)
    {
        Address address = _context.Addresses.Find(addressDto.Id);
        if (address == null)
        {
            throw new Exception("Address not found");
        }
        address.Street = addressDto.Street;
        address.City = addressDto.City;
        address.ZipCode = addressDto.ZipCode;
        address.Region = addressDto.Region;
        _context.SaveChanges();
        return address;
    }
    
    public MenuItem CreateMenuItem(MenuItemDTO menuItemDto)
    {
        Restaurant restaurant = GetRestaurant(menuItemDto.RestaurantId);
        MenuItem menuItem = new MenuItem(menuItemDto, restaurant);
        restaurant.MenuItems.Add(menuItem);
        _context.SaveChanges();
        return menuItem;
    }
    
    public MenuItem UpdateMenuItem(MenuItemDTO menuItemDto)
    {
        MenuItem menuItem = _context.MenuItems.Find(menuItemDto.Id);
        if (menuItem == null)
        {
            throw new Exception("MenuItem not found");
        }
        menuItem.ItemName = menuItemDto.ItemName;
        menuItem.ItemDescription = menuItemDto.ItemDescription;
        menuItem.Price = menuItemDto.Price;
        _context.SaveChanges();
        return menuItem;
    }

}