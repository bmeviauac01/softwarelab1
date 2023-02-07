# MongoDB

A labor során a MongoDB NoSQL adatbáziskezelő rendszer és a Mongo C# driver használatát gyakoroljuk komplexebb feladatokon keresztül.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows, Linux, vagy MacOS: Minden szükséges program platform független, vagy van platformfüggetlen alternatívája.
- MongoDB Community Server ([letöltés](https://www.mongodb.com/download-center/community))
    - Telepítés nélkül Docker segítségével az alábbi paranccsal futtathatod a szervert

        ```cmd
        docker run --name swlab1-mongo -p 27017:27017 -d mongo
        ```

- Studio 3T Free ([letöltés](https://studio3t.com/download-studio3t-free))
- Minta adatbázis kódja ([mongo.js](https://bmeviauac01.github.io/adatvezerelt/db/mongo.js))
- GitHub account és egy git kliens
- Microsoft Visual Studio 2022 [az itt található beállításokkal](../VisualStudio.md)
    - Linux és MacOS esetén Visual Studio Code és a .NET SDK-val települő [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/) használható.
- [.NET **6.0** SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

    !!! warning ".NET Core 6.0"
        A feladat megoldásához **6.0**-es .NET Core SDK telepítése szükséges.

        Windows-on Visual Studio verzió függvényében lehet, hogy telepítve van (lásd [itt](../VisualStudio.md#net-core-sdk-ellenorzese-es-telepitese) az ellenőrzés módját); ha nem, akkor a fenti linkről kell telepíteni (az SDK-t és _nem_ a runtime-ot.) Linux és MacOS esetén telepíteni szükséges.

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- MongoDB adatbáziskezelő rendszer és a C# driver használata
    - Lásd az Adatvezérelt rendszerek c. tárgy jegyzetei és [gyakorlati anyagai](https://bmeviauac01.github.io/adatvezerelt/) között
- Hivatalos Microsoft tutorial [Mongo-t használó Web API készítéséhez](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-6.0&tabs=visual-studio)
    - A labor során nem WebAPI-t készítünk, de a Mongo használat azonos formában történik.

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! warning "Jelszó a laborokban"
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adatbázis létrehozása

Kövesd a [gyakorlatanyagban](https://bmeviauac01.github.io/adatvezerelt/gyakorlat/mongodb/#feladat-0-adatbazis-letrehozasa-projekt-megnyitasa) leírt utasításokat az adatbáziskezelő rendszer elindításához és az adatbázis létrehozásához.

## 1. Feladat: Termékek lekérdezése és módosítása (7 pont)

Ebben a feladatban a `Product` entitáshoz tartozó CRUD (létrehozás, listázás/olvasás, módosítás és törlés) utasításokat fogjuk megvalósítani.

### Visual Studio solution megnyitása

Nyisd meg a letöltött repository-ban a Visual Studio solution-t (`.sln` fájl). Ha a megnyitás során a Visual Studio azt jelezné, hogy a projekt típus nem támogatott, akkor telepítsd a Visual Studio hiányzó komponenseit (lásd [itt](../VisualStudio.md)).

!!! warning "NE frissíts semmilyen verziót"
    Ne frissítsd se a projektet, se a .NET verziót, se a NuGet csomagokat! Ha ilyen kérdéssel találkozol a solution megnyitása során, akkor mindig mondj nemet!

Munkád során a `Dal.Repository` osztályba dolgozz! Ezen fájl tartalmát tetszőlegesen módosíthatod (feltéve, hogy továbbra is megvalósítja a `Dal.IRepository` interfészt, továbbra is van egy konstruktora egyetlen `IMongoDatabase` paraméterrel és természetesen továbbra is fordul a kód).

Az adatbázis elérése a `Dal.MongoConnectionConfig` osztályban van. Ha szükséges, az adatbázis nevét megváltoztathatod a forrásban jelölt helyen.

A projekt minden egyéb tartalma már elő van készítve a munkához, a fentieken kívül máshol **NE** módosítsd!

!!! info "Razor Pages"
    A webalkalmazás egy un. [_Razor Pages_](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/) típusú ASP.NET Core projekt. Ezt egy szerver oldalon renderelt megjelenítési réteg, ahol tehát a weboldal html kódját C# kód és a Razor template állítja elő. (A megjelenítési réteggel nem lesz feladatod, az már elő van készítve számodra.)

### Webalkalmazás elindítása

Próbáld ki, hogy elindul-e a program.

1. Fordítsd le és indítsd el Visual Studio-ból az alkalmazást.

1. Nyisd meg böngészőben a <http://localhost:5000/> oldalt.

!!! success ""
    Ha minden rendben ment, akkor bejött egy oldal, ahol az egyes feladatokat ki fogod tudni próbálni. (Az egyes feladatokra mutató linkek mögötti oldalak még nem működnek. Előbb még meg kell valósítani az adatelérési réteget.)

### Neptun kód megjelenítése a weboldalon

A forráskód melletti képernyőképeken szerepelnie kell a Neptun kódodnak.

1. Nyisd meg a `Pages\Shared\_Layout.cshtml` fájlt. A fájl közepe felé keresd meg az alábbi részt, és írd át benne a Neptun kódodat.

    ```csharp hl_lines="5"
    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>@ViewData["Title"] - NEPTUN</p>
        </footer>
    </div>
    ```

1. Fordítsd le és indítsd el az alkalmazást ismét, nézd meg a kezdőoldalt. Az alján kell lásd a saját Neptun kódod.

    ![Neptun kód a láblécben](../images/mongo/mongo-neptun-footer.png)

!!! warning "FONTOS"
    A Neptun kódnak a fentiek szerint szerepelnie kell minden képernyőképen!

### Listázás/olvasás

1. Első lépésként szükség lesz az adatbázisban található `products` kollekció elérésére a kódból. Ehhez vegyél fel egy új mezőt a `Repository` osztályban, és inicializáld ezt a konstruktorban. Az ehhez szükséges `IMongoDatabase` objektumot _Dependency Injection_ segítségével, konstruktorparaméterként kaphatod meg.

    ```csharp
    private readonly IMongoCollection<Entities.Product> _productCollection;

    public Repository(IMongoDatabase database)
    {
        this._productCollection = database.GetCollection<Entities.Product>("products");
    }
    ```

1. A `_productCollection` segítségével már tudunk lekérdező/listázó utasításokat írni. Valósítsuk meg először a `ListProducts` függvényt. Ez a függvény két lépésből áll: először lekérdezzük az adatbázisból a termékek listáját, majd pedig átkonvertáljuk az elvárt `Models.Product` osztály elemekből álló listára.

    A lekérdezés a következőképpen néz ki:

    ```csharp
    var dbProducts = _productCollection
        .Find(_ => true) // minden terméket listázunk — üres filter
        .ToList();
    ```

    Ezt a listát aztán transzformálva adjuk vissza.

    ```csharp
    return dbProducts
        .Select(t => new Product
        {
            Id = t.Id.ToString(),
            Name = t.Name,
            Price = t.Price,
            Stock = t.Stock
        })
        .ToList();
    ```

1. A `FindProduct(string id)` függvény megvalósítása nagyon hasonlít az előzőhöz, csupán annyiban különbözik, hogy itt egyetlen terméket kérdezünk le, `id` alapján. Ügyeljünk rá, hogy az `id`-t szövegként kapjuk, így `ObjectId`-vá kell alakítani.

    A modellé konvertáló lépés ugyanúgy megmarad. Ebben az esetben oda kell figyelnünk azonban, hogy ha nem találjuk az adott terméket, akkor adjunk vissza `null` értéket, ne próbáljunk konvertálni.

    A lekérdező lépés a következőre módosul:

    ```csharp
    var dbProduct = _productCollection
        .Find(t => t.Id == ObjectId.Parse(id))
        .SingleOrDefault();
    // ... model konverzio
    ```

    Figyeljük itt meg, hogy hogyan módosult a filter kifejezés! Fontos továbbá, hogy itt `ToList` helyett a `SingleOrDefault` kiértékelő kifejezést használjuk. Ez a megszokott módon vagy egy konkrét terméket ad vissza, vagy `null` értéket. Így tudunk tehát `Id` alapján megtalálni egy entitást az adatbázisban. Jegyezzük ezt meg, mert ez még sok következő feladatban hasznos lesz!

    A konvertáló kifejezést már megírtuk egyszer az előző kifejezésben, ezt használhatjuk itt is — figyeljünk azonban oda, hogy a `dbProduct` értéke lehet `null` is. Ebben az esetben ne konvertáljunk, csupán adjunk vissza `null` értéket mi is.

1. Próbáld ki a megírt függvények működését! Indítsd el a programot, és navigálj tetszőleges böngészőben a <http://localhost:5000> weboldalra. Itt a `Products` menüpontban már látnod kell a termékeket felsoroló táblázatot. Ha valamelyik sorban a `Details` menüpontra kattintasz, akkor egy adott termék részleteit is látnod kell.

!!! failure "Ha nem látsz egyetlen terméket se"
    Ha a weboldalon egyetlen termék se jelenik meg, de az oldal betöltésre kerül és nincs hiba, akkor az adatbázis elérésével van probléma. Valószínűleg nem létezik a megadott nevű adatbázis. Lásd fentebb az adatbázis konfigurálását.

### Létrehozás

1. Ebben a pontban az `InsertProduct(Product product)` függvényt valósítjuk meg. A bemenő `Models.Product` modell adatait a felhasználó szolgáltatja a felhasználói felületen keresztül.

2. Egy termék létrehozásához először létre kell hoznunk egy új adatbázisentitás objektumot. Jelen esetben ez egy `Entites.Product` objektum lesz. Az `Id` értéket nem kell megadnunk — ezt majd az adatbázis generálja. A `Name`, `Price` és `Stock` értékeket a felhasználó szolgáltatja. Két érték maradt ki: az `Vat` és a `CategoryId`. Az előbbinek adjunk tetszőleges értéket, az utóbbinak pedig keressünk egy szimpatikus kategóriát az adatbázisban a _Studio 3T_ segítségével, és annak az `_id` értékét drótozzuk be!

    ```csharp
    var dbProduct = new Entities.Product
    {
        Name = product.Name,
        Price = product.Price,
        Stock = product.Stock,
        Vat = new Entities.Vat
        {
            Name = "General",
            Percentage = 20
        },
        CategoryId = ObjectId.Parse("5d7e4370cffa8e1030fd2d99"),
    };
    _productCollection.InsertOne(dbProduct);
    ```

    Ha megvan az adatbázisentitás objektum, akkor az `InsertOne` utasítás segítségével tudjuk elmenteni az adatbázisba.

3. A függvény kipróbálásához indítsd el megint a programot, és a termékek táblázata feletti `Add new product` linkre kattints. Itt meg tudod adni a szükséges adatokat, amivel meghívódik a kódod.

### Törlés

1. A törlés megvalósításához a `DeleteProduct(string id)` függvényt kell implementálni. Itt a kollekció `DeleteOne` függvényét kell használni. Ehhez meg kell adni egy filter kifejezést: itt használjuk ugyanazt a filtert, amit az egy konkrét termék lekérdezéséhez használtunk a `FindProduct(string id)` metódusban.

1. Ezt a metódust is ki tudod próbálni, ha a termékek táblázatában valamelyik sorban a `Delete` linkre kattintasz.

### Módosítás

1. A termékek módosító utasításaként az eladást fogjuk megvalósítani, a `bool SellProduct(string id, int amount)` függvényben. A függvény akkor és csak akkor térjen vissza `true` értékkel, ha létezik az `id` azonosítójú termék, és legalább `amount` darab van belőle raktáron, amit el is tudtunk adni. Ha nem létezik a termék vagy nincs belőle elegendő, akkor térjen vissza `false` értékkel.

1. A MongoDB atomicitását kihasználva ezt a feladatot egyetlen utasítás kiadásával fogjuk megvalósítani. A filter kifejezésben fogunk rászűrni a megadott `id`-ra, és arra is, hogy elegendő termék van-e raktáron. A módosító kifejezésben fogjuk csökkenteni a raktárkészletet — csak akkor, ha létezik a termék, és elegendő van belőle raktáron.

    ```csharp
    var result = _productCollection.UpdateOne(
        filter: t => t.Id == ObjectId.Parse(id) && t.Stock >= amount,
        update: Builders<Entities.Product>.Update.Inc(t => t.Stock, -amount),
        options: new UpdateOptions { IsUpsert = false });
    ```

    Fontos megfigyelni, hogy az `UpdateOptions` segítségével jeleztük az adatbázisnak, hogy **NE** upsertet hajtson végre — tehát ne tegyen semmit ha nem találja a filterben megadott terméket.

    A módosító utasítást a `Builders`-ben található `Update` builder segítségével tudjuk összerakni. Esetünkben `amount`-tal akarjuk csökkenteni az értéket — ez `-amount`-tal történő _inkrementálást_ jelent.

    A visszatérési értéket az update `result`-jából tudjuk meghatározni. Amennyiben talált olyan terméket, ami megfelelt a filternek, akkor sikeres volt a módosítás, tehát `true` értékkel térhetünk vissza. Egyébként a visszatérési érték `false`.

    ```csharp
    return result.MatchedCount > 0;
    ```

1. Ezt a metódust is ki tudod próbálni, ha a termékek táblázatában valamelyik sorban a `Buy` linkre kattintasz. Próbáld ki úgy is, ha túl nagy értéket írsz be!

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** a termék listázó weboldalról **miután felvettél legalább egy új terméket**. A képet a megoldásban `f1.png` néven add be. A képernyőképen látszódjon a **termék lista**. Ellenőrizd, hogy a **Neptun kódod** (az oldal aljáról) látható-e a képen! A képernyőkép szükséges feltétele a pontszám megszerzésének.

## 2. Feladat: Kategóriák listázása (4 pont)

Ebben a feladatban a kategóriákat fogjuk listázni — az adott kategóriába tartozó termékek számával együtt. Ehhez már aggregációs utasítást is használnunk kell majd. Továbbra is a `Dal.Repository` osztályba dolgozunk.

A megvalósítandó függvény a `IList<Category> ListCategories()`. Ennek minden kategóriát vissza kell adnia. A `Models.Category` osztály 3 adattagot tartalmaz.

- `Name`: értelemszerűen a kategória neve
- `ParentCategoryName`: a kategória szülőkategóriájának neve. Amennyiben nincs szülőkategória, értéke legyen `null`.
- `NumberOfProducts`: a kategóriába tartozó termékek száma. Amennyiben nincs ilyen, értéke legyen **0**.

A megvalósítás lépései a következők.

1. Első lépésként a `_productCollection` mintájára vedd fel és inicializáld a `_categoryCollection`-t is. Az adatbázisban a kollekció neve `categories` — ezt _Studio 3T_ segítségével tudod ellenőrizni.

1. A `ListCategories()` metódusban először kérdezzük le a kategóriák teljes listáját. Ez pontosan ugyanúgy történik, mint az előző feladatban a termékek esetében. A lekérdezés értékét tegyük a `dbCategories` változóba.

1. Ezután kérdezzük le, hogy az egyes `CategoryId`-khez hány darab termék tartozik. Ehhez aggregációs pipeline-t kell használnunk, azon belül pedig a [`$group`](https://docs.mongodb.com/manual/reference/operator/aggregation/group/) lépést.

    ```csharp
    var productCounts = _productCollection
        .Aggregate()
        .Group(t => t.CategoryID, g => new { CategoryID = g.Key, NumberOfProducts = g.Count() })
        .ToList();
    ```

    Ez az utasítás egy olyan listával tér vissza, melyben minden elem egy `CategoryID` értéket tartalmaz a hozzá tartozó termékek számával együtt.

1. Ezen a ponton minden számunkra szükséges információ rendelkezésünkre áll — ismerjük az összes kategóriát (a szülőkategória megkereséséhez), és ismerjük a kategóriákhoz tartozó termékek számát. Egyetlen dolgunk van, hogy ezeket az információkat (C# kódból) "összefésüljük".

    ```csharp
    return dbCategories
        .Select(k => new Category
        {
            Name = k.Name,
            ParentCategoryName = k.ParentCategoryId.HasValue
                ? dbCategories.Single(p => p.Id == k.ParentCategoryId.Value).Name
                : null,
            NumberOfProducts = productCounts.SingleOrDefault(pc => pc.CategoryID == k.Id)?.NumberOfProducts ?? 0
        })
        .ToList();
    ```

    Láthatjuk, hogy mind a két lépést egyszerűen elintézhetjük C#-ban LINQ segítségével. A szülőkategória nevéhez a kategóriák között kell keresnünk, a termékek darabszámához pedig az aggregáció eredményében keresünk.

    !!! tip "Join Mongo-ban"
        Nem ez az egyetlen módja a gyűjtemények "összekapcsolásának" MongoDB-ben. Ugyan `join` nincs, de léteznek megoldások a gyűjteményeken átívelő lekérdezésekre. A fenti megoldás az adatbázis helyett C# kódban végzi el az összekapcsolást. Ez akkor praktikus, ha nem nagy méretű adathalmazzal dolgozunk, és nincs (jó) szűrésünk. Ha szűrni kellene az adathalmazt, a fenti megoldás is bonyolultabb lenne a hatékonyság érdekében.

1. A kód kipróbálásához a weboldal `Categories` menüpontjára kell navigálni. Itt táblázatos formában megjelenítve láthatod az összegyűjtött információkat. Teszteléshez alkalmazhatod az előző feladatban elkészített `Add new product` funkciót — itt a hozzáadás esetén az egyik kategória mellett növekednie kell a hozzá tartozó termékek darabszámának. (A termék beszúrásakor a kategória ID-ját bedrótoztuk a kódba, tehát annak a kategóriának kell nőnie, amelyiknek az ID-ja a kódban szerepel.)

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** a kategória listázó weboldalról. A képet a megoldásban `f2.png` néven add be. A képernyőképen látszódjon a **kategória lista**. Ellenőrizd, hogy a **Neptun kódod** (az oldal aljáról) látható-e a képen! A képernyőkép szükséges feltétele a pontszám megszerzésének.

## 3. Feladat: Megrendelések lekérdezése és módosítása (5 pont)

Ebben a feladatban a Megrendelés (`Order`) entitáshoz tartozó CRUD (létrehozás, listázás/olvasás, módosítás és törlés) utasításokat fogjuk megvalósítani. Ez a feladat nagyon hasonlít az első feladathoz, amennyiben elakadnál nyugodtan meríts ihletet az ottani megoldásokból!

A `Models.Order` entitás adattagjai:

- `Id`: az adatbázisentitás `Id`-ja, `ToString`-gel sorosítva
- `Date`, `Deadline`, `Status`: egy az egyben másolandók az adatbázis entitásból
- `PaymentMethod`: az adatbázis entitásban található `PaymentMethod` komplex objektum `Method` mezője
- `Total`: a megrendelésben található tételek (`OrderItems`) `Amount` és `Price` szorzatainak összege

Ennek a feladatnak a megoldásához a megrendeléssel kapcsolatos metódusok implementációja szükséges (`ListOrders`, `FindOrder`, `InsertOrder`, `DeleteOrder` és `UpdateOrder`).

Az alábbi feladatok előtt ne felejtsd el felvenni és inicializálni a `_orderCollection`-t a repository osztályba a korábban látottak mintájára!

### Listázás/olvasás

1. A `ListOrders` függvény paraméterként kap egy `string status` értéket. Ha ez az érték üres vagy `null` (lásd: `string.IsNullOrEmpty`), akkor minden megrendelést listázz. Ellenkező esetben csak azokat a megrendeléseket listázd, melyeknek a `Status` értéke teljesen egyezik a paraméterben érkező `status` értékkel.

1. A `FindOrder` metódus egy konkrét megrendelés adatait adja vissza a `string id` érték alapján szűrve. Figyelj oda, ha az adott `ID` érték nem található az adatbázisban, akkor `null` értéket adj vissza!

### Létrehozás

1. Az `InsertOrder` metódusban a létrehozást kell megvalósítanod. Ehhez háromféle információt kapsz: `Order order`, `Product product` és `int amount`.

1. Az adatbázisentitás létrehozásához a következő információkra van szükség:

    - `CustomerId`, `SiteId`: az adatbázisból keresd ki egy tetszőleges vevőhöz (`Customer`) tartozó dokumentum `_id` és `mainSiteId` értékét. Ezeket az értékeket drótozd bele a kódodba.
    - `Date`, `Deadline`, `Status`: ezeket az értékeket az `order` paraméterből veheted
    - `PaymentMethod`: hozz létre egy új `PaymentMethod` objektumot. A `Method` legyen az `order` paraméterben található `PaymentMethod` érték. A `Deadline` maradjon `null`!
    - `OrderItems`: egyetlen tételt hozz létre! Ennek adattagjai:
        - `ProductId` és `Price`: a `product` paraméterből veheted
        - `Amount`: a függvényparaméter `amount` paraméterből jön
        - `Status`: az `order` paraméter `Status` mezőjével egyezik meg
    - minden más adattag (a számlázással kapcsolatos információk) maradjon `null` értéken!

### Törlés

A `DeleteOrder` törölje a megadott `Id`-hoz tartozó megrendelést.

### Módosítás

A módosító utasításban (`UpdateOrder`) arra figyelj oda, hogy csak azokat a mezőket írd felül, melyek a `Models.Order` osztályban megtalálhatóak: `Date`, `Deadline`, `Status` és `PaymentMethod`. Az `Total`-t itt nem kell figyelembe venni, ennek értéke nem fog változni.

!!! tip "Tipp"
    Több módosító kifejezést a `Builders<Entities.Order>.Update.Combine` segítségével lehet összevonni.

    Itt is figyelj oda, hogy az update során az `IsUpsert` beállítás értéke legyen `false`!

A metódus visszatérési értéke akkor és csak akkor legyen `true`, ha létezik megrendelés a megadott `ID`-val — azaz volt illeszkedés a filterre.

### Kipróbálás

A megírt függvényeket a weboldalon a `Orders` menüpont alatt tudod kipróbálni. Teszteld le a `Filter`, `Add new order`, `Edit`, `Details` és `Delete` opciókat is!

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** a megrendelés listázó weboldalról **miután felvettél legalább egy új megrendelést**. A képet a megoldásban `f3.png` néven add be. A képernyőképen látszódjon a **megrendelések listája**. Ellenőrizd, hogy a **Neptun kódod** (az oldal aljáról) látható-e a képen! A képernyőkép szükséges feltétele a pontszám megszerzésének.

## 4. Feladat: Vevők listázása (4 pont)

Ebben a feladatban a vevőket fogjuk listázni — az általuk megrendelt termékek összértékével együtt. Ehhez a második feladathoz hasonlatosan aggregációs utasítást és C# kódban történő "összefésülést" kell majd használnunk.

A megvalósítandó metódus az `IList<Customer> ListCustomers()`. Ennek minden vevőt vissza kell adnia. A `Models.Customer` entitás adattagjai:

- `Name`: a vevő neve
- `ZipCode`, `City`, `Street`: a vevő központi telephelyéhez tartozó cím
- `TotalOrders`: a vevőhöz tartozó összes megrendelés összértékének (lásd előző feladat) összege. Amennyiben még nincs az adott vevőhöz tartozó megrendelés, akkor ennek az értéke legyen `null`!

A feladat megoldásához ajánlott lépések:

1. Vedd fel és inicializáld a `_customerCollection`-t!

1. Listázd ki az összes vevőt. A vevő entitásban megtalálod a telephelyek listáját (`Sites`) és a központi telephely azonosítóját (`MainSiteId`) is. Ez utóbbit kell megkeresned az előbbi listában, hogy megkapd a központi telephelyet (és így a hozzá tartozó címet).

1. A megrendelések kollekcióján aggregációs pipeline-t használva meg tudod állapítani az adott `CustomerId`-hez tartozó összmegrendelések értékét.

1. Végezetül csak a meglevő információkat kell "összefésülnöd". Központi telephelye minden vevőnek van, viszont megrendelése nem garantált — figyelj oda, hogy ekkor a `TotalOrders` elvárt értéke `null`!

1. A kód kipróbálásához a weboldal `Customers` menüpontjára kell navigálni. Itt táblázatos formában megjelenítve láthatod az összegyűjtött információkat. Teszteléshez alkalmazhatod az előző feladatban elkészített `Add new order` funkciót — itt új megrendelés felvétele esetén az előző feladatban bedrótozott vevő mellett növekednie kell a hozzá tartozó megrendelések összértékének.

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** a vevő listázó weboldalról. A képet a megoldásban `f4.png` néven add be. A képernyőképen látszódjon a **vevők listája**. Ellenőrizd, hogy a **Neptun kódod** (az oldal aljáról) látható-e a képen! A képernyőkép szükséges feltétele a pontszám megszerzésének.

## 5. Feladat: Opcionális (iMSc) feladat (3 iMsc pont)

Ebben a feladatban az adatbázisban található megrendeléseket fogjuk dátum szerint csoportosítani — kíváncsiak vagyunk ugyanis az elmúlt időszakokban a megrendelések mennyiségének és összértékének alakulására. Ennek megvalósításához a [`$bucket`](https://docs.mongodb.com/manual/reference/operator/aggregation/bucket/) aggregációs lépést fogjuk használni.

### Követelmények

A megvalósítandó metódus az `OrderGroups GroupOrders(int groupsCount)`. Ez az adatbázisban található megrendeléseket `groupsCount` egyenlő időintervallumra csoportosítja. A visszatérési érték két adattagot tartalmaz:

- `IList<DateTime> Thresholds`: Az időpontok amik az időintervallumok határait jelentik.
    - Az intervallumok alsó határa inkluzív, a felső határ exkluzív
    - `n` darab intervallum esetén a `Thresholds` lista `n + 1` elemű
    - _Pl.: Ha a `Thresholds` lista elemei `a, b, c, d`, akkor az időintervallumok a következők: `[a, b[`, `[b, c[` és `[c, d[`._
- `IList<OrderGroup> Groups`: A megrendelések csoportjai. A `OrderGroup` entitás adattagjai:
    - `Date`: Az adott intervallum _kezdő_ dátuma. Tehát `[a, b[` intervallum esetén `a`.
    - `Pieces`: Az adott intervallumba eső megrendelések darabszáma.
    - `Total`: Az adott intervallumba tartozó összes megrendelés összértékének összege (lásd előző feladat).

További követelmények:

1. Pontosan `groupsCount` intervallumra bontsd a megrendeléseket.
    - A `Thresholds` lista elemszáma így **pontosan** `groupsCount + 1`.
    - A `Groups` elemszáma **legfeljebb** `groupsCount` — az üres (megrendelést nem tartalmazó) intervallumoknak nem kell listaelem
1. A legelső határ legyen az adatbázisban található legrégebbi megrendelés dátuma
1. A legutolsó határ legyen az adatbázisban található legújabb megrendelés dátuma + 1 óra
    - Ez azért kell, mert az intervallum felső határa exkluzív. Így garantáljuk, hogy az adatbázisban található minden megrendelés belekerül egy intervallumba.
    - _Tipp: a következő módon tudsz egy órát hozzáadni egy dátumhoz: `date.AddHours(1)`._
1. Az intervallumok legyenek egyenlő méretűek
    - _Tipp: a C# nyelv beépítve kezeli matematikai műveletek végzését dátum (`DateTime`) és időtartam (`TimeSpan`) objektumokon — lásd pl. előző pont._

A megoldás során feltételezheted a következőket:

- Az adatbázisban található minden megrendelésnek van `Date` értéke, annak ellenére, hogy az adattag típusa _nullable_ (`DateTime?`).
    - Használd tehát nyugodtan a `date.Value` értéket a `date.HasValue` érték ellenőrzése nélkül.
- A `groupsCount` egy pozitív egész szám (tehát értéke **legalább 1**).

### Megoldás vázlata

1. Kérdezd le az adatbázisból a legrégebbi és a legújabb megrendelés dátumát.

    - _Tipp: Ezt megoldhatod két lekérdezéssel vagy akár egyetlen aggregáció segítségével is._

1. Számold ki az intervallumok határait a követelményeknek megfelelően.

    - Ezzel már meg is fogod kapni a visszatérési értékben szükséges `Thresholds` listát.

1. A megrendelések kollekción hajts végre egy `$bucket` aggregációt. Ennek dokumentációját [itt](https://docs.mongodb.com/manual/reference/operator/aggregation/bucket/) találhatod meg.

    - a `groupBy` kifejezés a megrendelés dátuma lesz
    - a `boundaries` kifejezés pontosan abban a formában várja az értékeket, amit a követelmények között is olvashattál, tehát az előző pontban előállított lista pontosan megfelel
    - az `output` kifejezésben fogalmazhatod meg a szükséges kiszámolandó értékeket (darabszám, összérték)

    !!! tip "Tipp"
        Ha `"Element '...' does not match any field or property of class..."` kezdetű exceptiont kapsz, akkor az `output` kifejezésben írj át minden property-t **kisbetűsre** (pl.: `Pieces` -> `pieces`). Úgy tűnik, hogy a Mongo C# driver ezen a ponton nem konzisztensen transzformálja az adattagok neveit.

1. A `$bucket` aggregáció az elvárt követelményeknek pontosan megfelelő intervallumokkal fog visszatérni, így az eredményét már csak transzformálni kell a megfelelő `OrderGroup` objektumok listájává, majd pedig előállítani a visszatérési értéket.

1. A kód kipróbálásához a weboldal `Group orders` menüpontjára kell navigálni. Itt diagramokon megjelenítve láthatod az összegyűjtött információkat. Tesztelés során módosítsd a csoportok számát, és vegyél fel új megrendeléseket különböző dátumokkal korábbi feladatokban elkészített `Add new order` segítségével!

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** a diagramokat mutató weboldalról. A képet a megoldásban `f5.png` néven add be. A képernyőképen látszódjon a **két diagram** (ehhez csökkentsd a böngészőben a nagyítást, hogy kiférjenek egy képernyőre). Ellenőrizd, hogy a **Neptun kódod** (az oldal aljáról) látható-e a képen! A képernyőkép szükséges feltétele a pontszám megszerzésének.
