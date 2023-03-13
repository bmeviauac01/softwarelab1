# Lekérdezés optimalizálás

A labor során a lekérdezés optimalizálást vizsgáljuk Microsoft SQL Server platformon. Azért, hogy a feladatok során megfelelően megértsük a működést, és dokumentálni is tudjuk, az **első 5 feladat** során megadjuk a megoldást és a magyarázatot is. A **továbbiak önálló feladatok**, ahol a magyarázat kitalálása a feladat része. A közös feladatmegoldás és az önálló feladatmegoldás eredményét is dokumentálni kell és be kell adni.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows, Linux vagy MacOS: Minden szükséges program platform független, vagy van platformfüggetlen alternatívája.
- Microsoft SQL Server
    - Express változat ingyenesen használható, avagy Visual Studio mellett feltelepülő _localdb_ változat is megfelelő
    - Van [Linux változata](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup) is.
    - MacOS-en Docker-rel futtatható.
- [Visual Studio Code](https://code.visualstudio.com/) vagy más, markdown kompatibilis szerkesztő
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms), vagy kipróbálható a platformfüggetlen [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download) is
- Az adatbázist létrehozó script: [mssql.sql](../db/mssql.sql)
- GitHub account és egy git kliens

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Markdown formátum [rövid ismertetője](https://guides.github.com/features/mastering-markdown/) és [részletes dokumentációja](https://help.github.com/en/github/writing-on-github/basic-writing-and-formatting-syntax)
- Microsoft SQL Server használata: [leírás](https://bmeviauac01.github.io/datadriven/hu/db/mssql/)
- Az adatbázis [sémájának leírása](https://bmeviauac01.github.io/datadriven/hu/db/)
- [Microsoft SQL Server lekérdezés optimalizálása](../images/queryopt/07-Lekerdezes_optimalizalas.pdf)

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! warning "Jelszó a laborokban"
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Markdown fájl megnyitása

A feladatok megoldása során a dokumentációt markdown formátumban készítsd. Az előbb letöltött git repository-t nyisd meg egy markdown kompatibilis szerkesztővel. Javasolt a Visual Studio Code használata:

1. Indítsd el a VS Code-ot.

1. A _File > Open Folder..._ menüvel nyisd meg a git repository könyvtárát.

1. A bal oldali fában keresd meg a `README.md` fájlt és dupla kattintással nyisd meg.

   - Ezt a fájlt szerkeszd.
   - Ha képet készítesz, azt is tedd a repository alá a többi fájl mellé. Így relatív elérési útvonallal (fájlnév) fogod tudni hivatkozni.

    !!! warning "Fájlnév: csupa kisbetű ékezet nélkül"
        A képek fájlnevében ne használj ékezetes karaktereket, szóközöket, se kis- és nagybetűket keverve. A különböző platformok és a git eltérően kezelik a fájlneveket. A GitHub webes felületén akkor fog minden rendben megjelenni, ha csak az angol ábécé kisbetűit használod a fájlnevekben.

1. A kényelmes szerkesztéshez nyisd meg az [előnézet funkciót](https://code.visualstudio.com/docs/languages/markdown#_markdown-preview) (_Ctrl-K + V_).

!!! note "Más szerkesztőeszköz"
    Ha nem szimpatikus ez a szerkesztő, használhatod a [GitHub webes felületét is](https://help.github.com/en/github/managing-files-in-a-repository/editing-files-in-your-repository) a dokumentáció szerkesztéséhez, itt is van előnézet. Ekkor a [fájlok feltöltése](https://help.github.com/en/github/managing-files-in-a-repository/adding-a-file-to-a-repository) kicsit körülményesebb lesz.

### Adatbázis létrehozása

1. Kapcsolódj Microsoft SQL Serverhez SQL Server Management Studio Segítségével. Indítsd el az alkalmazást, és az alábbi adatokkal kapcsolódj.

    - Server name: `(localdb)\mssqllocaldb` vagy `.\sqlexpress` (ezzel egyenértékű: `localhost\sqlexpress`)
    - Authentication: `Windows authentication`

1. Hozz létre egy új adatbázist (ha még nem létezik). Az adatbázis neve legyen a Neptun kódod: _Object Explorer_-ben Databases-en jobb kattintás, és _Create Database_.

1. Hozd létre a minta adatbázist a [generáló script](../db/mssql.sql) lefuttatásával. Nyiss egy új _Query_ ablakot, másold be a script tartalmát, és futtasd le. Ügyelj az eszköztáron levő legördülő menüben a megfelelő adatbázis kiválasztására.

    ![Adatbázis kiválasztása](../images/sql-management-database-dropdown.png)

1. Ellenőrizd, hogy létrejöttek-e a táblák. Ha a _Tables_ mappa ki volt már nyitva, akkor frissíteni kell.

    ![Adatbázis kiválasztása](../images/sql-managment-tablak.png).

### Lekérdezési terv bekapcsolása

!!! tip "Nem Windows platformon"
    A lekérdezési tervhez alapvetően Microsoft SQL Server Management Studio-t használunk. Ha nem Windows platformon dolgozol, kipróbálhatod az Azure Data Studio-t, ebben is [lekérdezhető a végrehajtási terv](https://richbenner.com/2019/02/azure-data-studio-execution-plans/).

Az összes feladat során szükségünk lesz a legjobb lekérdezési tervre, amit a szerver végeredményben választott. Ezt SQL Server Management Studio-ban a _Query_ menüben az [_Include Actual Execution Plan_ opcióval](https://docs.microsoft.com/en-us/sql/relational-databases/performance/display-an-actual-execution-plan) kapcsolhatjuk be.

![Lekérdezési terv bekapcsolása](../images/queryopt/queryopt-include-plan.png)

A tervet a lekérdezés lefuttatása után az ablak alján, az eredmények nézet helyett választható _Execution plan_ lapon találjuk.

![Lekérdezési terv bekapcsolása](../images/queryopt/queryopt-plan-result.png)

A lekérdezési terv diagramot adatfolyamként kell olvasnunk, az adat folyásának iránya a lekérdezés végrehajtása. Az egyes elemek a lekérdezési terv műveletei, a százalékos érték pedig az egész lekérdezéshez viszonyított relatív költség.

## Közös feladatok

!!! example "BEADANDÓ"
    A feladatok megoldása során dokumentáld a `README.md` markdown fájlba:

    - a használt SQL utasítást (amennyiben ezt a feladat szövege kérte),
    - a lekérdezési tervről készített képet (csak a tervet, _ne_ az egész képernyőt),
    - és a lekérdezési terv magyarázatát: _mit_ látunk és _miért_.

    A dokumentációnak a képekkel együtt helyesen kell megjelenniük a GitHub webes felületén is! Ezt ellenőrizd a beadás során: nyisd meg a repository-d webes felületét, váltsd át a megfelelő ágra, és a GitHub automatikusan renderelni fogja a `README.md` fájlt a képekkel együtt.

!!! warning "Önálló munka"
    Annak ellenére, hogy a megoldások megtalálhatóak alább, az SQL utasítások kiadása, a lekérdezési terv képernyőkép elkészítése és a saját magyarázattal együtt való dokumentálás szükséges része a feladatnak. A lentebb található magyarázatok bemásolása nem elfogadható megoldás!

Amennyiben egyes (rész)feladatok lekérdezési terve és/vagy a magyarázat azonos, vagy legalábbis nagyon hasonló, elég egyszer elkészíteni a lekérdezési tervről a képet, és a magyarázatot is elég egyszer megadni, csak jelezd, hogy ez mely feladatokra vonatkozik.

### 1. Feladat (2p)

Dobd el a `CustomerSite` => `Customer` idegen kulcsot és a `Customer` elsődleges kulcs kényszerét. Legegyszerűbb, ha az _Object Explorer_-ben megkeresed ezeket, és törlöd (a _PK..._ kezdetűek az elsődleges kulcsok, az _FK..._  kezdetűek a külső kulcsok - két külön táblában kell keresd a törlendőket!):

![Kulcs törlése](../images/queryopt/queryopt-delete-key.png)

Vizsgáld meg a következő lekérdezések végrehajtási tervét a `Customer` táblán – mindig teljes rekordot kérjünk vissza (`SELECT *`):

- a) teljes tábla lekérdezése
- b) egy rekord lekérdezése elsődleges kulcs alapján
- c) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke nem egy konstans érték (használd a `<>` összehasonlító operátort)
- d) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke nagyobb, mint egy konstans érték
- e) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke nagyobb, mint egy konstans érték, ID szerint csökkenő sorrendbe rendezve

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

??? success "Megoldás"
    A kiadott **parancsok**:

    - a) `SELECT * FROM customer`
    - b) `SELECT * FROM customer WHERE id = 1`
    - c) `SELECT * FROM customer WHERE id <> 1`
    - d) `SELECT * FROM customer WHERE id > 1`
    - e) `SELECT * FROM customer WHERE id > 1 ORDER BY id DESC`

    **a)-d)**

    A **lekérdezési terv** mindegyikre nagyon hasonló, mindegyik _table scan_-t használt:

    ![](../images/queryopt/f1-1.png)

    **Magyarázat**: az optimalizáló nem tud indexet használni, így minden lekérdezés _table scan_ lesz.

    **e)**

    Egyedül ez különbözik, az order by miatt még egy sort is lesz benne.

    ![](../images/queryopt/f1-2.png)

    **Magyarázat**: A _table scan_ marad, és még rendezni is kell, amihez nincs index segítség, tehát külön lépés lesz.

### 2. Feladat (2p)

Hozd létre újra az elsődleges kulcsot a `Customer` táblán:

- Job kattintás a táblán > Design > az ID oszlopon "Set Primary Key " és Mentés gomb,
- vagy az `ALTER TABLE [dbo].[Customer] ADD PRIMARY KEY CLUSTERED ([ID] ASC)` SQL utasítás lefuttatása.

Futtasd újra az előbbi lekérdezéseket. Mit tapasztalsz?

??? success "Megoldás"
    A kiadott parancsok megegyeznek az 1.-es feladattal.

    **a)**

    ![](../images/queryopt/f2-1.png)

    Clustered Index Scan végigmegy a clustered index mentén. Az elsődleges kulcs hatására létrejött egy Clustered Index, azaz innentől a tábla rekordjai ID szerinti sorrendben vannak tárolva. Ha végigmegyünk ezen a struktúrán meglesz az összes sor. Ha van clustered index, már nem fogunk Table Scan-nel találkozni, legrosszabb esetben teljes Clustered Index Scan lesz. Attól, hogy nem Table Scan a neve, a teljes Clustered Index Scan is valójában egy table scan, a teljes tábla adattartalmát felolvassuk. Általános esetben ez probléma, de itt épp ezt kértük a lekérdezésben.

    **b)**
    
    ![](../images/queryopt/f2-2.png)

    Ez egy Clustered Index Seek lesz. Mivel a rendezési kulcsra fogalmazunk meg egyezési feltételt, a rendezett tárolású rekordok közül nagyon gyorsan eljuthatunk a keresetthez. Ez egy jó terv, a Clustered Index egyik alapfeladata a rekord gyors megtalálása, erre van optimalizálva.

    **c)**

    Az előzőhöz nagyon hasonló a terv: ez is Clustered Index Seek lesz két intervallummal (`< konstans`, `> konstans`). Az optimalizáló két intervallumra bontja a <>-t. Mivel a feltétel a rendezési kulcsra vonatkozik, megint ki tudja használni a Clustered Indexet. Ez is egy jó terv, a rendezés miatt csak a szükséges rekordokat fogjuk felolvasni.

    **d)**

    Ez is Clustered Index Seek alapú range scan lesz egy intervallummal. Az előbbihez nagyon hasonló.

    **e)**
    
    ![](../images/queryopt/f2-3.png)

    Ismét Clustered Index Seek backward seek order-rel. Megnézhetjük a _Properties_ ablakban a range-et és a Seek Order-t: kikeressük a határon lévő rekordot és onnan visszafelé indulunk el, így eleve rendezve lesz az eredményhalmaz. Ez egy jó terv, a rendezés miatt csak a szükséges rekordokat fogjuk felolvasni és pont a megfelelő sorrendben.

### 3. Feladat (2p)

Futtasd az alábbi lekérdezéseket a `Product` táblán megfogalmazva.

- f) teljes tábla lekérdezése
- g) egyenlőség alapú keresés a `Price` oszlopra
- h) olyan rekordok lekérdezése, ahol a `Price` értéke nem egy konstans érték (<>)
- i) olyan rekordok lekérdezése, ahol a `Price` értéke nagyobb, mint egy konstans érték
- j) olyan rekordok lekérdezése, ahol a `Price` értéke nagyobb, mint egy konstans érték, `Price` szerint csökkenő sorrendbe rendezve

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

??? success "Megoldás"
    A kiadott **parancsok**:

    - f) `SELECT * FROM product`
    - g) `SELECT * FROM product WHERE price = 800`
    - h) `SELECT * FROM product WHERE price <> 800`
    - i) `SELECT * FROM product WHERE price > 800`
    - j) `SELECT * FROM product WHERE price > 800 ORDER BY price DESC`

    **f)-i)**

    ![](../images/queryopt/f3-1.png)

    Clustered Index Scan végigmegy a clustered index mentén. Ez továbbra is a teljes tábla felolvasása, hiszen a szűrésre nincs index. Ha van szűrési feltétel, minden soron végig kell menni és ki kell értékelni a feltételt. Mivel van Clusterd Index, így amentén haladunk, de nem sokra megyünk ezzel, lényegében egy Table Scan. Ezek nem hatékony lekérdezések. Mivel nem a Clustered Index rendezési kulcsra fogalmaztuk meg a feltételt, így az index nem sokat segít nekünk (ezért lesz Scan és nem Seek).

    **j)**
    
    ![](../images/queryopt/f3-2.png)
    
    Ez is Clustered Index Scan lesz, de ami az érdekes, hogy a rendezés költsége jelentős (nagyobb mint a kikeresésé). Miután az elég költséges Index Scan-t megcsináltuk, még rendeznünk is kell, hiszen a kiolvasott rekordok ID szerint sorrendezettek nem `Price` szerint. Ez a lekérdezés nagyon költséges, az amúgy is drága scan után még rendezni is kell. A jó index nem csak a keresést segíti - de most nincs megfelelő indexünk.

### 4. Feladat (2p)

Vegyél fel indexet a `Price` oszlopra. Hogyan változnak az előbbi lekérdezések végrehajtási tervei?

Az index felvételéhez használd az _Object Explorer_-t, a fában a táblát kibontva az _Indexes_-en jobbklikk -> _New index_ > _Non-Clustered Index..._

![Index hozzáadása](../images/queryopt/queryopt-add-index.png)

Adj az indexeknek értelmes, egységes konvenció szerinti nevet, pl. `IX_Tablanev_MezoNev`. Add a _Price_ oszlopot az _Index key columns_ listához.

![Index tulajdonságok](../images/queryopt/queryopt-index-properties.png)

Ismételd meg az előbbi lekérdezéseket, és értelmezd a terveket!

??? success "Megoldás"
    A parancsok megegyeznek az előző feladatéval.

    **f)**

    Hiába az új index, ez még mindíg Index Scan lesz - hiszen a teljes tábla tartalmát kértük.

    **g)-i)**
    
    Clustered Index Scan lesz, lényegében megegyezik a szűréshez elvileg használható index nélküli esettel.
    
    Miért nem használja az új indexünket? A nem túl nyilvánvaló ok a projekcióban rejlik, azaz, hogy teljes rekordokat kérünk vissza. Az NonClustered Index-ből csak egy halom rekordreferenciát kapnánk, amik alapján még utána fel kellene olvasni a szükséges rekordokat. Az optimalizáló – főleg kis táblák esetén- dönthet úgy, hogy ennek összköltsége nagyobb lenne, mint egy index scan-nek.

    **j)**
    
    ![](../images/queryopt/f4-1.png)
    
    A NonClustered Index Seek-ből kikeresett megfelelő kulcsoknak megfelelő rekordokat kikeressük a Clustered Index-ből. Lényegében egy join a két index között.
    
    A többi lekérdezésnél is valami ilyet vártunk volna. A Clustered Index-re szükség van, mert teljes rekordokat kérünk vissza, a NonClustered Index csak referenciákat ad. A referenciák sorrendben vannak, így ha ezekhez rendre kigyűjtjük a teljes rekordokat a Clustered Index-ből akkor megspóroljuk az utólagos rendezést. Ha csak a Clustered Index-et használnánk (teljes Clustered Index Scan), akkor kellene utólagos rendezés. Ez egy elfogadható terv, mert a NonClustered Index segítségével megúsztuk a külön rendezést.

### 5. Feladat (2p)

Szaporítsd meg a _Product_ tábla sorait az alábbi SQL szkripttel. Hogyan változnak az előbbi végrehajtási tervek?

Az i) típusú lekérdezést próbáld ki úgy is, hogy a választott konstans miatt kicsi legyen az eredményhalmaz, és úgy is, hogy lényegében a teljes tábla benne legyen. Adj magyarázatot is a változásokra.

```sql
SELECT TOP (1000000) n = ABS(CHECKSUM(NEWID()))
INTO dbo.Numbers
FROM sys.all_objects AS s1 CROSS JOIN sys.all_objects AS s2
OPTION (MAXDOP 1);

CREATE CLUSTERED INDEX n ON dbo.Numbers(n);

INSERT INTO Product(Name, Price, Stock, VATID, CategoryID)
SELECT 'Apple', n%50000, n%100, 3, 13
FROM Numbers
```

??? success "Megoldás"
    A parancsok megegyeznek a korábbiakkal.

    **f)**
    
    Megegyezik az előző feladattal.

    **g)**
    
    ![](../images/queryopt/f5-1.png)

    A NonClustered Index Seek-ből kikeresett kulcsoknak megfelelő rekordokat kikeressük a Clustered Index-ből. Lényegében egy join a két index között.

    Vessük össze az előző, kis táblás változattal. Miért nem használja most a Clustered Index Scan-t? Nagy tábláknál megnő a _szelektivitás_ szerepe és jelentős a NonClustered Index használatából fakadó előny. A Clustered Index Scan nagy méretnél nagyon drága, ha van esély rá, hogy az NonClustered Index használatával csökkenthető a felolvasható sorok száma, akkor szinte biztosan érdemes használni. A `Price` statisztikái alapján tudható, hogy az = operátor jól szűr. Fontos! Az `=` használatából még nem következik a jó szűrés, ha szinte minden sorban ugyanaz az érték van, akkor pl. nem fog jól szűrni - ezért kell a statisztika is!
    
    Ez a terv elfogadható. Ha a feltételünk jól szűr, akkor tényleg ez lehet a jó irány.

    **h)**
    
    ![](../images/queryopt/f5-2.png)

    Clustered Index Scan végigmegy a clustered index mentén, megegyezik a korábban látottakkal.
    
    Miért nem használjuk az előző módszert? Ha az `=` ezen tábla esetében jól szűrt, akkor a `<>` nem fog. Ha ezt tudja a statisztikák alapján az optimalizáló, akkor összességében nem éri meg trükközni, úgyis fel kell olvasni a teljes táblát.

    **i)**

    Attól függ, hogy milyen konstanst választunk. Ha nagyon jól szűr (pl. nagyon nagy szám a konstans), akkor a g)-nél, ha nem akkor a h)-nál látott módszert követi.

    **j)**
    
    Mint a g) esetében. Számít itt az `order by desc`? Az optimalizáló megpróbálja elkerülni a rendezést, azt pedig csak ezzel a módszerrel tudja. Ez egy elfogadható terv. Az NonClustered Index segítségével itt is megúsztuk a rendezést.

## Önálló feladatok

!!! example "BEADANDÓ"
    A feladatok megoldása során a közös feladatoknak megfelelő dokumentálást kérjük.

    Ne felejtsd, hogy a dokumentációnak a képekkel együtt helyesen kell megjelenniük a GitHub webes felületén is! Ezt ellenőrizd a beadás során: nyisd meg a repository-d webes felületét, váltsd át a megfelelő ágra, és a GitHub automatikusan renderelni fogja a `README.md` fájlt a képekkel együtt.

### 6. Feladat (1p)

Ismételd meg a `Product` kereséseket, de ezúttal ne a teljes sort, csak a `Price` és az elsődleges kulcs értékét kérd vissza a lekérdezésben. Hogyan változnak a végrehajtási tervek? Adj magyarázatot is a változásokra.

### 7. Feladat (1p)

Elemezd a következő két, `Product` táblából történő lekérdezés végrehajtási tervét:

- k) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke két érték között van (használd a `BETWEEN` operátort)
- l) olyan rekordok lekérdezése, ahol az elsődleges kulcs értéke két érték között van (itt is használd a `BETWEEN`-t), **vagy** megegyezik egy intervallumon kívüli értékkel

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

### 8. Feladat (1p)

A 6-os feladatban `WHERE Price =` feltétel egy egész és egy lebegőpontos szám egyenlőségét vizsgálja. Nézzük meg máshogy is a számkezelést: kérdezd le a `Product` táblából az alábbi feltéteknek megfelelő rekordokat.

- m) `WHERE CAST(Price AS INT) = egész szám`
- n) `WHERE Price BETWEEN egész szám - 0.0001 AND egész szám + 0.0001`

Válassz egy tetszőleges egész számot a lekérdezésekhez, és így kérd le **csak az elsődleges kulcs értéket**. Elemezd a végrehajtási terveket.

### 9. Feladat (1p)

Elemezd a következő, `Product` táblából történő lekérdezések végrehajtási tervét:

- o) olyan teljes rekordok lekérdezése, ahol a `Price` kisebb, mint egy kis konstans érték (legyen az érték jól szűrő, azaz kevés rekordot adjon vissza), elsődleges kulcs szerint csökkenő sorrendbe rendezve
- p) mint az előző, de csak az `Id` és `Price` adatokat kérjük vissza
- q) olyan teljes rekordok lekérdezése, ahol a `Price` nagyobb, mint egy kis konstans érték (nem szűr jól, sok eredménye legyen), elsődleges kulcs szerint csökkenő sorrendbe rendezve
- r) mint az előző, de csak az `Id` és `Price` adatokat kérjük vissza

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

### 10. Feladat (1p)

Rakj indexet a `Name` oszlopra, majd elemezd a következő, `Product` táblából történő lekérdezések végrehajtási tervét:

- s) olyan név-azonosító párok lekérdezése, ahol a termék neve B-vel kezdődik, [`SUBSTRING`](https://docs.microsoft.com/en-us/sql/t-sql/functions/substring-transact-sql)-et használva
- t) mint az előző, de [`LIKE`](https://docs.microsoft.com/en-us/sql/t-sql/language-elements/like-transact-sql)-ot használva
- u) olyan név-azonosító párok lekérdezése, ahol a termék neve B-t tartalmaz (LIKE)
- v) egy konkrét termék azonosítójának kikeresése pontos név egyezés (=) alapján
- w) mint az előző, de kisbetű-nagybetű-érzéketlenül ([`UPPER`](https://docs.microsoft.com/en-us/sql/t-sql/functions/upper-transact-sql?view=sql-server-ver15) használatával)

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

### 11. Feladat (1p)

Elemezd a következő, `Product` táblából történő lekérdezések végrehajtási tervét:

- x) a maximális `Id` lekérdezése
- y) a minimális `Price` lekérdezése

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

### 12. Feladat (1p)

Kérd le termék kategóriánként (`CategoryId`) a kategóriához tartozó termékek (`Product`) számát.

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk!

### 13. Feladat (1p)

Hogyan javítható az előző feladat lekérdezéseinek teljesítménye? Add meg a megoldást, és utána elemezd újra az előző lekérdezés tervét.

!!! tip "Tipp"
    Fel kell venni egy új indexet. De vajon mire?

### 14. Feladat (1p)

Listázd a 2-es `CategoryId`-val rendelkező `Product` rekordokból a nevet (`Name`). 

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és adj magyarázatot rájuk! Térj ki arra, hogy az előző feladatban felvett index segített-e, és miért?

### 15. Feladat (1p)

Javíts az előző feladat lekérdezésének teljesítményén. Az előbb felvett indexet bővítsük a névvel: indexen jobbklikk -> _Properties_ -> a táblázatban az _Included Columns_ fül alatt vegyük fel a `Name` oszlopot.

Így ismételd meg az előző lekérdezést, és elemezd a tervet.

## Opcionális feladatok

### 16. Feladat (1 iMSc pont)

Elemezd a következő `Invoice`-`InvoiceItem` táblákból történő lekérdezés végrehajtási tervét: minden számlatétel névhez kérdezzük le a megrendelő nevét.

```sql
SELECT CustomerName, Name
FROM Invoice JOIN InvoiceItem ON Invoice.ID = InvoiceItem.InvoiceID
```

Mutasd meg, milyen _join_ stratégiát választott a rendszer. Adj magyarázatot, miért választhatta ezt!

### 17. Feladat (2 iMSc pont)

Hasonlítsd össze a különböző JOIN stratégiák költségét a következő lekérdezés esetében: összetartozó `Product`-`Category` rekordpárok lekérdezése.

!!! tip "Tipp"
    Használj [query hinteket](https://docs.microsoft.com/en-us/sql/t-sql/queries/hints-transact-sql-join) vagy az [option parancsot](https://docs.microsoft.com/en-us/sql/t-sql/queries/option-clause-transact-sql) a join stratégia kiválasztásához.

    Tedd a 3 lekérdezést (3 fajta join stratégia) egy végrehajtási egységbe (egyszerre futtasd őket). Így látni fogod az egymáshoz viszonyított költségüket.

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és magyarázd el a látottakat!
