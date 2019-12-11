# Feladatok

A feladatok megoldása során dokumentáld:

- a használt SQL utasítást (amennyiben ezt a feladat szövege kérte),
- a lekérdezési tervről készített képet,
- és a lekérdezési terv magyarázatát: _mit_ látunk és _miért_.

## Feladat 1 (2p)

Dobd el a _Telephely_ => _Vevo_ idegen kulcsot és a _Vevo_ elsődleges kulcs kényszerét. Legegyszerűbb, ha az _Object Explorer_-ben megkeresed ezeket, és törlöd:

![Kulcs törlése](../images/queryopt-delete-key.png)

Vizsgáld meg a következő lekérdezések végrehajtási tervét a _Vevo táblán_ – mindig teljes rekordot kérjünk vissza (`select *`):

- a) teljes tábla lekérdezése
- b) egy rekord lekérdezése elsődleges kulcs alapján
- c) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke nem egy konstans érték (használd a `<>` összehasonlító operátort)
- d) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke nagyobb, mint egy konstans érték
- e) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke nagyobb, mint egy konstans érték, ID szerint csökkenő sorrendbe rendezve

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 2 (2p)

Hozd létre újra az elsődleges kulcsot a _Vevo_ táblán.

> Index létrehozása: Job kattintás a táblán > Design > az ID oszlopon "Set Primary Key " és Mentés gomb, vagy az alábbi SQL utasítás lefuttatása
>
> `ALTER TABLE [dbo].[Vevo] ADD PRIMARY KEY CLUSTERED ([ID] ASC)`

Futtasd újra az előbbi lekérdezéseket. Mit tapasztalsz?

## Feladat 3 (2p)

Futtasd az alábbi lekérdezéseket a _Termek_ táblán megfogalmazva.

- f) teljes tábla lekérdezése
- g) egyenlőség alapú keresés a NettoAr oszlopra
- h) olyan rekordok lekérdezése, ahol a NettoAr értéke nem egy konstans érték (<>)
- i) olyan rekordok lekérdezése, ahol a NettoAr értéke nagyobb, mint egy konstans érték
- j) olyan rekordok lekérdezése, ahol a NettoAr értéke nagyobb, mint egy konstans érték, NettoAr szerint csökkenő sorrendbe rendezve

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

## Feladat 4 (2p)

Vegyél fel indexet a NettoAr oszlopra. Hogyan változnak az előbbi lekérdezések végrehajtási tervei?

Az index felvételéhez használd az _Object Explorer_-t, a fában a táblát kibontva az _Indexes_-en jobbklikk -> _New index_ > _Non-Clustered Index..._

![Index hozzáadása](../images/queryopt-add-index.png)

Adj az indexeknek értelmes, egységes konvcenció szerinti nevet, pl. `IX_Tablanev_MezoNev`, és add a _NettoAr_ oszlopot az _Index key columns_ listában.

![Index tulajdonságok](../images/queryopt-index-properties.png)

Ismételd meg az előbbi lekérdezéseket, és értelmezd a terveket!

## Feladat 5 (2p)

Szaporítsd meg a Termek tábla sorait az alábbi SQL szkripttel. Hogyan változnak az előbbi végrehajtási tervek?

Az i) típusú lekérdezést próbáld ki úgy is, hogy a választott konstans miatt kicsi legyen az eredményhalmaz, és úgy is, hogy lényegében a teljes tábla bennelegyen. Adj magyarázatot is a változásokra.

```sql
-- Generator Aux Table
SELECT TOP (1000000) n = ABS(CHECKSUM(NEWID()))
INTO dbo.Numbers
FROM sys.all_objects AS s1 CROSS JOIN sys.all_objects AS s2
OPTION (MAXDOP 1);

CREATE CLUSTERED INDEX n ON dbo.Numbers(n)
-- WITH (DATA_COMPRESSION = PAGE)
;


INSERT INTO Termek(Nev, NettoAr,Raktarkeszlet, AFAID, KategoriaID)
SELECT 'Alma', n%50000, n%100, 3, 13
FROM Numbers
```

## Feladat 6 (1p)

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

Folytasd az [opcionális feladatokkal](Feladat-imsc.md) vagy add be a megoldásod az [itt](README.md) leírtak szerint.
