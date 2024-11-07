namespace RestaurantService.DTOs;

public class RestaurantDTO
{
    public int Id { get; set; } //primary key
    public String Name { get; set; }
    public AddressDTO Address { get; set; }
    public List<MenuItemDTO>? MenuItems { get; set; }
    public double Rating { get; set; }
    public String CuisineType { get; set; }
    
    public RestaurantDTO(int id, String name, AddressDTO address, List<MenuItemDTO> menuItems, double rating, String cuisineType)
    {
        Id = id;
        Name = name;
        Address = address;
        MenuItems = menuItems;
        Rating = rating;
        CuisineType = cuisineType;
    }
    
    public RestaurantDTO()
    {
        
    }
    
    //for creating a new restaurant
    public RestaurantDTO(String name, AddressDTO address, double rating, String cuisineType)
    {
        Name = name;
        Address = address;
        Rating = rating;
        CuisineType = cuisineType;
    }
}