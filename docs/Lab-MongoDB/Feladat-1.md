# Feladat 1: Termékek lekérdezése és módosítása (7p)

Ebben a feladatban a Termék entitáshoz tartozó CRUD (létrehozás, listázás/olvasás, módosítás és törlés) utasításokat fogjuk megvalósítani.

## Visual Studio solution megnyitása

Nyisd meg a letöltött repository-ban a Visual Studio solution-t (`.sln` fájl). Ha a megnyitás során a Visual Studio azt jelezni, hogy a projekt típus nem támogatott, akkor telepítsd a Visual Studio hiányzó komponenseit (lásd az előző oldalon a linket).

> **NE** frissítsd a projektet, se a .NET Core verziót, se a Nuget csomagokat! Ha ilyen kérdéssel találkozol a solution megnyitása során, akkor mindig mondj nemet!

Munkád során a `MongoLabor.DAL.AdatvezRepository` osztályba dolgozz! Ezen fájl tartalmát tetszőlegesen módosíthatod (feltéve, hogy továbbra is megvalósítja a `MongoLabor.DAL.IAdatvezRepository` interfészt, és természetesen továbbra is fordul a kód). A projekt minden egyéb tartalma már elő van készítve a munkához, ezeket **NE** módosítsd!

## Listázás/olvasás

1. Első lépésként szükség lesz az adatbázisban található `termekek` kollekció elérésére a kódból. Ehhez végy fel egy új mezőt az `AdatvezRepository` osztályban, és inicializáld ezt a konstruktorban. Az ehhez szükséges `IMongoDatabase` objektumot _Dependency Injection_ segítségével, konstruktorparaméterként kaphatod meg.

   ```csharp
   private readonly IMongoCollection<Entities.Termek> termekCollection;

   public AdatvezRepository(IMongoDatabase database)
   {
       this.termekCollection = database.GetCollection<Entities.Termek>("termekek");
   }
   ```

1. A `termekCollection` segítségével már tudunk lekérdező/listázó utasításokat írni. Valósítsuk meg először a `ListTermekek` függvényt. Ez a függvény két lépésből áll: először lekérdezzük az adatbázisból a termékek listáját, majd pedig átkonvertáljuk az elvárt `Models.Termek` osztály elemekből álló listára.

   A lekérdezés a következőképpen néz ki:

   ```csharp
   var dbTermekek = termekCollection
       .Find(_ => true) // minden terméket listázunk — üres filter
       .ToList();
   ```

   Ezt a listát aztán transzformálva adjuk vissza.

   ```csharp
   return dbTermekek
       .Select(t => new Termek
       {
           ID = t.ID.ToString(),
           Nev = t.Nev,
           NettoAr = t.NettoAr,
           Raktarkeszlet = t.Raktarkeszlet
       })
       .ToList();
   ```

1. A `FindTermek(string id)` függvény megvalósítása nagyon hasonlít az előzőhöz, csupán annyiban különbözik, hogy itt egyetlen terméket kérdezünk le, `ID` alapján. A konvertáló lépés ugyanúgy megmarad. Ebben az esetben oda kell figyelnünk azonban, hogy ha nem találjuk az adott terméket, akkor adjunk vissza `null` értéket, ne próbáljunk konvertálni.

   A lekérdező lépés a következőre módosul:

   ```csharp
   var dbTermek = termekCollection
       .Find(t => t.ID == ObjectId.Parse(id))
       .SingleOrDefault();
   ```

   Figyeljük itt meg, hogy hogyan módosult a filter kifejezés! Fontos továbbá, hogy itt `ToList` helyett a `SingleOrDefault` kiértékelő kifejezést használjuk. Ez a megszokott módon vagy egy konkrét terméket ad vissza, vagy `null` értéket. Így tudunk tehát `ID` alapján megtalálni egy entitást az adatbázisban. Jegyezzük ezt meg, mert ez még sok következő feladatban hasznos lesz!

   A konvertáló kifejezést már megírtuk egyszer az előző kifejezésben, ezt használhatjuk itt is — figyeljünk azonban oda, hogy a `dbTermek` értéke lehet `null` is. Ebben az esetben ne konvertáljunk, csupán adjunk vissza `null` értéket mi is.

1. Próbáld ki a megírt függvények működését! Indítsd el a programot, és navigálj tetszőleges böngészőben a `localhost:5000` weboldalra. Itt a `Termékek` menüpontban már látnod kell a termékeket felsoroló táblázatot. Ha valamelyik sorban a `Részletek` menüpontra kattintasz, akkor egy adott termék részleteit is látnod kell.

## Létrehozás

1. Ebben a pontban az `InsertTermek(Termek termek)` függvényt valósítjuk meg. A bemenő `Models.Termek` entitás adatait a felhasználó szolgáltatja a felhasználói felületen keresztül.

1. Egy termék létrehozásához először létre kell hoznunk egy új adatbázisentitás objektumot. Jelen esetben ez egy `Entities.Termek` objektum lesz. Az `ID` értéket nem kell megadnunk — ezt majd az adatbázis generálja. A `Nev`, `NettoAr` és `Raktarkeszlet` értékeket a felhasználó szolgáltatja. Két érték maradt ki: az `AFA` és a `KategoriaID`. Az előbbinek adjunk tetszőleges értéket, az utóbbinak pedig keressünk egy szimpatikus kategóriát az adatbázisban _Robo3T_ segítségével, és annak az `_id` értékét drótozzuk be!

   ```csharp
   var dbTermek = new Entities.Termek
   {
       Nev = termek.Nev,
       NettoAr = termek.NettoAr,
       Raktarkeszlet = termek.Raktarkeszlet,
       AFA = new Entities.AFA { Nev = "Általános", Kulcs = 20 },
       KategoriaID = ObjectId.Parse("5d7e42adcffa8e1b64f7dbbb"),
   };
   ```

   Ha megvan az adatbázisentitás objektum, akkor az `InsertOne` utasítás segítségével tudjuk elmenteni az adatbázisba.

1. A függvény kipróbálásához indítsd el megint a programot, és a termékek táblázata feletti `Új termék hozzáadása` linkre kattints. Itt meg tudod adni a szükséges adatokat, amivel maghívódik a kódod.

## Törlés

1. A törlés megvalósításához a `DeleteTermek(string id)` függvényt kell implementálni. Itt a kollekció `DeleteOne` függvényét kell használni. Ehhez meg kell adni egy filter kifejezést: itt használjuk ugyanazt a filtert, amit az egy konkrét termék lekérdezéséhez használtunk a `FindTermek(string id)` metódusban.

1. Ezt a metódust is ki tudod próbálni, ha a termékek táblázatában valamelyik sorban a `Törlés` linkre kattintasz.

## Módosítás

1. A termékek módosító utasításaként az eladást fogjuk megvalósítani, a `bool TermekElad(string id, int mennyiseg)` függvényben. A függvény akkor és csak akkor térjen vissza `true` értékkel, ha létezik az `id` azonosítójú termék, és legalább `mennyiseg` darab van belőle raktáron, amit el is tudtunk adni. Ha nem létezik a termék vagy nincs belőle elegendő, akkor térjen vissza `false` értékkel.

1. A MongoDB atomicitását kihasználva ezt a feladatot egyetlen utasítás kiadásával fogjuk megvalósítani. A filter kifejezésben fogunk rászűrni a megadott `id`-ra, és arra is, hogy elegendő termék van-e raktáron. A módosító kifejezésben fogjuk csökkenteni a raktárkészletet — csak akkor, ha létezik a termék, és elegendő van belőle raktáron.

   ```csharp
   var result = termekCollection.UpdateOne(
       filter: t => t.ID == ObjectId.Parse(id) && t.Raktarkeszlet >= mennyiseg,
       update: Builders<Entities.Termek>.Update.Inc(t => t.Raktarkeszlet, -mennyiseg),
       options: new UpdateOptions { IsUpsert = false });
   ```

   Fontos megfigyelni, hogy az `UpdateOptions` segítségével jeleztük az adatbázisnak, hogy **NE** upsertet hajtson végre — tehát ne tegyen semmit ha nem találja a filterben megadott terméket.

   A módosító utasítást a `Builders`-ben található `Update` builder segítségével tudjuk összerakni. Esetünkben `mennyiseg`-gel akarjuk csökkenteni az értéket — ez `-mennyiseg`-gel történő _inkrementálást_ jelent.

   A visszatérési értéket az update `result`-jából tudjuk meghatározni. Amennyiben talált olyan terméket, ami megfelelt a filternek, akkor sikeres volt a módosítás, tehát `true` értékkel térhetünk vissza. Egyébként a visszatérési érték `false`.

   ```csharp
   return result.MatchedCount > 0;
   ```

1. Ezt a metódust is ki tudod próbálni, ha a termékek táblázatában valamelyik sorban a `Vásárlás` linkre kattintasz. Próbáld ki úgy is, ha túl nagy értéket írsz be!

## Képernyőkép

Készíts egy képernyőképet az implementált függvények kódjáról. (A képernyőképpel kapcsolatos elvárásokról lásd a kezdőoldalon.)

> A képet a megoldásban `f1-termekek.png` néven add be. A képernyőképen szerepeljen legalább egy metódus kódja teljesen.
>
> A képernyőkép szükséges feltétele a pontszám megszerzésének.

## Következő feladat

Folytasd a [következő feladattal](Feladat-2.md).
