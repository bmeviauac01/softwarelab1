# MongoDB

A labor során a MongoDB NoSQL adatbáziskezelő rendszer és a Mongo C# driver használatát gyakorjuk komplexebb feladatokon keresztül.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows, Linux, vagy MacOS: Minden szükséges program platform független, vagy van platformfüggetlen alternatívája.
- MongoDB Community Server ([letöltés](https://www.mongodb.com/download-center/community))
- Robo 3T ([letöltés](https://robomongo.org/download))
- Minta adatbázis kódja ([mongo.js](https://raw.githubusercontent.com/bmeviauac01/gyakorlatok/master/mongo.js))
- Microsoft Visual Studio 2019 [az itt található beállításokkal](../VisualStudio.md)
    - Linux és MacOS esetén Visual Studio Code és a .NET Core SDK-val települő [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/) használható.
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)
    - Visual Studio esetén települ, de ha mégse, akkor a fenti linkről kell telepíteni (az SDK-t és _nem_ a runtime-ot.)
    - Linux és MacOS esetén telepíteni szükséges.
- GitHub account és egy git kliens

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- MongoDB adatbáziskezelő rendszer és a C# driver használata
    - Lásd az Adatvezérelt rendszerek c. tárgy [jegyzetei](https://www.aut.bme.hu/Course/adatvezerelt) és [gyakorlati anyagai](https://bmeviauac01.github.io/gyakorlatok/) között
- Hivatalos Microsoft tutorial [Mongo-t használó Web API készítéséhez](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio)
    - A labor során nem WebAPI-t készítünk, de a Mongo használat azonos formában törénik.

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub.md).

### Git repository létrehozása és letöltése

1. Az alábbi URL-en keresztül hozd létre a saját repository-dat: <TBD>

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! tip ""
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adatbázis létrehozása

Kövesd a [gyakorlatanyagban](https://bmeviauac01.github.io/gyakorlatok/Gyak-MongoDB/#feladat-0-adatb%C3%A1zis-l%C3%A9trehoz%C3%A1sa-projekt-megnyit%C3%A1sa) leírt utasításokat az adatbáziskezelő rendszer elindításához és az adatbázis létrehozásához.
