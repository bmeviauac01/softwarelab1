create trigger SzamlaTetelszamKarbantart
on SzamlaTetel
after insert, update, delete
as
begin

    update Szamla
set TetelSzam = TetelSzam + Valtozas
from Szamla
        inner join
        (select SzamlaID, sum(Mennyiseg) as Valtozas
        from inserted
        group by SzamlaID) SzamlatetelValtozas
        on Szamla.ID = SzamlatetelValtozas.SzamlaID

    update Szamla
set TetelSzam = TetelSzam - Valtozas
from Szamla
        inner join
        (select SzamlaID, sum(Mennyiseg) as Valtozas
        from deleted
        group by SzamlaID) SzamlatetelValtozas
        on Szamla.ID = SzamlatetelValtozas.SzamlaID

end
