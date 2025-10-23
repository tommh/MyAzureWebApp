using Microsoft.EntityFrameworkCore;
using MyAzureWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure database connection with environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString))
{
    // Replace placeholders with environment variables
    var dbUsername = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "your_username";
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "your_password";
    
    connectionString = connectionString
        .Replace("{DB_USERNAME}", dbUsername)
        .Replace("{DB_PASSWORD}", dbPassword);
}

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

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
