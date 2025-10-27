using System.ComponentModel.DataAnnotations;

namespace MyAzureWebApp.Models;

// Model for ViStarterMedServe dropdown
public class ViStarterMedServe
{
    public int Verdi { get; set; }
    public string Beskrivelse { get; set; } = string.Empty;
}

// Model for Vår Spiller Rolle dropdown
public class VaarSpillerRolle
{
    public string VaarRolle { get; set; } = string.Empty;
}

// Model for Deres Spiller Rolle dropdown
public class DeresSpillerRolle
{
    public string DeresRolle { get; set; } = string.Empty;
}

// Model for Deres Start Rotasjon dropdown
public class DeresStartRotasjon
{
    public int Verdi { get; set; }
    public string Beskrivelse { get; set; } = string.Empty;
}

// Model for the calculation result
public class OptimalRotasjonResult
{
    public string VaarSpillerRolle { get; set; } = string.Empty;
    public string DeresSpillerRolle { get; set; } = string.Empty;
    public int OptimalVaarStartRotasjon { get; set; }
    public int DeresStartRotasjon { get; set; }
    public string HvemStarterServe { get; set; } = string.Empty;
    public int AntallMatchups { get; set; }
    public int VaartAntallMedLeggerFremme { get; set; }
    public int DeresAntallMedLeggerFremme { get; set; }
}

// Model for the form input
public class MatchRolleInput
{
    [Required(ErrorMessage = "Du må velge hvem som starter med serve")]
    [Range(0, 1, ErrorMessage = "Ugyldig verdi for serve starter")]
    public int? ViStarterServe { get; set; }
    
    [Required(ErrorMessage = "Du må velge vår spiller rolle")]
    public string VaarSpillerRolle { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Du må velge deres spiller rolle")]
    public string DeresSpillerRolle { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Du må velge deres start rotasjon")]
    [Range(1, 6, ErrorMessage = "Start rotasjon må være mellom 1 og 6")]
    public int? DeresStartRotasjon { get; set; }
}