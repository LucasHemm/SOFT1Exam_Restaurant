using RestaurantService.Models;

namespace RestaurantService.DTOs;

public class RestaurantDTO
{
    public int Id { get; set; } //primary key
    public String Name { get; set; }
    public AddressDTO Address { get; set; }
    public List<MenuItemDTO>? MenuItems { get; set; }
    public double Rating { get; set; }
    public int NumberOfRatings { get; set; }
    public String CuisineType { get; set; }
    public String Description { get; set; }
    public String PhoneNumber { get; set; }
    
    public RestaurantDTO(int id, String name, AddressDTO address, double rating, int numberOfRatings, String cuisineType, String description, String phoneNumber)
    {
        Id = id;
        Name = name;
        Address = address;
        Rating = rating;
        NumberOfRatings = numberOfRatings;
        CuisineType = cuisineType;
        Description = description;
        PhoneNumber = phoneNumber;
    }
    
    public RestaurantDTO()
    {
    }
    
    public RestaurantDTO(Restaurant restaurant)
    {
        Id = restaurant.Id;
        Name = restaurant.Name;
        Address = new AddressDTO(restaurant.Address);
        MenuItems = restaurant.MenuItems?.Select(menuItem => new MenuItemDTO(menuItem)).ToList();
        Rating = restaurant.Rating;
        NumberOfRatings = restaurant.NumberOfRatings;
        CuisineType = restaurant.CuisineType;
        Description = restaurant.Description;
        PhoneNumber = restaurant.PhoneNumber;
    }
}