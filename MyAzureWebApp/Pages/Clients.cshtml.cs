using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyAzureWebApp.Data;
using MyAzureWebApp.Models;

namespace MyAzureWebApp.Pages;

public class ClientsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ClientsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Client> Clients { get; set; } = new List<Client>();

    public async Task OnGetAsync()
    {
        try
        {
            // Execute the SQL query: SELECT TOP (1000) [ClientID], [ClientName] FROM [dbo].[Client]
            Clients = await _context.Clients
                .Take(1000)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            // For now, we'll just set an empty list
            Clients = new List<Client>();
        }
    }
}
