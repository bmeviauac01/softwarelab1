# Lekérdezés optimalizálás

A labor során a lekérdezés optimalizálást vizsgáljuk Microsoft SQL Server platformon. Azért, hogy a feladatok során megfelelően megértsük a működést, és dokumentálni is tudjuk, az **első 5 feladat** során megadjuk a megoldást és a magyarázatot is. A **továbbiak önálló feladatok**, ahol a magyarázat kitalálása a feladat része. A közös feladatmegoldás és az önálló feladatmegoldás eredményét is dokumentálni kell és be kell adni.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows, Linux vagy MacOS: Minden szükséges program platform független, vagy van platformfüggetlen alternatívája.
- Microsoft SQL Server
    - Express változat ingyenesen használható, avagy Visual Studio mellett feltelepülő _localdb_ változat is megfelelő
    - Van [Linux változata](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup) is.
    - MacOS-en Docker-rel futtatható.
- [Visual Studio Code](https://code.visualstudio.com/) vagy más, markdown kompatibilis szerkesztő
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms), vagy kipróbálható a platformfüggetlen [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download) is
- Az adatbázist létrehozó script: [mssql.sql](https://raw.githubusercontent.com/bmeviauac01/adatvezerelt/master/docs/db/mssql.sql)
- GitHub account és egy git kliens

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Markdown formátum [rövid ismertetője](https://guides.github.com/features/mastering-markdown/) és [részletes dokumentációja](https://help.github.com/en/github/writing-on-github/basic-writing-and-formatting-syntax)
- Microsoft SQL Server használata: [leírás](https://bmeviauac01.github.io/adatvezerelt/db/mssql/) és [videó](https://web.microsoftstream.com/video/e3a83d16-b5c4-4fe9-b027-703347951621)
- Az adatbázis [sémájának leírása](https://bmeviauac01.github.io/adatvezerelt/db/)
- Microsoft SQL Server lekérdezés optimalizálása
    - Lásd az Adatvezérelt rendszerek c. tárgy [jegyzetei](https://www.aut.bme.hu/Course/adatvezerelt) között

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! tip ""
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Markdown fájl megnyitása

A feladatok megoldása során a dokumentációt markdown formátumban készítsd. Az előbb letöltött git repository-t nyisd meg egy markdown kompatibilis szerkesztővel. Javasolt a Visual Studio Code használata:

1. Indítsd el a VS Code-ot.

1. A _File > Open Folder..._ menüvel nyisd meg a git repository könyvtárát.

1. A bal oldali fában keresd meg a `README.md` fájlt és dupla kattintással nyisd meg.

   - Ezt a fájlt szerkeszd.
   - Ha képet készítesz, azt is tedd a repository alá a többi fájl mellé. Így relatív elérési útvonallal (fájlnév) fogod tudni hivatkozni.

    !!! warning "Fájlnév: csupa kisbetű ékezet nélkül"
        A képek fájlnevében ne használj ékezetes karaktereket, szóközöket, se kis- és nagybetűket keverve. A különböző platformok és a git eltérően kezelik a fájlneveket. A GitHub webes felületén akkor fog minden rendben megjelenni, ha csak az angol ábécé kisbetűit használod a fájlnevekben.

1. A kényelmes szerkesztéshez nyisd meg az [előnézet funkciót](https://code.visualstudio.com/docs/languages/markdown#_markdown-preview) (_Ctrl-K + V_).

!!! note "Más szerkesztőeszköz"
    Ha nem szimpatikus ez a szerkesztő, használhatod a [GitHub webes felületét is](https://help.github.com/en/github/managing-files-in-a-repository/editing-files-in-your-repository) a dokumentáció szerkesztéséhez, itt is van előnézet. Ekkor a [fájlok feltöltése](https://help.github.com/en/github/managing-files-in-a-repository/adding-a-file-to-a-repository) kicsit körülményesebb lesz.

### Adatbázis létrehozása

1. Kapcsolódj Microsoft SQL Serverhez SQL Server Management Studio Segítségével. Indítsd el az alkalmazást, és az alábbi adatokkal kapcsolódj.

    - Server name: `(localdb)\mssqllocaldb` vagy `.\sqlexpress` (ezzel egyenértékű: `localhost\sqlexpress`)
    - Authentication: `Windows authentication`

1. Hozz létre egy új adatbázist (ha még nem létezik). Az adatbázis neve legyen a Neptun kódod: _Object Explorer_-ben Databases-en jobb kattintás, és _Create Database_.

1. Hozd létre a minta adatbázist a [generáló script](https://raw.githubusercontent.com/bmeviauac01/adatvezerelt/master/docs/db/mssql.sql) lefuttatásával. Nyiss egy új _Query_ ablakot, másold be a script tartalmát, és futtasd le. Ügyelj az eszköztáron levő legördülő menüben a megfelelő adatbázis kiválasztására.

    ![Adatbázis kiválasztása](../images/sql-management-database-dropdown.png)

1. Ellenőrizd, hogy létrejöttek-e a táblák. Ha a _Tables_ mappa ki volt már nyitva, akkor frissíteni kell.

    ![Adatbázis kiválasztása](../images/sql-managment-tablak.png).

### Lekérdezési terv bekapcsolása

!!! tip "Nem Windows platformon"
    A lekérdezési tervhez alapvetően Microsoft SQL Server Management Studio-t használunk. Ha nem Windows platformon dolgozol, kipróbálhatod az Azure Data Studio-t, ebben is [lekérdezhető a végrehajtási terv](https://richbenner.com/2019/02/azure-data-studio-execution-plans/).

Az összes feladat során szükségünk lesz a legjobb lekérdezési tervre, amit a szerver végeredményben választott. Ezt SQL Server Management Studio-ban a _Query_ menüben az [_Include Actual Execution Plan_ opcióval](https://docs.microsoft.com/en-us/sql/relational-databases/performance/display-an-actual-execution-plan) kapcsolhatjuk be.

![Lekérdezési terv bekapcsolása](../images/queryopt/queryopt-include-plan.png)

A tervet a lekérdezés lefuttatása után az ablak alján, az eredmények nézet helyett választható _Execution plan_ lapon találjuk.

![Lekérdezési terv bekapcsolása](../images/queryopt/queryopt-plan-result.png)

A lekérdezési terv diagramot adatfolyamként kell olvasnunk, az adat folyásának iránya a lekérdezés végrehajtása. Az egyes elemek a lekérdezési terv műveletei, a százalékos érték pedig az egész lekérdezéshez viszonyított relatív költség.
