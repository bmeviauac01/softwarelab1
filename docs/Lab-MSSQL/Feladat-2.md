# Feladat 2: Számla ellenőrzése

**A feladat megoldása 6 pontot ér.**

## Tárolt eljárás

Írj tárolt eljárást `CheckInvoice` néven, aminek bemeneti paramétere egy `int` típusú és `@invoiceid` nevű számlaazonosító.

- Az eljárás ellenőrizze, hogy a paraméterben kapott számlához (`Invoice`) kapcsolódó számlatételeken (`InvoiceItem`) szereplő mennyiség (`Amount`) egyezik-e a kapcsolódó megrendeléstétel (`OrderItem`) mennyiségével. (Az `InvoiceItem` rekord hivatkozik a kapcsolódó `OrderItem`-re.)
- Amennyiben eltérés található a kettőben, úgy mindkettő értékét, valamint a termék nevét írd ki a standard outputra az alábbi séma szerint: `Difference: Ball (invoice 5 order 6)`
- Csak akkor írjon bármit a kimenetre a tárolt eljárás, ha problémát talált. Semmiképpen se hagyj teszteléshez használt kiírást az eljárásban!
- Legyen az eljárás `int` típusú visszatérési értéke 0, ha nem kellett semmit kiírni a kimenetre, és 1, ha kellett. Ez az érték `return` kulcsszóval kerüljön visszaadásra (ne `output` paraméter legyen).

A kiíráshoz használd a `print` parancsot: `PRINT 'Szoveg' + @valtozo + 'Szoveg'` Ügyelj rá, hogy a változónak char típusúnak kell lennie, egyéb típus, pl. szám konvertálása: `convert(varchar(5), @valtozo)`, pl. `PRINT 'Szoveg' + convert(varchar(5), @valtozo)`

!!! example "BEADANDÓ"
    Az eljárás kódját az `f2-proc.sql` fájlba írd. A fájlban csak ez az egyetlen `create proc` utasítás legyen! A feladat helyes megoldása 4 pontot ér. Helyesség függvényében részpontszám is kapható.

## Minden számla ellenőrzése

Írj T-SQL kódblokkot, ami az előző feladatban megírt eljárást hívja meg egyenként az összes számlára. Érdemes ehhez egy kurzort használnod, ami a számlákon fut végig.

A kód minden számla ellenőrzése előtt írja ki a standard outputra a számla azonosítóját (pl. `InvoiceID: 12`), és amennyiben nem volt eltérés, írja ki a kimenetre, hogy 'Invoice ok'. Ezt a kimenetet a query ablak alatti [_Output_ panelen](https://docs.microsoft.com/en-us/sql/ssms/scripting/transact-sql-debugger-output-window) fogod látni.

!!! tip "Tárolt eljárás meghívása"
    Tárolt eljárás meghívása kódból az `exec` paranccsal lehetséges, pl.

    ```sql
    declare @checkresult int
    exec @checkresult = CheckInvoice 123
    ```
Ellenőrizd a kódblokk viselkedését. Ahhoz, hogy eltérést tapasztalj, ha szükséges, változtass meg egy megrendelés tételt vagy számla tételt az adatbázisban. (Az ellenőrzéshez tartozó kódot nem kell beadni.)

!!! example "BEADANDÓ"
    Az összes számlát ellenőrző kódot az `f2-exec.sql` fájlba írd. A fájlban csak a lefuttatható kódblokk szerepeljen. Ne legyen benne a tárolt eljárás kódja, ne legyen benne se `[use]` se `go` utasítás. A feladatrésszel 2 pont szerezhető.

!!! example "BEADANDÓ"
    Készíts egy képernyőképet az kód futásának eredményéről, amikor valamilyen **eltérést megtalált**. A képet `f2.png` néven add be. A képernyőképen látszódjon az _Object Explorer_-ben az **adatbázis neve (a Neptun kódod)** és **a kiírt üzenet** is!
