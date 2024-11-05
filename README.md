# Ninja Manager

Ninja Manager is een webapplicatie ontwikkeld met ASP.NET MVC voor het beheren van ninja's en hun uitrusting. Gebruikers kunnen hun ninja's verbeteren door gear aan te schaffen in een winkel en hun statistieken bij te houden.

## Projectomschrijving

Deze applicatie stelt gebruikers in staat om:
- Ninja's aan te maken, te bewerken, en verwijderen (CRUD).
- Een inventaris van uitrusting toe te voegen aan ninja’s en deze visueel weer te geven.
- Een winkel te openen waar ninja’s uitrusting kunnen kopen, mits ze genoeg goud hebben.
- Uitrusting te verkopen en het terugbetaalde goud te ontvangen.

## Functionaliteiten

### Ninja
- **CRUD**: Voeg ninja’s toe, bewerk hun eigenschappen en verwijder ze.
- **Inventaris**: Beheer een inventaris van uitrusting, waar elk type uitrusting slechts één keer kan worden toegevoegd.
- **Statistieken**: Bekijk totale strength, intelligence, agility en de waarde in goud op basis van de uitrusting.

### Uitrusting
- **CRUD**: Voeg uitrusting toe, bewerk en verwijder het.
- **Categorieën**: Beschikbaar in de categorieën: Head, Chest, Hand, Feet, Ring, en Necklace.
- **Statistieken**: Elke uitrusting bevat strength, intelligence, agility, en een waarde in goud.

### Winkel
- **Winkelen**: Koop uitrusting voor een ninja met voldoende goud.
- **Filteren**: Bekijk items per categorie.
- **Verkopen**: Verkoop uitrusting om het goud terug te krijgen.

## Technische Informatie

- **Framework**: ASP.NET MVC met Entity Framework voor databasebeheer.
- **Database**: MSSQL (Entity Framework met Model First).
- **Relaties**: Ninja en uitrusting hebben een veel-op-veel relatie.

## Installatie

1. Clone deze repository:
   ```bash
   git clone https://github.com/Riomdrion/NinjaManagerProg5.git
2. Open de oplossing in Visual Studio of rider.
3. zet de database string correct in de appsettings.json
4. Configureer de MSSQL-database en voer eventuele migraties uit.
5. Start de applicatie.
