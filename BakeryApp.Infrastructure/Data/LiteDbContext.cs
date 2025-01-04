using BakeryApp.Core.Entities;
using LiteDB;
using Microsoft.Extensions.Configuration;
namespace BakeryApp.Infrastructure.Data
{
    public class LiteDbContext
    {
        private readonly string _connectionString;
        private LiteDatabase _database;
        public LiteDbContext(IConfiguration configuation)
        {

            _connectionString = configuation.GetConnectionString("LiteDb");
            EnsureDirectoryExists(_connectionString);
            SeedUsers();
        }
        private void EnsureDirectoryExists(string connectionString)
        {
            var filePath = connectionString.Replace("Filename=", "").Trim();
            var directory = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);

            }

        }
        public LiteDatabase GetDb()
        {
            try
            {

                return new LiteDatabase(_connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

        }
        private void SeedUsers()
        {
            using (var db = GetDb())
            {
                var usersCollection = db.GetCollection<User>("users");
               
               
                if (usersCollection.Count() == 0)
                {
                    var users = new List<User>
                    {
                        new User
                        {
                            Id = Guid.NewGuid(),
                            Username = "admin",
                            Email = "admin@bakery.com",
                            Password = "AQAAAAIAAYagAAAAEOn4Vox1r4QiaxRUg2pG5DppHND4pzEcJfX6I9HR4DG7IQSerKYzwSMR8vfggzvmmA==", // Use a hashed password in a real app
                            Role = UserRole.Admin
                        },
                        new User
                        {
                            Id = Guid.NewGuid(),
                            Username = "owner",
                            Email = "owner@bakery.com",
                            Password = "AQAAAAIAAYagAAAAEJkPfFNYUp9oFjJEuQd7Dko2eWpi4bREXNqZCnWqiYyV5OgvuWAN3ANgZ5Hs6HVXSQ==", // Use a hashed password in a real app
                            Role = UserRole.Owner
                        },
                        new User
                        {
                            Id = Guid.NewGuid(),
                            Username = "test",
                            Email = "test@bakery.com",
                            Password = "AQAAAAIAAYagAAAAEK0geUl0apj2nIfei0kOOEfGQsxKhUHj9V2BeminZ1S0G8AXnnkrOu2PPy/5fRz8gw==", // Use a hashed password in a real app
                            Role = UserRole.Customer
                        }
                    };

                    
                    usersCollection.InsertBulk(users);
                    Console.WriteLine("Seeded users successfully.");
                }

            }
        }
        public void Dispose()
        {
            _database?.Dispose();
        }
    }
}
