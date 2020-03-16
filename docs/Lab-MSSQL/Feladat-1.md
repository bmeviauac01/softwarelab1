# Feladat 1: Kategória nézet és adatbeszúrás (8p)

## Nézet létrehozása

Hozz létre egy nézetet `KategoriaSzulovel` néven a `Kategoria` tábla kényelmesebb használatához. A nézetben két oszlop legyen: a kategória megnevezése, és a szülőkategória megnevezése, amennyiben létezik, vagy null.

1. Nyiss egy új _Query_ ablakot. Ügyelj rá, hogy a jó adatbázis legyen kiválasztva. Hozd létre a nézetet az alábbi utasítás lefuttatásával.

   ```sql
   create view KategoriaSzulovel
   as
   select k.Nev KategoriaNev, sz.Nev SzuloKategoriaNev
   from Kategoria k
   left outer join Kategoria sz on k.SzuloKategoria = sz.ID
   ```

1. Próbáld ki a nézetet: kérdezd le a nézet tartalmát!

   ![Nézet tartalmának listázása](../images/sql-management-query-view.png)

## Beszúrás a nézeten keresztül

Készíts triggert `KategoriaSzulovelBeszur` néven, ami lehetővé teszi új kategória beszúrását az előbb létrehozott nézeten keresztül (tehát a kategória nevét és a szülőkategória nevét megadva). Amennyiben a szülő kategória nem létezik, dobj hibát, és ne vedd fel az adatot a táblába.

A megoldásban egy _instead of_ típusú triggerre lesz szükségünk, mert ez ad lehetőséget a nézeten keresztül történő adatbeszúrásra. A trigger vázát láthatod alább.

```sql
create trigger KategoriaSzulovelBeszur -- a trigger neve
on KategoriaSzulovel -- a nézet neve
instead of insert    -- beszúrás helyetti trigger
as
begin
  declare @ujnev nvarchar(255) -- a kapott adatok
  declare @szulonev nvarchar(255)

  -- kurzort használunk, mert az inserted implicit táblában
  -- több elem is lehet, és mi egyesével dolgozzuk fel
  declare ic cursor for select * from inserted
  open ic
  -- standard kurzor használat
  fetch next from ic into @ujnev, @szulonev
  while @@FETCH_STATUS = 0
  begin
    -- ide jön a változóban elérhető adatok ellenőrzése
    -- ha hiba van, akkor dobjunk hibát
    -- ha minden rendben van, akkor jöhet beszúrás
    fetch next from ic into @ujnev, @szulonev
  end

  close ic -- kurzor lezárása és felszabadítása
  deallocate ic
end
```

1. Egészítsd ki a trigger vázat a ciklusban.

   1. Ha érkezik szülő kategória név, ellenőrizd, hogy létezik-e kategória olyan névvel, mint ami a `@szulonev` változóban van.

   1. Ha nincs ilyen, akkor dobj hibát, amivel leáll a trigger futása.

   1. Ha minden rendben van, akkor szúrd be az új adatokat a `Kategoria` táblába (ne a nézetbe, hiszen a nézet nem írható, pont ezért készül a trigger).

   > A trigger kódját az `f1-trigger.sql` fájlba írd. A fájlban csak ez az egyetlen `create trigger` utasítás legyen!
   >
   > A feladat 4 pontot ér.

1. Próbáld ki, jól működik-e a trigger! Írj egy olyan beszúró utasítást, amely sikeresen felvesz egy új elemet, és egy olyan teszt utasítást, amely során nem sikerül a beszúrás.

   A helyes és helytelen viselkedéshez feltételezheted, hogy az adatbázis a kiinduló állapotban van: olyan kategória rekordok léteznek csupán, amelyeket a létrehozó script beszúrt. (A két teszt _ne_ épüljön egymásra! Mindkettő az elvárt eredményt adja attól függetlenül, hogy a másik lefutott-e már!)

   Érdemes olyan tesz adatokat választani, amiben nincs ékezet! Különben problémákat okozhat az sql fájl kódolásának helytelen beállítása. Hogy ezt elkerüljük, használható például a _LEGO_, mint szülő kategória név.

   > A teszt utasításokat az `f1-trigger-teszt-ok.sql` és `f1-trigger-teszt-hiba.sql` fájlokba írd. Mindkét fájlban csak egyetlen `insert` utasítás legyen! Semmiképpen ne legyen `[use]`, se `go` utasítás bennük!
   >
   > Mindkét utasítás 2-2 pont.

1. Készíts egy képernyőképet a trigger kódjáról. A képernyőképpel kapcsolatos elvárásokat lásd [itt](../README.md#képernyőképek).

   > A képet a megoldásban `f1.png` néven add be. A képernyőképen szerepeljen a trigger kódja, és látszódjon az _Object Explorer_-ben a trigger (a nézet alatti mappákat kibontva).
   >
   > A képernyőkép szükséges feltétele a pontszám megszerzésének.

## Következő feladat

Folytasd a [következő feladattal](Feladat-2.md).
