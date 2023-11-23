# Visual Studio & .NET SDK telepítése

Egyes laborokhoz a Microsoft Visual Studio **2022 17.8 vagy újabb** verziója szükséges. Az ingyenes, [Community változata](https://visualstudio.microsoft.com/vs/community/) is elegendő a feladatok megoldásához.

A verziót a _Visual Studio Installer_ elindításával ellenőrizhetjük:

![Visual Studio verzió](images/visual-studio/vs-verzio.png)

!!! info "VS Code"
    A laborok Visual Studio nélkül, **Visual Studio Code**-dal is megoldhatóak. A kiadott kód váz Visual Studio-hoz készült, annak konfigurációit tartalmazza. Ha VS Code-dal dolgozol, magadnak kell konfigurálni a környezetet.

## Visual Studio Workload-ok telepítése

A Visual Studio telepítésekor ki kell pipálni az alábbi [workload-okat](https://learn.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2022#step-4---choose-workloads):

- ASP.NET and web development
- Data Storage and Processing

![Visual Studio workloadok](images/visual-studio/vs-workload.png)

Meglevő telepítés a _Visual Studio Installer_-ben a [_Modify_](https://docs.microsoft.com/en-us/visualstudio/install/modify-visual-studio?view=vs-2022) gombbal módosítható, ill. ellenőrizhető.

![Visual Studio komponensek telepítése](images/visual-studio/vs-installer-modify.png)

## .NET SDK ellenőrzése és telepítése

Visual Studio mellett bizonyos .NET SDK-k telepítésre kerülnek. A megfelelő verzió ellenőrzéséhez legegyszerűbb a `dotnet` CLI-t használni: konzolban add ki a `dotnet --list-sdks` parancsot. Ez a parancs Linux és Mac esetén is működik. A kimenete hasonló lesz:

```hl_lines="2"
PS C:\Users\toth.tibor> dotnet --list-sdks
8.0.100 [C:\Program Files\dotnet\sdk]
```

Ha ebben a listában látsz **8.0.x**-es verziót, akkor jó. Ha nem, akkor telepíteni kell az SDK-t [innen](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
