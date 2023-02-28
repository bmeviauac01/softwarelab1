# Power BI

A labor során egy új eszközzel, a _Microsoft Power BI_ szolgáltatással ismerkedünk meg, így a labor részben vezetett. Az első feladat laborvezetővel együtt megoldott, a továbbiak önálló feladatok. A közös feladatmegoldás és az önálló feladatmegoldás eredményét is be kell adni.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows
- [Power BI Desktop](#powerbi-desktop-telepítése)
- GitHub account és egy git kliens

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Power BI platform: [dokumentáció](https://learn.microsoft.com/en-us/power-bi/)

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! tip ""
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Ingyenes Power BI regisztráció

1. A Power BI ingyenes előfizetési szintjét fogjuk használni. Ennek használata előzetes regisztrációhoz kötött, melyhez keresd fel a [Power BI honlapját](https://powerbi.microsoft.com/) jelentkezz be a ***@edu.bme.hu emailcímeddel, majd kattints az ingyenes regisztráció gombra. Sikeres regisztráció esetén az alábbi üzenet fogad:

    ![Regisztráció sikeres](../images/powerbi/pb-register-licenseassigned.png)

1. A regisztráció a [Power BI weboldalára](https://app.powerbi.com/) visz minket. Ismerkedjünk meg vele.

### PowerBI Desktop telepítése

Power BI jelentések készítéséhez a Power BI Desktop alkalmazást fogjuk használni.

1. Ellenőrizd, hogy telepítve van-e a gépedre, ha igen, nincs szükség a lenti lépésekre. Start....

    A Power BI Desktop alkalmazást a Microsoft Store-ból telepítjük. Laborgépeken ez praktikus, mert nincs szükség hozzá rendszergazdai jogosultságokra, saját gépen választhatjuk a [letölthető telepítőt is](https://www.microsoft.com/en-us/download/details.aspx?id=58494).

1. Nyisd meg a [Power Bi Desktop oldalát a Microsoft Store](https://aka.ms/pbidesktopstore)-ban! A telepítéshez be kell lépned a Microsoft Store-ba, ehhez használd a ***@edu.bme.hu felhasználód.

    ![Telepítés](../images/powerbi/pb-install-store.png)

1. Amennyiben laborgépen dolgozol, a telepítést követően léptesd ki a felhasználód a Store alkalmazásból.

## Feladat 1: Táblázatos riport

A feladat a laborvezetővel **közösen megoldott**.

A Power BI jelentések létrehozása általában egy jellemző munkafolyamatot követ, melynek lépései a következők:

![Power BI Workflow](../images/powerbi/pb-intro-workflow.png)

### Hozzuk létre az első adathalmazt

A Power BI-ban feldolgozandó adatokat az egyszerűség kedvéért relációs adatbázis helyett annak egy Excelbe exportált változatából fogjuk kinyerni, ezt fogjuk a következő lépések során regisztrálni.

1. Töltsd le az adatbázisunkat [AdventureWorksSales.xlsx](AdventureWorksSales.xlsx). Nyisd meg a fájlt, ismerkedj meg a benne lévő adatokkal!

1. Indítsd el a Power BI desktop alkalmazást! Jelentkezz be a saját ***@edu.bme.hu felhasználóddal.

    ![Belépés](../images/powerbi/pb-install-signin.png)

1. Zárd be a felugró dialógusablakokat, majd mentsd el a projektet (File/Save) tetszőleges könyvtárba. **A projekt neve a Neptun kódod legyen**!

1. Töltsd be a korábban letöltött _AdventureWorksSales.xlsx_ fájlt! (Get data / Excel workbook)

    ![Betöltés](../images/powerbi/pb-load-excel.png)

1. Válaszd ki az összes adattáblát (aminek a nevében nincs _data_ postfix) majd nyomd meg a Load gombot!

    ![Adattáblák kiválasztása](../images/powerbi/pb-load-tables.png)

1. A betöltés eredményét a _Model view_ és _Data view_ nézetekben tudod ellenőrizni. Figyeljük meg, hogy az elnevezési konvenciók alapján a betöltő rögtön felismerte az idegen kulcs kapcsolatok egy részét is.

    ![Nézetek](../images/powerbi/pb-load-model.png)

1. Figyeljük meg a modellben, hogy a dátumok esetében a relációkat nem ismerte fel a betöltő. Adjuk ezt meg kézzel! Húzzuk rá egyesével a Sales tábla _DueDateKey_, _OrderDateKey_ és _ShipDateKey_ mezőit a _Date_ tábla _DateKey_ oszlopára.

    ![Dátumok relációi](../images/powerbi/pb-load-datekey.png)

### Hozzuk létre az első jelentést (5p)

1. Váltsunk át _Report view_-ra.

1. Adjunk hozzá egy új táblázatot az aktuális oldalhoz!

    ![Új táblázat hozzáadása](../images/powerbi/pb-1streport-addtable.png)

1. A _Data_ eszköztárról húzzuk be a táblázatra a _Product_ tábla _Category_, _Model_ és _Product_ oszlopait, továbbá a _Sales_ tábla _Sales Amount_ oszlopát! Az eredmény valahogy így néz ki:

    ![Új táblázat hozzáadása](../images/powerbi/pb-1streport-addcolumns.png)

1. Figyeljük meg, hogy bár a termék és az eladások egy-több viszonyban vannak, az eladások esetében automatikusan egy összegzés történik.

1. Formázzuk meg a táblázatot! Ehhez a _Visualizations_ eszköztár _Format your visual_ eszköztárát használjuk, miközben a táblázat folyamatosan ki van jelölve.

    1. _Style presets_ esetében válaszuk az _Alternating rows_ opciót. Ez ad egy alapértelmezett formázást a táblázatnak.

    1. _Values_ blokkban állítsuk be a betűméretett 14-re, _Text color_ és _Alternate text color_ esetében állítsunk be egy-egy szimpatikus, eredetitől eltérő színt.

    1. Méretezzük át a táblázatot, hogy a nagyobb fontméret mellett is szépen kiférjen a tartalom.

        ![Táblázat átméretezése](../images/powerbi/pb-1streport-resize.png)

    1. Emeljük meg a fejléc betűméretét is (_Column headers_)

    1. Kapcsoljuk ki az összegzés (_Totals_/_Values_ jobb felső sarkát állítsuk off-ra)

    1. Emeljük ki az összeg oszlopot! Ehhez a _Specific column_ blokkban válasszuk ki a _Sum of Extended Amount_ oszlopot (_Series_ mező), majd a _Text color_ értékét állítsuk fehérre, _Background color_ értékét pedig egy sötétebb színre.

    1. Ezen a ponton a táblázatunk valahogy így néz ki:

    ![Formázott táblázat](../images/powerbi/pb-1streport-formattedtable.png)

1. Adjunk hozzá egyedi szűrőket a táblázathoz, illetve szabjuk testre azok megjelenését!

    1. Kattintsunk a táblázat alatti szűrő ikonra, ami előhozza a _Filters_ eszköztárat. Látható, hogy a 4 oszlophoz eleve létrejöttek szűrők.

        ![Szűrők](../images/powerbi/pb-1streport-filters.png)

    1. Húzzuk be a _SalesTerritory_ _Country_ oszlopát a szűrök közé. Most már erre is szűrhetünk igény szerint.

    1. Rejtsük el az eladási árra vonatkzó szűrőt. Ehhez kattintsunk a _Sum of Extended Amount_ szűrőn belül a kis szem ikonra. Bár a szűrő számunkra továbbra is látható marad, a publikált jelentésben már nem fog megjelenni így.

        ![Szűrő elrejtése](../images/powerbi/pb-1streport-hidefilter.png)

    1. Nevezzük át magyarra a megmaradt szűrőket jobb egérgombal rájuk kattintva és a _rename_ opciót választva. Az új magyar nevek:_Kategória_, _Model_, _Ország_, _Termék_

    1. Kattintsunk az oldalon egy olyan részre, ahol nincs táblázat, ezzel magát az oldalt (_Page_) kijelölve. A _Visualisations_ eszköztárban ekkor a formázások az egész oldalra vonatkozó beállításokat fognak tartalmazni (_Format page_). Ezen belül szabjuk testre a szűrők megjelenését a következő lépésekben.

    1. Állítsuk a _Filter pane_/_Search_ tulajdonságot egy másik világos színre.

    1. Állítsuk a _Filter pane_/_Background_ tulajdonságot egy világos színre.

    1. Állítsuk a _Filter **cards**_/_Background_ tulajdonságot az előzővel azonos színre. A végeredmény pl. ilyesmi lehet:

        ![Formázott szűrők](../images/powerbi/pb-1streport-formattedfilters.png)

1. Mentsük el a változtatásokat, majd publikáljuk az elkészült jelentést az online szolgáltatásba a _Home_ lap _Publish_ gombjával!

    ![Publikálás](../images/powerbi/pb-1streport-publishbutton.png)

1. A publikálásnál jelöljük meg az alapértelmezett munkaterületet célként (_My Workspace_)

    ![Munkaterület választás](../images/powerbi/pb-1streport-publishdialog.png)

1. Sikeres publikálást követően kattintsunk a dialógusablakban megjelenő linkre a jelentés megnyitásához.

    ![Munkaterület választás](../images/powerbi/pb-1streport-publishready.png)

1. Kísérletezzünk az elkészült jelentésünkkel, hogy lássuk, mit csináltunk.

    ![Munkaterület választás](../images/powerbi/pb-1streport-online.png)


!!! example "BEADANDÓ"
    Készíts egy képernyőképet a **report előnézetéről** a preview fülön. A képet a megoldásban `f1.png` néven add be. A képernyőképen a Visual Studio ablaka, és azon belül a riport előnézete látszódjon **az összegző sorokkal együtt** (lapozz, ha szükséges!). Ismét ellenőrizd, hogy a **Neptun kódod** látható-e!

    A megváltozottott Visual Studio projektet (és annak fájljait) is töltsd fel.

## Feladat 2: Vizualizáció

**A feladat önálló munka, és 5 pontot ér.**

### Diagram létrehozása

A táblázatos megjelenítés részletesen mutatja az eladási adatokat. Egy diagram azonban gyorsabban áttekinthető. Készítsünk egy diagramot, ami az egyes termékkategóriák eladási darabszámait mutatja.

1. A diagramhoz hozzunk létre egy új oldalt a jelentésben.

    ![Oldal hozzáadása](../images/powerbi/pb-diagram-addpage.png)

1. Ragadjuk meg az alkalmaz, hogy az oldalaknak beszédesebb neveket adjunk. Az oldalak elnevezésére duplán kattintva átírhatjuk azokat. Legyen az első oldal neve 'Táblázat', az új oldalé 'Diagram'

1. Adjunk hozzá egy csoportosított oszlop diagrammot (_clustered column chart_) az új oldalhoz.

    ![Oszlopdiagram](../images/powerbi/pb-diagram-statechart.png)

1. A diagramon termékkategóriánként szeretnénk összegezni az eladási darabszámokat. Ezt a következőképpen tudjuk elérni:
    
    1. Jelöljük ki a diagramot.

    1. A _Visualizations_ eszköztár _Build visual_ oldalán tudjuk a paramétereket megadni. Húzzuk rá az _X axis_ mezőre a _Product_/_Category_ oszlopot, illetve az _Y axis_ mezőre a _Sales_/_Order quantity_ oszlopot.

    ![Oszlopdiagram adatokkal](../images/powerbi/pb-diagram-quantities.png)

### Éves bontás

Következő lépésként szeretnénk az előbbi adatokat éves bontásban is megnézni. Mivel az eladáshoz háromféle dátum is (OrderDate, DueDate, ShipDate) is tartozik el kell döntenünk melyiket használjuk. Számunkra most a megrendelés dátuma lesz a releváns, ezért azt kell elérnünk, hogy ha a _Date_ tábla szerint szűrünk, vagy csoportosítunk, akkor ezt vegye figyelembe a rendszer.

1. Nyissuk meg a modellezőt (_Mode view_)! Figyeljük meg, hogy a _Sales_ és a _Date_ tábla között három kapcsolat is van, ezek közül azonban csak egy van folytonos vonallal kiemelve, a másik kettő esetében szaggatott a vonal. A folytonos vonal jelenti az aktív kapcsolatot és ez lesz a későbbi csoportosítás és szűrés alapja. Az egyes kapcsolatok fölé mozgatva az egérkurzort azt is láthatjuk, hogy azok mely oszlopokat kötik össze. Két tábla között legfeljebb egy aktív kapcsolat lehet.

    ![Dátumok kapcsolata](../images/powerbi/pb-diagram-orderdate.png)

1. Amennyiben **nem** az _OrderDateKey_-_DateKey_ kapcsolat az aktív, akkor először is meg kell szüntetnünk a meglévő aktív kapcsolatot. Ehhez jelöljük ki azt, majd jobb oldalt a _Make this relationship active_ opciót kapcsoljuk ki, majd kattintsunk az _Apply changes_ parancsra.

    ![Aktív kapcsolat](../images/powerbi/pb-diagram-relationship.png)

1. Az előző lépéssor analógiájára válasszuk ki most az _OrderDateKey_-_DateKey_ kapcsolathoz tartozó vonalat és tegyük azt aktívvá. A végén ne maradjon el az _Apply changes_ lépés sem.

!!! tip "Tipp"
    Előfordulhatna olyan szituáció, ahol egyszerre kétféle dátum alapján is szeretnénk mondjuk kategorizálni. Ebben az esetben a _Date_ tábla duplikálásával tudjuk megkerülni az egy reláció-egy aktív kapcsolat korlátot.

1. Visszatérve a jelentés nézetre, a diagram _Legend_ tulajdonságához húzzuk be a _Date_/_Fiscal Year_ oszlopot.

    ![Éves csoportosítás](../images/powerbi/pb-diagram-year.png)

!!! tip "Tipp"
    Lehetőségünk lenne arra is, hogy többszintű kategóriák szerint csoportosítsuk az oszlopainkat. Ilyen esetekben lehetőség van az egyes mezőkre több oszlopot is ráhúzni.

### Térkép

A Power BI számos látványos és intelligens diagrammodellel rendelkezik. A következőkben megjelenítjük egy világtérképen az egyes országok eladási darabszámait kategóriánkénti bontásban.

1. Vegyünk fel egy új lapot _Térkép_ néven és töltsük ki a tulajdonságait a következők szerint

1. _Location_ mező értéke legyen _SalesTerritory_/_Country_

1. _Bubble size_ mező értéke legyen _Sales_/_Order quantity_

1. Méretezzük át a térképet, hogy szebben kitöltse a lapot

Ezen a ponton láthatjuk már az országos eladási adatokkal arányos buborékokat a térképen. Az utolsó lépésben még a kategóriánkénti bontást kell megvalósítanunk, melyet az oszlopdiagramhoz hasonlóan a _Legend_ mező kitöltésével tudunk megtenni. 

1. _Legend_ mező értéke legyen _Product_/_Category_

    ![Térkép](../images/powerbi/pb-diagram-map.png)

## Feladat 3: Értékesítői riport

**A feladat önálló munka, és 5 pontot ér.**

Ebben a feladatban megismerkedünk az összetett szűrőkkel, a vonaldiagrammal és a komplex adatokkal

### Riport tartalma

Készíts táblázatos riportot arról, hogy az egyes értékesítők mikor, mit adtak el. Csoportosíts termék kategória és értékesítő szerint. Készíts összegző sort, ami az egyes értékesítők mindenkori eladásait összegzi! Ügyelj a számértékek formázására!

A megoldás kulcsa az alábbi táblázat és csoportosítás összeállítása. A kategóriát a _Subcat_ mező tartalmazza.

![Javasolt csoportok](../images/powerbi/pb-slicer.png)

Az alábbihoz hasonló legyen a végső riport:

![Összegzés kinézete](../images/reportingservices/rs-sales-person-total.png)

!!! tip "Tipp"
    Ugyanúgy az _Add Total_ > _After_ funkciót kell használni, mint a közös feladatoknál. Amit itt el lehet rontani, hogy az _Add Total_ > _After_-t **nem** a `[SalesPersonName]`-en jobb klikkelve kell kiválasztani, hanem a `[Subcat]`-en jobbklikkelve – hiszen őket akarjuk összegezni, nem a Sales Person sorokat. (Ha a SalesPersonName-re kattintva csináljuk, abból "teljes összeg" lesz, hiszen minden értékesítőt összegzünk.)

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a **report előnézetéről** a preview fülön. A képet a megoldásban `f3.png` néven add be. A képernyőképen a Visual Studio ablaka, és azon belül a riport előnézete látszódjon. Ellenőrizd, hogy a **Neptun kódod** látható-e táblázatban!

    A megváltozottott Visual Studio projektet (és annak fájljait) is töltsd fel.

## Feladat 4: Opcionális feladat

**A feladat megoldásával 3 IMSc pont szerezhető.**

Készíts tortadiagramot az egyes termékkategóriákban létrejövő tranzakciók számáról! **A diagram címébe írd bele a Neptun kódod!**.

- A kategóriák színei a lentiek szerint piros, sárga, zöld és kék legyenek

- Feliratok a tortadiagramon belül legyenek

- A kész jelentést töltsd fel az online Power BI szolgáltatásba is!

- A cél az alábbi, vagy legalábbis ehhez hasonlító grafikon elkészítése.

- A diagram címe középre igazítva, vastag betűkkel a Neptun kódod legyen!

![Elvárt tortadiagram](../images/powerbi/pb-diagram-pie.png)

!!! tip "Tipp"
    A tortadiagram használata nagyon hasonló az oszlopdiagraméhoz. 

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a **publikált jelentésről**. A képet a megoldásban `f4.png` néven add be. A képernyőképen a Visual Studio ablaka, és azon belül a riport előnézete látszódjon. Ellenőrizd, hogy a **Neptun kódod** látható-e a diagram címében!

    A megváltozottott Power BI jelentés fájlt is töltsd fel.
