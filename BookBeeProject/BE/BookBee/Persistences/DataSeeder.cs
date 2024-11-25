using BookBee.Model;
using BookBee.Utilities;

namespace BookBee.Persistences
{
    public static class DataSeeder
    {
        public static async Task SeedData(this DataContext dbContext)
        {
            await dbContext.SeedRoles();
            await dbContext.SeedUsers();
            await dbContext.SeedAddresses();
        }

        private static async Task SeedRoles(this DataContext dbContext)
        {
            if (!dbContext.Roles.Any())
            {
                await dbContext.Roles.AddRangeAsync(new Role
                {
                    Name = "admin"
                }, new Role
                {
                    Name = "user"
                });
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedUsers(this DataContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                const string defaultPassword = "1234@Abcd";
                PasswordHelper.CreatePasswordHash(defaultPassword, out var passwordHash, out var passwordSalt);
                var adminRole = dbContext.Roles.First(r => r.Name == "admin");
                var userRole = dbContext.Roles.First(r => r.Name == "user");
                await dbContext.Users.AddRangeAsync(new User
                {
                    Username = "admin",
                    FirstName = "Admin",
                    LastName = string.Empty,
                    Email = "admin@test.com",
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                    RoleId = dbContext.Roles.First(r => r.Name == "admin").Id
                }, new User
                {
                    Username = "user",
                    FirstName = "User",
                    LastName = string.Empty,
                    Email = "user@test.com",
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                    RoleId = dbContext.Roles.First(r => r.Name == "user").Id
                }, new User
                {
                    Username = "guest",
                    FirstName = "Khách vãng lai",
                    LastName = string.Empty,
                    Email = "guest@test.com",
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                    RoleId = dbContext.Roles.First(r => r.Name == "user").Id
                });
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedAddresses(this DataContext dbContext)
        {
            if (!dbContext.Addresses.Any())
            {
                await dbContext.Addresses.AddRangeAsync(new Address
                {
                    UserId = 1,
                    Name = "Admin",
                    Phone = "0123456789",
                    Street = "Admin Street",
                    City = "Admin City",
                    State = "Admin State",
                    Create = DateTime.Now,
                    Update = DateTime.Now
                }, new Address
                {
                    UserId = 2,
                    Name = "User",
                    Phone = "0123456789",
                    Street = "User Street",
                    City = "User City",
                    State = "User State",
                    Create = DateTime.Now,
                    Update = DateTime.Now
                }, new Address
                {
                    UserId = 3,
                    Name = string.Empty,
                    Phone = string.Empty,
                    Street = string.Empty,
                    City = string.Empty,
                    State = "Mua hàng tại quầy",
                    IsDeleted = true,
                    Create = DateTime.Now,
                    Update = DateTime.Now
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
