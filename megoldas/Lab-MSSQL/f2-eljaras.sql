create procedure SzamlaEllenoriz
    @szamlaazon int
as
begin

    declare @hiba int
    set @hiba = 0

    declare c cursor for
		select t.Nev, t.Mennyiseg, m.Mennyiseg
    from SzamlaTetel t
        left outer join MegrendelesTetel m on m.ID = t.MegrendelesTetelID
    where t.SzamlaID = @szamlaazon

    declare @termekNev nvarchar(255)
    declare @szamlaMennyiseg int, @megrendelesMennyiseg int
    open c
    fetch next from c into @termekNev, @szamlaMennyiseg, @megrendelesMennyiseg
    while @@FETCH_STATUS = 0
	begin

        if @szamlaMennyiseg != @megrendelesMennyiseg
        begin
            print 'Elteres ' + convert(varchar(50), @termekNev)
                        + ' (szamlan ' + convert(varchar(5), @szamlaMennyiseg)
                        + ' megrendelesen ' + convert(varchar(5), @megrendelesMennyiseg) +')'
            set @hiba = 1
        end

        fetch next from c into @termekNev, @szamlaMennyiseg, @megrendelesMennyiseg
    end

    close c
    deallocate c

    return @hiba

end