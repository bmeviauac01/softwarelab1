# Feladat 3: Számla tábla denormalizálása (6 pont)

## Új oszlop

1. Módosítsd a `Szamla` táblát: vegyél fel egy új oszlopot `TetelSzam` néven a számlához tartozó összes tétel darabszámának tárolásához.

   > Az oszlopot hozzáadó kódot az `f3-oszlop.sql` fájlba írd bele. A fájlban csak egyetlen `alter table` utasítás szerepeljen, ne legyen benne se `[use]` se `go` utasítás.
   >
   > A feladatrésszel 1 pont szerezhető. Ez a feladat előkövetelménye a következőnek.

1. Készíts T-SQL programblokkot, amely kitölti az újonnan felvett oszlopot az aktuális adatok alapján.

   Ha például egy számlán megrendeltek 2 darab piros pöttyös labdát és 1 darab tollasütőt, akkor 3 tétel szerepel a számlán.

   > A kódot az `f3-kitolt.sql` fájlba írd. A fájlban csak egy T-SQL kódblokk legyen, ne használj tárolt eljárást vagy triggert, és ne legyen a kódban se `[use]` se `go` utasítás.
   >
   > A feladatrésszel 1 pont szerezhető.

## Oszlop karbantartása triggerrel

1. Készíts triggert `SzamlaTetelszamKarbantart` néven, amely a számla tartalmának változásával együtt karbantartja az előzőleg felvett tételszám mezőt. Ügyelj rá, hogy hatékony legyen a trigger! A teljes újraszámolás nem elfogadható megoldás.

   Tipp: A triggert a `SzamlaTetel` táblára kell készíteni, bár a változott érték a `Szamla` táblában lesz.

   > A kódot az `f3-trigger.sql` fájlba írd. A fájlban csak egyetlen `create trigger` utasítás legyen, és ne legyen a kódban se `[use]` se `go` utasítás.
   >
   > A feladatrész 4 pontot ér. Helyesség függvényében részpontszám is kapható.

1. Próbáld ki, jól működik-e a trigger. A teszteléshez használt utasításokat nem kell beadnod a megoldásban, de fontos, hogy ellenőrizd a viselkedést.

1. Készíts egy képernyőképet a trigger kódjáról. (A képernyőképpel kapcsolatos elvárásokról lásd a kezdőoldalon.)

   > A képet a megoldásban `f3-trigger.png` néven add be. A képernyőképen szerepeljen a trigger kódja, és látszódjon az _Object Explorer_-ben a trigger (a megfelelő tábla alatti mappákat kibontva).
   >
   > A képernyőkép szükséges feltétele a pontok megszerzésének.

## Következő feladat

Folytasd az [opcionális feladattal](Feladat-4.md) vagy add be a megoldásod az [itt](README.md#végezetül-a-megoldások-feltöltése) leírtak szerint.
