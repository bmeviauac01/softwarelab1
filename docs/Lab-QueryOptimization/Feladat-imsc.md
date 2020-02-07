# Opcionális feladatok (3 iMsc pont)

## Feladat 16

Elemezd a következő Szamla-SzamlaTetel táblákból történő lekérdezés végrehajtási tervét: minden számlatétel névhez kérdezzük le a megrendelő nevét.

```sql
SELECT MegrendeloNev, Nev
FROM SZAMLA JOIN SzamlaTetel ON Szamla.ID = SzamlaTetel.SzamlaID
```

Mutasd meg, milyen join stratégiát választott a rendszer. Adj magyarázatot, miért választhatta ezt!

## Feladat 17

Hasonlítsd össze a különböző JOIN stratégiák költségét a következő lekérdezés esetében: összetartozó termék-kategória rekordpárok lekérdezése.

> Tipp 1: használj [query hinteket](https://docs.microsoft.com/en-us/sql/t-sql/queries/hints-transact-sql-join) vagy az [option parancsot](https://docs.microsoft.com/en-us/sql/t-sql/queries/option-clause-transact-sql) a join stratégia kiválasztásához.

> Tipp 2: tedd a 3 lekérdezést (3 fajta join stratégia) egy végrehajtási egysége (egyszerre futtasd az összeset). Így látni fogod az egymáshoz viszonyított költségüket.

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és magyarázd el a látottakat!

## Következő feladat

Nincs több feladat. Add be a megoldásod az [itt](README.md#végezetül-a-megoldások-feltöltése) leírtak szerint.
