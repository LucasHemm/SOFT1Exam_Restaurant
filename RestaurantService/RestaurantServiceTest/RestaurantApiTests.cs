using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestaurantService;
using RestaurantService.DTOs;
using RestaurantService.Models;
using Testcontainers.MsSql;

namespace RestaurantServiceTest;

public class RestaurantApiTests: IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer;
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        public RestaurantApiTests()
        {
            _msSqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest") // Use the correct SQL Server image
                .WithPassword("YourStrong!Passw0rd") // Set a strong password
                .WithCleanUp(true)
                .Build();

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove the existing ApplicationDbContext registration
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<ApplicationDbContext>));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        // Add ApplicationDbContext using the test container's connection string
                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseSqlServer(_msSqlContainer.GetConnectionString());
                        });

                        // Ensure the database is created and migrations are applied
                        var sp = services.BuildServiceProvider();
                        using (var scope = sp.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            db.Database.Migrate();
                        }
                    });
                });
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();
            _client = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            _client.Dispose();
            await _msSqlContainer.DisposeAsync();
            _factory.Dispose();
        }

        private StringContent GetStringContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        
          [Fact]
        public async Task CreateRestaurant_ShouldReturnOk_WhenCreationIsSuccessful()
        {
            // Arrange
            RestaurantDTO restaurantDto = new RestaurantDTO(0,"McDonalds", new AddressDTO("123 Main St", "Springfield", "12345", "IL"), 4.5, 100, "Fast Food","test","12345678");

            // Act
            var response = await _client.PostAsJsonAsync("/api/RestaurantApi", restaurantDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Restaurant created successfully", result);
        }

        [Fact]
        public async Task UpdateRestaurant_ShouldReturnOk_WhenUpdateIsSuccessful()
        {
            RestaurantDTO restaurantDto1 = new RestaurantDTO(0,"McDonalds", new AddressDTO("123 Main St", "Springfield", "12345", "IL"), 4.5, 100, "Fast Food","test","12345678");

            // Act
            var response1 = await _client.PostAsJsonAsync("/api/RestaurantApi", restaurantDto1);
            // Arrange
            RestaurantDTO restaurantDto2 = new RestaurantDTO(1,"McDonalds2", new AddressDTO("123 Main St2", "Springfield2", "123456", "ILl"), 5, 1000, "Fast 1Food","tes1t","123145678");

            // Act
            var response2 = await _client.PutAsJsonAsync("/api/RestaurantApi", restaurantDto2);

            // Assert
            response2.EnsureSuccessStatusCode();
            var result = await response2.Content.ReadAsStringAsync();
            Assert.Equal("Restaurant updated successfully", result);
        }

        [Fact]
        public async Task CreateMenuItem_ShouldReturnOk_WhenCreationIsSuccessful()
        {
            // Arrange: First, create a restaurant
            var restaurantDto = new RestaurantDTO(0, "McDonalds", 
                new AddressDTO("123 Main St", "Springfield", "12345", "IL"), 
                4.5, 100, "Fast Food", "test", "12345678");

            var restaurantResponse = await _client.PostAsJsonAsync("/api/RestaurantApi", restaurantDto);
            restaurantResponse.EnsureSuccessStatusCode();

            // Now create a MenuItem for the existing restaurant
            var menuItemDto = new MenuItemDTO(0, "Big Mac", 4.99, "this is a burger", 1, "image");

            // Act
            var response = await _client.PostAsJsonAsync("/api/RestaurantApi/MenuItem", menuItemDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Menu item created successfully", result);
        }

        [Fact]
        public async Task UpdateMenuItem_ShouldReturnOk_WhenUpdateIsSuccessful()
        {
            // Arrange: First, create a restaurant
            var restaurantDto = new RestaurantDTO(0, "McDonalds", 
                new AddressDTO("123 Main St", "Springfield", "12345", "IL"), 
                4.5, 100, "Fast Food", "test", "12345678");

            var restaurantResponse = await _client.PostAsJsonAsync("/api/RestaurantApi", restaurantDto);
            restaurantResponse.EnsureSuccessStatusCode();

            // Then, create an initial MenuItem
            var initialMenuItemDto = new MenuItemDTO(0, "Big Mac", 4.99, "this is a burger", 1, "image");
            var createMenuItemResponse = await _client.PostAsJsonAsync("/api/RestaurantApi/MenuItem", initialMenuItemDto);
            createMenuItemResponse.EnsureSuccessStatusCode();

            // Update the MenuItem
            var updatedMenuItemDto = new MenuItemDTO(1, "Big Mac Updated", 5.49, "Updated burger description", 1, "updated_image");

            // Act
            var response = await _client.PutAsJsonAsync("/api/RestaurantApi/MenuItem", updatedMenuItemDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Menu item updated successfully", result);
        }
        
        [Fact]
        public async Task GetRestaurant_ShouldReturnOk_WhenRestaurantExists()
        {
            // Arrange
            RestaurantDTO restaurantDto = new RestaurantDTO(0,"McDonalds", new AddressDTO("123 Main St", "Springfield", "12345", "IL"), 4.5, 100, "Fast Food","test","12345678");

           
            var response1 = await _client.PostAsJsonAsync("/api/RestaurantApi", restaurantDto);
            
            response1.EnsureSuccessStatusCode();
            // Act: Send a GET request to retrieve the restaurant by ID
            var response = await _client.GetAsync($"/api/RestaurantApi/1");

            // Assert: Verify response is 200 OK and contains the restaurant data
            response.EnsureSuccessStatusCode();
            var restaurant = await response.Content.ReadFromJsonAsync<Restaurant>();
            Assert.NotNull(restaurant);
            Assert.Equal(restaurantDto.Name, restaurant.Name);
        }
        
        [Fact]
        public async Task GetAllRestaurants_ShouldReturnOK()
        {
            // Act: Send a GET request to retrieve all restaurants
            var response = await _client.GetAsync("/api/RestaurantApi");

            // Assert: Verify response is 200 OK
            response.EnsureSuccessStatusCode();
        }
}