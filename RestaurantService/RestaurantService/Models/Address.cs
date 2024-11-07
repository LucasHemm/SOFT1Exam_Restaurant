using RestaurantService.DTOs;

namespace RestaurantService.Models;

public class Address
{
    //id
    public int Id { get; set; } //primary key
    public String Street { get; set; }
    public String City { get; set; }
    public String ZipCode{ get; set; }
    public String Region { get; set; }
    
    
    public Address(int id, String street, String city, String zipCode, String region)
    {
        Id = id;
        Street = street;
        City = city;
        ZipCode = zipCode;
        Region = region;
    }
    //empty
    public Address(AddressDTO restaurantDtoAddress)
    {
        Id = restaurantDtoAddress.Id;
        Street = restaurantDtoAddress.Street;
        City = restaurantDtoAddress.City;
        ZipCode = restaurantDtoAddress.ZipCode;
        Region = restaurantDtoAddress.Region;
    }
    
    //for creating a new address
    public Address(String street, String city, String zipCode, String region)
    {
        Street = street;
        City = city;
        ZipCode = zipCode;
        Region = region;
    }
    
}