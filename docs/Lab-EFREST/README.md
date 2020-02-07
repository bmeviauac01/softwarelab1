# Entity Framework és REST

## Célkitűzés

Az Entity Framework alapú adatbázis elérés és a REST API készítésének gyakorlása komplexebb feladatokon keresztül.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows, Linux, vagy MacOS
  - Minden szükséges program platform független, vagy van platformfüggetlen alternatívája.
- Microsoft Visual Studio 2017/2019 [az itt található beállításokkal](../VisualStudio-install.md)
  - Linux és MacOS esetén Visual Studio Code és a .NET Core SDK-val települő [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x) használható.
- [.NET Core 2.1 SDK](<(https://dotnet.microsoft.com/download/dotnet-core/2.1)>)
  - Visual Studio esetén települ, de ha mégse, akkor innen kell telepíteni (az SDK-t és _nem_ a runtime-ot.)
  - Linux és MacOS esetén telepíteni szükséges.
- [Postman](https://www.getpostman.com/)
- [DB Browser for SQLite](https://sqlitebrowser.org/), ha az adatbázisba szeretnél belezézni (nem feltétlenül szükséges)
- GitHub account és egy git kliens

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Entity Framework, REST API, Web API elméleti háttere és mintapéldái, valamint a Postman használata
  - Lásd az Adatvezérelt rendszerek c. tárgy [jegyzetei](https://www.aut.bme.hu/Course/adatvezerelt) és [gyakorlati anyagai](https://bmeviauac01.github.io/gyakorlatok/) között
- Hivatalos Microsoft tutorial [Web API készítéséhez](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1&tabs=visual-studio)

## Feladat áttekintése

A feladok elkészítése során egy egyszerű feladatkezelő webalkalmazás backendjét készítjük el. Az alkalmazás **két féle entitást kezel: státusz és task**, ahol egy státuszhoz több task rendelhető (1-\* kapcsolat). (A feladatszövegben _task_ néven fogunk hivatkozni a második entitásra, és kerüljük a "feladat" megnevezést, amely félreérthető lenne.)

Ha lenne frontendünk is, akkor egy [Kanban-tábla](https://en.wikipedia.org/wiki/Kanban_board) szerű feladatkezelőt készítenénk. A frontendtől eltekintünk, csak a szükséges REST Api-t és Entity Framework + ASP.NET Core Web Api alapú kiszolgálót készítjük el.

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub-hasznalat.md).

### Git repository létrehozása és letöltése

1. Az alábbi URL-en keresztül hozd létre a saját repository-dat: <https://classroom.github.com/a/TLrieihF>

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

   > Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd a kezdőoldalon a leírást), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adatbázis létrehozása

Ebben a feladatban nem Microsoft SQL Server-t használunk, hanem Sqlite-ot. Ez egy pehelysúlyú relációs adatbázis, amelyet elsősorban kliensoldali alkalmazásokban szokás használni, kiszolgáló oldalon nem javasolt a használata. Mi most az egyszerűség végett fogjuk használni. **Ezt nem szükséges telepíteni**.

Az adatbázis sémát a _code first_ modell szerint fogjuk létrehozni, így C# kóddal fogjuk leírni az adatbázisunkat. Ezért az adatbázis sémáját se kell SQL kóddal létrehoznunk.

## Feladatok

Összesen 4 feladat van. [Itt kezdd](Feladat-1.md) az első feladattal.

## Végezetül: a megoldások feltöltése

Emlékeztetőül: a beadás módjának részletes lépéseit [itt](../GitHub-hasznalat.md#a-megoldás-beadása) találod.

1. Ellenőrizd, hogy a `neptun.txt`-ben szerepel-e a Neptun kódod.
1. Nézd meg, hogy a megoldásaidat a feladatban meghatározott fájlnévvel mentetted-e el. Minden fájl már ott volt a letöltött repository-ban, neked csak felül kellett őket írnod.
1. Ellenőrizd, hogy a kért képernyőképeket elkészítetted-e.
1. Kommitold és pushold a megoldásod az újonnan készített ágra. Ellenőrizd a GitHub webes felületén, hogy minden rendben van-e.
1. Nyiss egy pull request-et a megoldásoddal és rendeld a laborvezetődhöz.
1. Ha tanszéki laborban vagy, [töröld a GitHub belépési adatokat](../README.md#egyetemi-laborokban-github-belépési-adatok-törlése).
