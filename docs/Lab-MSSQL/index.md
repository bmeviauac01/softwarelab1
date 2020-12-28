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
- Az adatbázist létrehozó script: [mssql.sql](https://raw.githubusercontent.com/bmeviauac01/adatvezerelt/master/docs/db/mssql.sql)
- GitHub account és egy git kliens

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Microsoft SQL Server használata: [leírás](https://bmeviauac01.github.io/adatvezerelt/db/mssql/) és [videó](https://web.microsoftstream.com/video/e3a83d16-b5c4-4fe9-b027-703347951621)
- Az adatbázis [sémájának leírása](https://bmeviauac01.github.io/adatvezerelt/db/)
- Microsoft SQL Server programozási lehetőségei és az SQL nyelv
    - Lásd az Adatvezérelt rendszerek c. tárgy [jegyzetei](https://www.aut.bme.hu/Course/adatvezerelt) és [gyakorlati anyagai](https://bmeviauac01.github.io/adatvezerelt/) között

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Az alábbi URL-en keresztül hozd létre a saját repository-dat: <https://classroom.github.com/a/_MNDrIc3>

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! tip ""
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

1. Hozd létre a minta adatbázist a [generáló script](https://raw.githubusercontent.com/bmeviauac01/adatvezerelt/master/docs/db/mssql.sql) lefuttatásával. Nyiss egy új _Query_ ablakot, másold be a script tartalmát, és futtasd le. Ügyelj az eszköztáron levő legördülő menüben a megfelelő adatbázis kiválasztására.

    ![Adatbázis kiválasztása](../images/sql-management-database-dropdown.png)

1. Ellenőrizd, hogy létrejöttek-e a táblák. Ha a _Tables_ mappa ki volt már nyitva, akkor frissíteni kell.

    ![Adatbázis kiválasztása](../images/sql-managment-tablak.png).
