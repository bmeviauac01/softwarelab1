create trigger KategoriaSzulovelBeszur
on KategoriaSzulovel
instead of insert
as
begin
    declare @ujnev nvarchar(255)
    declare @szulonev nvarchar(255)
    declare ic cursor for select *
    from inserted
    open ic

    fetch next from ic into @ujnev, @szulonev
    while @@FETCH_STATUS = 0
	begin

        declare @szuloid int

        if @szulonev is not null
		begin
            select @szuloid = ID
            from Kategoria
            where Nev = @szulonev
            if @szuloid is null
				throw 51000, 'Szulo nev nem letezik', 1;
        end

        insert into Kategoria
            (Nev,SzuloKategoria)
        values(@ujnev, @szuloid)

        fetch next from ic into @ujnev, @szulonev
    end

    close ic
    deallocate ic
end
