# Feladat 4: Opcionális feladat

**A feladat megoldása 3 iMsc pontot ér.**

A termék kategória rendszerünk (`Category` tábla) többszintű, de szeretnénk egyszerűsíteni, egy szintűvé tenni.

Írj T-SQL kódblokkot, amely minden kategóriát, amely nem legfelső szintű (amelynek van `ParentCategoryId` értéke), töröl, de előbb az ezen kategóriához tartozó termékeket átsorolja a szülő kategóriába. Mivel a kategória rendszer hierarchikus, ezt többször is meg kell ismételni, amíg még van alkategória. Ügyelj rá, hogy a kategória hierarchia aljáról kell indulni, azaz minden iterációban azon kategóriákat kell megszüntetni, amelynek van szülője, de amely maga nem szülő.

!!! example "BEADANDÓ"
    A kódblokkot az `f4.sql` fájlba írd. A fájlban csak a lefuttatható kódblokk szerepeljen. Ne legyen benne se `[use]` se `go` utasítás. A megoldásod egyetlen T-SQL kódblokk legyen. Ne használj tárolt eljárást vagy triggert. (A feleslegessé vált `ParentCategoryId` oszlopot **nem** kell törölni.)

!!! example "BEADANDÓ"
    Készíts egy képernyőképet a a lefutás eredményeképpen előálló `Category` tábla tartalmáról. A képet a megoldásban `f4.png` néven add be. A képernyőképen látszódjon az _Object Explorer_-ben az **adatbázis neve (a Neptun kódod)** és a **`Category` tábla tartalma** is! A képernyőkép szükséges feltétele a pontok megszerzésének.
