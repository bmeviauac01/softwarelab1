# Feladat 2: Kategóriák listázása (4p)

Ebben a feladatban a kategóriákat fogjuk listázni — az adott kategóriába tartozó termékek számával együtt. Ehhez már aggregációs utasítást is használnuk kell majd. (Továbbra is a `MongoLabor.DAL.AdatvezRepository` osztályba dolgozunk.)

A megvalósítandó függvény a `IList<Kategoria> ListKategoriak()`. Ennek minden kategóriát vissza kell adnia. A `Models.Kategoria` osztály 3 adattagot tartalmaz.

- `Nev`: értelemszerűen a kategória neve
- `SzuloKategoriaNev`: a kategória szülókategóriájának neve. Amennyiben nincs szülőkategória, értéke legyen `null`.
- `TermekDarab`: a kategóriába tartozó termékek száma. Amennyiben nincs ilyen, értéke legyen **0**.

A megvalósítás lépései a következők.

1. Első lépésként a `termekCollection` mintájára vedd fel és inicializáld a `kategoriaCollection`-t is. Az adatbázisban a kollekció neve `kategoriak` — ezt _Robo3T_ segítségével tudod ellenőrizni.

1. A `ListKategoriak()` metódusban először kérdezzük le a kategóriák teljes listáját. Ez pontosan ugyanúgy történik, mint az előző feladatban a termékek esetében. A lekérdezés értékét tegyük a `dbKategoriak` változóba.

1. Ezután kérdezzük le, hogy az egyes `KategoriaID`-khez hány darab termék tartozik. Ehhez már az aggregációs pipeline-t kell használnunk, azon belül pedig a [`$group`](https://docs.mongodb.com/manual/reference/operator/aggregation/group/) lépést.

   ```csharp
   var termekDarabok = termekCollection
       .Aggregate()
       .Group(t => t.KategoriaID, g => new { KategoriaID = g.Key, TermekDarab = g.Count() })
       .ToList();
   ```

   Ez az utasítás egy olyan listával tér vissza, melyben minden elem egy `KategoriaID` értéket tartalmaz a hozzá tartozó termékek számával együtt.

1. Ezen a ponton minden számunkra szükséges információ rendelkezésünkre áll — ismerjük az összes kategóriát (a szülőkategória megkereséséhez), és ismerjük a kategóriákhoz tartozó termékek számát. Egyetlen dolgunk van, hogy ezeket az információkat (C# kódból) "összefésüljük".

   ```csharp
   return dbKategoriak
       .Select(k =>
       {
           string szuloKategoriaNev = null;
           if (k.SzuloKategoriaID.HasValue)
               szuloKategoriaNev = dbKategoriak.Single(sz => sz.ID == k.SzuloKategoriaID.Value).Nev;

           var termekDarab = termekDarabok.SingleOrDefault(td => td.KategoriaID == k.ID)?.TermekDarab ?? 0;

           return new Kategoria { Nev = k.Nev, SzuloKategoriaNev = szuloKategoriaNev, TermekDarab = termekDarab };
       })
       .ToList();
   ```

   Láthatjuk, hogy mind a két lépést egyszerűen elintézhetjük C#-ban LINQ segítségével. A szülőkategória nevéhez a kategóriák között kell keresnünk, a termékek darabszámához pedig az aggregáció eredményében keresünk.

1. A kód kipróbálásához a weboldal `Kategóriák` menüpontjára kell navigálni. Itt táblázatos formában megjelenítve láthatod az összegyűjtött információkat. Teszteléshez alkalmazhatod az előző feladatban elkészített `Új termék hozzáadása` funkciót — itt a hozzáadás esetén az egyik kategória mellett növekednie kell a hozzá tartozó termékek darabszámának.

1. Készíts egy képernyőképet implementált függvény kódjáról. A képernyőképpel kapcsolatos elvárásokat lásd [itt](../README.md#képernyőképek).

   > A képet a megoldásban `f2-kategoriak.png` néven add be.
   >
   > A képernyőkép szükséges feltétele a pontszám megszerzésének.

## Következő feladat

Folytasd a [következő feladattal](Feladat-3.md).
