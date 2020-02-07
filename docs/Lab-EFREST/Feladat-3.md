# Feladat 3: Task-on végzett műveletek (6p)

Implementálj két új http végpontot a task-ot kezelő controllerben, amellyel az alábbiak szerint módosítható egy-egy task példány.

### Kész állapot jelzése

A `Task.Done` flag jelzi a task kész voltát. Készíts egy új http végpontot az alábbiak szerint, amely az `ITasksRepository.MarkDone` műveleten keresztül beállítja a flaget a megadott task példányon.

Kérés: `PATCH /api/tasks/{id}/done`, ahol `{id}` a task azonosítója.

Válasz:

- `404 Not found`, ha nem létezik a task
- `200 OK` ha sikeres a művelet - a body-ban adja vissza a módosítás utáni task entitást.

### Átsorolás másik státuszba

Egy task egy státuszhoz tartozik (`Task.StatusId`). Készíts egy új http végpontot az alábbiak szerint, amely az `ITasksRepository.MoveToStatus` műveleten keresztül áthelyezi a megadott task-ot egy másik státuszba. Ha az új státusz nem létezik, akkor hozzon létre újat a művelet.

Kérés: `PATCH /api/tasks/{id}/move?newStatusName=newname`, ahol

- `{id}` a task azonosítója,
- az új státusz **neve** pedig a `newStatusName` query paraméterben érkezik.

Válasz:

- `404 Not found`, ha nem létezik a task
- `200 OK` ha sikeres a művelet - a body-ban adja vissza a módosítás utáni task entitást.

### Képernyőkép

A kész forráskódról (akár a repository, akár a controller releváns részéről) készíts egy képernyőképet. A képernyőképpel kapcsolatos elvárásokról lásd a kezdőoldalon.

> A képet a megoldásban `f3.png` néven add be. A képernyőképen látszódjon a repository vagy a controller kódjnak releváns része.
>
> A képernyőkép szükséges feltétele a pontszám megszerzésének.

## Következő feladat

Folytasd az [opcionális feladattal](Feladat-4.md) vagy add be a megoldásod az [itt](README.md#végezetül-a-megoldások-feltöltése) leírtak szerint.
