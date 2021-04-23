# Önálló feladatok

!!! example "BEADANDÓ"
    A feladatok megoldása során a közös feladatoknak megfelelő dokumentálást kérjük.

## Feladat 6 (1p)

Ismételd meg a `Product` kereséseket, de ezúttal ne a teljes sort, csak a `Price` és az elsődleges kulcs értékét kérd vissza a lekérdezésben. Hogyan változnak a végrehajtási tervek? Adj magyarázatot is a változásokra.

## Feladat 7 (1p)

Elemezd a következő két, `Product` táblából történő lekérdezés végrehajtási tervét:

- k) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke két érték között van (használd a `BETWEEN` operátort)
- l) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke két érték között van (itt is használd a `BETWEEN`-t), **vagy** megegyezik egy intervallumon kívüli értékkel

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 8 (1p)

A 6-os feladatban `WHERE Price=` feltétel egy egész és egy lebegőpontos szám egyenlőségét vizsgálja. Nézzük meg máshogy is a számkezelést: kérdezd le a `Product` táblából az alábbi feltéteknek megfelelő rekordokat.

- m) `where cast(Price as int) = egész szám`
- n) `where Price BETWEEN egész szám-0.0001 AND egész szám+0.0001`

Válassz egy tetszőleges egész számot a lekérdezésekhez, és így kérd le **csak az elsődleges kulcs értéket**. Elemezd a végrehajtási terveket.

## Feladat 9 (1p)

Elemezd a következő, `Product` táblából történő lekérdezések végrehajtási tervét:

- o) olyan teljes rekordok lekérdezése, ahol a `Price` kisebb, mint egy kis konstans érték (legyen az érték jól szűrő, azaz kevés rekordot adjon vissza), elsődleges kulcs szerint csökkenő sorrendbe rendezve
- p) mint az előző, de csak az `Id` és `Price` adatokat kérjük vissza
- q) olyan teljes rekordok lekérdezése, ahol a `Price` nagyobb, mint egy kis konstans érték (nem szűr jól, sok eredménye legyen), elsődleges kulcs szerint csökkenő sorrendbe rendezve
- r) mint az előző, de csak az `Id` és `Price` adatokat kérjük vissza

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 10 (1p)

Rakj indexet a `Name` oszlopra, majd elemezd a következő, `Product` táblából történő lekérdezések végrehajtási tervét:

- s) olyan név-azonosító párok lekérdezése, ahol a termék neve B-vel kezdődik, [`SUBSTRING`](https://docs.microsoft.com/en-us/sql/t-sql/functions/substring-transact-sql)-et használva
- t) mint az előző, de [`LIKE`](https://docs.microsoft.com/en-us/sql/t-sql/language-elements/like-transact-sql)-ot használva
- u) olyan név-azonosító párok lekérdezése, ahol a termék neve B-t tartalmaz (LIKE)
- v) egy konkrét termék azonosítójának kikeresése pontos név egyezés (=) alapján
- w) mint az előző, de kisbetű-nagybetű-érzéketlenül ([`UPPER`](https://docs.microsoft.com/en-us/sql/t-sql/functions/upper-transact-sql?view=sql-server-ver15) használatával)

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 11 (1p)

Elemezd a következő, `Product` táblából történő lekérdezések végrehajtási tervét:

- x) a maximális `Id` lekérdezése
- y) a minimális `Price` lekérdezése

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 12 (1p)

Kérd le termék kategóriánként (`CategoryId`) a kategóriához tartozó termékek (`Product`) számát.

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 13 (1p)

Hogyan javítható az előző feladat lekérdezéseinek teljesítménye? Add meg a megoldást, és utána elemezd újra az előző lekérdezés tervét.

!!! tip "Tipp"
    Fel kell venni egy új indexet. De vajon mire?

## Feladat 14 (1p)

Listázd a 2-es `CategoryId`-val rendelkező `Product` rekordokból a nevet (`Name`). 

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk! Térj ki arra, hogy az előző feladatban felvett index segített-e, és miért?

## Feladat 15 (1p)

Javíts az előző feladat lekérdezésének teljesítményén. Az előbb felvett indexet bővítsük a névvel: indexen jobbklikk -> _Properties_ -> a táblázatban az _Included Columns_ fül alatt vegyük fel a `Name` oszlopot.

Így ismételd meg az előző lekérdezést, és elemezd a tervet.
