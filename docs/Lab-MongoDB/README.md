# MongoDB laboratórium

## Célkitűzés

A MongoDB NoSQL adatbáziskezelő rendszer és a Mongo C# driver használatának gyakorlása komplexebb feladatokon keresztül.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- MongoDB Community Server ([letöltés](https://www.mongodb.com/download-center/community))
- Robo 3T ([letöltés](https://robomongo.org/download))
- Gyakorlatokon is használt minta adatbázis kódja ([mongo.js](https://raw.githubusercontent.com/bmeviauac01/gyakorlatok/master/mongo.js)).
- Microsoft Visual Studio 2017/2019 [az itt található beállításokkal](../VisualStudio-install.md)

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- MongoDB adatbáziskezelő rendszer és a C# driver használata
  - Lásd az Adatvezérelt rendszerek c. tárgy [jegyzetei](https://www.aut.bme.hu/Course/adatvezerelt) és [gyakorlati anyagai](https://bmeviauac01.github.io/gyakorlatok/) között

## Előkészület

A feladatok megoldása során ne felejtsd el követni a [feladat beadás folyamatát](../GitHub-hasznalat.md).

### Git repository létrehozása és letöltése

1. Az alábbi URL-en keresztül hozd létre a saját repository-dat: TBD

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

   > Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd a kezdőoldalon a leírást), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz.

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Adatbázis létrehozása

Kövesd a [gyakorlatanyagban](https://bmeviauac01.github.io/gyakorlatok/Gyak4-MongoDB/#feladat-0-adatb%C3%A1zis-l%C3%A9trehoz%C3%A1sa-projekt-megnyit%C3%A1sa) leírt utasításokat az adatbáziskezelő rendszer elindításához és az adatbázis létrehozásához.

## Feladatok

Összesen 4 feladat van. [Itt kezdd](Feladat-1.md) az első feladattal.

## Végezetül: a megoldások feltöltése

1. Ellenőrizd, hogy a `neptun.txt`-ben szerepel-e a Neptun kódod.
1. Nézd meg, hogy a megoldásaidat a feladatban meghatározott fájlnévvel mentetted-e el. Minden fájl már ott volt a letöltött repository-ban, neked csak felül kellett őket írnod.
1. Ellenőrizd, hogy a kért képernyőképeket elkészítetted-e.
1. Kommitold és pushold a megoldásod. Ellenőrizd a GitHub webes felületén, hogy minden rendben van-e.
1. Nyiss egy pull request-et a megoldásoddal és rendeld a laborvezetődhöz.
1. Ha tanszéki laborban vagy, töröld a GitHub belépési adatokat. A lépésekhez nézd meg a kezdőoldalon a leírást.
