using Microsoft.EntityFrameworkCore;
using MyAzureWebApp.Data;

namespace MyAzureWebApp;

public static class TestConnection
{
    public static async Task<bool> TestDatabaseConnection(string connectionString)
    {
        try
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var context = new ApplicationDbContext(options);
            
            // Prøv å koble til databasen
            await context.Database.CanConnectAsync();
            
            Console.WriteLine("✅ Database connection successful!");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Database connection failed: {ex.Message}");
            return false;
        }
    }
}
