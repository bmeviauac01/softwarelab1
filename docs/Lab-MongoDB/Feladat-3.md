# Feladat 3: Megrendelések lekérdezése és módosítása (5p)

Ebben a feladatban a Megrendelés entitáshoz tartozó CRUD (létrehozás, listázás/olvasás, módosítás és törlés) utasításokat fogjuk megvalósítani. Ez a feladat nagyon hasonlít az első feladathoz, amennyiben elakadnál nyugodtan meríts ihletet az ottani megoldásokból!

A `Models.Megrendeles` entitás adattagjai:

- `ID`: az adatbázisentitás `ID`-ja, `ToString`-gel sorosítva
- `Datum`, `Hatarido`, `Statusz`: egy az egyben másolandók az adatbázis entitásból
- `FizetesMod`: az adatbázis entitásban található `FizetesMod` komplex objektum `Mod` mezője
- `OsszErtek`: a megrendelésben található megrendeléstételek `Mennyiseg` és `NettoAr` szorzatainak összege

Ennek a feladatnak a megoldásához a megrendeléssel kapcsolatos metódusok implementációja szükséges (`List`, `Find`, `Insert`, `Delete` és `Update`).

Az alábbi feladatok előtt ne felejtsd el felvenni és inicializálni a `megrendelesCollection`-t a repository osztályba a korábban látottak mintájára!

## Listázás/olvasás

1. A `ListMegrendelesek` függvény paraméterként kap egy `string keresetStatusz` értéket. Ha ez az érték üres vagy `null` (lásd: `string.IsNullOrEmpty`), akkor minden megrendelést listázz. Ellenkező esetben csak azokat a megrendeléseket listázd, melyeknek a `Statusz` értéke teljesen egyezik a `keresettStatusz` értékkel.

   Az `OsszErtek` kiszámolásához használd a `Project` utasítást. Ebben összetett LINQ kifejezéseket is megfogalmazhatunk, ezeket a driver a megfelelő _Mongo_ kifejezésekre fordítja. Ennek szintaktikája a következő.

   ```csharp
   collection
       .Find(x => /* filter kifejezés */)
       .Project(x => new { ID = x.ID, Ossz = x.Dolgok.Sum(d => d.Ertek), /* ... */ })
       .ToList();
   ```

1. A `FindMegrendeles` metódus egy konkrét megrendelés adatait adja vissza a `string id` érték alapján szűrve. Figyelj oda, ha az adott `ID` érték nem található az adatbázisban, akkor `null` értéket adj vissza!

## Létrehozás

1. Az `InsertMegrendeles` metódusban a létrehozást kell megvalósítanod. Ehhez háromféle információt kapsz: `Megrendeles megrendeles`, `Termek termek` és `int mennyiseg`.

1. Az adatbázisentitás létrehozásához a következő információkra van szükség:
   - `VevoID`, `TelephelyID`: az adatbázisból keresd ki a `Grosz János`-hoz tartozó dokumentum `_id` és `kozpontiTelephelyID` értékét. Ezeket az értékeket drótozd bele a kódodba.
   - `Datum`, `Hatarido`, `Statusz`: ezeket az értékeket a `megrendeles` paraméterből veheted
   - `FizetesMod`: hozz létre egy `FizetesMod` objektumot. A `Mod` legyen a `megrendeles` paraméterben található `FizetesMod` érték. A `Hatarido` maradjon `null`!
   - `MegrendelesTetelek`: egyetlen megrendeléstételt hozz létre! Ennek adattagjai:
     - `TermekID` és `NettoAr`: a `termek` paraméterből veheted
     - `Mennyiseg`: a `mennyiseg` paraméterből jön
     - `Statusz`: a `megrendeles` paraméter `Statusz` mezőjével egyezik meg
   - minden más adattag (a számlázással kapcsolatos információk) maradjon `null` értéken!

## Törlés

A `DeleteMegrendeles` törölje a megadott `ID`-hoz tartozó megrendelést.

## Módosítás

A módosító utasításban (`UpdateMegrendeles`) arra figyelj oda, hogy csak azokat a mezőket írd felül, melyek a `Models.Megrendeles` osztályban megtalálhatóak: `Datum`, `Hatarido`, `Statusz` és `FizetesMod`. Az `OsszErtek`-et itt nem kell figyelembe venni, ennek értéke nem fog változni.

> Tipp: több módosító kifejezést a `Builders<Entities.Megrendeles>.Update.Combine` segítségével lehet összevonni.

Itt is figyelj oda, hogy az update során az `IsUpsert` beállítás értéke legyen `false`!

A metódus visszatérési értéke akkor és csak akkor legyen `true`, ha létezik megrendelés a megadott `ID`-val — azaz volt illeszkedés a filterre.

## Kipróbálás

A megírt függvényeket a weboldalon a `Megrendelések` menüpont alatt tudod kipróbálni. Teszteld le a `Szűrés`, `Új megrendelés felvétele`, `Módosítás`, `Részletek` és `Törlés` opciókat is!

## Képernyőkép

Készíts egy képernyőképet az implementált függvények kódjáról. A képernyőképpel kapcsolatos elvárásokat lásd [itt](../README.md#képernyőképek).

> A képet a megoldásban `f3-megrendelesek.png` néven add be. A képernyőképen szerepeljen legalább egy metódus kódja teljesen.
>
> A képernyőkép szükséges feltétele a pontszám megszerzésének.

## Következő feladat

Folytasd a [következő feladattal](Feladat-4.md).
