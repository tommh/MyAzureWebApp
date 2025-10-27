# Database Setup Guide

## Lokal utvikling

### 1. SQL Server LocalDB
Lokal utvikling bruker SQL Server LocalDB som er inkludert med Visual Studio.

**Connection String:**
```
Server=(localdb)\\mssqllocaldb;Database=VolleyballRotations_Local;Trusted_Connection=true;MultipleActiveResultSets=true
```

**Database er allerede opprettet og migrert!** ✅

### 2. Test lokal tilkobling
```bash
cd MyAzureWebApp
dotnet run
```
Gå til: https://localhost:7246/Clients

---

## Azure SQL Database Setup

### 1. Opprett Azure SQL Database
1. Gå til [Azure Portal](https://portal.azure.com)
2. Søk etter "SQL databases"
3. Klikk "Create"
4. Velg:
   - **Resource Group**: Velg din eksisterende resource group
   - **Database name**: `VolleyballRotations`
   - **Server**: Opprett ny server eller velg eksisterende
   - **Authentication method**: SQL authentication
   - **Server admin login**: `tomm_admin` (eller ditt valg)
   - **Password**: Velg et sterkt passord
   - **Location**: West Europe (sammme som din Web App)

### 2. Konfigurer Firewall
1. Gå til din SQL Server i Azure Portal
2. Klikk "Networking" i venstre meny
3. Under "Firewall rules":
   - Legg til regel for din IP-adresse
   - Aktiver "Allow Azure services and resources to access this server"

### 3. Opprett database schema
```bash
# Sett environment variables
$env:DB_USERNAME="tomm_admin"
$env:DB_PASSWORD="ditt_passord"

# Kjør migration mot Azure
dotnet ef database update --connection "Server=tcp:tomm.database.windows.net,1433;Initial Catalog=VolleyballRotations;Persist Security Info=False;User ID=tomm_admin;Password=ditt_passord;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

### 4. Azure Web App Configuration ✅
**Connection String allerede satt opp i Azure!**

Din Azure Web App har allerede en `DefaultConnection` environment variable med komplett connection string. Koden vil automatisk bruke denne når den kjører på Azure.

**Sjekk at den er satt:**
1. Gå til din Web App i Azure Portal
2. Klikk "Configuration" i venstre meny
3. Under "Connection strings", sjekk at `DefaultConnection` eksisterer

---

## Sikkerhet

### Best Practices
1. **Bruk Azure Key Vault** for produksjon (ikke hardcode passord)
2. **Bruk Managed Identity** for Azure-tjenester
3. **Aktiver Always Encrypted** for sensitive data
4. **Sett opp backup og point-in-time recovery**

### Environment Variables for Azure
```bash
# I Azure Web App Configuration
DB_USERNAME=your_username
DB_PASSWORD=your_secure_password
```

---

## Testing

### Lokalt
- ✅ Database opprettet
- ✅ Migration kjørt
- ✅ Applikasjon kjører på https://localhost:7246

### Azure
- ⏳ Trenger Azure SQL Database opprettet
- ⏳ Trenger environment variables satt
- ⏳ Trenger migration kjørt mot Azure

---

## Troubleshooting

### Vanlige problemer
1. **Connection timeout**: Sjekk firewall rules
2. **Login failed**: Sjekk username/password
3. **Database not found**: Sjekk database navn i connection string
4. **Migration fails**: Sjekk at du har riktige rettigheter

### Debug connection string
```csharp
// Legg til i Program.cs for debugging
Console.WriteLine($"Connection String: {connectionString}");
```
