# Feladat 3: Megrendelések lekérdezése és módosítása

**A feladat megoldásával 5 pont szerezhető.**

Ebben a feladatban a Megrendelés (`Order`) entitáshoz tartozó CRUD (létrehozás, listázás/olvasás, módosítás és törlés) utasításokat fogjuk megvalósítani. Ez a feladat nagyon hasonlít az első feladathoz, amennyiben elakadnál nyugodtan meríts ihletet az ottani megoldásokból!

A `Models.Order` entitás adattagjai:

- `ID`: az adatbázisentitás `ID`-ja, `ToString`-gel sorosítva
- `Date`, `Deadline`, `Status`: egy az egyben másolandók az adatbázis entitásból
- `PaymentMethod`: az adatbázis entitásban található `PaymentMethod` komplex objektum `Method` mezője
- `Total`: a megrendelésben található tételek (`OrderItems`) `Amount` és `Price` szorzatainak összege

Ennek a feladatnak a megoldásához a megrendeléssel kapcsolatos metódusok implementációja szükséges (`List`, `Find`, `Insert`, `Delete` és `Update`).

Az alábbi feladatok előtt ne felejtsd el felvenni és inicializálni a `orderCollection`-t a repository osztályba a korábban látottak mintájára!

## Listázás/olvasás

1. A `ListOrders` függvény paraméterként kap egy `string status` értéket. Ha ez az érték üres vagy `null` (lásd: `string.IsNullOrEmpty`), akkor minden megrendelést listázz. Ellenkező esetben csak azokat a megrendeléseket listázd, melyeknek a `Status` értéke teljesen egyezik a paraméterben érkező `statusz` értékkel.

1. A `FindOrder` metódus egy konkrét megrendelés adatait adja vissza a `string id` érték alapján szűrve. Figyelj oda, ha az adott `ID` érték nem található az adatbázisban, akkor `null` értéket adj vissza!

## Létrehozás

1. Az `InsertOrder` metódusban a létrehozást kell megvalósítanod. Ehhez háromféle információt kapsz: `Order order`, `Product product` és `int amount`.

1. Az adatbázisentitás létrehozásához a következő információkra van szükség:

    - `CustomerID`, `SiteID`: az adatbázisból keresd ki egy tetszőleges vevőhöz (`Customer`) tartozó dokumentum `_id` és `mainSiteId` értékét. Ezeket az értékeket drótozd bele a kódodba.
    - `Date`, `Deadline`, `Status`: ezeket az értékeket ay `order` paraméterből veheted
    - `PaymentMethod`: hozz létre egy új `PaymentMethod` objektumot. A `Method` legyen az `order` paraméterben található `PaymentMethod` érték. A `Deadline` maradjon `null`!
    - `OrderItems`: egyetlen tételt hozz létre! Ennek adattagjai:
        - `ProductID` és `Price`: a `product` paraméterből veheted
        - `Amount`: a függvényparaméter `amount` paraméterből jön
        - `Status`: az `order` paraméter `Status` mezőjével egyezik meg
    - minden más adattag (a számlázással kapcsolatos információk) maradjon `null` értéken!

## Törlés

A `DeleteOrder` törölje a megadott `ID`-hoz tartozó megrendelést.

## Módosítás

A módosító utasításban (`UpdateOrder`) arra figyelj oda, hogy csak azokat a mezőket írd felül, melyek a `Models.Order` osztályban megtalálhatóak: `Date`, `Deadline`, `Status` és `PaymentMethod`. Az `Total`-t itt nem kell figyelembe venni, ennek értéke nem fog változni.

!!! tip ""
    Több módosító kifejezést a `Builders<Entities.Order>.Update.Combine` segítségével lehet összevonni.

Itt is figyelj oda, hogy az update során az `IsUpsert` beállítás értéke legyen `false`!

A metódus visszatérési értéke akkor és csak akkor legyen `true`, ha létezik megrendelés a megadott `ID`-val — azaz volt illeszkedés a filterre.

## Kipróbálás

A megírt függvényeket a weboldalon a `Orders` menüpont alatt tudod kipróbálni. Teszteld le a `Filter`, `Add new order`, `Edit`, `Details` és `Delete` opciókat is!

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** a megrendelés listázó weboldalról. A képet a megoldásban `f3.png` néven add be. A képernyőképen látszódjon a **megrendelések listája**. Ellenőrizd, hogy a **Neptun kódod** (az oldal aljáról) látható-e a képen! A képernyőkép szükséges feltétele a pontszám megszerzésének.
