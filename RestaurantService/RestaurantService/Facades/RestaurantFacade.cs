﻿using RestaurantService.DTOs;
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
        Restaurant restaurant = new Restaurant(restaurantDto.Name, address, restaurantDto.Rating, restaurantDto.CuisineType);
        _context.Restaurants.Add(restaurant);
        _context.SaveChanges();
        return restaurant;
    }
    
    public Restaurant GetRestaurant(int id)
    {
        Restaurant restaurant = _context.Restaurants.Find(id);
        if (restaurant == null)
        {
            throw new Exception("Restaurant not found");
        }
        return restaurant;
    }
    
    public Restaurant UpdateRestaurant(RestaurantDTO restaurantDto)
    {
        Restaurant restaurant = GetRestaurant(restaurantDto.Id);
        Address address = _context.Addresses.Find(restaurantDto.Address.Id) ?? new Address(restaurantDto.Address);
        restaurant.Name = restaurantDto.Name;
        restaurant.Address = address;
        restaurant.Rating = restaurantDto.Rating;
        restaurant.CuisineType = restaurantDto.CuisineType;
        _context.SaveChanges();
        return restaurant;
    }

}