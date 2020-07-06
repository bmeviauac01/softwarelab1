# Feladat 2: Task alapműveletek

Ebben a feladatban az alkalmazásban kezelt másik entitás, a task alapműveleteit implementáljuk az előzőekhez hasonlóan.

**A feladat megoldásával 6 pont szerezhető.**

## Entity Framework séma előkészítése

A task-ot a `Model.Task` entitás osztály reprezentálja. A task rendelkezik egy azonosítóval (`Id`), egy címmel (`Title`), a `Done` flag jelzi ha készen van, és a `Status` mutatja azon státuszt, amelyhez a task rendelve van (1-\* multiplicitással).

Első lépésként az Entity Framework modellt kell elkészítened:

1. Definiáld a `DAL.EfDbContext.DbTask` osztályban az adatbázis tároláshoz szükséges tulajdonságokat. Figyelj arra, hogy a státusz kapcsolat valódi _navigation property_ legyen!

1. Vedd fel a `TasksDbContext`-be az új `DbSet` típusú property-t.

1. A korábbiakhoz hasonlóan definiáld az adatbázis leképzés pontos konfigurációját az `OnModelCreating`-ben. Itt is figyelj a _navigation property_ pontos beállítására!

1. Célszerű lesz minta adatokat is felvenni a korábban látott módon.

## Műveletek repository szinten

Készíts a `DAL` mappában egy új osztályt `TasksRepository` néven, amely implementálja a már létező `ITasksRepository` interfészt. Valósítsd meg az alábbi műveleteit:

- `IReadOnlyCollection<Task> List()`: listázza az összes task-ot
- `Task FindById(int taskId)`: adja vissza azt a task-ot, melynek illeszkedik az id-ja a paraméterre; vagy térjen vissza null értékkel, ha nincs ilyen
- `Task Insert(CreateTask value)`: vegyen fel egy új task-ot adatbázisba a megadott címmel, és rendelje hozzá a megadott státuszhoz; ha nem létezik státusz a megadott névvel, akkor vegyen fel egy új státuszt is; visszatérési értéke az új task entitás az új azonosítóval
- `Task Delete(int taskId)`: törölje a megadott task példányt; visszatérési értéke az törölt task entitás (a törlés előtti állapotban), vagy null, ha nem létezik

A többi műveletet egyelőre ne valósítsd meg, azonban azok is kell rendelkezzenek implementációval, hogy a kód leforduljon. Elegendő egyelőre, ha ezeknek a törzse egyszerűen hibát dob: `throw new NotImplementedException();`

!!! tip "Tipp"
    Az adatbázisban használt C# osztály és a modell entitás osztály közötti leképzéshez célszerű lesz egy `ToModel` segédfüggvényt definiálni a korábban látott módon. Ahhoz, hogy a task-hoz kapcsolt státusz entitást is lekérdezze az adatbázis (amire a névhez szükség lesz), fontos lesz a megfelelő `Include` használata.

## Műveletek REST Api-n

Készíts egy új controller osztály a `Controllers` mappában `TasksController` néven. A controller a `/api/tasks/neptun` URL-en kezelje a REST kéréseket, ahol az URL vége a **saját Neptun kódod** kisbetűsen.

A controller konstruktor paraméterben egy `ITasksRepository` példányt vegyen át. Ahhoz, hogy a dependency injection keretrendszer ezt fel tudja oldani futási időben, szükséges lesz még konfigurációra is. A `Startup` osztályban a `ConfigureServices`-ben kell a másik repository-hoz hasonlóan beregisztrálni ezt az interfészt. (A controller-t _nem_ kell regisztrálni.)

A fent implementált repository műveletekre építve valósítsd meg az alábbi műveleteket:

- `GET /api/tasks/neptun`: minden task listázása, válasza `200 OK`
- `GET /api/tasks/neptun/{id}`: adott azonosítójú task lekérdezése, válasza `200 OK` vagy `404 Not found`
- `POST /api/tasks/neptun`: új task felvétele, body-ban egy `Dto.CreateTask` entitást vár, válasza `201 Created`, az új entitás body-ban, és a megfelelő _Location_ header
- `DELETE /api/tasks/neptun/{id}`: adott azonosítójú task törlése, válasza `204 No content` vagy `404 Not found`

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** Postman-ből (avagy más, hasonló eszközből, ha nem Postman-t használtál), amely egy **tetszőleges** kérést és válaszát mutatja a fentiek közül. A képet a megoldásban `f2.png` néven add be. A képernyőképen látszódjon a **kimenő kérés és a válasz is minden részletével** (URL, body, válasz kód, válasz body). Ellenőrizd, hogy a **Neptun kódod** az URL-ben szerepel-e! A képernyőkép szükséges feltétele a részpontszám megszerzésének.
