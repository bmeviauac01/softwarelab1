# Önálló feladatok

A feladatok megoldása során dokumentáld a `README.md` markdown fájlba:

- a használt SQL utasítást (amennyiben ezt a feladat szövege kérte),
- a lekérdezési tervről készített képet (csak a tervet, _ne_ az egész képernyőt),
- és a lekérdezési terv magyarázatát: _mit_ látunk és _miért_.

## Feladat 6 (önálló, 1p)

Ismételd meg a termék kereséseket, de ezúttal ne a teljes sort, csak a nettoárat és az elsődleges kulcs értékét kérd vissza a lekérdezésben. Hogyan változnak a végrehajtási tervek? Adj magyarázatot is a változásokra.

## Feladat 7 (1p)

Elemezd a következő két, Termek táblából történő lekérdezés végrehajtási tervét:

- k) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke két érték között van (használd a `BETWEEN` operátort)
- l) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke két érték között van (itt is használd a `BETWEEN`-t) **vagy** megegyezik egy intervallumon kívüli értékkel

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 8 (1p)

A 6-os feladatban `WHERE NettoAr=` feltétel egy egész és egy lebegőpontos szám egyenlőségét vizsgálja. Nézzük meg máshogy is a számkezelést:

- m) `where cast(NettoAr as int) = egész szám`
- n) `where NettoAr BETWEEN egész szám-0.0001 AND egész szám+0.0001`

Válassz egy tetszőleges egész számot a lekérdezésekhez, és így kérd le **csak az elsődleges kulcs értéket**. Elemezd a végrehajtási terveket.

## Feladat 9 (1p)

Elemezd a következő termék táblából történő lekérdezések végrehajtási tervét:

- o) olyan rekordok lekérdezése, ahol a nettó ár kisebb, mint egy kis konstans érték (legyen az érték jól szűrő, azaz kevés rekordot adjon vissza), elsődleges kulcs szerint csökkenő sorrendbe rendezve
- p) Mint az előző, de csak a nettó ár, azonosító párokat kérjük vissza
- q) olyan rekordok lekérdezése, ahol a nettó ár nagyobb, mint egy kis konstans érték (nem szűr jól, sok eredménye legyen), elsődleges kulcs szerint csökkenő sorrendbe rendezve
- r) Mint az előző, de csak a nettó ár, azonosító párokat kérjük vissza

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 10 (1p)

Rakj indexet a Nev oszlopra, majd elemezd a következő Termek táblából történő lekérdezések végrehajtási tervét:

- s) olyan név-azonosító párok lekérdezése, ahol a termék neve Z-vel kezdődik, [`SUBSTRING`](https://docs.microsoft.com/en-us/sql/t-sql/functions/substring-transact-sql)-et használva
- t) Mint az előző, de [`LIKE`](https://docs.microsoft.com/en-us/sql/t-sql/language-elements/like-transact-sql)-ot használva
- u) olyan név-azonosító párok lekérdezése, ahol a termék neve Z-t tartalmaz (LIKE)
- v) egy konkrét termék azonosítójának kikeresése pontos név egyezés (=) alapján
- w) Mint az előző, de kisbetű-nagybetű-érzéketlenül ([`UPPER`](https://docs.microsoft.com/en-us/sql/t-sql/functions/upper-transact-sql?view=sql-server-ver15) használatával)

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 11 (1p)

Elemezd a következő Termek táblából történő lekérdezések végrehajtási tervét:

- x) a maximális azonosító lekérdezése
- y) a minimális nettó ár lekérdezése

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 12 (1p)

Kérd le termék kategóriánként a kategóriában levé a termékek számát.

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 13 (1p)

Hogyan javítható az előző feladat lekérdezéseinek teljesítménye? Add meg a megoldást, és utána elemezd újra az előző lekérdezés tervét.

> Tipp: fel kell venni egy új indexet. De vajon hova, mire?

## Feladat 14 (1p)

Listázd egy adott kategóriába tartozó terméket nevét. Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk! Térj ki arra, hogy az előző feladatban felvett index segített-e, és miért?

## Feladat 15 (1p)

Javíts az előző feladat lekérdezésének teljesítményén. Az előbb felvett indexet bővítsük a névvel: indexen jobbklikk -> _Properties_ -> a táblázatban az _Included Columns_ fül alatt vegyük fel a _Nev_ oszlopot.

Így ismételd meg az előző lekérdezést, és elemezd a tervet.

## Következő feladat

Folytasd az [opcionális feladatokkal](Feladat-imsc.md) vagy add be a megoldásod az [itt](README.md#végezetül-a-megoldások-feltöltése) leírtak szerint.
