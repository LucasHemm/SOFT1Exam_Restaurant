﻿using Microsoft.EntityFrameworkCore;
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
        Address address = (from a in _context.Addresses where a.City == restaurantDto.Address.City && a.Street == restaurantDto.Address.Street && a.ZipCode == restaurantDto.Address.ZipCode select a).FirstOrDefault() ?? new Address(restaurantDto.Address);
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
        UpdateAddress(restaurantDto.Address,restaurant);
        restaurant.Rating = restaurantDto.Rating;
        restaurant.CuisineType = restaurantDto.CuisineType;
        restaurant.NumberOfRatings = restaurantDto.NumberOfRatings;
        _context.SaveChanges();
        return restaurant;
    }
    
    public List<Restaurant> GetAllRestaurants()
    {
        return _context.Restaurants
            .Include(restaurant => restaurant.Address)
            .Include(restaurant => restaurant.MenuItems)
            .ToList();
    }
    
    private void UpdateAddress(AddressDTO addressDto,Restaurant restaurant)
    {
        restaurant.Address.Street = string.IsNullOrEmpty(addressDto.Street) ? restaurant.Address.Street : addressDto.Street;
        restaurant.Address.City = addressDto.City;
        restaurant.Address.ZipCode = addressDto.ZipCode;
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