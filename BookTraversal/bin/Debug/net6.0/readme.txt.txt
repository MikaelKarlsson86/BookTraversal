Här är min lösning för uppgiften som jag fick, att traversera och spara ner en webbsda lokalt för att sedan kunna öppna den.
Lösningen är baserad på att rekursivt traversera trädet av alla länkar mellan sidor och spara ner dem lokalt. 
Algoritmen för att göra detta är inte särskilt optimerad, vilket gör att det tar en stund att köra, men resultatet ser korrekt ut. 
Optimering kan definitivt göras med hjälp av trådning och liknande tekniker, det enda som görs nu är att anropen är asynkrona, men det hjälper nog lite i alla fall

För att köra applikationen:

 - Kör programmet som ligger med i denna mapp, och data som hämtas kommer att sparas ner i en struktur under mappen "Books"
 - Det kommer ta en stund tills alla data är sparad, men det går att gå in i mappen "Books" och navigera runt bland den data som har hämtats
 - För att öppna sidan lokalt, öppna index.html som ligger direkt under "Books"