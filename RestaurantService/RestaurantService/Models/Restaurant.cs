namespace RestaurantService.Models;

public class Restaurant
{
    public int Id { get; set; } //primary key
    public String Name { get; set; }
    public Address Address { get; set; }
    public List<MenuItem> MenuItems { get; set; }
    public double Rating { get; set; }
    public String CuisineType { get; set; }
    
    public Restaurant(int id, String name, Address address, List<MenuItem> menuItems, double rating, String cuisineType)
    {
        Id = id;
        Name = name;
        Address = address;
        MenuItems = menuItems;
        Rating = rating;
        CuisineType = cuisineType;
    }
    
    public Restaurant()
    {
        
    }
    
    //for creating a new restaurant
    public Restaurant(String name, Address address, double rating, String cuisineType)
    {
        Name = name;
        Address = address;
        Rating = rating;
        CuisineType = cuisineType;
    }
    
    
    
    
}