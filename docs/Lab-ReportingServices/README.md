# Microsoft SQL Server Reporting Services használata

A labor során egy új eszközzel ismerkedünk meg, így a labor részben vezetett. Az első feladat laborvezetővel együtt megoldott, a továbbiak önálló feladatok. A közös feladatmegoldás és az önálló feladatmegoldás eredményét is be kell adni.

## Célkitűzés

A Microsoft SQL Server Reporting Services képességeinek megismerése, és egyszerű riportok készítése.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows
- Microsoft SQL Server
  - Express változat ingyenesen használható, avagy Visual Studio mellett feltelepülő _localdb_ változat is megfelelő
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Az adatbázist létrehozó script: [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip)
- Microsoft Visual Studio 2017 / 2019
  - Community verzió is megfelelő
- Report Server Projekt támogatás Visual Studio-hoz
  - Visual Studio 2017 esetén: [SQL Server Data Tools](https://docs.microsoft.com/en-us/sql/ssdt/download-sql-server-data-tools-ssdt?view=sql-server-2017#install-analysis-services-integration-services-and-reporting-services-tools)
  - Visual Studio 2019 esetén: [Microsoft Reporting Services Projects extension](https://marketplace.visualstudio.com/items?itemName=ProBITools.MicrosoftReportProjectsforVisualStudio)
    - (Érdemes [frissen tartani](https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-update-a-visual-studio-extension?view=vs-2019) ezt az extension-t, mert gyakran van belőle kiadás.)

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Microsoft SQL Server használata: [leírás](https://bmeviauac01.github.io/gyakorlatok/Adatbazis/mssql-server.html) és [videó](https://youtu.be/gmY8reqSL7U)
- SQL Reporting Services [hivatalos tutorial](https://docs.microsoft.com/en-us/sql/reporting-services/create-a-basic-table-report-ssrs-tutorial)

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub-hasznalat.md).

### Git repository létrehozása és letöltése

1. Az alábbi URL-en keresztül hozd létre a saját repository-dat: <https://classroom.github.com/a/n-Sj90sm>

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

   > Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd a kezdőoldalon a leírást), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adventure Works 2014 adatbázis létrehozása

A feladatok során az _Adventure Works_ minta adatbázissal dolgozunk. Az adatbázis egy kereskedelmi cég értékesítéseit tartalmazza, amelyből mi a teljes adatbázis megértése helyett előre definiált lekérdezésekkel dolgozunk csak, melyek termékek eladásainak adatait tartalmazza.

1. Töltsd le és csomagold ki az [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip) fájlt a `c:\work\Adventure Works 2014 OLTP Script` könyvtárba.

   Mindenképpen ez a mappa legyen, különben az sql fájlban az alábbi helyen ki kell javítani a könyvtár elérési útvonalát:

   ```sql
   -- NOTE: Change this path if you copied the script source to another path
   :setvar SqlSamplesSourceDataPath "C:\work\Adventure Works 2014 OLTP Script\"
   ```

1. Kapcsolódj Microsoft SQL Serverhez SQL Server Management Studio segítségével. Az alábbi adatokkal kapcsolódj.

   - Server name: `(localdb)\mssqllocaldb`
   - Authentication: `Windows authentication`

1. A _File / Open / File..._ menüpont használatával nyisd meg az előbbi mappából az `instawdb.sql` fájlt. **Még ne futtasd!** Előbb kapcsold be az SQLCMD módot: a _Query_ menüben _SQLCMD Mode_, és csak ezt követően válasszuk az _Execute_ lehetőséget.

   ![SQLCMD mód](../images/sql-management-sqlcmd-mode.png)

1. Ellenőrizd, hogy létrejött-e az adatbázis és a táblák. Ha a baloldali fában a _Databases_-en _Refresh_-t nyomsz, meg kell jelenjen az _AdventureWorks2014_ adatbázis a listában, és alatta számtalan tábla.

   ![AdventureWorks adatbázis táblák](../images/rs-adventureworks-tablak.png).

## Feladatok

Összesen 3 + 1 feladat van, az utolsó opcionális. [Itt kezdd](Feladat-1.md) az első feladattal.

## Végezetül: a megoldások feltöltése

Emlékeztetőül: a beadás módjának részletes lépéseit [itt](../GitHub-hasznalat.md#a-megoldás-beadása) találod.

1. Ellenőrizd, hogy a `neptun.txt`-ben szerepel-e a Neptun kódod.
1. A feladatok megoldásaként új `.rdl` fájlok készültek a git repository alatt egy alkönyvtárban. Ezeket is fel kell töltened.
1. Ellenőrizd, hogy a kért képernyőképeket elkészítetted-e.
1. Kommitold és pushold a megoldásod az újonnan készített ágra. Ellenőrizd a GitHub webes felületén, hogy minden rendben van-e.
1. Nyiss egy pull request-et a megoldásoddal és rendeld a laborvezetődhöz.
1. Ha tanszéki laborban vagy, [töröld a GitHub belépési adatokat](../README.md#egyetemi-laborokban-github-belépési-adatok-törlése).
