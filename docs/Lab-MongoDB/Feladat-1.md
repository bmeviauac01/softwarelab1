# Feladat 1: Termékek lekérdezése és módosítása

**A feladat megoldásával 7 pont szerezhető.**

Ebben a feladatban a `Product` entitáshoz tartozó CRUD (létrehozás, listázás/olvasás, módosítás és törlés) utasításokat fogjuk megvalósítani.

## Visual Studio solution megnyitása

Nyisd meg a letöltött repository-ban a Visual Studio solution-t (`.sln` fájl). Ha a megnyitás során a Visual Studio azt jelezné, hogy a projekt típus nem támogatott, akkor telepítsd a Visual Studio hiányzó komponenseit (lásd [itt](../VisualStudio.md)).

!!! warning "NE frissíts semmilyen verziót"
    Ne frissítsd se a projektet, se a .NET Core verziót, se a Nuget csomagokat! Ha ilyen kérdéssel találkozol a solution megnyitása során, akkor mindig mondj nemet!

Munkád során a `mongolab.DAL.Repository` osztályba dolgozz! Ezen fájl tartalmát tetszőlegesen módosíthatod (feltéve, hogy továbbra is megvalósítja a `mongolab.DAL.IRepository` interfészt, továbbra is van egy konstruktora egyetlen `IMongoDatabase` paraméterrel és természetesen továbbra is fordul a kód).

Az adatbázis elérése a `mongolab.DAL.MongoConnectionConfig` osztályban van. Ha szükséges, az adatbázis nevét megváltoztathatod a forrásban jelölt helyen.

A projekt minden egyéb tartalma már elő van készítve a munkához, a fentieken kívül máshol **NE** módosítsd!

!!! info ""
    A webalkalmazás egy un. [_Razor Pages_](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/) típusú ASP.NET Core projekt. Ezt egy szerver oldalon renderelt megjelenítési réteg, ahol tehát a weboldal html kódját C# kód és a Razor állítja elő. (A megjelenítési réteggel nem lesz feladatod, az már elő van készítve számodra.)

## Webalkalmazás elindítása

Próbáld ki, hogy elindul-e a program.

1. Fordítsd le és indítsd el Visual Studio-ból az alkalmazást.

1. Nyisd meg böngészőben a <http://localhost:5000/> oldalt.

!!! success ""
    Ha minden rendben ment, akkor bejött el oldal, ahol az egyes feladatokat ki fogod tudni próbálni. (Az egyes feladatokra mutató linkek mögötti oldalak még nem működnek. Előbb még meg kell valósítani az adatelérési réteget.)

## Neptun kód megjelenítése a weboldalon

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

## Listázás/olvasás

1. Első lépésként szükség lesz az adatbázisban található `products` kollekció elérésére a kódból. Ehhez végy fel egy új mezőt a `Repository` osztályban, és inicializáld ezt a konstruktorban. Az ehhez szükséges `IMongoDatabase` objektumot _Dependency Injection_ segítségével, konstruktorparaméterként kaphatod meg.

    ```csharp
    private readonly IMongoCollection<Entities.Product> productCollection;

    public Repository(IMongoDatabase database)
    {
        this.productCollection = database.GetCollection<Entities.Product>("products");
    }
    ```

1. A `productCollection` segítségével már tudunk lekérdező/listázó utasításokat írni. Valósítsuk meg először a `ListProducts` függvényt. Ez a függvény két lépésből áll: először lekérdezzük az adatbázisból a termékek listáját, majd pedig átkonvertáljuk az elvárt `Models.Product` osztály elemekből álló listára.

    A lekérdezés a következőképpen néz ki:

    ```csharp
    var dbProducts = productCollection
        .Find(_ => true) // minden terméket listázunk — üres filter
        .ToList();
    ```

    Ezt a listát aztán transzformálva adjuk vissza.

    ```csharp
    return dbProducts
        .Select(t => new Product
        {
            ID = t.ID.ToString(),
            Name = t.Name,
            Price = t.Price,
            Stock = t.Stock
        })
        .ToList();
    ```

1. A `FindProduct(string id)` függvény megvalósítása nagyon hasonlít az előzőhöz, csupán annyiban különbözik, hogy itt egyetlen terméket kérdezünk le, `ID` alapján. Ügyeljünk rá, hogy az `ID`-t szövegként kapjuk, így `ObjectId`-vá kell alakítani.

    A modellé konvertáló lépés ugyanúgy megmarad. Ebben az esetben oda kell figyelnünk azonban, hogy ha nem találjuk az adott terméket, akkor adjunk vissza `null` értéket, ne próbáljunk konvertálni.

    A lekérdező lépés a következőre módosul:

    ```csharp
    var dbProduct = productCollection
        .Find(t => t.ID == ObjectId.Parse(id))
        .SingleOrDefault();
    // ... model konverzio
    ```

    Figyeljük itt meg, hogy hogyan módosult a filter kifejezés! Fontos továbbá, hogy itt `ToList` helyett a `SingleOrDefault` kiértékelő kifejezést használjuk. Ez a megszokott módon vagy egy konkrét terméket ad vissza, vagy `null` értéket. Így tudunk tehát `ID` alapján megtalálni egy entitást az adatbázisban. Jegyezzük ezt meg, mert ez még sok következő feladatban hasznos lesz!

    A konvertáló kifejezést már megírtuk egyszer az előző kifejezésben, ezt használhatjuk itt is — figyeljünk azonban oda, hogy a `dbProduct` értéke lehet `null` is. Ebben az esetben ne konvertáljunk, csupán adjunk vissza `null` értéket mi is.

1. Próbáld ki a megírt függvények működését! Indítsd el a programot, és navigálj tetszőleges böngészőben a <http://localhost:5000> weboldalra. Itt a `Products` menüpontban már látnod kell a termékeket felsoroló táblázatot. Ha valamelyik sorban a `Details` menüpontra kattintasz, akkor egy adott termék részleteit is látnod kell.

!!! error "Ha nem látsz egyetlen terméket se"
    Ha a weboldalon egyetlen termék se jelenik meg, de az oldal betöltésre kerül és nincs hiba, akkor az adatbázis elérésével van probléma. Valószínűleg nem létezik a megadott nevű adatbázis. Lásd fentebb az adatbázis konfigurálását.

## Létrehozás

1. Ebben a pontban az `InsertProduct(Product product)` függvényt valósítjuk meg. A bemenő `Models.Product` entitás adatait a felhasználó szolgáltatja a felhasználói felületen keresztül.

1. Egy termék létrehozásához először létre kell hoznunk egy új adatbázisentitás objektumot. Jelen esetben ez egy `Entites.Product` objektum lesz. Az `ID` értéket nem kell megadnunk — ezt majd az adatbázis generálja. A `Name`, `Price` és `Stock` értékeket a felhasználó szolgáltatja. Két érték maradt ki: az `VAT` és a `CategoryID`. Az előbbinek adjunk tetszőleges értéket, az utóbbinak pedig keressünk egy szimpatikus kategóriát az adatbázisban _Robo3T_ segítségével, és annak az `_id` értékét drótozzuk be!

    ```csharp
    var dbProduct = new Entities.Product
    {
        Name = product.Name,
        Price = product.Price,
        Stock = product.Stock,
        VAT = new Entities.VAT { Name = "General", Percentage = 20 },
        CategoryID = ObjectId.Parse("5d7e4370cffa8e1030fd2d99"),
    };
    // ... beszuras
    ```

    Ha megvan az adatbázisentitás objektum, akkor az `InsertOne` utasítás segítségével tudjuk elmenteni az adatbázisba.

1. A függvény kipróbálásához indítsd el megint a programot, és a termékek táblázata feletti `Add new product` linkre kattints. Itt meg tudod adni a szükséges adatokat, amivel meghívódik a kódod.

## Törlés

1. A törlés megvalósításához a `DeleteProduct(string id)` függvényt kell implementálni. Itt a kollekció `DeleteOne` függvényét kell használni. Ehhez meg kell adni egy filter kifejezést: itt használjuk ugyanazt a filtert, amit az egy konkrét termék lekérdezéséhez használtunk a `FindProduct(string id)` metódusban.

1. Ezt a metódust is ki tudod próbálni, ha a termékek táblázatában valamelyik sorban a `Delete` linkre kattintasz.

## Módosítás

1. A termékek módosító utasításaként az eladást fogjuk megvalósítani, a `bool SellProduct(string id, int amount)` függvényben. A függvény akkor és csak akkor térjen vissza `true` értékkel, ha létezik az `id` azonosítójú termék, és legalább `amount` darab van belőle raktáron, amit el is tudtunk adni. Ha nem létezik a termék vagy nincs belőle elegendő, akkor térjen vissza `false` értékkel.

1. A MongoDB atomicitását kihasználva ezt a feladatot egyetlen utasítás kiadásával fogjuk megvalósítani. A filter kifejezésben fogunk rászűrni a megadott `id`-ra, és arra is, hogy elegendő termék van-e raktáron. A módosító kifejezésben fogjuk csökkenteni a raktárkészletet — csak akkor, ha létezik a termék, és elegendő van belőle raktáron.

    ```csharp
    var result = productCollection.UpdateOne(
        filter: t => t.ID == ObjectId.Parse(id) && t.Stock >= amount,
        update: Builders<Entities.Product>.Update.Inc(t => t.Stock, -amount),
        options: new UpdateOptions { IsUpsert = false });
    ```

    Fontos megfigyelni, hogy az `UpdateOptions` segítségével jeleztük az adatbázisnak, hogy **NE** upsertet hajtson végre — tehát ne tegyen semmit ha nem találja a filterben megadott terméket.

    A módosító utasítást a `Builders`-ben található `Update` builder segítségével tudjuk összerakni. Esetünkben `amount`-gel akarjuk csökkenteni az értéket — ez `-amount`-gel történő _inkrementálást_ jelent.

    A visszatérési értéket az update `result`-jából tudjuk meghatározni. Amennyiben talált olyan terméket, ami megfelelt a filternek, akkor sikeres volt a módosítás, tehát `true` értékkel térhetünk vissza. Egyébként a visszatérési érték `false`.

    ```csharp
    return result.MatchedCount > 0;
    ```

1. Ezt a metódust is ki tudod próbálni, ha a termékek táblázatában valamelyik sorban a `Buy` linkre kattintasz. Próbáld ki úgy is, ha túl nagy értéket írsz be!

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** a termék listázó weboldalról **miután felvettél legalább egy új terméket**. A képet a megoldásban `f1.png` néven add be. A képernyőképen látszódjon a **termék lista**. Ellenőrizd, hogy a **Neptun kódod** (az oldal aljáról) látható-e a képen! A képernyőkép szükséges feltétele a pontszám megszerzésének.
