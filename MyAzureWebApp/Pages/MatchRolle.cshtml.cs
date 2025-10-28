using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyAzureWebApp.Data;
using MyAzureWebApp.Models;
using System.Linq;

namespace MyAzureWebApp.Pages;

public class MatchRolleModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MatchRolleModel> _logger;

    public MatchRolleModel(ApplicationDbContext context, ILogger<MatchRolleModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public MatchRolleInput Input { get; set; } = new();

    public List<ViStarterMedServe> ViStarterMedServeList { get; set; } = new();
    public List<VaarSpillerRolle> VaarSpillerRolleList { get; set; } = new();
    public List<DeresSpillerRolle> DeresSpillerRolleList { get; set; } = new();
    public List<DeresStartRotasjon> DeresStartRotasjonList { get; set; } = new();
    
    public OptimalRotasjonResult? Result { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        await LoadDropdownDataAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownDataAsync();
            return Page();
        }

        try
        {
            await LoadDropdownDataAsync();
            
            // Execute the stored procedure
            var results = _context.Database
                .SqlQueryRaw<OptimalRotasjonResult>(
                    "EXEC [dbo].[FinnOptimalStartRotasjon] @ViStarterServe = {0}, @VaarSpillerRolle = {1}, @DeresSpillerRolle = {2}, @DeresStartRotasjon = {3}",
                    Input.ViStarterServe!.Value,
                    Input.VaarSpillerRolle,
                    Input.DeresSpillerRolle,
                    Input.DeresStartRotasjon!.Value)
                .AsEnumerable()
                .FirstOrDefault();

            if (results != null)
            {
                Result = results;
            }
            else
            {
                ErrorMessage = "Ingen resultat funnet for de valgte parameterne.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing FinnOptimalStartRotasjon stored procedure");
            ErrorMessage = $"Feil ved beregning: {ex.Message}";
        }

        return Page();
    }

    private async Task LoadDropdownDataAsync()
    {
        try
        {
            // Load ViStarterMedServe data
            ViStarterMedServeList = _context.Database
                .SqlQueryRaw<ViStarterMedServe>("EXEC [dbo].[ListViStarterMedServe]")
                .AsEnumerable()
                .ToList();

            // Load VaarSpillerRolle data
            VaarSpillerRolleList = _context.Database
                .SqlQueryRaw<VaarSpillerRolle>("EXEC [dbo].[ListVaarSpillerRolle]")
                .AsEnumerable()
                .ToList();

            // Load DeresSpillerRolle data
            DeresSpillerRolleList = _context.Database
                .SqlQueryRaw<DeresSpillerRolle>("EXEC [dbo].[ListDeresSpillerRolle]")
                .AsEnumerable()
                .ToList();

            // Load DeresStartRotasjon data
            DeresStartRotasjonList = _context.Database
                .SqlQueryRaw<DeresStartRotasjon>("EXEC [dbo].[ListDeresStartRotasjon]")
                .AsEnumerable()
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading dropdown data");
            ErrorMessage = "Feil ved lasting av dropdown-data fra databasen.";
        }
    }
}
