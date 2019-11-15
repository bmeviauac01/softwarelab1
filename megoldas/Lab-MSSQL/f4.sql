DECLARE @Alkategoria INT=1
--A kezdeti érték mindegy, de legyen nagyobb 0-nál

--addig fusson, amíg tudtunk terméket szülõkategóriába feljebb vinni
WHILE @Alkategoria>0
BEGIN
    --Termékek frissítése
    UPDATE Termek
	SET KategoriaID=KategoriaSzulovelTabla.SzuloKategoria FROM
        (SELECT k.ID, k.SzuloKategoria
        FROM Kategoria k
        WHERE k.SzuloKategoria IS NOT NULL) KategoriaSzulovelTabla
		WHERE KategoriaID=KategoriaSzulovelTabla.ID

    --Hány rekord módosult?
    SET @Alkategoria=@@ROWCOUNT
    IF (@Alkategoria=0)
		BREAK

END

--Itt már biztonságosan törölhetõ minden kategória, ami nem szülõ
DELETE FROM Kategoria
FROM Kategoria k
WHERE k.SzuloKategoria IS NOT NULL
