# Opcionális feladatok

**A feladat megoldásával 1+2 iMsc pont szerezhető.**

## Feladat 16

Elemezd a következő `Invoice`-`InvoiceItem` táblákból történő lekérdezés végrehajtási tervét: minden számlatétel névhez kérdezzük le a megrendelő nevét.

```sql
SELECT CustomerName, Name
FROM Invoice JOIN InvoiceItem ON Invoice.ID = InvoiceItem.InvoiceID
```

Mutasd meg, milyen _join_ stratégiát választott a rendszer. Adj magyarázatot, miért választhatta ezt!

## Feladat 17

Hasonlítsd össze a különböző JOIN stratégiák költségét a következő lekérdezés esetében: összetartozó `Product`-`Category` rekordpárok lekérdezése.

!!! tip "Tipp"
    Használj [query hinteket](https://docs.microsoft.com/en-us/sql/t-sql/queries/hints-transact-sql-join) vagy az [option parancsot](https://docs.microsoft.com/en-us/sql/t-sql/queries/option-clause-transact-sql) a join stratégia kiválasztásához.

    Tedd a 3 lekérdezést (3 fajta join stratégia) egy végrehajtási egységbe (egyszerre futtasd őket). Így látni fogod az egymáshoz viszonyított költségüket.

Add meg a használt SQL utasításokat, majd vizsgáld meg a lekérdezési terveket, és magyarázd el a látottakat!
