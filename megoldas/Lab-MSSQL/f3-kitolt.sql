update Szamla
set TetelSzam = (select sum(Mennyiseg)
from SzamlaTetel
where SzamlaTetel.SzamlaID = Szamla.ID)