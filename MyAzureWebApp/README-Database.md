# Database Configuration

## Sikker konfigurasjon av database tilkobling

For å koble til SQL Server databasen på `tomm.database.windows.net`, må du sette opp miljøvariabler.

### Lokal utvikling

1. **Windows (PowerShell):**
```powershell
$env:DB_USERNAME="din_username"
$env:DB_PASSWORD="ditt_passord"
```

2. **Windows (Command Prompt):**
```cmd
set DB_USERNAME=din_username
set DB_PASSWORD=ditt_passord
```

3. **Visual Studio:**
   - Høyreklikk på prosjektet → Properties → Debug → Environment Variables
   - Legg til:
     - `DB_USERNAME` = din_username
     - `DB_PASSWORD` = ditt_passord

### Azure App Service

I Azure App Service, legg til disse som Application Settings:

1. Gå til din App Service i Azure Portal
2. Settings → Configuration → Application settings
3. Legg til:
   - `DB_USERNAME` = din_username
   - `DB_PASSWORD` = ditt_passord

### Alternativ: Azure Key Vault

For produksjon, anbefales det å bruke Azure Key Vault:

1. Opprett en Key Vault i Azure
2. Legg til secrets for username og password
3. Konfigurer App Service til å hente secrets fra Key Vault

## Sikkerhet

- **ALDRIG** legg sensitive data direkte i `appsettings.json`
- **ALDRIG** commit miljøvariabler til Git
- Bruk Azure Key Vault for produksjon
- Roter passord regelmessig
