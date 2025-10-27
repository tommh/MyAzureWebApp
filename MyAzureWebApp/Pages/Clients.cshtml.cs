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
            // Execute the specific SQL query: SELECT [ClientID], [ClientName] FROM [Tomm_SQL_Server].dbo.[Client]
            Clients = await _context.Clients
                .FromSqlRaw("SELECT [ClientID], [ClientName] FROM [Tomm_SQL_Server].dbo.[Client]")
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
