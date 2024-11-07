using RestaurantService.DTOs;

namespace RestaurantService.Models;

public class MenuItem
{
    public int Id { get; set; } //primary key
    public String ItemName { get; set; }
    public double Price  { get; set; }
    public String ItemDescription { get; set; }
    public string Image { get; set; }
    public Restaurant Restaurant { get; set; }
    
    
    public MenuItem(int id, String itemName, double price, String itemDescription, string image, Restaurant restaurant)
    {
        Id = id;
        ItemName = itemName;
        Price = price;
        ItemDescription = itemDescription;
        Image = image;
        Restaurant = restaurant;
    }
    
    public MenuItem()
    {
    }
    
    public MenuItem(MenuItemDTO menuItemDto, Restaurant restaurant)
    {
        Id = menuItemDto.Id;
        ItemName = menuItemDto.ItemName;
        Price = menuItemDto.Price;
        ItemDescription = menuItemDto.ItemDescription;
        Image = menuItemDto.Image;
        Restaurant = restaurant;
    }
    
    
    

}