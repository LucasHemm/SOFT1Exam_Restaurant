namespace RestaurantService.DTOs;

public class MenuItemDTO
{
    public int Id { get; set; } //primary key
    public String ItemName { get; set; }
    public double Price  { get; set; }
    public String ItemDescription { get; set; }
    
    public MenuItemDTO(int id, String itemName, double price, String itemDescription)
    {
        Id = id;
        ItemName = itemName;
        Price = price;
        ItemDescription = itemDescription;
    }
    
    public MenuItemDTO()
    {
        
    }
    
    //for creating a new menu item
    public MenuItemDTO(String itemName, double price, String itemDescription)
    {
        ItemName = itemName;
        Price = price;
        ItemDescription = itemDescription;
    }

}