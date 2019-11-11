# Feladat 4: Opcionális feladat 3 iMsc pontért

A termék kategória rendszerünk többszintű, de szeretnénk egyszerűsíteni, egy szintűvé tenni. Írj T-SQL kódblokkot, amely minden kategóriát, amely nem legfelső szintű (amelynek van `SzuloKategoria` értéke), töröl, de előbb az ezen kategóriához tartozó termékeket átsorolja a szülő kategóriába.

Tipp: Mivel a kategória rendszer hierarchikus, ezt többször is meg kell ismételni, amíg még van alkategória. Ügyelj rá, hogy a kategória hierarchia aljáról kell indulni, azaz minden iterációban azon kategóriákat kell megszüntetni, amelynek van szülője, de amely maga nem szülő.

A megoldásod egyetlen T-SQL kódblokk legyen. Ne használj tárolt eljárást vagy triggert. (A feleslegessé vált `SzuloKategoria` oszlopot **nem** kell törölni.)

> A megoldást az `f4.sql` fájlba írd. A fájlban csak a lefuttatható kódblokk szerepeljen. Ne legyen benne se `[use]` se `go` utasítás.

Készíts egy képernyőképet a kódblokkal.

> A képet a megoldásban `f4.png` néven add be. A képernyőképen szerepeljen az T-SQL kód.
>
> A képernyőkép szükséges feltétele a pontok megszerzésének.

## Következő feladat

Nincs több feladat. Add be a megoldásod az [itt](README.md) leírtak szerint.
