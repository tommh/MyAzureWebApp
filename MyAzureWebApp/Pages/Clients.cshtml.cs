using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyAzureWebApp.Data;
using MyAzureWebApp.Models;

namespace MyAzureWebApp.Pages;

public class ClientsModel : PageModel
{
    private readonly ApplicationDbContext? _context;
    private readonly ILogger<ClientsModel> _logger;

    public ClientsModel(ApplicationDbContext? context, ILogger<ClientsModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<Client> Clients { get; set; } = new List<Client>();
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        if (_context == null)
        {
            ErrorMessage = "Database connection is not available.";
            Clients = new List<Client>();
            return;
        }

        try
        {
            // Execute the SQL query: SELECT TOP (1000) [ClientID], [ClientName] FROM [dbo].[Client]
            Clients = await _context.Clients
                .Take(1000)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading clients from database");
            ErrorMessage = "Error loading clients from database. Please check the database connection.";
            Clients = new List<Client>();
        }
    }
}
