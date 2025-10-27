# Azure Database Migration Guide

## Problem
Du får feilmelding: "Database Feil - Error loading clients from database. Please check the database connection."

## Løsning
Du må kjøre database migration mot Azure SQL Database.

## Steg-for-steg guide:

### 1. Hent Connection String fra Azure Portal
1. Gå til [Azure Portal](https://portal.azure.com)
2. Finn din Web App "Rotations" (rotations-h6fpbmavafbzebca)
3. Klikk "Configuration" i venstre meny
4. Under "Connection strings", finn `DefaultConnection`
5. Klikk på "Value" for å se hele connection string
6. **Kopier hele connection string** (den starter med "Server=tcp:...")

### 2. Kjør Migration mot Azure
Åpne PowerShell i `MyAzureWebApp` mappen og kjør:

```powershell
# Erstatt "DIN_CONNECTION_STRING_HER" med den faktiske connection string fra Azure
dotnet ef database update --connection "DIN_CONNECTION_STRING_HER"
```

**Eksempel:**
```powershell
dotnet ef database update --connection "Server=tcp:tomm.database.windows.net,1433;Initial Catalog=Tomm_SQL_Server;Persist Security Info=False;User ID=tomm_admin;Password=din_password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

### 3. Test Resultatet
Etter migration er ferdig:
1. Gå til https://rotations-h6fpbmavafbzebca.westeurope-01.azurewebsites.net/Clients
2. Du skal nå se en tom tabell (ingen feilmelding)
3. For å teste, kan du legge til test-data direkte i Azure Portal

## Alternativ: Legg til test-data

### Via Azure Portal:
1. Gå til din SQL Database i Azure Portal
2. Klikk "Query editor" i venstre meny
3. Logg inn med SQL server credentials
4. Kjør denne SQL:

```sql
INSERT INTO Clients (ClientName) VALUES 
('Test Klient 1'),
('Test Klient 2'),
('Volleyball Lag A'),
('Volleyball Lag B');
```

### Via SQL Server Management Studio:
1. Åp SSMS
2. Koble til din Azure SQL server
3. Velg din database
4. Kjør INSERT kommandoen over

## Feilsøking

### Hvis migration feiler:
- Sjekk at connection string er korrekt
- Sjekk at SQL server tillater tilkobling fra din IP
- Sjekk at username/password er riktig

### Hvis du fortsatt får feil:
- Sjekk Azure Portal → Web App → Logs for mer detaljer
- Prøv å koble til databasen med SQL Server Management Studio først

## Suksess!
Når migration er ferdig, vil `/Clients` siden fungere perfekt! 🎉
