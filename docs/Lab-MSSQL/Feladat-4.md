# Feladat 4: Opcionális feladat

**A feladat megoldása 3 iMsc pontot ér.**

Kérdezd le a kategóriákat (`Category` tábla) úgy, hogy az alábbi eredményt kapjuk:

| Name           | Count | Rank |
| -------------- | ----- | ---- |
| Building items |     3 |    1 |
| Months 0-6     |     2 |    2 |
| DUPLO          |     1 |    3 |
| LEGO           |     1 |    4 |
| Months 18-24   |     1 |    5 |
| Months 6-18    |     1 |    6 |
| Play house     |     1 |    7 |

Az első oszlop a kategória neve, a második a kategóriában található termékek száma, a harmadik oszlop pedig a sorrend, ahol is a kategóriák a termék darabszámok szerint csökkenő sorrendben kell megjelenjenek, és ha egyezik a termék darabszám, akkor a kategória név alapján növekvő sorrendben. A sorrendezés folyamatos kell legyen, és a végeredmény a számított sorrend szerint növekvően kell érkezzen. A lekérdezést egyetlen utasítással kell megoldani. A lekérdezés eredményében az oszlop nevek egyezzenek meg a fenti példában látható nevekkel.

!!! tip "Tipp"
    A harmadik oszlop neve nem véletlen "rank".

!!! example "BEADANDÓ"
    A lekérdezést az `f4.sql` fájlba írd. A fájlban egyetlen `select` utasítás szerepeljen, és semmiképpen ne legyen benne se `[use]` se `go` utasítás.

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a lekérdezés eredményéről. A képet a megoldásban `f4.png` néven add be. A képernyőképen látszódjon az _Object Explorer_-ben az **adatbázis neve (a Neptun kódod)** és a **lekérdezés eredménye** is! A képernyőkép szükséges feltétele a pontok megszerzésének.
