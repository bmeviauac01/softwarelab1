# Feladat 5: Opcionális feladat (3 iMsc pont)

Ebben a feladatban az adatbázisban található megrendeléseket fogjuk dátum szerint csoportosítani — kíváncsiak vagyunk ugyanis az elmúlt időszakokban a megrendelések mennyiségének és összértékének alakulására. Ennek megvalósításához a `$bucket` aggregációs lépést fogjuk használni.

## Követelmények

A megvalósítandó metódus a `MegrendelesCsoportok MegrendelesCsoportosit(int csoportDarab)`. Ez az adatbázisban található megrendeléseket `csoportDarab` egyenlő időintervallumra csoportosítja. A visszatérési érték két adattagot tartalmaz:

* `IList<DateTime> Hatarok`: Az időpontok amik az időintervallumok határait jelentik.
  * Az intervallumok alsó határa inkluzív, a felső határ exkluzív
  * `n` darab intervallum esetén a `Hatarok` lista `n + 1` elemű
  * _Pl.: Ha a `Hatarok` lista elemei `a, b, c, d`, akkor az időintervallumok a következők: `[a, b[`, `[b, c[` és `[c, d[`._
* `IList<MegrendelesCsoport> Csoportok`: A megrendelések csoportjai. A `MegrendelesCsoport` entitás adattagjai:
  * `Datum`: Az adott intervallum **kezdő** dátuma. Tehát `[a, b[` intervallum esetén `a`.
  * `Darab`: Az adott intervallumba eső megrendelések darabszáma.
  * `OsszErtek`: Az adott intervallumba tartozó összes megrendelés összértékének összege (lásd előző feladat).

További követelmények:

1. Pontosan `csoportDarab` intervallumra bontsd a megrendeléseket.
   * A `Hatarok` lista elemszáma így **pontosan** `csoportDarab + 1`.
   * A `Csoportok` elemszáma **legfeljebb** `csoportDarab` — az üres (megrendelést nem tartalmazó) intervallumoknak nem kell listaelem
1. A legelső határ legyen az adatbázisban található legrégebbi megrendelés dátuma
1. A legutolsó határ legyen az adatbázisban található legújabb megrendelés dátuma + 1 óra
   * Ez azért kell, mert az intervallum felső határa exkluzív. Így garantáljuk, hogy az adatbázisban található minden megrendelés belekerül egy intervallumba.
   * _Tipp: a következő módon tudsz egy órát hozzáadni egy dátumhoz: `datum.AddHours(1)`._
1. Az intervallumok legyenek egyenlő méretűek
   * _Tipp: a C# nyelv beépítve kezeli matematikai műveletek végzését dátum és `TimeSpan` objektumokon — lásd pl. előző pont._

A megoldás során feltételezheted a következőket:

* Az adatbázisban található minden megrendelésnek van `Datum` értéke, annak ellenére, hogy az adattag típusa _nullable_ `DateTime?`.
  * Használd tehát nyugodtan a `datum.Value` értéket a `datum.HasValue` érték ellenőrzése nélkül.
* A `csoportDarab` egy pozitív egész szám (tehát értéke **legalább 1**).

## Megoldás vázlata

1. Kérdezd le az adatbázisból a legrégebbi és a legújabb megrendelés dátumát.
   * _Tipp: Ezt megoldhatod két lekérdezéssel vagy akár egyetlen aggregáció segítségével is._

1. Számold ki az intervallumok határait a követelményeknek megfelelően.
   * Ezzel már meg is fogod kapni a visszatérési értékben szükséges `Hatarok` listát.

1. A megrendelések kollekción hajts végre egy `$bucket` aggregációt. Ennek dokumentációját [itt](https://docs.mongodb.com/manual/reference/operator/aggregation/bucket/) találhatod meg.
   * a `groupBy` kifejezés a megrendelés dátuma lesz
   * a `boundaries` kifejezés pontosan abban a formában várja az értékeket, amit a követelmények között is olvashattál, tehát az előző pontban előállított lista pontosan megfelel
   * az `output` kifejezésben fogalmazhatod meg a szükséges kiszámolandó értékeket (darabszám, összérték)

   > _Tipp: ha `"Element '...' does not match any field or property of class..."` kezdetű exceptiont kapsz, akkor az `output` kifejezésben írj át minden property-t **kisbetűsre** (pl.: `Darab` -> `darab`). Sajnos úgy tűnik, hogy a Mongo C# driver ezen a ponton nem konzisztensen transzformálja az adattagok neveit._

1. A `$bucket` aggregáció az elvárt követelményeknek pontosan megfelelő intervallumokkal fog visszatérni, így az eredményét már csak transzformálni kell a megfelelő `MegrendelesCsoport` objektumok listájává, majd pedig előállítani a visszatérési értéket.

1. A kód kipróbálásához a weboldal `Megrendelések csoportosítása` menüpontjára kell navigálni. Itt diagramokon megjelenítve láthatod az összegyűjtött információkat. Tesztelés során módosítsd a csoportok számát, és vegyél fel új megrendeléseket különböző dátumokkal korábbi feladatokban elkészített `Új megrendelés felvétele` segítségével!

1. Készíts egy képernyőképet az egyik diagramról! A diagramon látsszon, hogy vettél fel új megrendelést is! (A képernyőképpel kapcsolatos elvárásokról lásd a kezdőoldalon.)

   > A képet a megoldásban `f5-megrendelescsoportok.png` néven add be.
   >
   > A képernyőkép szükséges feltétele a pontszám megszerzésének.

## Következő feladat

Nincs több feladat. Add be a megoldásod az [itt](README.md#végezetül-a-megoldások-feltöltése) leírtak szerint.
