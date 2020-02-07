# Feladat 4: Opcionális feladat (3 iMsc pont)

A task-ok listázása nem praktikus, ha sok van belőle, minden elemet nem célszerű egy http válaszban visszaadni. Implementálj lapozást erre a funkcióra az alábbi követelményekkel.

- Determinisztikusan az ID alapján sorrendben adja vissza az elemeket.
- A kérésben `count` nevű query paraméterben megadott darabszámú elemet ad vissza minden lapon.
- A következő lapot egy `from` érték bemondásával lehessen lekérni. Ezen `from` a lapozásban a soron következő elem azonosítója.
- A http kérés két paramétere `from` és `count` query paraméterben érkezzen.
- A lapozás a `GET /api/tasks/paged` címen legyen elérhető (szükséges a `/paged` is, hogy a korábbi listázás művelet is megmaradjon és a kettő eltérő URL-en legyen.)
- A lapozás válasza a `Controllers.Dto.PagedTaskList` osztály példánya legyen. Ebben szerepel:
  - a lapon található elemek tömbje (`items`),
  - a lapon található elemek száma (`count`)
  - a következő lap lekéréséhez szükséges `from` érték (`nextId`),
  - és segítségként az URL, amivel a következő lap lekérhető (`nextUrl`).
    - Az Url előállításához használd a controller osztályon elérhető `Url.Action` segédfüggvényt. Ne égesd be a kódba se a "localhost:5000", se a "/api/tasks/paged" URL részleteket! Az URL előállításához _nem_ string műveletekre lesz szükséged!
- Mindig 200 OK válasszal tér vissza, csak legfeljebb üres a visszaadott válaszban az elemek tömbje.

Az alábbi kérés-válasz sorozat mutatja a működést:

1. `GET /api/tasks/paged?count=2`

   Ez az első kérés. Itt nincs `from` paraméter, ez a legelső elemtől indul.

   Válasz:

   ```json
   {
     "items": [
       {
         "id": 1,
         "title": "doing homework",
         "done": false,
         "statusz": "pending"
       },
       {
         "id": 2,
         "title": "doing more homework",
         "done": false,
         "statusz": "new"
       }
     ],
     "count": 2,
     "nextId": 3,
     "nextUrl": "http://localhost:5000/api/tasks/paged?from=3&count=2"
   }
   ```

2. `GET /api/tasks/paged?from=3&count=2`

   Ez a második lap tartalmának lekérése.

   Válasz:

   ```json
   {
     "items": [
       {
         "id": 3,
         "title": "hosework",
         "done": true,
         "statusz": "done"
       }
     ],
     "count": 1,
     "nextId": null,
     "nextUrl": null
   }
   ```

   A válasz mutatja, hogy nincs további lap, mind a `nextId`, mind a `nextUrl` null.

3. `GET /api/tasks/paged?from=999&count=999`

   Ez egy üres lap.

   Válasz:

   ```json
   {
     "items": [],
     "count": 0,
     "nextId": null,
     "nextUrl": null
   }
   ```

Készíts egy képernyőképet Postman-ből (avagy más, hasonló eszközből, ha nem Postman-t használtál), amely egy tetszőleges REST kérést és választ mutat. A képernyőképpel kapcsolatos elvárásokról lásd a kezdőoldalon.

> A képet a megoldásban `f4.png` néven add be. A képernyőképen látszódjon a kimenő kérés és a válasz is minden részletével.
>
> A képernyőkép szükséges feltétele a pontszám megszerzésének.

## Következő feladat

Nincs több feladat. Add be a megoldásod az [itt](README.md#végezetül-a-megoldások-feltöltése) leírtak szerint.
