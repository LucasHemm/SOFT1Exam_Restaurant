using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantService;
using RestaurantService.API;
using RestaurantService.DTOs;
using RestaurantService.Facades;
using RestaurantService.Models;
using Testcontainers.MsSql;

namespace RestaurantServiceTest;

public class IntegrationTest : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest") // Use the correct SQL Server image
        .WithPassword("YourStrong!Passw0rd") // Set a strong password
        .Build();

    private string _connectionString;

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        // Create the connection string for the database
        _connectionString = _msSqlContainer.GetConnectionString();

        // Initialize the database context and apply migrations
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            context.Database.Migrate(); // Apply any pending migrations
        }
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync().AsTask();
    }

    
    [Fact]
    public void ShouldCreateRestaurant()
    {
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            RestaurantFacade restaurantFacade = new RestaurantFacade(context);
            RestaurantDTO restaurantDto = new RestaurantDTO("Test Restaurant", new AddressDTO(0, "Test Street", "Test City", "12345", "Test Region"), 5, "Test Cuisine");
            Restaurant restaurant = restaurantFacade.CreateRestaurant(restaurantDto);
            Restaurant createdRestaurant = context.Restaurants.Find(restaurant.Id);
            Assert.NotNull(createdRestaurant);
            Assert.Equal("Test Restaurant", createdRestaurant.Name);
        }
    }
    
    [Fact]
    public void ShouldUpdateRestaurant()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            RestaurantFacade restaurantFacade = new RestaurantFacade(context);
            RestaurantDTO restaurantDto = new RestaurantDTO("Test Restaurant", new AddressDTO(0, "Test Street", "Test City", "12345", "Test Region"), 5, "Test Cuisine");
            Restaurant restaurant = restaurantFacade.CreateRestaurant(restaurantDto);
            restaurantDto.Id = restaurant.Id;
            restaurantDto.Name = "Updated Restaurant";
            restaurantDto.Address = new AddressDTO(0, "Updated Street", "Updated City", "54321", "Updated Region");
            restaurantDto.Rating = 4;
            restaurantDto.CuisineType = "Updated Cuisine";
            restaurantFacade.UpdateRestaurant(restaurantDto);
            Restaurant updatedRestaurant = context.Restaurants.Find(restaurant.Id);
            Assert.NotNull(updatedRestaurant);
            Assert.Equal("Updated Restaurant", updatedRestaurant.Name);
            Assert.Equal("Updated Street", updatedRestaurant.Address.Street);
            Assert.Equal("Updated City", updatedRestaurant.Address.City);
            Assert.Equal("54321", updatedRestaurant.Address.ZipCode);
            Assert.Equal("Updated Region", updatedRestaurant.Address.Region);
            Assert.Equal(4, updatedRestaurant.Rating);
            Assert.Equal("Updated Cuisine", updatedRestaurant.CuisineType);
        }
    }
}