using Microsoft.EntityFrameworkCore;
using MyAzureWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// For Azure deployment, check if we have a complete connection string from environment
if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("{DB_USERNAME}"))
{
    // Try to get the complete connection string from Azure environment variable
    connectionString = Environment.GetEnvironmentVariable("DefaultConnection") ?? connectionString;
    
    // If still has placeholders, replace with individual environment variables
    if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("{DB_USERNAME}"))
    {
        var dbUsername = Environment.GetEnvironmentVariable("DB_USERNAME") ?? 
                        Environment.GetEnvironmentVariable("AZURE_SQL_USERNAME") ?? 
                        "your_username";
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? 
                        Environment.GetEnvironmentVariable("AZURE_SQL_PASSWORD") ?? 
                        "your_password";
        
        connectionString = connectionString
            .Replace("{DB_USERNAME}", dbUsername)
            .Replace("{DB_PASSWORD}", dbPassword);
    }
}

// Add Entity Framework with error handling
if (!string.IsNullOrEmpty(connectionString))
{
    try
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        // Test connection in development
        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine($"Connection String: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");
        }
    }
    catch (Exception ex)
    {
        // Log the error but don't crash the app
        Console.WriteLine($"Error configuring database: {ex.Message}");
    }
}
else
{
    Console.WriteLine("Warning: No database connection string found. Database features will not be available.");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
