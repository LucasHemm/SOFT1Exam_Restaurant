using RestaurantService.DTOs;

namespace RestaurantService.Models;

public class Restaurant
{
    public int Id { get; set; } //primary key
    public String Name { get; set; }
    public Address Address { get; set; }
    public List<MenuItem>? MenuItems { get; set; }
    public double Rating { get; set; }
    public int NumberOfRatings { get; set; }
    public String CuisineType { get; set; }
    
    public Restaurant(int id, String name, Address address, List<MenuItem>? menuItems, double rating, int numberOfRatings, String cuisineType)
    {
        Id = id;
        Name = name;
        Address = address;
        MenuItems = menuItems;
        Rating = rating;
        NumberOfRatings = numberOfRatings;
        CuisineType = cuisineType;
    }
    
    public Restaurant()
    {
    }
    
    public Restaurant(RestaurantDTO restaurantDto, Address address)
    {
        Id = restaurantDto.Id;
        Name = restaurantDto.Name;
        Address = address;
        MenuItems = restaurantDto.MenuItems?.Select(menuItem => new MenuItem(menuItem, this)).ToList();
        Rating = restaurantDto.Rating;
        NumberOfRatings = restaurantDto.NumberOfRatings;
        CuisineType = restaurantDto.CuisineType;
    }
}