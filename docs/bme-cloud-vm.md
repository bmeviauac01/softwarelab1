# BME Cloud használata

A feladatok megoldásához a szükséges szoftverek feltelepítése mellett lehetőség van a BME Cloud igénybevételére is. Alapvetően javasoljuk, hogy a saját gépeden dolgozz, mert az általában kényelmesebb és gyorsabb. Ha ez nem oldható meg, akkor használhatod a BME Cloud-ot.

Nyisd meg a <https://cloud.bme.hu> oldalt és jelentkezz be EduID-val. A "Smallville" nevű adatközpontban keresd meg a "Data-driven systems" nevű template-et. Ezt a template-et példányosítva elindul egy virtuális gép, amin minden szükséges szoftver telepítve van.

A virtuális gép eléréséhez RDP kliensre van szükség.

- Windows-ban a beépített _Remote Desktop Connection_ a legkényelmesebb,
- Mac esetén a [Microsoft Remote Desktop](https://apps.apple.com/us/app/microsoft-remote-desktop-10/id1295203466?mt=12) javasolt,
- Linux-ra pedig a [Remmina](https://remmina.org/how-to-install-remmina/) - vagy bármilyen más RDP kliens.

A virtuális gépre a Visual Studio már telepítve van, de valószínűleg [be kell jelentkezni](https://visualstudio.microsoft.com/vs/support/community-edition-expired-buy-license/) egy Microsoft account-tal (pl. a @edu.bme.hu is megfelel).

!!! note "VM teljesítménye"
    A virtuális gép teljesítménye függ a cloud aktuális terhelésétől. A virtuális gép lassabb lesz, mint egy jó laptop vagy PC. Ennek ellenére minden feladat megoldható a virtuális gépen - kipróbáltuk. Sokat segít a virtuális gépen, ha minél kevesebb program fut, például csak egyetlen Visual Studio példány, és a böngésző se a VM-ben legyen megnyitva.
