# Microsoft SQL Server Reporting Services használata

## Célkitűzés

A Microsoft SQL Server Reporting Services képességeinek megismerése, és egyszerű riportok készítése.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

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

### Git repository létrehozása és letöltése

1. Az alábbi URL-en keresztül hozd létre a saját repository-dat: TBD

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adventure Works 2014 adatbázis létrehozása

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

1. Ellenőrizd, hogy létrejött-e az adatbázis és a táblák. Ha a baloldali fában a _Databases_-en _Refresh_-t nyomsz: meg kell jelenjen az _AdventureWorks2014_ adatbázis a listában, és alatta számtalan tábla.

   ![AdventureWorks adatbázis táblák](../images/rs-adventureworks-tablak.png).

## Feladatok

Összesen 3 + 1 feladat van, az utolsó opcionális. [Itt kezdd](Feladat-1.md) az első feladattal.

## Végezetül: a megoldások feltöltése

1. Ellenőrizd, hogy a `neptun.txt`-ben szerepel-e a Neptun kódod.
1. A feladatok megoldásaként új `.rdl` fájlok készültek a git repository alatt egy alkönyvtárban. Ezeket is fel kell töltened.
1. Ellenőrizd, hogy a kért képernyőképeket elkészítetted-e.
1. Kommitold és pushold a megoldásod. Ellenőrizd a GitHub webes felületén, hogy minden rendben van-e.
1. Ha tanszéki laborban vagy, töröld a GitHub belépési adatokat. A lépésekhez nézd meg a kezdőoldalon a leírást.
