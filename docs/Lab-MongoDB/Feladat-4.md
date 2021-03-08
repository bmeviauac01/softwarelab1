# Feladat 4: Vevők listázása

**A feladat megoldásával 4 pont szerezhető.**

Ebben a feladatban a vevőket fogjuk listázni — az általuk megrendelt termékek összértékével együtt. Ehhez a második feladathoz hasonlatosan aggregációs utasítást és C# kódban történő "összefésülést" kell majd használnunk.

A megvalósítandó metódus az `IList<Customer> ListCustomers()`. Ennek minden vevőt vissza kell adnia. A `Models.Customer` entitás adattagjai:

- `Name`: a vevő neve
- `ZipCode`, `City`, `Street`: a vevő központi telephelyéhez tartozó cím
- `TotalOrders`: a vevőhöz tartozó összes megrendelés összértékének (lásd előző feladat) összege. Amennyiben még nincs az adott vevőhöz tartozó megrendelés, akkor ennek az értéke legyen `null`!

A feladat megoldásához ajánlott lépések:

1. Vedd fel és inicializáld a `customerCollection`-t!

1. Listázd ki az összes vevőt. A vevő entitásban megtalálod a telephelyek listáját (`Sites`) és a központi telephely azonosítóját (`MainSiteID`) is. Ez utóbbit kell megkeresned az előbbi listában, hogy megkapd a központi telephelyet (és így a hozzá tartozó címet).

1. A megrendelések kollekcióján aggregációs pipeline-t használva meg tudod állapítani az adott `CustomerID`-hez tartozó összmegrendelések értékét.

1. Végezetül csak a meglevő információkat kell "összefésülnöd". Központi telephelye minden vevőnek van, viszont megrendelése nem garantált — figyelj oda, hogy ekkor a `TotalOrders` elvárt értéke `null`!

1. A kód kipróbálásához a weboldal `Customers` menüpontjára kell navigálni. Itt táblázatos formában megjelenítve láthatod az összegyűjtött információkat. Teszteléshez alkalmazhatod az előző feladatban elkészített `Add new order` funkciót — itt új megrendelés felvétele esetén az előző feladatban bedrótozott vevő mellett növekednie kell a hozzá tartozó megrendelések összértékének.

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** a vevő listázó weboldalról. A képet a megoldásban `f4.png` néven add be. A képernyőképen látszódjon a **vevők listája**. Ellenőrizd, hogy a **Neptun kódod** (az oldal aljáról) látható-e a képen! A képernyőkép szükséges feltétele a pontszám megszerzésének.
