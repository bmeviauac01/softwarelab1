# Feladat 3: Számla tábla denormalizálása

**A feladat megoldása 6 pontot ér.**

## Új oszlop

Módosítsd az `Invoice` táblát: vegyél fel egy új oszlopot `ItemCount` néven a számlához tartozó összes tétel (`InvoiceItem`) darabszámának tárolásához.

!!! example "BEADANDÓ"
    Az oszlopot hozzáadó kódot az `f3-column.sql` fájlba írd bele. A fájlban csak egyetlen `alter table` utasítás szerepeljen, ne legyen benne se `[use]` se `go` utasítás. A feladatrésszel 1 pont szerezhető. Ez a feladat előkövetelménye a következőnek.

Készíts T-SQL programblokkot, amely kitölti az újonnan felvett oszlopot az aktuális adatok alapján.

!!! tip ""
    Ha például egy **számlán** (`Invoice`) szerepel 2 darab piros pöttyös labda és 1 darab tollasütőt, akkor 3 tétel szerepel a számlán. Ügyelj rá, hogy számlához (és nem a megrendeléshez) tartozó tételeket kell nézni!

!!! example "BEADANDÓ"
    Az oszlopot kitöltő kódot az `f3-fill.sql` fájlba írd. A fájlban csak egy T-SQL kódblokk legyen, ne használj tárolt eljárást vagy triggert, és ne legyen a kódban se `[use]` se `go` utasítás. A feladatrésszel 1 pont szerezhető.

## Oszlop karbantartása triggerrel

Készíts triggert `InvoiceItemCountMaintenance` néven, amely a számla tartalmának változásával együtt karbantartja az előzőleg felvett tételszám mezőt. Ügyelj rá, hogy hatékony legyen a trigger! A teljes újraszámolás nem elfogadható megoldás.

!!! tip "Tipp"
    A triggert a `InvoiceItem` táblára kell készíteni, bár a frissítendő érték az `Invoice` táblában van.

!!! warning "Fontos"
    Ne felejtsd, hogy a trigger **utasítás szintű**, azaz nem soronként fut le, így pontenciálisan több változás is lehet az implicit táblákban! Az `inserted` és `deleted` **tábla**, és mindenképpen ennek megfelelően kell őket kezelni.

!!! example "BEADANDÓ"
    A kódot az `f3-trigger.sql` fájlba írd. A fájlban csak egyetlen `create trigger` utasítás legyen, és ne legyen a kódban se `[use]` se `go` utasítás. A feladatrész 4 pontot ér. Helyesség függvényében részpontszám is kapható.

Próbáld ki, jól működik-e a trigger. A teszteléshez használt utasításokat nem kell beadnod a megoldásban, de fontos, hogy ellenőrizd a viselkedést. Ellenőrizd olyan utasítással is a trigger működését, amely egyszerre több rekordot érint (pl. `where` feltétel nélküli `update` a tábla teljes tartalmára)!

!!! example "BEADANDÓ"
    Készíts egy képernyőképet, amelyen látható a kitöltött `ItemCount` oszlop (az `Invoice` tábla tartalmával együtt). A képet a megoldásban `f3.png` néven add be. A képernyőképen látszódjon az _Object Explorer_-ben az **adatbázis neve (a Neptun kódod)** és az **`Invoice` tábla tartalma** is! A képernyőkép szükséges feltétele a pontok megszerzésének.
