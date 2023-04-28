Här är min lösning för uppgiften som jag fick, att traversera och spara ner en webbsda lokalt för att sedan kunna öppna den.
Lösningen är baserad på att rekursivt traversera trädet av alla länkar mellan sidor och spara ner dem lokalt. 
Algoritmen för att göra detta är inte särskilt optimerad, vilket gör att det tar en stund att köra, men resultatet ser korrekt ut. 
Optimering kan definitivt göras med hjälp av trådning och liknande tekniker, det enda som görs nu är att anropen är asynkrona, men det hjälper nog lite i alla fall

För att köra applikationen:

 - Ta ner koden och bygg projektet
 - I undermappen bin/Debug/net6.0 kommer du att hitta en exekverbar fil, kör den för att starta programmet. Här skall det finnas en mapp som heter "Books", om den saknas
   så skapa upp den. Det är i den hela den lokala strukturen kommer sparas. 
 - Det kommer ta en stund att köra klart programmet, men även medans det kör kan du börja titta på sidan lokalt. Öppna sidan index.html som sparats direkt under mappen 
   "Books" så kommer du kunna surfa på sidan lokalt, även om du bara kommer åt det data som hunnit sparas ner. 
