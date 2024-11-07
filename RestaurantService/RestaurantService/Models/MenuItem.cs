namespace RestaurantService.Models;

public class MenuItem
{
    public int Id { get; set; } //primary key
    public String ItemName { get; set; }
    public double Price  { get; set; }
    public String ItemDescription { get; set; }
    
    public MenuItem(int id, String itemName, double price, String itemDescription)
    {
        Id = id;
        ItemName = itemName;
        Price = price;
        ItemDescription = itemDescription;
    }
    
    public MenuItem()
    {
        
    }
    
    //for creating a new menu item
    public MenuItem(String itemName, double price, String itemDescription)
    {
        ItemName = itemName;
        Price = price;
        ItemDescription = itemDescription;
    }
    
    
    
    
    
}