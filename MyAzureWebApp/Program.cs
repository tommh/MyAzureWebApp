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
    if (connectionString.Contains("{DB_USERNAME}"))
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

// Add Entity Framework
if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
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
