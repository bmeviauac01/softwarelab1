# Feladat 2: Kategóriák listázása

**A feladat megoldásával 4 pont szerezhető.**

Ebben a feladatban a kategóriákat fogjuk listázni — az adott kategóriába tartozó termékek számával együtt. Ehhez már aggregációs utasítást is használnuk kell majd. (Továbbra is a `MongoLabor.DAL.Repository` osztályba dolgozunk.)

A megvalósítandó függvény a `IList<Category> ListCategories()`. Ennek minden kategóriát vissza kell adnia. A `Models.Category` osztály 3 adattagot tartalmaz.

- `Name`: értelemszerűen a kategória neve
- `ParentCategoryName`: a kategória szülőkategóriájának neve. Amennyiben nincs szülőkategória, értéke legyen `null`.
- `NumberOfProducts`: a kategóriába tartozó termékek száma. Amennyiben nincs ilyen, értéke legyen **0**.

A megvalósítás lépései a következők.

1. Első lépésként a `productCollection` mintájára vedd fel és inicializáld a `categoryCollection`-t is. Az adatbázisban a kollekció neve `categories` — ezt _Robo3T_ segítségével tudod ellenőrizni.

1. A `ListCategories()` metódusban először kérdezzük le a kategóriák teljes listáját. Ez pontosan ugyanúgy történik, mint az előző feladatban a termékek esetében. A lekérdezés értékét tegyük a `dbCategories` változóba.

1. Ezután kérdezzük le, hogy az egyes `CategoryID`-khez hány darab termék tartozik. Ehhez aggregációs pipeline-t kell használnunk, azon belül pedig a [`$group`](https://docs.mongodb.com/manual/reference/operator/aggregation/group/) lépést.

    ```csharp
    var productCounts = productCollection
        .Aggregate()
        .Group(t => t.CategoryID, g => new { CategoryID = g.Key, NumberOfProducts = g.Count() })
        .ToList();
    ```

    Ez az utasítás egy olyan listával tér vissza, melyben minden elem egy `CategoryID` értéket tartalmaz a hozzá tartozó termékek számával együtt.

1. Ezen a ponton minden számunkra szükséges információ rendelkezésünkre áll — ismerjük az összes kategóriát (a szülőkategória megkereséséhez), és ismerjük a kategóriákhoz tartozó termékek számát. Egyetlen dolgunk van, hogy ezeket az információkat (C# kódból) "összefésüljük".

    ```csharp
    return dbCategories
    .Select(c =>
    {
        string parentCategoryName = null;
        if (c.ParentCategoryID.HasValue)
            parentCategoryName = dbCategories.Single(p => p.ID == c.ParentCategoryID.Value).Name;

        var numProd = productCounts.SingleOrDefault(pc => pc.CategoryID == c.ID)?.NumberOfProducts ?? 0;
        return new Category { Name = c.Name, ParentCategoryName = parentCategoryName, NumberOfProducts = numProd };
    })
    .ToList();
    ```

    Láthatjuk, hogy mind a két lépést egyszerűen elintézhetjük C#-ban LINQ segítségével. A szülőkategória nevéhez a kategóriák között kell keresnünk, a termékek darabszámához pedig az aggregáció eredményében keresünk.

    !!! note ""
        Nem ez az egyetlen módja a gyűjtemények "összekapcsolásának" MongoDB-ben. Ugyan `join` nincs, de léteznek megoldások a gyűjteményeken átívelő lekérdezésekre. A fenti megoldás az adatbázis helyett C# kódban végzi el az összekapcsolást. Ez akkor praktikus, ha nem nagy méretű adathalmazzal dolgozunk, és nincs (jó) szűrésünk. Ha szűrni kellene az adathalmazt, a fenti megoldás is bonyolultabb lenne a hatékonyság érdekében.

1. A kód kipróbálásához a weboldal `Categories` menüpontjára kell navigálni. Itt táblázatos formában megjelenítve láthatod az összegyűjtött információkat. Teszteléshez alkalmazhatod az előző feladatban elkészített `Add new product` funkciót — itt a hozzáadás esetén az egyik kategória mellett növekednie kell a hozzá tartozó termékek darabszámának. (A termék beszúrásakor a kategória ID-ját bedrótoztuk a kódba, tehát annak a kategóriának kell nőnie, amelyiknek az ID-ja a kódban szerepel.)

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** a kategória listázó weboldalról. A képet a megoldásban `f2.png` néven add be. A képernyőképen látszódjon a **kategória lista**. Ellenőrizd, hogy a **Neptun kódod** (az oldal aljáról) látható-e a képen! A képernyőkép szükséges feltétele a pontszám megszerzésének.
