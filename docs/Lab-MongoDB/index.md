# MongoDB

A labor során a MongoDB NoSQL adatbáziskezelő rendszer és a Mongo C# driver használatát gyakorjuk komplexebb feladatokon keresztül.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows, Linux, vagy MacOS: Minden szükséges program platform független, vagy van platformfüggetlen alternatívája.
- MongoDB Community Server ([letöltés](https://www.mongodb.com/download-center/community))
- Robo 3T ([letöltés](https://robomongo.org/download))
- Minta adatbázis kódja ([mongo.js](https://bmeviauac01.github.io/adatvezerelt/db/mongo.js))
- GitHub account és egy git kliens
- Microsoft Visual Studio 2019/2022 [az itt található beállításokkal](../VisualStudio.md)
    - Linux és MacOS esetén Visual Studio Code és a .NET Core SDK-val települő [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/) használható.
- [.NET Core **3.1** SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)

    !!! warning ".NET Core 3.1"
        A feladat megoldásához **3.1**-es .NET Core SDK telepítése szükséges.

        Windows-on Visual Studio verzió függvényében lehet, hogy telepítve van (lásd [itt](../VisualStudio.md#net-core-sdk-ellenorzese-es-telepitese) az ellenőrzés módját); ha nem, akkor a fenti linkről kell telepíteni (az SDK-t és _nem_ a runtime-ot.) Linux és MacOS esetén telepíteni szükséges.

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- MongoDB adatbáziskezelő rendszer és a C# driver használata
    - Lásd az Adatvezérelt rendszerek c. tárgy jegyzetei és [gyakorlati anyagai](https://bmeviauac01.github.io/adatvezerelt/) között
- Hivatalos Microsoft tutorial [Mongo-t használó Web API készítéséhez](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio)
    - A labor során nem WebAPI-t készítünk, de a Mongo használat azonos formában történik.

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! tip ""
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adatbázis létrehozása

Kövesd a [gyakorlatanyagban](https://bmeviauac01.github.io/adatvezerelt/gyakorlat/mongodb/#feladat-0-adatbazis-letrehozasa-projekt-megnyitasa) leírt utasításokat az adatbáziskezelő rendszer elindításához és az adatbázis létrehozásához.
