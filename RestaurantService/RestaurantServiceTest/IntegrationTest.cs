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
using Address = Docker.DotNet.Models.Address;

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

            RestaurantDTO restaurantDto = new RestaurantDTO(0,"McDonalds", new AddressDTO("123 Main St", "Springfield", "12345", "IL"), 4.5, 100, "Fast Food");
            Restaurant restaurant = restaurantFacade.CreateRestaurant(restaurantDto);
            Restaurant createdRestaurant = context.Restaurants.Find(restaurant.Id);
            Assert.NotNull(createdRestaurant);
            Assert.Equal(restaurantDto.Name, createdRestaurant.Name);
            
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

            RestaurantDTO restaurantDto = new RestaurantDTO(0,"McDonalds", new AddressDTO("123 Main St", "Springfield", "12345", "IL"), 4.5, 100, "Fast Food");
            Restaurant restaurant = restaurantFacade.CreateRestaurant(restaurantDto);
            restaurantDto = new RestaurantDTO(restaurantFacade.GetRestaurant(restaurant.Id));
            restaurantDto.Id = restaurant.Id;
            restaurantDto.Name = "Burger King";
            restaurantDto.Address.Street = "456 Elm St";
            restaurantDto.Address.City = "Chicago";
            restaurantDto.Address.ZipCode = "54321";
            restaurantDto.Address.Region = "IL";
            restaurantDto.Rating = 4.0;
            restaurantDto.NumberOfRatings = 50;
            restaurantDto.CuisineType = "Fast Food";
            restaurantFacade.UpdateRestaurant(restaurantDto);
            Restaurant updatedRestaurant = context.Restaurants.Find(restaurant.Id);
            Assert.NotNull(updatedRestaurant);
            Assert.Equal(restaurantDto.Name, updatedRestaurant.Name);
            Assert.Equal(restaurantDto.Address.Street, updatedRestaurant.Address.Street);
            Assert.Equal(restaurantDto.Address.City, updatedRestaurant.Address.City);
            Assert.Equal(restaurantDto.Address.ZipCode, updatedRestaurant.Address.ZipCode);
            Assert.Equal(restaurantDto.Address.Region, updatedRestaurant.Address.Region);
            Assert.Equal(restaurantDto.Rating, updatedRestaurant.Rating);
            Assert.Equal(restaurantDto.NumberOfRatings, updatedRestaurant.NumberOfRatings);
            Assert.Equal(restaurantDto.CuisineType, updatedRestaurant.CuisineType);
           
        }
    }
    
    [Fact]
    public void ShouldGetRestaurant()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            RestaurantFacade restaurantFacade = new RestaurantFacade(context);

            RestaurantDTO restaurantDto = new RestaurantDTO(0,"McDonalds", new AddressDTO("123 Main St", "Springfield", "12345", "IL"), 4.5, 100, "Fast Food");
            Restaurant restaurant = restaurantFacade.CreateRestaurant(restaurantDto);
            Restaurant createdRestaurant = restaurantFacade.GetRestaurant(restaurant.Id);
            Assert.NotNull(createdRestaurant);
            Assert.Equal(restaurantDto.Name, createdRestaurant.Name);
            
        }
    }
    
    [Fact]
    public void ShouldThrowExceptionWhenRestaurantNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            RestaurantFacade restaurantFacade = new RestaurantFacade(context);

            Assert.Throws<Exception>(() => restaurantFacade.GetRestaurant(1));
            
        }
    }
    
    [Fact]
    public void ShouldCreateMenuItem()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            RestaurantFacade restaurantFacade = new RestaurantFacade(context);

            RestaurantDTO restaurantDto = new RestaurantDTO(0,"McDonalds", new AddressDTO("123 Main St", "Springfield", "12345", "IL"), 4.5, 100, "Fast Food");
            Restaurant restaurant = restaurantFacade.CreateRestaurant(restaurantDto);
            MenuItemDTO menuItemDto = new MenuItemDTO(0, "Big Mac", 4.99,"this is a burger",restaurant.Id,"image");
            MenuItem menuItem = restaurantFacade.CreateMenuItem(menuItemDto);
            MenuItem createdMenuItem = context.MenuItems.Find(menuItem.Id);
            Assert.NotNull(createdMenuItem);
            Assert.Equal(menuItemDto.ItemName, createdMenuItem.ItemName);
            
        }
    }
}