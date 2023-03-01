# Reporting Services

!!! danger "A LABOR ÁTDOLGOZÁS ALATT ÁLL!"
    **A LABOR ÁTDOLGOZÁS ALATT ÁLL!**
    
    **A LABOR ÁTDOLGOZÁS ALATT ÁLL!**
    
    **A LABOR ÁTDOLGOZÁS ALATT ÁLL!**

A labor során egy új eszközzel, a _Microsoft SQL Server Reporting Services_-zel ismerkedünk meg, így a labor részben vezetett. Az első feladat laborvezetővel együtt megoldott, a továbbiak önálló feladatok. A közös feladatmegoldás és az önálló feladatmegoldás eredményét is be kell adni.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows
- Microsoft SQL Server: Express változat ingyenesen használható, avagy Visual Studio mellett feltelepülő _localdb_ változat is megfelelő
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Az adatbázist létrehozó script: [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip)
- Microsoft Visual Studio 2019 (2022 **nem** jó): Community verzió is megfelelő
- Report Server Projekt támogatás Visual Studio-hoz: [Microsoft Reporting Services Projects extension](https://marketplace.visualstudio.com/items?itemName=ProBITools.MicrosoftReportProjectsforVisualStudio) (Érdemes [frissen tartani](https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-update-a-visual-studio-extension?view=vs-2019) ezt az extension-t, mert gyakran van belőle kiadás.)
- GitHub account és egy git kliens

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Microsoft SQL Server használata: [leírás](https://bmeviauac01.github.io/adatvezerelt/db/mssql/) és [videó](https://web.microsoftstream.com/video/e3a83d16-b5c4-4fe9-b027-703347951621)
- SQL Reporting Services [hivatalos tutorial](https://docs.microsoft.com/en-us/sql/reporting-services/create-a-basic-table-report-ssrs-tutorial)

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! tip ""
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adventure Works 2014 adatbázis létrehozása

A feladatok során az _Adventure Works_ minta adatbázissal dolgozunk. Az adatbázis egy kereskedelmi cég értékesítéseit tartalmazza, amelyből mi a teljes adatbázis megértése helyett előre definiált lekérdezésekkel dolgozunk csak, melyek termékek eladásainak adatait tartalmazza.

1. Töltsd le és csomagold ki az [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip) fájlt a `c:\work\Adventure Works 2014 OLTP Script` könyvtárba (hozd létre a könyvtárat, ha nem létezik).

    !!! important ""
        Mindenképpen ez a mappa legyen, különben az sql fájlban az alábbi helyen ki kell javítani a könyvtár elérési útvonalát:

        ```sql
        -- NOTE: Change this path if you copied the script source to another path
        :setvar SqlSamplesSourceDataPath "C:\work\Adventure Works 2014 OLTP Script\"
        ```

        Ha beleszerkesztesz az elérési útvonalba, ügyelj hogy a végén maradjon perjel!

1. Kapcsolódj Microsoft SQL Serverhez SQL Server Management Studio segítségével. Az alábbi adatokkal kapcsolódj.

    - Server name: `(localdb)\mssqllocaldb` LocalDB esetén, vagy `localhost\sqlexpress` SQL Express használatakor
    - Authentication: `Windows authentication`

1. A _File / Open / File..._ menüpont használatával nyisd meg az előbbi mappából az `instawdb.sql` fájlt. **Még ne futtasd!** Előbb kapcsold be az SQLCMD módot: a _Query_ menüben _SQLCMD Mode_, és csak ezt követően válasszuk az _Execute_ lehetőséget.

    ![SQLCMD mód](../images/sql-management-sqlcmd-mode.png)

1. Ellenőrizd, hogy létrejött-e az adatbázis és a táblák. Ha a baloldali fában a _Databases_-en _Refresh_-t nyomsz, meg kell jelenjen az _AdventureWorks2014_ adatbázis a listában, és alatta számtalan tábla.

    ![AdventureWorks adatbázis táblák](../images/reportingservices/rs-adventureworks-tablak.png).

1. Nyiss egy új SQL Query ablakot ezen az adatbázison (az adatbázison jobb egérrel kattintva _New query_), és futtasd le az alábbi SQL utasítást **a saját Neptun kódodat** behelyettesítve:

    ```sql
    update Production.Product set Name='NEPTUN'+Name
    ```

    Ellenőrizd a `Production.Product` tábla tartalmát, hogy a Neptun kódod ott van-e a nevek elején: jobb egérrel kattintás a táblán és _Select top 1000 rows_.

    !!! warning "FONTOS"
        Fontos, hogy szerepeljen a Neptun kód a nevekben. A feladatok során képernyőképeket kérünk, amelyen szerepelnie **kell** a Neptun kódodnak.

## Feladat 1: Táblázatos riport

A feladat a laborvezetővel **közösen megoldott**.

Nyisd meg a checkoutolt git repository-ban a `reportserver.sln` fájlt. Ez egy üres _Report Server_ típusú projekt, amelyet Visual Studio-ban fejlesztünk.

A Report Server projekt elsősorban úgynevezett _Report Definition_ (.rdl) fájlokból áll, amelyek definiálják a riport előállításához szükséges adatforrásokat (lekérdezéseket), és a kinézet sablonját, amit adatokkal feltöltve kapjuk az eredmény riportot. A neve onnan ered, hogy ezeket a riportokat nem csak a fejlesztői gépen lehet lefuttatni, hanem egy un. _Report Server_-re publikálhatóak, ahonnan a vállalat megfelelő üzleti szereplői mindig friss riportokat kérhetnek, az aktuális adatok alapján.

!!! note ""
    Ezen a laboron nem tudjuk megmutatni a Report Server-t. Ennek csupán technikai okai vannak, a Report Server telepítés után kézi konfigurációt igényel, amit a laborokban megfelelő jogosultság nélkül nem tudunk megtenni. Ezért csak a Visual Studio-ban fogjuk látni a riportot.

### Hozzuk létre az első Report Definition fájlt.

1. _Solution Explorer_-ben jobb klikk a _Reports_-ra és _Add_ > _New Item_.

    ![Új report hozzáadása](../images/reportingservices/rs-add-new-report.png)

1. A sablonok közül válasszuk a _Report_ lehetőséget. Nevezzük el _Sales Orders.rdl_-nek, majd nyomjunk rá az Add-re.

    ??? fail "Ha nem sikerül létrehozni az új riport fájlt"
        Bizonyos Visual Studio és Report Server projekt verzió esetén az új riport fájl létrehozása nem sikerül. Ha hibaüzenetet kapsz, akkor kövesd az alábbi lépéseket.

        1. Tölts le [ezt az üres rdl fájlt](empty.rdl).
        1. Tedd a fájlt a repository-d alatt a `reportserver` mappába a megfelelő névvel (ez a mappa már létezik).
        1. Visual Studio-ban a _Reports_ mappára jobb kattintás és _Add_ > _Existing Item_, majd keresd ki az előbbi fájlt.

1. Nyisd meg a riport fájlt a Report Designer nézethez. Itt látható az új .rdl fájlt Design nézetben.

    ![Report Desinger](../images/reportingservices/rs-report-designer.png)

    A Report Designer a fejlesztőfelületünk. Két nézete van: _Design_ és _Preview_. Emellett a Report Data panel is megnyílik, itt lehet definiálni az adatforrásokat. Ha megvagyunk az adatforrások megadásával, a Design fülön tudjuk a riportot vizuálisan megtervezni, majd ha már kellőképpen előrehaladtunk a riport készítésével, a _Preview_ fülön tudjuk kipróbálni.

### Adatforrás (data source) beállítása

Az adatforrás definálja, a riport adatai honnan származnak. A mi esetünkben ez a korábban lérehozott SQL Server adatbázis lesz.

1. A _Report Data_ panelen _New_ > _Data Source_. A neve legyen "AdventureWorks2014".

    ![Add datasource](../images/reportingservices/rs-add-datasource.png)

1. A megjelenő űrlapon válasszuk a _Microsoft SQL Server_ típust és a _connection string_ mező melletti gombra kattintva adjuk meg ismét az adatbázis elérését

    - Server name: `(localdb)\mssqllocaldb`
    - Authentication: `Windows Authentication`
    - Select or enter database name: `AdventureWorks2014`

1. OK-ézzuk le a dialógusokat. Majd nyissuk meg **újra** a Data Source tulajdonságait (jobb egérrel és _Data Source Properties_), és ellenőrizzük a _Credentials_ fület, mert a Visual Studio néha "elfelejti" a beállítást. Az alábbi checkbox-nak kell kijelölve lennie:

    ![Data source credentials beállítás](../images/reportingservices/rs-data-source-properties.png)

### Adathalmaz (data set) megadása

Ahhoz, hogy riportokat készíthessük, az adatforráshoz adathalmazokat (dataset) is meg kell adnunk. Ez gyakorlatilag egy lekérdezést fog jelenteni az adatforrás felé.

1. A _Report Data_ panelen válasszuk a _New_ > _Dataset_ opciót. Nevezzük el a datasetet "AdventureWorksDataset"-nek. Data source-ot a legördülő menüből tudunk választani, használjuk az előzőleg elészítettet, és alkalmazzuk az alábbi beállításokat:

    ![Data set tulajdonságok](../images/reportingservices/rs-data-set-properties.png)

1. Írjuk be a következő Query-t.

    ```sql
    SELECT
    soh.OrderDate AS [Date],
    soh.SalesOrderNumber AS [Order],
    pps.Name AS Subcat, pp.Name as Product,
    SUM(sd.OrderQty) AS Qty,
    SUM(sd.LineTotal) AS LineTotal
    FROM Sales.SalesPerson sp
    INNER JOIN Sales.SalesOrderHeader AS soh
          ON sp.BusinessEntityID = soh.SalesPersonID
    INNER JOIN Sales.SalesOrderDetail AS sd
          ON sd.SalesOrderID = soh.SalesOrderID
    INNER JOIN Production.Product AS pp
          ON sd.ProductID = pp.ProductID
    INNER JOIN Production.ProductSubcategory AS pps
          ON pp.ProductSubcategoryID = pps.ProductSubcategoryID
    INNER JOIN Production.ProductCategory AS ppc
          ON ppc.ProductCategoryID = pps.ProductCategoryID
    GROUP BY ppc.Name, soh.OrderDate, soh.SalesOrderNumber,
             pps.Name, pp.Name, soh.SalesPersonID
    HAVING ppc.Name = 'Clothing'
    ```

    Ha megvagyunk, nyomjuk meg a _Refresh fields_ gombot.

    !!! note ""
        A query-t elvileg vizuálisan is összekattinthattuk volna (a Query Designer segítségével), de az a felület elég ódon, és lassan is töltődik be.

    A többi fülön most nincs dolgunk, kattintsunk az OK-ra.

### Táblázatos riport készítése (5p)

Most, hogy megvan a kapcsolatunk az adatbázis felé, és a lekérdezést is megírtuk, elkezdhetünk riportokat gyártani. A riport nem más, mint a lekérdezés eredménye megjelenítve táblázatokban, diagramokban.

1. Nyissuk meg a _Toolbox_ panelt, ha még nem látszana (_View_ menüben megtalálható).

1. Válasszuk ki a _Table_ eszközt, majd a Design fül középső részén elhelyezkedő üres, fehér téglalapra "rajzoljunk" egy táblázatot, mintha csak egy négyszöget rajzolnánk Paintben:

    ![Table hozzáadása](../images/reportingservices/rs-add-table.png)

1. Váltsunk vissza a _Report Data_ panelre, és nyissuk le az AdventureWorksDataset-et.

    ![Dataset mezők](../images/reportingservices/rs-dataset-fields.png)

    !!! info ""
        Ha ez üres vagy nem nyitható le, általában az a baj, hogy a dataset létrehozásakor nem nyomtuk meg a _Refresh Fields_ gombot. Ezt még nem késő megtenni: jobb kattintás a dataseten > _Dataset properties_, majd a megjelenő ablakban nyomjuk meg a _Refresh Fields_ gombot.

1. A _Date_ mezőt húzzuk rá az imént "rajzolt" táblázat első oszlopára. Ilyesmi eredményt kell kapjunk:

    ![Date oszlop hozzáadása](../images/reportingservices/rs-table-add-date-col.png)

    !!! note ""
        A második sorban látható `[Date]` jelöli a kiértékelendő kifejezést, míg az első sorban látható "Date" felirat lesz az oszlop fejléce a riportban – ez utóbbit át is írhatjuk.

1. Az előbbi módszerrel húzzuk be a második oszlopba az _Order_, a harmadikba a _Product_ mezőt. A _Qty_ mezőt is húzzuk be a jobb szélső oszlop szélére úgy, hogy felengedés előtt egy + jelet lássunk az egérkurzor alatt, és egy függőleges kék vonalat a táblázat szélén. Így egy új, negyedik oszlopba fog kerülni a mező. Ugyanígy eljárva húzzuk be a _LineTotal_ mezőt is ötödik oszlopnak.

    ![További oszlopok](../images/reportingservices/rs-table-add-order-product-qty-col.png)

1. Ezzel el is készült az első riportunk. Nézzük meg a Preview fülön. Elsőre kicsit lassan töltődik be, erre számítsunk. A továbbiakban már gyorsabb lesz! Ellenőrizd, hogy a Neptun kódod megjelenik-e! (Ha nem, kifelejtettél az előkészítő lépések közül egyet. Menj vissza, és pótold!)

    ![Riport előnézete](../images/reportingservices/rs-table-preview-1.png)

    Az elkészült riportot például kinyomtathatjuk, vagy exportálhatjuk több féle formátumba (Word, Excel, PowerPoint, PDF). Jelen állapotában azért van rajta még mit csiszolni, pl. a végösszeg mezőnél nincs jelölve a valuta, és az értéket is bőven elég lenne 2 tizedesjelre kerekítve megmutatni. A dátum formázása és az oszlopok szélessége sem az igazi.

1. Menjünk vissza a _Design_ fülre, és a táblázatunkban kattintsunk jobb egérgombbal a `[Date]` kifejezésen, majd válasszuk a _Text Box Properties_ opciót. Itt a _Number_ fülön válasszuk a _Date_ kategóriát, és válasszunk ki egy szimpatikus dátumformátumot.

    ![Date oszlop formázása](../images/reportingservices/rs-table-date-col-properties.png)

1. A `[LineTotal]` kifejezésen jobb klikkelve az előbbivel analóg módon a _Text Box Properties_-t kiválasztva formázzuk _Number_ alatt _Currency_-ként az összeget.

    ![Line total formázása](../images/reportingservices/rs-table-linetotal-col-properties.png)

1. A táblázat fejléc sor fölötti szürke téglalapok szélei fölé mozgatva az egeret a szokásos átméretező kurzor ikonokkal találkozhatunk. (Kb. mintha Excelben vagy Wordben próbálnánk táblázatot méretezni.) Ennek segítségével méretezzük át kicsit szélesebbre a táblázatot, és esetleg vegyük szűkebbre a _Qty_ és _Line Total_ oszlopokat a többihez képest.

    Végül vastagítsuk ki a fejléc sor feliratait. Ehhez jelöljük ki a teljes sort a bal szélén található szürke négyzetre kattintva, majd a fenti eszköztáron kattintsunk a _Bold_ gombra

    ![Első sor formázása](../images/reportingservices/rs-table-bolt-header-row.png)

    Ha ránézünk a Preview fülre, ilyesmit kell látnunk:

    ![Riport előnézete](../images/reportingservices/rs-table-preview-2.png)

!!! example "BEADANDÓ"
    _Amennyiben folytatod a következő feladattal, a képernyőkép készítést itt kihagyhatod._

    Készíts egy képernyőképet a **report előnézetéről** a preview fülön. A képet a megoldásban `f1.png` néven add be. A képernyőképen a Visual Studio ablaka, és azon belül a riport előnézete látszódjon. Ismét ellenőrizd, hogy a **Neptun kódod** látható-e!

    A megváltozottott Visual Studio projektet (és annak fájljait) is töltsd fel.

### Csoportosítás és összegzés (5p)

A riport jelenleg ömlesztve tartalmazza az adatokat. Ezek eladási adatok, adott termékekből adott napon eladott mennyiség. Rendezzük csoportokba az adatokat.

1. Térjünk vissza a _Design_ fülre. Győződjünk meg róla, hogy a táblázatunk alatt látjuk a _Row Groups_ panelt – ha nem lenne ott, jobb klikkeljünk a dizájn felületen, és a _View_ menüben pipáljuk ki a _Grouping_ opciót.

1. A _Report Data_ panelről húzzuk a _Date_ mezőt a _Row Groups_ panelre, azon belül is a _(Details)_ sor fölé.

    ![Csoportosítás dátum alapján](../images/reportingservices/rs-group-by-date.png)

    A táblázatunk megjelenése a következőképpen fog változni:

    ![Csoportosítás hatása a táblázatra](../images/reportingservices/rs-group-by-date-table-designer.png)

1. Húzzuk az _Order_ mezőt is a _Row Groups_ panelre a _Date_ és a _(Details)_ közé.

    ![Csoportotsítás megrendelés szerint](../images/reportingservices/rs-group-by-order.png)

1. A csoportként megadott elemeknek automatikusan létrehozott oszlopokat a táblázatban a rendszer. Mivel már korábban is felvettük őket, ezért most kétszer is szerepelnek; töröljük őket. A felettük található szürke téglalapra kattintva jelöljük ki a **jobb oldali** _Date_ és _Order_ oszlopokat, és töröljük ki őket (jobb kattintás és _Delete Columns_).

    ![Duplikált oszlopok](../images/reportingservices/rs-group-by-duplicated-columns.png)

    Az új _Date_ oszlop formátuma ezzel visszaállt az eredeti formátumra, de a _Text Box Properties_ segítségével újra be tudjuk állítani.

    Ha most megnézzük a _Preview_ fület, láthatjuk, hogy az általunk megadott szempontok szerint (és az általunk megadott sorrendben) csoportosításra kerülnek a riport sorai.

    ![Csoportosított táblázat](../images/reportingservices/rs-table-preview-3.png)

1. Váltsunk vissza _Design_ nézetre. Kattintsunk jobb egérgombbal a `[LineTotal]` cellára, és válasszuk az _Add Total_ opciót. Ezzel az egyes _Order_-ekhez (amik mentén csoportosítottunk) meg fog jelenni a rendelések összege. Ehhez alapból nem rendelődik címke, de beírhatunk egyet: bal gombbal kattintsunk a megfelelő üres cellába, és írjuk be: "Order Total"

    ![Rendelés összege](../images/reportingservices/rs-add-total-order.png)

1. CTRL billentyűt nyomva tartva kattintsunk az _Order Total_ cellájára, majd a tőle jobbra levő két cellára is, hogy kijelöljük őket, és a _Format_ menüből válasszunk új háttérszínt nekik.

    ![Rendelés összeg színezése](../images/reportingservices/rs-add-total-order-color.png)

1. Az eredményt szokás szerint megnézhetjük a _Preview_ fülön:

    ![Előnézet](../images/reportingservices/rs-table-preview-4.png)

1. Készítsünk napi összegzést is!

    - Váltsunk vissza _Design_ nézetre
    - Jobb klikk az `[Order]` cellán, válasszuk az _Add Total_ > _After_ lehetőséget.
    - Az `[Order]` cella alatt megjelenik egy "Total" feliratú cella. Kattintsunk bele, és írjuk át "Daily Total"-ra.
    - Válasszuk ki ezt a cellát, és mellette a másik hármat (pl. a CTRL nyomvatartása mellett végigkattintgatva őket), majd adjunk nekik valamilyen háttérszínt (_Format_ > _Background color_).

1. Mivel az adatbázisban egy naphoz nagyon sok megrendelés is tartozhat, a _Preview_ fülön akár 4-5 oldalt is le kell görgetni, mire megpillantjuk munkánk gyümölcsét:

   ![Napi összeg](../images/reportingservices/rs-table-preview-5.png)

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a **report előnézetéről** a preview fülön. A képet a megoldásban `f1.png` néven add be. A képernyőképen a Visual Studio ablaka, és azon belül a riport előnézete látszódjon **az összegző sorokkal együtt** (lapozz, ha szükséges!). Ismét ellenőrizd, hogy a **Neptun kódod** látható-e!

    A megváltozottott Visual Studio projektet (és annak fájljait) is töltsd fel.

## Feladat 2: Vizualizáció

**A feladat önálló munka, és 5 pontot ér.**

A táblázatos megjelenítés részletesen mutatja az eladási adatokat. Egy diagram azonban gyorsabban áttekinthető. Készítsünk egy diagramot, ami az egyes termékkategóriák eladásait mutatja.

### Diagram beszúrása

1. Váltsunk _Design_ nézetre, és húzzunk be egy _Chart_-ot a _Toolbox_-ról a táblázat mellé. Ennek hatására elég sokáig fog tölteni a diagram varázsló, de egy idő után megnyílik. Válasszuk ki az oszlopdiagram típust.

1. A _Report Data_ panelről húzzuk a _LineTotal_ mezőt a diagramra. **Még ne engedjük fel a bal egérgombot.** Meg fog jelenni a diagram mellett a _Chart Data_ ablak – itt a "∑ values" mező (a fehér téglalap) fölé vigyük az egeret. Most már elengedhetjük.

    ![Chart hozzáadása](../images/reportingservices/rs-chart-data.png)

    Ezzel azt állítottuk be, hogy az eladás értékeiének összegét szeretnénk függőleges tengelynek.

1. Húzzuk a _Chart data_ alatt a _Category Groups_ mezőbe a _Subcat_ mezőt, a _Series Groups_-ba pedig a _Date_-et.

    ![Diagram értékei](../images/reportingservices/rs-chart-values.png)

    Ezzel azt érjük el, hogy a vízszintes tengelyen az alkategória szerint külön oszlop csoportokat kapunk, és a dátum szerint pedig külön oszlop sorozatokat.

1. A `[Date]` feliraton jobb klikkeljünk, és válasszuk a _Series Groups Properties_-t. Itt nyomjuk meg a _Group Expressions_ csoportban az **_fx_** gombot.

    ![Expression megadása](../images/reportingservices/rs-chart-group-expression.png)

    A megjelenő ablakban írjuk be: `=Year(Fields!Date.Value)`

    ![Expression értéke](../images/reportingservices/rs-chart-group-expression2.png)

    Ezzel a dátum éve szerinti oszlopokat fogunk kapni.

1. Nyomjunk OK-t mindkét ablakban. Mielőtt megnéznénk a _Preview_-t, növeljük meg a diagram magasságát bőséggel, különben a jelmagyarázat nem fog kiférni:

    ![Diagram átnéretezése](../images/reportingservices/rs-chart-resize.png)

1. Ha most megnézzük a Preview-t, az egyes kategóriák termelte bevételt fogjuk látni, év szerint csoportosítva:

    ![Diagram előnézete](../images/reportingservices/rs-chart-preview-1.png)

### A diagram formázása

A megjelenés még nem az igazi, de ezen könnyen segíthetünk.

1. _Chart Title_-re kattintva írjuk át a diagram címét, pl. "Revenue by category NEPTUN" **a saját Neptun kódodat behelyettesítve**.

1. A _Series Groups_ mezőben az `<<Expr>>` feliratra jobb klikkelve válasszuk ki a _Series Groups Properties_-t, és itt a _Label_ mező mellett nyomjuk meg az **_fx_** gombot. Értéknek adjuk meg: `=Year(Fields!Date.Value)`. Ezzel a felirat maga is csak az évet fogja mutatni.

1. Jobb klikkeljünk a függőleges tengely címkéin, és válasszuk a _Vertical Axis Properties_ lehetőséget.

    ![Tengely formázása](../images/reportingservices/rs-y-axis-properties.png)

    Itt a _Number_ fülön válasszuk a _Currency_ lehetőséget, és töltsük ki a már ismert módon:

    ![Tengely formázása](../images/reportingservices/rs-y-axis-properties-currency.png)

1. Ha most megnézzük a _Preview_ fület, már szép lesz a diagramunk (és a Neptun kódunk is szerepel rajta):

    ![Előnézet](../images/reportingservices/rs-chart-preview-2.png)

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a **report előnézetéről** a preview fülön. A képet a megoldásban `f2.png` néven add be. A képernyőképen a Visual Studio ablaka, és azon belül a riport előnézete látszódjon. Ellenőrizd, hogy a **Neptun kódod** látható-e a diagram címében!

    A megváltozottott Visual Studio projektet (és annak fájljait) is töltsd fel.

## Feladat 3: Értékesítői riport

**A feladat önálló munka, és 5 pontot ér.**

Ebben a feladatban az értékesítőkről fogunk riportot készíteni.

### Data set kiegészítése

Az új riporthoz új adatokra lesz szükségünk. Bővítsük ki a lekérdezésünket, azaz a _dataset_-et.

1. A _Report Data_ panelen a _DataSets_ alatt az _AdventureWorksDataset_-en jobb kattintással válasszuk a _Dataset properties_-t, majd bővítsük a query-t:

    ```sql hl_lines="7 9 17"
    SELECT
      soh.OrderDate AS [Date],
      soh.SalesOrderNumber AS [Order],
      pps.Name AS Subcat, pp.Name as Product,
      SUM(sd.OrderQty) AS Qty,
      SUM(sd.LineTotal) AS LineTotal
     , CONCAT(pepe.FirstName, ' ', pepe.LastName) AS SalesPersonName
    FROM Sales.SalesPerson sp
      INNER JOIN Person.Person as pepe ON sp.BusinessEntityID = pepe.BusinessEntityID
      INNER JOIN Sales.SalesOrderHeader AS soh ON sp.BusinessEntityID = soh.SalesPersonID
      INNER JOIN Sales.SalesOrderDetail AS sd ON sd.SalesOrderID = soh.SalesOrderID
      INNER JOIN Production.Product AS pp ON sd.ProductID = pp.ProductID
      INNER JOIN Production.ProductSubcategory AS pps ON pp.ProductSubcategoryID = pps.ProductSubcategoryID
      INNER JOIN Production.ProductCategory AS ppc ON ppc.ProductCategoryID = pps.ProductCategoryID
    GROUP BY ppc.Name, soh.OrderDate, soh.SalesOrderNumber,
             pps.Name, pp.Name, soh.SalesPersonID
            , pepe.FirstName, pepe.LastName
    HAVING ppc.Name = 'Clothing'
    ```

    A _Refresh Fields_ gombra kattintva ellenőrizhetjük, sikerült-e jól beírnunk. Ha nem jön hibaüzenet, akkor jók vagyunk. Zárjuk be a szerkesztő ablakot.

1. A _Report data_ ablakban az _AdventureWorksDataset_-et nyissuk ki, vagy ha ki van nyitva, akkor csukjuk be és nyissuk ki újra. Ekkor meg kell jelenjen egy új _SalesPersonName_ mező.

1. Ezután jobb klikk a Data Sources-ben az _AdventureWorks2014_-re > _Convert to shared Data Source_, majd jobb klikk az _AdventureWorksDataset_-re > _Convert to shared Dataset_. Ezzel a data source és a dataset is megosztható több riport fájl között.

## Új riport és adatforrások

Az előbb megosztottá konvertáltuk a data source-t és dataset-et. Ezeket fogjuk egy új riportban felhasználni.

1. _Solution Explorer_-ben jobb klikk a _Reports_ mappára > _Add new item_ > _Report_. Az új riport neve legyen "Sales People".

1. Nyissuk meg az új riportot. Az új riporthoz még nincsenek adatforrások. A _Report Data_ panelen vegyük fel a már meglevő adatforrásokat:

    - Jobb klikk a _Data Sources_ node-on > _Add Data Source_

    - Válasszuk a _Use shared data source reference_ opciót, és válasszuk ki az "AdventureWorks2014" nevűt.

        ![Megosztott adatforrás](../images/reportingservices/rs-add-datasource-shared.png)

    - Jobb klikk a _Datasets_-en > _Add Dataset_

    - Válasszuk ki a _Use a shared dataset_ opciót, és alatta válasszuk ki a már létező AdventureWorksDataset-et

        ![Megosztott adathalmaz](../images/reportingservices/rs-add-dataset-shared.png)

### Riport tartalma

Készíts táblázatos riportot arról, hogy az egyes értékesítők mikor, mit adtak el. Csoportosíts termék kategória és értékesítő szerint. Készíts összegző sort, ami az egyes értékesítők mindenkori eladásait összegzi! Ügyelj a számértékek formázására!

A megoldás kulcsa az alábbi táblázat és csoportosítás összeállítása. A kategóriát a _Subcat_ mező tartalmazza.

![Javasolt csoportok](../images/reportingservices/rs-sales-person-groups.png)

Az alábbihoz hasonló legyen a végső riport:

![Összegzés kinézete](../images/reportingservices/rs-sales-person-total.png)

!!! tip "Tipp"
    Ugyanúgy az _Add Total_ > _After_ funkciót kell használni, mint a közös feladatoknál. Amit itt el lehet rontani, hogy az _Add Total_ > _After_-t **nem** a `[SalesPersonName]`-en jobb klikkelve kell kiválasztani, hanem a `[Subcat]`-en jobbklikkelve – hiszen őket akarjuk összegezni, nem a Sales Person sorokat. (Ha a SalesPersonName-re kattintva csináljuk, abból "teljes összeg" lesz, hiszen minden értékesítőt összegzünk.)

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a **report előnézetéről** a preview fülön. A képet a megoldásban `f3.png` néven add be. A képernyőképen a Visual Studio ablaka, és azon belül a riport előnézete látszódjon. Ellenőrizd, hogy a **Neptun kódod** látható-e táblázatban!

    A megváltozottott Visual Studio projektet (és annak fájljait) is töltsd fel.

## Feladat 4: Opcionális feladat

**A feladat megoldásával 3 iMsc pont szerezhető.**

Készíts tortadiagramot az egyes értékesítők teljesítményének összehasonlítására, a hozzájuk tartozó összbevétel szerint! **A diagram címébe írd bele a Neptun kódod!** A cél az alábbi, vagy legalábbis ehhez hasonlító grafikon elkészítése.

![Elvárt tortadiagram](../images/reportingservices/rs-sales-person-pie-chart.png)

!!! tip "Tipp"
    A tortadiagram használata nagyon hasonló az oszlopdiagraméhoz. A lényeg, hogy a _∑ Values_ mezőbe a _LineTotal_-t, a _Category Groups_-ba a _SalesPersonName_-et húzzuk. (A _Series Groups_ ezúttal üres.)

    Ügyelj arra is, hogy a megfelelő nevek jelenjenek meg a jelmagyarázatban. A diagram magasságát valószínűleg növelni kell, hogy a teljes jelmagyarázat kiférjen.

    ![Tortadiagramban használandó értékek](../images/reportingservices/rs-sales-person-pie-char-valuest.png)

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a **report előnézetéről** a preview fülön. A képet a megoldásban `f4.png` néven add be. A képernyőképen a Visual Studio ablaka, és azon belül a riport előnézete látszódjon. Ellenőrizd, hogy a **Neptun kódod** látható-e a diagram címében!

    A megváltozottott Visual Studio projektet (és annak fájljait) is töltsd fel.
