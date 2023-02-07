# Entity Framework és REST

A labor során egy Entity Framework Core adatelérésre épülő REST API (ASP.NET Core Web Api) alkalmazást készítésünk.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows, Linux, vagy MacOS: Minden szükséges program platform független, vagy van platformfüggetlen alternatívája.
- [Postman](https://www.getpostman.com/)
- [DB Browser for SQLite](https://sqlitebrowser.org/), ha az adatbázisba szeretnél belezézni (nem feltétlenül szükséges)
- GitHub account és egy git kliens
- Microsoft Visual Studio 2022 [az itt található beállításokkal](../VisualStudio.md)
    - Linux és MacOS esetén Visual Studio Code és a .NET Core SDK-val települő [dotnet CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/) használható.
- [.NET **6** SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

    !!! warning ".NET 6.0"
        A feladat megoldásához **6.0**-ás .NET SDK telepítése szükséges.

        Windows-on Visual Studio verzió függvényében lehet, hogy telepítve van (lásd [itt](../VisualStudio.md#net-core-sdk-ellenorzese-es-telepitese) az ellenőrzés módját); ha nem, akkor a fenti linkről kell telepíteni (az SDK-t és _nem_ a runtime-ot.) Linux és MacOS esetén telepíteni szükséges.

        Az Entity Framework Core viszont **7.0**-ás verzióval szerepel a kiinduló projektben, mivel az nem az SDK része, hanem NuGet csomag, illetve teljes mértékben kompatibilis a .NET 6.0 LTS verzióval.

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Entity Framework Core, REST API, Web API elméleti háttere és mintapéldái, valamint a Postman használata
    - Lásd az Adatvezérelt rendszerek c. tárgy jegyzetei és [gyakorlati anyagai](https://bmeviauac01.github.io/adatvezerelt/) között
- Hivatalos Microsoft tutorial [Web API készítéséhez](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio)

## Feladat áttekintése

A feladok elkészítése során egy egyszerű feladatkezelő webalkalmazás backendjét készítjük el. Az alkalmazás **két féle entitást kezel: státusz és task**, ahol egy státuszhoz több task rendelhető (1-\* kapcsolat). (A feladatszövegben _task_ néven fogunk hivatkozni a második entitásra, és kerüljük a "feladat" megnevezést, amely félreérthető lenne.)

Ha lenne frontendünk is, akkor egy [Kanban-tábla](https://en.wikipedia.org/wiki/Kanban_board) szerű feladatkezelőt készítenénk. A frontendtől eltekintünk, csak a szükséges REST Api-t és Entity Framework Core + ASP.NET Core Web Api alapú kiszolgálót készítjük el.

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! tip ""
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adatbázis ~~létrehozása~~

Ebben a feladatban nem Microsoft SQL Server-t használunk, hanem _Sqlite_-ot. Ez egy pehelysúlyú relációs adatbázis, amelyet elsősorban kliensoldali alkalmazásokban szokás használni, kiszolgáló oldalon nem javasolt a használata. Mi most az egyszerűség végett fogjuk használni. **Ezt nem szükséges telepíteni**.

Az adatbázis sémát a _code first_ modell szerint fogjuk létrehozni, így C# kóddal fogjuk leírni az adatbázisunkat. Ezért az adatbázis sémáját se kell SQL kóddal létrehoznunk.

## 1. Feladat: Státuszok kezelése (8 pont)

Ebben a feladatban a státusz entitáshoz tartozó alapműveleteket fogjuk megvalósítani.

### Visual Studio solution megnyitása

Nyisd meg a letöltött repository-ban a Visual Studio solution-t (`.sln` fájl). Ha a megnyitás során a Visual Studio azt jelezné, hogy a projekt típus nem támogatott, akkor telepítsd a Visual Studio hiányzó komponenseit (lásd [itt](../VisualStudio.md)).

!!! warning "NE frissíts semmilyen verziót"
    Ne frissítsd se a projektet, se a .NET verziót, se a NuGet csomagokat! Ha ilyen kérdéssel találkozol a solution megnyitása során, akkor mindig mondj nemet!

A solution struktúrája a többrétegű alkalmazás felépítésének megfelelő:

- A `Controllers` mappa tartalmazza a Web Api controllereket, amik a REST kéréseket kiszolgálják ki.
- A `Dal` mappa tartalmazza az adatelérést, amely az Entity Framework Core Code First modellt tartalmaz.
- A `Services` mappában az üzleti logikai réteg (BLL) szolgáltatás osztályai találhatóak.
- A `Dtos` a Data Transfer Object-ek osztályai találhatóak, amik a hálózaton utazó adatot reprezentálják.

!!! note "DTO a BLL rétegben?"
    Mivel a BLL rétegnek most az egyszerűség kedvéért nincs külön ún. Domain adatmodellje, a BLL rétegben vegyesen használjuk a DTO-kat és az Entitásokat. Entitásokat ment el, és kérdez le a szolgáltatás, de DTO-kat vár, és ad vissza a függvények szignatúrájában.

!!! note "Repository minta"
    A Repository és a Unit-of-Work tervezési minták egy absztrakciót nyújtanának az adatelérésünk számára. Ha jobban belegondolunk az Entity Framework pont ezt a mintát valósíja meg a DbContext és a DbSet-ek által. Ettől függetlenül néha célszerű lehet saját repository absztrakciót készíteni, ha azt is el szeretnénk absztrahálni, hogy EF-fel működik az adatelérés.

    Most szintén az egyszerűség kedvéért nem fogunk Repository mintát használni.

Munkád során a `StatusService` és `StatusController` osztályokba dolgozz! Ezen fájlok tartalmát tetszőlegesen módosíthatod (feltéve, hogy a service továbbra is megfelel a `IStatusService` interfésznek, és természetesen továbbra is fordul a kód).

### Webalkalmazás elindítása

Próbáld ki, hogy elindul-e a program.

1. Fordítsd le és indítsd el Visual Studio-ból az alkalmazást.

1. Nyisd meg böngészőben a <http://localhost:5000/api/ping> oldalt.

!!! success ""
    Ha minden rendben ment, akkor a böngészőben a "pong" szöveget látod, és a futó alkalmazás logjában látható a kiszolgált kérés.

### Minden státusz listázása (4p)

Valósítsuk meg az első műveletet, amely minden státuszt listáz.

1. Nyisd meg a `Dtos.Status` osztályt. Ez írja le az üzleti logika számára milyen tulajdonságokkal rendelkezik a státusz.

    !!! warning ""
        Ezt az osztályt **NE** módosítsd.

    !!! tip "Rekordok C#-ban"
        A `record` kulcsszó egy olyan típust reprezentál (alapértelmezetten `class`), ami a fejlécben meghatározott konstruktorral és [`init` only setterrel](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/init) rendelkező tulajdonságokkal rendelkezik. Ezáltal egy record immutable viselkedéssel bír, ami jobban illeszkedik egy DTO viselkedéséhez. A rekordok ezen kívül egyéb kényelmi szolgáltatásokkal is rendelkeznek ([lásd bővebben](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record)), de ezeket mi nem fogjuk itt kihasználni.

2. Nyisd meg a `Dal.Entities.DbStatus` osztályt. Ez a státusz entitás adatbázisbeli reprezentációja, de még üres. Írjuk meg az osztályt:

    ```csharp hl_lines="3-4"
    public class DbStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    ```

    Az `Id` lesz az adatbázisbeli kulcs, a `Name` pedig a státusz neve.

3. Nyisd meg a `Dal.TasksDbContext` osztályt. Ide fel kell vegyük a státuszokhoz tartozó `DbSet`-et, és az `OnModelCreating` függvényben konfigurálnunk kell a C# osztály - relációs adatbázis leképzést:

    ```csharp title="TasksDbContext.cs"
    public DbSet<DbStatus> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbStatus>()
            .ToTable("statuses");
            
        modelBuilder.Entity<DbStatus>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<DbStatus>()
            .Property(s => s.Name)
            .HasMaxLength(50)
            .IsRequired(required: true)
            .IsUnicode(unicode: true);
    }
    ```

    A fentiekkel megadtuk a használandó tábla nevét, a kulcsot (amely így automatikusan inkrementált lesz, azaz nem kell értéket adnunk neki), és végül a névhez tartozó megkötéseinket.

4. Menj a `StatusService.List()` függvényhez. Listázzuk ki az összes státuszt az adatbázisból:

    ```csharp hl_lines="3"
    public IReadOnlyCollection<Dtos.Status> List()
    {
        return _dbContext.Statuses.Select(ToModel).ToList();
    }
    ```

    A `_dbContext` változónk a 7, amely a dependency injection keretrendszer használatával injektálásra kerül a konstruktorban.

5. A `ToModel` függvény egy segédfüggvény lesz, amelyet többször használunk. Ez képezi le az adatbázisból érkező C# osztályt a modellként használt másik C# osztályra. Ezt is írjuk meg most ide a service osztályba.

    ```csharp
    private Status ToModel(DbStatus value)
    {
        return new Status(value.Id, value.Name);
    }
    ```

6. A BLL réteg után következik a controller. Nyisd meg a `Controllers.StatusController` osztályt. **Fűzd bele a Neptun kódod a controller URL-jének végére**, tehát a controller a `/api/status/neptun` címre érkező kérésekkel foglalkozik, ahol az utolsó 6 kisbetűs karakter a saját Neptun kódod.

    ```csharp hl_lines="1"
    [Route("api/[controller]/neptun")]
    [ApiController]
    public class StatusController : ControllerBase
    ```

    !!! warning "Neptun kód fontos"
        A Neptun kód a későbbiekben a kért képernyőképen fog megjelenni. Fontos, hogy ne hagyd ki!

7. Írjuk meg a `GET /api/status/neptun` kérésre válaszoló végpontot: A dependency injection már konfigurálva van, így a konstruktor átveszi a service interfészét (_nem_ a service osztályt, amit írtunk!).

    ```csharp title="StatusController.cs"
    [HttpGet]
    public IEnumerable<Status> List()
    {
        return _statusService.List();
    }
    ```

8. Fordítsd le és indítsd el az alkalmazást.

9. Nyisd meg a Postman-t és küldj a <http://localhost:5000/api/status/neptun> címre egy GET kérést (a saját Neptun kódodat kell itt is az URL végére írni).

    ![Státuszok lekérdezése Postman-nel](../images/efrest/rest-postman-get-statuses.png)

    !!! success ""
        Akkor sikeres a hívás, ha a Postman szerint 200 a válaszkód, és üres a válasz. Ha valami hiba lenne, akkor a Visual Studio Output ablakát ill. a futó konzol alkalmazás logját érdemes nézni.

10. Üres adatbázissal nehéz tesztelni. Állítsd le a futó alkalmazást, navigálj a `Dal.TasksDbContext.OnModelCreating` függvényhez, és illessz be egy  un. _seed_ adatot az adatbázisba:

    ```csharp title="TasksDbContext.cs" hl_lines="5-10"
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ...

        modelBuilder.Entity<DbStatus>()
            .HasData(new[]
            {
                new DbStatus() { Id = 1, Name = "new" },
                new DbStatus() { Id = 2, Name = "in progress" },
            });
    }
    ```

11. Fordítsd le és futtasd újból az alkalmazást, majd add ki ismét az előbbi GET kérést. Most már nem üres a válasz, meg kell kapnod a két státuszt.

    !!! important "Ha nem látod az új rekordokat"
        Ha mégse jelenne meg a válaszban a két _seed_ objektum, akkor lehet, hogy a DB nem frissült, és nem jutott érvényre a `HasData` művelet. Töröld ki a `tasks.db` SQLite állományt, aminek hatására újból létrejön az adatbázisfájl az app indulásakor a tesztadatainkkal.

        Az ilyen jellegű séma-, és adatmódosításokat éles környezetben [migrációkkal](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) szokás megoldani. Az egyszerűség végett mi ezt kerülni fogjuk, és ha változik a séma, egyszerűen törölheted a `tasks.db` fájlt.

### Lekérdezés és beszúrás műveletek (4p)

Az összes státusz listázása mellett még vár ránk pár alapvető művelet:

- név alapján létezés ellenőrzése (`HEAD /api/status/neptun/{nev}`),
- ID alapján keresés (`GET /api/status/neptun/{id}`),
- és új létrehozása (`POST /api/status/neptun`).

Rétegről rétegre haladva valósítsuk meg a funkciókat.

1. Implementáljuk az első kettőt először a `StatusService`-ben. Ügyeljünk rá, hogy a név alapú keresésnél kisbetű-nagybetű függetlenül keressünk!

    ```csharp hl_lines="3 8-9"
    public bool ExistsWithName(string statusName)
    {
        return _dbContext.Statuses.Any(s => EF.Functions.Like(s.Name, statusName));
    }

    public Status FindById(int statusId)
    {
        var status = _dbContext.Statuses.SingleOrDefault(s => s.Id == statusId);
        return status == null ? null : ToModel(status);
    }
    ```

    Az `EF.Functions.Like` utasítással egy SQL utasítást "képezünk le" Entity Framework-re. Amikor ebből a rendszer az SQL utasítást készíti, akkor a platformnak megfelelő `LIKE` operátor fog születni. Ez a megoldás a kis- és nagybetű független összehasonlítást szolgálja, mivel a Contains alapértelmezetten **SQLite** esetében nem így működne.

2. A hozzájuk tartozó controller végpontok pedig:

    ```csharp
    [HttpHead("{statusName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsWithName(string statusName)
    {
        return _statusService.ExistsWithName(statusName) ? Ok() : NotFound();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Status> Get(int id)
    {
        var value = _statusService.FindById(id);
        return value != null ? Ok(value) : NotFound();
    }
    ```

    Figyeld meg a controller metódusokon az attribútumokat és a visszatérési értékeket! Ha van tartalma a válasznak (body a http csomagban), akkor `ActionResult<T>` a visszatérési érték, ha csak státusz kódot adunk vissza, akkor `ActionResult`. Az `Ok` és `NotFound` függvények segédfüggvények a válasz előállításához.

    Az URL-ek tekintetében csak az URL végével kellett foglalkozunk. A `/api/status/neptun` részt a controller osztályra raktuk, tehát az az összesre érvényes.

3. Az új státusz beszúrásához ismét a service felől induljunk. A létrehozáshoz egy DTO osztályt, a `CreateStatus`-t kapjuk, ebben csak egy név van. Garantálni szeretnénk a nevek egyediségét, hogy ne legyen két státusz ugyanazzal a névvel. A beszúrásnál ezt ellenőrizni fogjuk, méghozzá itt is kisbetű-nagybetű függetlenül.

    ```csharp
    public Status Insert(CreateStatus value)
    {
        using var tran = _dbContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        if (_dbContext.Statuses.Any(s => EF.Functions.Like(s.Name, value.Name)))
            throw new ArgumentException("Name must be unique");

        var status = new DbStatus() { Name = value.Name };
        _dbContext.Statuses.Add(status);

        _dbContext.SaveChanges();
        tran.Commit();

        return ToModel(status);
    }
    ```

    !!! important "Konkurencia kezelése"
        Figyeljünk a tranzakcióra! Először ellenőriznünk kell, van-e már hasonló név. Ha igen, akkor a hibát kivétellel jelezzük. Ha beszúrható a rekord, akkor a beszúrás után a tranzakciót is kommitálnunk kell. És mivel az ID-t az adatbázis generálja, a service függvény visszaadja a létrehozott entitást, benne az új ID-val.

4. A POST http kérést az alábbi controller metódus fogja kiszolgálni:

    ```csharp
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Status> Create([FromBody]CreateStatus value)
    {
        try
        {
            var created = _statusService.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(nameof(CreateStatus.Name), ex.Message);
            return ValidationProblem(ModelState);
        }
    }
    ```

    Figyeljük meg a sikeres és a sikertelen válaszokat is. Ha sikerült a beszúrás, akkor a `CreatedAtAction` segédfüggvény olyan válasszal fog visszatérni, ahol a body tartalmazza az új entitást, a _Location_ header pedig a linket, ahol az entitás lekérdezhető (ezért a hivatkozás a `nameof(Get)`-tel).

    Ha viszont a service-ben eldobott kivétel érkezik, akkor jelezzük a problémát a hívónak. Ebben a válaszban a státuszkód 400 lesz, és itt is lesz body, ami a Problem Details RFC szabvány szerinti formátumot követi. Amennyiben csaj 400-as hibával szeretnénk visszatérni tetszőleges formátumú body-val, használhattuk volna a `BadRequest()` függvényt is.

    A `CreateStatus` DTO-ban lévő `Name` property-n szerepel a `[Required]` attribútum is. Ezek a validációs attribútumok a kontrolleren lévő `[ApiController]` attribútum miatt az action meghívása előtt kiértékelődnek, és szintén 400-as hibát eredényeznek Problem Details formátumban.

5. Fordítsd le az alkalmazást és indítsd el. Próbáld ki a megírt kéréseket! Állítsd elő a sikeres és a sikertelen ágakat is.

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** Postman-ből (avagy más, hasonló eszközből, ha nem Postman-t használtál), amely egy **sikertelen** beszúrást mutat. A sikertelenség oka a már létező név legyen. A képet a megoldásban `f1.png` néven add be. A képernyőképen látszódjon a **kimenő kérés és a válasz is minden részletével** (URL, body, válasz kód, válasz body). Ellenőrizd, hogy a **Neptun kódod** az URL-ben szerepel-e! A képernyőkép szükséges feltétele a pontszám megszerzésének.

## 2. Feladat: Task alapműveletek (6 pont)

Ebben a feladatban az alkalmazásban kezelt másik entitás, a task alapműveleteit implementáljuk az előzőekhez hasonlóan.

### Entity Framework séma előkészítése

A task-ot a `Dtos.Task` DTO osztály reprezentálja. A task rendelkezik egy azonosítóval (`Id`), egy címmel (`Title`), a `IsDone` flag jelzi ha készen van, és a `Status` mutatja azon státuszt, amelyhez a task rendelve van (1-\* multiplicitással).

Első lépésként az Entity Framework modellt kell elkészítened:

1. Definiáld a `DbTask` osztályban az adatbázis tároláshoz szükséges tulajdonságokat. Figyelj arra, hogy a státusz kapcsolat valódi _navigation property_ legyen!

2. Vedd fel a `TasksDbContext`-be az új `DbSet` típusú property-t.

3. A korábbiakhoz hasonlóan definiáld az adatbázis leképzés pontos konfigurációját az `OnModelCreating`-ben. Itt is figyelj a _navigation property_ pontos beállítására!

4. Célszerű lesz minta adatokat is felvenni a korábban látott módon.

### Műveletek service szinten

Készíts a `Dal` mappában egy új osztályt `TaskService` néven, amely implementálja a már létező `ITaskService` interfészt. Valósítsd meg az alábbi műveleteit:

- `IReadOnlyCollection<Task> List()`: listázza az összes task-ot
- `Task FindById(int taskId)`: adja vissza azt a task-ot, melynek illeszkedik az id-ja a paraméterre; vagy térjen vissza `null` értékkel, ha nincs ilyen
- `Task Insert(CreateTask value)`: vegyen fel egy új task-ot adatbázisba a megadott címmel, és rendelje hozzá a megadott státuszhoz; ha nem létezik státusz a megadott névvel, akkor vegyen fel egy új státuszt is; visszatérési értéke az új task entitás az új azonosítóval
- `Task Delete(int taskId)`: törölje a megadott task példányt; visszatérési értéke az törölt task entitás (a törlés előtti állapotban), vagy `null`, ha nem létezik

A többi műveletet egyelőre ne valósítsd meg, azonban azok is kell rendelkezzenek implementációval, hogy a kód leforduljon. Elegendő egyelőre, ha ezeknek a törzse egyszerűen hibát dob: `throw new NotImplementedException();`

!!! tip "Tipp"
    Az adatbázisban használt C# osztály és a modell entitás osztály közötti leképzéshez célszerű lesz egy `ToModel` segédfüggvényt definiálni a korábban látott módon. Ahhoz, hogy a task-hoz kapcsolt státusz entitást is lekérdezze az adatbázis (amire a névhez szükség lesz), fontos lesz a megfelelő `Include` használata.

### Műveletek REST Api-n

Készíts egy új controller osztály a `Controllers` mappában `TaskController` néven. A controller a `/api/task/neptun` URL-en kezelje a REST kéréseket, ahol az URL vége a **saját Neptun kódod** kisbetűsen.

A controller konstruktor paraméterben egy `ITaskService` példányt vegyen át. Ahhoz, hogy a dependency injection keretrendszer ezt fel tudja oldani futási időben, szükséges lesz még konfigurációra is. A `Program` osztályban kell a másik service-hez hasonlóan beregisztrálni ezt az interfészt. (A controller-t _nem_ kell regisztrálni.)

A fent implementált service műveletekre építve valósítsd meg az alábbi műveleteket:

- `GET /api/task/neptun`: minden task listázása, válasza `200 OK`
- `GET /api/task/neptun/{id}`: adott azonosítójú task lekérdezése, válasza `200 OK` vagy `404 Not found`
- `POST /api/task/neptun`: új task felvétele, body-ban egy `Dto.CreateTask` entitást vár, válasza `201 Created`, az új entitás body-ban, és a megfelelő _Location_ header
- `DELETE /api/task/neptun/{id}`: adott azonosítójú task törlése, válasza `204 No content` vagy `404 Not found`

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** Postman-ből (avagy más, hasonló eszközből, ha nem Postman-t használtál), amely egy **tetszőleges** kérést és válaszát mutatja a fentiek közül. A képet a megoldásban `f2.png` néven add be. A képernyőképen látszódjon a **kimenő kérés és a válasz is minden részletével** (URL, body, válasz kód, válasz body). Ellenőrizd, hogy a **Neptun kódod** az URL-ben szerepel-e! A képernyőkép szükséges feltétele a részpontszám megszerzésének.

## 3. Feladat: Task-on végzett műveletek (6 pont)

Implementálj két új http végpontot a task-ot kezelő controllerben, amellyel az alábbiak szerint módosítható egy-egy task példány.

### Kész állapot jelzése (3p)

A `Task.IsDone` flag jelzi a task kész voltát. Készíts egy új http végpontot az alábbiak szerint, amely az `ITaskService.MarkDone` műveleten keresztül beállítja a flaget a megadott task példányon.

Kérés: `PATCH /api/task/neptun/{id}/done`, ahol `{id}` a task azonosítója.

Válasz:

- `404 Not found`, ha nem létezik a task
- `200 OK` ha sikeres a művelet - a body-ban adja vissza a módosítás utáni task entitást.

### Átsorolás másik státuszba (3p)

Egy task egy státuszhoz tartozik (`Task.StatusId`). Készíts egy új http végpontot az alábbiak szerint, amely az `ITaskService.MoveToStatus` műveleten keresztül áthelyezi a megadott task-ot egy másik státuszba. Ha az új státusz nem létezik, akkor hozzon létre újat a művelet.

Kérés: `PATCH /api/task/neptun/{id}/move`, ahol

- `{id}` a task azonosítója,
- az új státusz **neve** pedig a bodyban érkezik egy `status` property-ben.

Válasz:

- `404 Not found`, ha nem létezik a task
- `200 OK` ha sikeres a művelet - a body-ban adja vissza a módosítás utáni task entitást.

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** Postman-ből (avagy más, hasonló eszközből, ha nem Postman-t használtál), amely a fentiek közül egy **tetszőleges** kérést és válaszát mutatja. A képet a megoldásban `f3.png` néven add be. A képernyőképen látszódjon a **kimenő kérés és a válasz is minden részletével** (URL, body, válasz kód, válasz body). Ellenőrizd, hogy a **Neptun kódod** az URL-ben szerepel-e! A képernyőkép szükséges feltétele a pontszám megszerzésének.

## 4. Feladat: Opcionális iMSc feladat (3 iMSc pont)

Amennyiben sok task van, nem célszerű egyszerre mindet visszaadni listázáskor. Implementálj lapozást erre a funkcióra az alábbi követelményekkel.

- Determinisztikusan az ID alapján sorrendben adja vissza az elemeket.
- A kérésben egy opcionális `count` nevű query paraméterben megadott darabszámú elemet ad vissza minden lapon. alapértelmezetten legyen 5 az értéke, ha a kliens nem küldi.
- A következő lapot egy opcionális `fromId` érték bemondásával lehessen lekérni. Ezen `fromId` a lapozásban a soron következő elem **azonosítója**.
- A http kérés két paramétere `fromId` és `count` opcionális query paraméterben érkezzen.
- A lapozás a meglévő `GET /api/task/neptun/paged` címen legyen elérhető.
- A lapozás során a válaszhoz csak azok az entitások legyenek lekérdezve, amelyekre tényleg szükség is van (tehát ne rántsd be feleslegesen a teljes táblát memóriába).
- A lapozás válasza a `Dto.PagedTaskList` osztály példánya legyen. Ebben szerepel:
    - a lapon található elemek tömbje (`Items`),
    - a lapon található elemek száma (`Count`)
    - a következő lap lekéréséhez szükséges `fromId` érték (`NextId`),
    - és segítségként az URL, amivel a következő lap lekérhető (`NextUrl`), vagy `null`, ha nincs több lap.

        !!! tip ""
            Az Url előállításához használd a controller osztályon elérhető `Url.Action` segédfüggvényt. Ne égesd be a kódba se a `localhost:5000`, se a `/api/task/neptun/paged` URL részleteket! Az URL előállításához _nem_ string műveletekre lesz szükséged!

            Az `Url.Action` akkor fog abszolút URL-t visszaadni, ha minden paraméterét (`action`, `controller`, `values`, `protocol` és `host`) kitöltöd; utóbbiakhoz a `HttpContext.Request` tud adatokat nyújtani.

- A kérés mindig 200 OK válasszal tér vissza, csak legfeljebb üres a visszaadott válaszban az elemek tömbje.

??? info "Az alábbi kérés-válasz sorozat mutatja a működést"

    1. `GET /api/task/neptun/paged?count=2`

        Ez az első kérés. Itt nincs `from` paraméter, ez a legelső elemtől indul.

        Válasz:

        ```json
        {
          "items": [
            {
              "id": 1,
              "title": "doing homework",
              "done": false,
              "status": "pending"
            },
            {
              "id": 2,
              "title": "doing more homework",
              "done": false,
              "status": "new"
            }
          ],
          "count": 2,
          "nextId": 3,
          "nextUrl": "http://localhost:5000/api/task/neptun/paged?fromId=3&count=2"
        }
        ```

    2. `GET /api/task/neptun/paged?fromId=3&count=2`

        Ez a második lap tartalmának lekérése.

        Válasz:

        ```json
        {
          "items": [
            {
              "id": 3,
              "title": "hosework",
              "done": true,
              "status": "done"
            }
          ],
          "count": 1,
          "nextId": null,
          "nextUrl": null
        }
        ```

        A válasz mutatja, hogy nincs további lap, mind a `nextId`, mind a `nextUrl` null.

    3. `GET /api/task/neptun?fromId=999&count=999`

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

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** Postman-ből (avagy más, hasonló eszközből, ha nem Postman-t használtál), amely a fenti bemutatott példához hasonlóan egy **tetszőleges** lap lekérését és válaszát mutatja. A képet a megoldásban `f4.png` néven add be. A képernyőképen látszódjon a **kimenő kérés és a válasz is minden részletével** (URL, body, válasz kód, válasz body). Ellenőrizd, hogy a **Neptun kódod** az URL-ben szerepel-e! A képernyőkép szükséges feltétele a pontszám megszerzésének.