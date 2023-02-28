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

