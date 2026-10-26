# Review

## Klasser

### Vehicle
Basklass som innehåller allt som är gemensamt med de andra fordonen. Båt är undantaget.

Underklasser till Vehicle är: Car.cs, Airplane.cs, Boat.cs, Motorcycle.cs, Bus.cs

### Garage
Jag har skapat en klass för separata Garage

samt en samlingsklass för flera Garage

### General
Här ligger Enums och sökklassen

**Dessa hittar ni i Domain-mappen**

ConsoleUtilities hanterar inputvalideringar samt tabeller och andra utskrifter som är console-relaterat.

CRUD funktionaliteten ligger mestadels inom Service-mappen.

## Jag önskar...
att ni har extra koll på om emojisar visas,
hur jag kan strukturera bättre klasser,
syntax och generella förbättringar

## Reviews
1: Förslag om bättre struktur och seperation mellan ui och businesslogik
2: Over-engineered
3: Hantera validering av registreringsskyltar tidigare
4: **Refactorering** Ta bort svenska felmeddelanden för json-hantering
5: **Buggfix** Jag hade missat att lägga till options så det visades ett felmeddelande med att visa alla/efter typ fordon
6: **Buggfix** Jag hade av misstag råkat ta bort `-1` vid optionshantering vilket resulterade i att fel garage visades.
7: **Refactorering** Emojisar för car och bus kunde inte visas så jag lägger till text för stats 

Ändringar jag har implementerat:
4
5
6
7

Jag väljer att inte lägga fokus på punkt 1-3 då dem går hand i hand och jag tar med mig dem till nästa version.