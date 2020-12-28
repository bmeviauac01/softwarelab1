# Feladat 1: Táblázatos riport

A feladat a laborvezetővel **közösen megoldott**.

Nyisd meg a checkoutolt git repository-ban a `reportserver.sln` fájlt. Ez egy üres _Report Server_ típusú projekt, amelyet Visual Studio-ban fejlesztünk.

A Report Server projekt elsősorban úgynevezett _Report Definition_ (.rdl) fájlokból áll, amelyek definiálják a riport előállításához szükséges adatforrásokat (lekérdezéseket), és a kinézet sablonját, amit adatokkal feltöltve kapjuk az eredmény riportot. A neve onnan ered, hogy ezeket a riportokat nem csak a fejlesztői gépen lehet lefuttatni, hanem egy un. _Report Server_-re publikálhatóak, ahonnan a vállalat megfelelő üzleti szereplői mindig friss riportokat kérhetnek, az aktuális adatok alapján.

!!! note ""
    Ezen a laboron nem tudjuk megmutatni a Report Server-t. Ennek csupán technikai okai vannak, a Report Server telepítés után kézi konfigurációt igényel, amit a laborokban megfelelő jogosultság nélkül nem tudunk megtenni. Ezért csak a Visual Studio-ban fogjuk látni a riportot.

## Hozzuk létre az első Report Definition fájlt.

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

## Adatforrás (data source) beállítása

Az adatforrás definálja, a riport adatai honnan származnak. A mi esetünkben ez a korábban lérehozott SQL Server adatbázis lesz.

1. A _Report Data_ panelen _New_ > _Data Source_. A neve legyen "AdventureWorks2014".

    ![Add datasource](../images/reportingservices/rs-add-datasource.png)

1. A megjelenő űrlapon válasszuk a _Microsoft SQL Server_ típust és a _connection string_ mező melletti gombra kattintva adjuk meg ismét az adatbázis elérését

    - Server name: `(localdb)\mssqllocaldb`
    - Authentication: `Windows Authentication`
    - Select or enter database name: `AdventureWorks2014`

1. OK-ézzuk le a dialógusokat. Majd nyissuk meg **újra** a Data Source tulajdonságait (jobb egérrel és _Data Source Properties_), és ellenőrizzük a _Credentials_ fület, mert a Visual Studio néha "elfelejti" a beállítást. Az alábbi checkbox-nak kell kijelölve lennie:

    ![Data source credentials beállítás](../images/reportingservices/rs-data-source-properties.png)

## Adathalmaz (data set) megadása

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

## Táblázatos riport készítése (5p)

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
        A második sorban látható `[Date]` jelöli a kiértékelendő kifejezést, míg az eső sorban látható "Date" felirat lesz az oszlop fejléce a riportban – ez utóbbit át is írhatjuk.

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

## Csoportosítás és összegzés (5p)

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

    Az új _Date_ oszlop formátuma ezzel visszaállt az eredeti formátumra, de a _Text Style Properties_ segítségével újra be tudjuk állítani.

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
