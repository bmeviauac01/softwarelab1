# BMEVIAUAC09 Szoftverfejlesztés laboratórium 1 - labor feladatok

## Laborok feladatai

[Microsoft SQL Server Reporting Services](Lab-ReportingServices/README.md) (laborban teljesített, részben vezetett feladat)

[Lekérdezés optimalizálás](Lab-QueryOptimization/README.md) (laborban teljesített, részben vezetett feladat)

[Microsoft SQL Server platform programozása](Lab-MSSQL/README.md) (otthoni feladat)

[MongoDB programozása](Lab-MongoDB/README.md) (otthoni feladat)

Entity Framework és REST API (otthoni feladat)

## Laborok megoldásainak beadása

Minden labor megoldását egy személyre szóló git repository-ban kell beadni. Ennek pontos [folyamatát lásd itt](GitHub-hasznalat.md). Kérünk, hogy alaposan olvasd végig a leírást!

> **FONTOS** A beadás menetét az első laboron személyesen is megmutatjuk. A továbbiakban az itt leírtak szerint **kell** eljárnod minden labor megoldásával. A nem ilyen formában beadott megoldásokat nem értékeljük.

## Képernyőképek

Egyes laborok kérik, hogy készíts képernyőképet a megoldás egy-egy részéről. Ez különösen akkor fontos, ha a feladatot otthon készíted el, mert ezzel bizonyítod, hogy a megoldásod saját magad készítetted. Amikor képernyőképet kérünk, az alábbiakra figyelj:

- Látszódjon a fejlesztéséhez használt eszköz (pl. SQL Server Management Studio, Visual Studio). Profi fejlesztők vagyunk, így mindig a megfelelő eszköz használjuk - jegyzettömbben nem lehet fejleszteni!
- Látszódjon a gép és a felhasználó neve, amin a fejlesztést végezted (pl. SQL Server Management Studio-ban az Object Explorer-ben a megnyitott kapcsolat nevében szerepel, vagy konzolban add ki a `whoami` parancsot és ezt a konzolt is rakd a képernyőképre),
- Az aktuális dátum (pl. az óra a tálcán).
- Illetve a feladat tartalma, amit a feladat pontosan megnevez.

A képernyőképeket a megoldás részeként kell beadni, így felkerülnek a git repository tartalmával együtt. Mivel a repository privát, azt az oktatókon kívül más nem látja. Ha mégis szeretnéd, a fent nem jelölt részek mellett kitakarhatod, ha olyan információ kerül a képernyőképre, amit nem szeretnél feltölteni.

Két példa erre: [SQL kód](images/img-screenshot-pl-sql.png) és [Visual Studio-ban C# kód](images/img-screenshot-pl-vs.png). Körülbelül ilyesmit várunk.

## A feladatok kiértékelése

A feladatok kiértékelése egyes laborok esetén részben **automatikusan** történik. A futtatható kódokat valóban le fogjuk futtatni, ezért minden esetben fontos a feladatleírások pontos követése (kiinduló kód váz használata, csak a megengedett fájlok változtatása, stb.)!

## Értékelés

Minden laboron 20 pont szerezhető. A teljesítés feltétele a határidőig történő beadás. A labor sikeres teljesítéséhez minimum 8 pont szükséges.

A labor eredményének osztályzása:

- 0-7.5 pont: elégtelen
- 8-10.5 pont: elégséges
- 11-13.5 pont: közepes
- 14-16.5 pont: jó
- 17-20 pont: jeles

Az iMsc feladat megoldása opcionális, a többi feladat értékelésébe nem számít bele. Egy iMsc feladat sikeres megoldásával 3 iMsc pont szerezhető. A pont csak jeles eredmény esetén kapható meg.

A félév végi jegy a laborokra kapott osztályzatok matematikailag kerekített számtani átlaga. Hiányzás, nem pótolt labor, határidőig nem leadott megoldás elégtelennel számítódik az átlagba.

## Egyetemi laborokban: GitHub belépési adatok törlése

Az egyetemi laborokban a gépek megjegyzik a GitHub belépési adatokat. Ezt a munka végeztével kézzel kell törölni.

1. Nyisd meg a `Credential Manager`-t a Start menüből.
1. A `Windows Credentials` oldalon keresd meg a GitHubra mutató bejegyzéseket, és töröld őket.
   ![GitHub belépési adat törlése](images/git-credential-remove.png)

## Elvárásaink a munkával kapcsolatban

**Egyéni munka? Otthoni munka?** Mivel a laborokra jegyet kapsz, elvárás, hogy mindenki saját megoldást készítsen el és adjon be. Ez nem zárja ki az egymásnak nyújtott segítséget. Kizárja viszont más megoldásának lemásolását. Ezért kérjük a képernyőképeket, mert így a munka folyamatával bizonyítod a megoldás saját elkészítését.

**Más munkájának lemásolása:** A BME etikai kódexe és a TVSZ szabályozza. Komolyan vesszük.

**Egy labor csak 4 óra, nem?** Nem. A tárgy 3 kredit, amely a félév során megközelítőleg 90 munkaóra befektetését igényli. A labor tehát nem csak a teremben eltöltött 4 óra, hanem az előzetes felkészülés és a feladat befejezése / otthoni elvégzése is.

**Egy apró elírás miatt nem működött a kódom, és nem értékeltétek.** A laborok során működő programot, kódot, kódrészletet kell készíteni. Azért számítógép laborban vagy otthon készítjük a feladatot, mert így tudod magad ellenőrizni. Minimum elvárás, hogy a beadott kód leforduljon, lefusson. Ha a viselkedés nem teljesen helyes, azt értékeljük. De ha egyáltalán nem működik, nem értékeljük a megoldást.

Azért így teszünk, mert mérnökként a feladatod a problémák _megoldása_ lesz, és nem csak egy _kísérlet_ a megoldásra. Mit gondolsz, ha a munkahelyeden a főnöködnek átadsz egy nem forduló kódot, mit fog tenni?

**Ha otthonról készítem el a megoldást, hogyan kapok segítséget?** Akár otthonról dolgozol, akár egyetemi laborban, egy laborvezetőhöz tartozol. Ő felel nem csak a kontaktóra megtartásáért, hanem azért is, hogy a félév közben a feladatok beadása és ellenőrzése rendben történjen.

**A laborvezető nem válaszol egy emailre, megkeresésre. Mit tegyek?** Adj neki időt. A kommunikáció aszinkron, órákban, akár napokban mérhető. Az oktatóknak is van munkaideje, ne várd el, hogy mindig elérhető legyen (főleg este és hétvégén ne). Ha a leadási határidő előtt egy órával kérdezel, bizony lehet, hogy nem kapsz választ időben.

**Bent vagyok az egyetemi laborban, mégse segít a laborvezető. Miért?** Dehogynem segít. Viszont ha egyből megmondaná a megoldást, csak azt tanulnád meg, hogy legközelebb is meg kell kérdezni. Próbáld magad megoldani, mutass alternatívákat, kérdezz konkrétan. Mutasd meg, hogy professzionális a hozzáállásod.

**Akkor mit kérdezhetek meg a laborvezetőtől?** Röviden: <https://stackoverflow.com/help/how-to-ask>. Hosszabban: Ha valamivel elakadsz, értsd meg a problémát. A probléma nem az, hogy "nem működik" vagy "nem tudom, hogyan csináljam". Akkor tudsz jól kérdezni, ha már körüljártad a problémát, és azt is meg tudod mutatni, mivel próbálkoztál már.

**Szóval Google és StackOverflow a megoldás?** Nem. Minden tudás, amire szükséged van, már előfordult egyetemi tanulmányaid során. A Google jó, a StackOverflow még jobb.... De! A választ is meg kell érteni. Lehet, hogy a megtalált válasz megoldás, csak épp nem a te problémádra.

**Sok a határidő, meg az előírás.** Ez nézőpont kérdése. A mérnök nem csak programozni tud, hanem meghatározott keretek között dolgozni. Mert a világ bonyolult, és a bonyolultságot szabályokkal lehet kordában tartani. Ha időd engedi, érdemes megnézni, mint mond Robert C. Martin (Bob Martin, "Uncle Bob") arról, honnan származik a szoftverfejlesztői szakmai: <https://www.youtube.com/watch?v=ecIWPzGEbFc>

---

Az itt található oktatási segédanyagok a BMEVIAUAC09 tárgy hallgatóinak készültek. Az anyagok oly módú felhasználása, amely a tárgy oktatásához nem szorosan kapcsolódik, csak a szerző(k) és a forrás megjelölésével történhet.

Az anyagok a tárgy keretében oktatott kontextusban értelmezhetőek. Az anyagokért egyéb felhasználás esetén a szerző(k) felelősséget nem vállalnak.
