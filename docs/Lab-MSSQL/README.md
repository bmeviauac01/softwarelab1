# Microsoft SQL Szerver programozása

## Célkitűzés

A Microsoft SQL Server programozási lehetőségeinek gyakorlása komplexebb feladatokon keresztül.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Microsoft SQL Server
  - Express változat ingyenesen használható, avagy Visual Studio mellett feltelepülő _localdb_ változat is megfelelő
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
  - Vagy kipróbálható a platformfüggetlen [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download) is
- Az adatbázist létrehozó script: [mssql.sql](https://raw.githubusercontent.com/bmeviauac01/gyakorlatok/master/mssql.sql)

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Microsoft SQL Server használata: [leírás](https://bmeviauac01.github.io/gyakorlatok/Adatbazis/mssql-server.html) és [videó](https://youtu.be/gmY8reqSL7U)
- Az adatbázis [sémájának leírása](https://bmeviauac01.github.io/gyakorlatok/Adatbazis/sema.html)
- Microsoft SQL Server programozási lehetőségei és az SQL nyelv
  - Lásd az Adatvezérelt rendszerek c. tárgy [jegyzetei](https://www.aut.bme.hu/Course/adatvezerelt) és [gyakorlati anyagai](https://bmeviauac01.github.io/gyakorlatok/) között

## Előkészület

### Git repository létrehozása és letöltése

1. Az alábbi URL-en keresztül hozd létre a saját repository-dat: TBD

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

   > Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd a kezdőoldalon a leírást), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adatbázis létrehozása

1. Kapcsolódj Microsoft SQL Serverhez SQL Server Management Studio Segítségével. Indítsd el az alkalmazást, és az alábbi adatokkal kapcsolódj.

   - Server name: `(localdb)\mssqllocaldb` vagy `.\sqlexpress` (ezzel egyenértékű: `localhost\sqlexpress`)
   - Authentication: `Windows authentication`

1. Hozz létre egy új adatbázist (ha még nem létezik). Az adatbázis neve legyen a Neptun kódod: _Object Explorer_-ben Databases-en jobb kattintás, és _Create Database_.

   > Fontos, hogy az adatbázis neve egyezzen meg a Neptun kódoddal. A labor megoldásában kérünk képernyőképeket, amelyeken így kell szerepelnie az adatbázisnak!

1. Hozd létre a minta adatbázist a generáló script lefuttatásával. Nyiss egy új _Query_ ablakot, másold be a script tartalmát, és futtasd le. Ügyelj az eszköztáron levő legördülő menüben a megfelelő adatbázis kiválasztására.

   ![Adatbázis kiválasztása](../images/sql-management-database-dropdown.png)

1. Ellenőrizd, hogy létrejöttek-e a táblák. Ha a _Tables_ mappa ki volt már nyitva, akkor frissíteni kell.

   ![Adatbázis kiválasztása](../images/sql-managment-tablak.png).

## Feladatok

Összesen 3 + 1 feladat van, az utolsó opcionális. [Itt kezdd](Feladat-1.md) az első feladattal.

## Végezetül: a megoldások feltöltése

1. Ellenőrizd, hogy a `neptun.txt`-ben szerepel-e a Neptun kódod.
1. Nézd meg, hogy a megoldásaidat a feladatban meghatározott fájlnévvel mentetted-e el. Minden fájl már ott volt a letöltött repository-ban, neked csak felül kellett őket írnod.
1. Ellenőrizd, hogy a kért képernyőképeket elkészítetted-e.
1. Kommitold és pushold a megoldásod. Ellenőrizd a GitHub webes felületén, hogy minden rendben van-e.
1. Nyiss egy pull request-et a megoldásoddal és rendeld a laborvezetődhöz.
1. Ha tanszéki laborban vagy, töröld a GitHub belépési adatokat. A lépésekhez nézd meg a kezdőoldalon a leírást.
