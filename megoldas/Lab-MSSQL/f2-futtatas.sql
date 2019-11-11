declare @szamlaid int
declare szc cursor for select ID
from Szamla
open szc
fetch next from szc into @szamlaid
while @@FETCH_STATUS = 0
begin

    print 'Szamla: ' + convert(varchar(5), @szamlaid)
    declare @eredmeny int
    exec @eredmeny = SzamlaEllenoriz @szamlaid
    if @eredmeny = 0
		print 'Helyes szamla'

    fetch next from szc into @szamlaid
end
close szc
deallocate szc