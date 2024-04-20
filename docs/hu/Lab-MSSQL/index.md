# MSSQL

A labor során a Microsoft SQL Server programozási lehetőségeit fogjuk gyakorolni komplexebb feladatokon keresztül.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows, Linux vagy MacOS: Minden szükséges program platform független, vagy van platformfüggetlen alternatívája.
- Microsoft SQL Server
    - Express változat ingyenesen használható, avagy Visual Studio mellett feltelepülő _localdb_ változat is megfelelő
    - Van [Linux változata](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup) is.
    - MacOS-en Docker-rel futtatható.
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms), vagy kipróbálható a platformfüggetlen [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download) is
- Az adatbázist létrehozó script: [mssql.sql](../db/mssql.sql)
- GitHub account és egy git kliens

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Microsoft SQL Server használata: [leírás](https://bmeviauac01.github.io/datadriven/hu/db/mssql/)
- Az adatbázis [sémájának leírása](https://bmeviauac01.github.io/datadriven/hu/db/)
- Microsoft SQL Server programozási lehetőségei és az SQL nyelv
    - Lásd az Adatvezérelt rendszerek c. tárgy jegyzetei és [gyakorlati anyagai](https://bmeviauac01.github.io/datadriven/hu/) között

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! warning "Jelszó a laborokban"
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adatbázis létrehozása

1. Kapcsolódj Microsoft SQL Serverhez SQL Server Management Studio Segítségével. Indítsd el az alkalmazást, és az alábbi adatokkal kapcsolódj.

    - Server name: `(localdb)\mssqllocaldb` vagy `.\sqlexpress` (ezzel egyenértékű: `localhost\sqlexpress`)
    - Authentication: `Windows authentication`

1. Hozz létre egy új adatbázist (ha még nem létezik). Az adatbázis neve legyen a **Neptun kódod**: _Object Explorer_-ben Databases-en jobb kattintás, és _Create Database_.

    !!! warning "FONTOS"
        Az adatbázis neve egyezzen meg a **Neptun kódoddal**. A labor megoldásában kérünk képernyőképeket, amelyeken így kell szerepelnie az adatbázisnak!

1. Hozd létre a minta adatbázist a [generáló script](../db/mssql.sql) lefuttatásával. Nyiss egy új _Query_ ablakot, másold be a script tartalmát, és futtasd le. Ügyelj az eszköztáron levő legördülő menüben a megfelelő adatbázis kiválasztására.

    ![Adatbázis kiválasztása](../images/sql-management-database-dropdown.png)

1. Ellenőrizd, hogy létrejöttek-e a táblák. Ha a _Tables_ mappa ki volt már nyitva, akkor frissíteni kell.

    ![Adatbázis kiválasztása](../images/sql-managment-tablak.png).

## 1. Feladat: Kategória nézet és adatbeszúrás (8 pont)

### Nézet létrehozása

Hozz létre egy nézetet `CategoryWithParent` néven a `Category` tábla kényelmesebb használatához. A nézetben két oszlop legyen: a kategória megnevezése (`Name`), és a szülőkategória (`ParentCategoryId`) megnevezése - amennyiben létezik, vagy null.

Nyiss egy új _Query_ ablakot. Ügyelj rá, hogy a jó adatbázis legyen kiválasztva. Hozd létre a nézetet az alábbi utasítás lefuttatásával.

```sql
CREATE VIEW CategoryWithParent
AS
SELECT c.Name CategoryName, p.Name ParentCategoryName
FROM Category c
LEFT OUTER JOIN Category p ON c.ParentCategoryId = p.ID
```

Próbáld ki a nézetet: kérdezd le a nézet tartalmát!

![Nézet tartalmának listázása](../images/sql-management-query-view.png)

### Beszúrás a nézeten keresztül

Készíts triggert `InsertCategoryWithParent` néven, ami lehetővé teszi új kategória beszúrását az előbb létrehozott nézeten keresztül (tehát a kategória nevét és opcionálisan a szülőkategória nevét megadva). A szülő kategória megadása nem kötelező, de amennyiben meg van adva a neve és ilyen névvel nem létezik rekord, dobj hibát, és ne vedd fel az adatot a táblába.

A megoldásban egy `instead of` típusú triggerre lesz szükségünk, mert ez ad lehetőséget a nézeten keresztül történő adatbeszúrásra. A trigger vázát láthatod alább.

```sql
CREATE TRIGGER InsertCategoryWithParent -- name of the trigger
ON CategoryWithParent -- name of the view
INSTEAD of INSERT    -- trigger code executed insted of insert
AS
BEGIN
  DECLARE @newname NVARCHAR(255) -- variables used below
  DECLARE @parentname NVARCHAR(255)

  -- using a cursor to navigate the inserted table
  DECLARE ic CURSOR for SELECT * FROM inserted
  OPEN ic
  -- standard way of managing a cursor
  FETCH NEXT FROM ic INTO @newname, @parentname
  WHILE @@FETCH_STATUS = 0
  BEGIN
    -- check the received values available in the variables
    -- find the id of the parent, if specified
    -- throw error if anything is not right
    -- or insert the record into the Category table
    FETCH NEXT FROM ic INTO @newname, @parentname
  END

  CLOSE ic -- finish cursor usage
  DEALLOCATE ic
END
```

1. Egészítsd ki a trigger vázat a ciklusban.

    - Ha érkezik szülő kategória név, ellenőrizd, hogy létezik-e kategória olyan névvel, mint ami a `@parentname` változóban van.

    - Ha nincs ilyen, akkor dobj hibát, amivel leáll a trigger futása.

    - Ha minden rendben van, akkor szúrd be az új adatokat a `Category` táblába (ne a nézetbe, hiszen a nézet nem írható, pont ezért készül a trigger).

    !!! example "BEADANDÓ"
        A trigger kódját az `f1-trigger.sql` fájlba írd. A fájlban csak ez az egyetlen `create trigger` utasítás legyen! Semmiképpen se legyen `[use]`, se `go` utasítás a fájlban! A helyes megoldás 4 pontot ér.

1. Próbáld ki, jól működik-e a trigger! Írj egy olyan beszúró utasítást, amely sikeresen felvesz egy új elemet, és egy olyan teszt utasítást, amely során nem sikerül a beszúrás.

    A helyes és helytelen viselkedéshez feltételezheted, hogy az adatbázis a kiinduló állapotban van: olyan kategória rekordok léteznek csupán, amelyeket a létrehozó script beszúrt. A két teszt _ne_ épüljön egymásra! Mindkettő az elvárt eredményt adja attól függetlenül, hogy a másik lefutott-e már!

    !!! warning "Ékezet kerülendő"
        Érdemes olyan teszt adatokat választani, amiben nincs ékezet! Különben problémákat okozhat az sql fájl kódolásának helytelen beállítása. Hogy ezt elkerüljük, használható például a _LEGO_, mint létező szülő kategória név.

    !!! example "BEADANDÓ"
        A teszt utasításokat az `f1-test-ok.sql` és `f1-test-error.sql` fájlokba írd. Mindkét fájlban csak egyetlen `insert` utasítás legyen! Semmiképpen ne legyen `[use]`, se `go` utasítás bennük! Mindkét utasítás 2-2 pontot ér.

## 2. Feladat: Számla ellenőrzése (6 pont)

### Tárolt eljárás

Írj tárolt eljárást `CheckInvoice` néven, aminek bemeneti paramétere egy `int` típusú és `@invoiceid` nevű számlaazonosító.

- Az eljárás ellenőrizze, hogy a paraméterben kapott számlához (`Invoice`) kapcsolódó számlatételeken (`InvoiceItem`) szereplő mennyiség (`Amount`) egyezik-e a kapcsolódó megrendeléstétel (`OrderItem`) mennyiségével. (Az `InvoiceItem` rekord hivatkozik a kapcsolódó `OrderItem`-re.)
- Amennyiben eltérés található a kettőben, úgy mindkettő értékét, valamint a termék nevét írd ki a standard outputra az alábbi séma szerint: `Error: Ball (invoice 5 order 6)`
- Csak akkor írjon bármit a kimenetre a tárolt eljárás, ha problémát talált. Semmiképpen se hagyj teszteléshez használt kiírást az eljárásban!
- Legyen az eljárás `int` típusú visszatérési értéke 0, ha nem kellett semmit kiírni a kimenetre, és 1, ha kellett. Ez az érték `return` kulcsszóval kerüljön visszaadásra (ne `output` paraméter legyen).

A kiíráshoz használd a `print` parancsot: `PRINT 'Szoveg' + @valtozo + 'Szoveg'` Ügyelj rá, hogy a változónak char típusúnak kell lennie, egyéb típus, pl. szám konvertálása: `convert(varchar(5), @valtozo)`, pl. `PRINT 'Szoveg' + convert(varchar(5), @valtozo)`

!!! example "BEADANDÓ"
    Az eljárás kódját az `f2-procedure.sql` fájlba írd. A fájlban csak ez az egyetlen `create proc` utasítás legyen! A feladat helyes megoldása 4 pontot ér. Helyesség függvényében részpontszám is kapható.

### Minden számla ellenőrzése

Írj T-SQL kódblokkot, ami az előző feladatban megírt eljárást hívja meg egyenként az összes számlára. Érdemes ehhez egy kurzort használnod, ami a számlákon fut végig.

A kód minden számla ellenőrzése előtt írja ki a standard outputra a számla azonosítóját (pl. `InvoiceID: 12`), és amennyiben nem volt eltérés, írja ki a kimenetre, hogy 'Invoice ok'. Ezt a kimenetet a query ablak alatti _Messages_ panelen fogod látni.

!!! tip "Tárolt eljárás meghívása"
    Tárolt eljárás meghívása kódból az `exec` paranccsal lehetséges, pl.

    ```sql
    DECLARE @checkresult INT
    EXEC @checkresult = CheckInvoice 123
    ```

Ellenőrizd a kódblokk viselkedését. Ahhoz, hogy eltérést tapasztalj, ha szükséges, változtass meg egy megrendelés tételt vagy számla tételt az adatbázisban. (Az ellenőrzéshez tartozó kódot nem kell beadni.)

!!! example "BEADANDÓ"
    Az összes számlát ellenőrző kódot az `f2-check-all.sql` fájlba írd. A fájlban csak a lefuttatható kódblokk szerepeljen. Ne legyen benne a tárolt eljárás kódja, ne legyen benne se `[use]` se `go` utasítás. A feladatrésszel 2 pont szerezhető.

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a kód futásának eredményéről, amikor valamilyen **eltérést megtalált**. A képet `f2.png` néven add be. A képernyőképen látszódjon az _Object Explorer_-ben az **adatbázis neve (a Neptun kódod)** és **a kiírt üzenet** is!

## 3. Feladat 3: Számla tábla denormalizálása (6 pont)

### Új oszlop

Módosítsd az `Invoice` táblát: vegyél fel egy új oszlopot `ItemCount` néven a számlához tartozó összes tétel (`InvoiceItem`) darabszámának tárolásához.

!!! example "BEADANDÓ"
    Az oszlopot hozzáadó kódot az `f3-column.sql` fájlba írd bele. A fájlban csak egyetlen `alter table` utasítás szerepeljen, ne legyen benne se `[use]` se `go` utasítás. A feladatrésszel 1 pont szerezhető. Ez a feladat előkövetelménye a következőnek.

Készíts T-SQL programblokkot, amely kitölti az újonnan felvett oszlopot az aktuális adatok alapján.

!!! tip ""
    Ha például egy **számlán** (`Invoice`) szerepel 2 darab piros pöttyös labda és 1 darab tollasütő, akkor 3 tétel szerepel a számlán. Ügyelj rá, hogy számlához (és nem a megrendeléshez) tartozó tételeket kell nézni!

!!! example "BEADANDÓ"
    Az oszlopot kitöltő kódot az `f3-fill.sql` fájlba írd. A fájlban csak egy T-SQL kódblokk legyen, ne használj tárolt eljárást vagy triggert, és ne legyen a kódban se `[use]` se `go` utasítás. A feladatrésszel 1 pont szerezhető.

### Oszlop karbantartása triggerrel

Készíts triggert `InvoiceItemCountMaintenance` néven, amely a számla tartalmának változásával együtt karbantartja az előzőleg felvett tételszám mezőt. Ügyelj rá, hogy hatékony legyen a trigger! A teljes újraszámolás nem elfogadható megoldás. Továbbá ügyelj arra is, hogy egyszerre több tétel is változhat, a triggernek ilyen esetben is jól kell működnie.

!!! tip "Tipp"
    A triggert a `InvoiceItem` táblára kell készíteni, bár a frissítendő érték az `Invoice` táblában van.

!!! warning "Fontos"
    Ne felejtsd, hogy a trigger **utasítás szintű**, azaz nem soronként fut le, így pontenciálisan több változás is lehet az implicit táblákban! Az `inserted` és `deleted` **tábla**, és mindenképpen ennek megfelelően kell őket kezelni.

!!! example "BEADANDÓ"
    A kódot az `f3-trigger.sql` fájlba írd. A fájlban csak egyetlen `create trigger` utasítás legyen, és ne legyen a kódban se `[use]` se `go` utasítás. A feladatrész 5 pontot ér. Helyesség függvényében részpontszám is kapható.

Próbáld ki, jól működik-e a trigger. A teszteléshez használt utasításokat nem kell beadnod a megoldásban, de fontos, hogy ellenőrizd a viselkedést. Ellenőrizd olyan utasítással is a trigger működését, amely egyszerre több rekordot érint (pl. `where` feltétel nélküli `update` a tábla teljes tartalmára)!

!!! example "BEADANDÓ"
    Készíts egy képernyőképet, amelyen látható a kitöltött `ItemCount` oszlop (az `Invoice` tábla tartalmával együtt). A képet a megoldásban `f3.png` néven add be. A képernyőképen látszódjon az _Object Explorer_-ben az **adatbázis neve (a Neptun kódod)** és az **`Invoice` tábla tartalma** is! A képernyőkép szükséges feltétele a részpontszám megszerzésének.

## 4. Feladat 4: Opcionális feladat (3 iMSc pont)

Kérdezd le a kategóriákat (`Category` tábla) úgy, hogy az alábbi eredményt kapjuk:

| Name           | Count | Rank |
| -------------- | ----- | ---- |
| Building items |     3 |    1 |
| Months 0-6     |     2 |    2 |
| DUPLO          |     1 |    3 |
| LEGO           |     1 |    4 |
| Months 18-24   |     1 |    5 |
| Months 6-18    |     1 |    6 |
| Play house     |     1 |    7 |

Az első oszlop a kategória neve, a második a kategóriában található termékek száma, a harmadik oszlop pedig a sorrend, ahol is a kategóriák a termék darabszámok szerint csökkenő sorrendben kell megjelenjenek, és ha egyezik a termék darabszám, akkor a kategória név alapján növekvő sorrendben. A sorrendezés folyamatos kell legyen, és a végeredmény a számított sorrend szerint növekvően kell érkezzen. A lekérdezést egyetlen utasítással kell megoldani. A lekérdezés eredményében az oszlop nevek egyezzenek meg a fenti példában látható nevekkel.

!!! tip "Tipp"
    A harmadik oszlop neve nem véletlen "rank".

!!! example "BEADANDÓ"
    A lekérdezést az `f4.sql` fájlba írd. A fájlban egyetlen `select` utasítás szerepeljen, és semmiképpen ne legyen benne se `[use]` se `go` utasítás.

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a lekérdezés eredményéről. A képet a megoldásban `f4.png` néven add be. A képernyőképen látszódjon az _Object Explorer_-ben az **adatbázis neve (a Neptun kódod)** és a **lekérdezés eredménye** is! A képernyőkép szükséges feltétele a pontok megszerzésének.
