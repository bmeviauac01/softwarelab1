# Feladat 5: Opcionális feladat

**A feladat megoldásával 3 iMsc pont szerezhető.**

Ebben a feladatban az adatbázisban található megrendeléseket fogjuk dátum szerint csoportosítani — kíváncsiak vagyunk ugyanis az elmúlt időszakokban a megrendelések mennyiségének és összértékének alakulására. Ennek megvalósításához a [`$bucket`](https://docs.mongodb.com/manual/reference/operator/aggregation/bucket/) aggregációs lépést fogjuk használni.

## Követelmények

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
    - _Tipp: a C# nyelv beépítve kezeli matematikai műveletek végzését dátum és `TimeSpan` objektumokon — lásd pl. előző pont._

A megoldás során feltételezheted a következőket:

- Az adatbázisban található minden megrendelésnek van `Date` értéke, annak ellenére, hogy az adattag típusa _nullable_ `DateTime?`.
    - Használd tehát nyugodtan a `date.Value` értéket a `date.HasValue` érték ellenőrzése nélkül.
- A `groupsCount` egy pozitív egész szám (tehát értéke **legalább 1**).

## Megoldás vázlata

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
