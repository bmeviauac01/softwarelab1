# Feladat 3: Task-on végzett műveletek

Implementálj két új http végpontot a task-ot kezelő controllerben, amellyel az alábbiak szerint módosítható egy-egy task példány.

**A feladatok megoldásával 3-3 pont szerezhető.**

### Kész állapot jelzése

A `Task.Done` flag jelzi a task kész voltát. Készíts egy új http végpontot az alábbiak szerint, amely az `ITasksRepository.MarkDone` műveleten keresztül beállítja a flaget a megadott task példányon.

Kérés: `PATCH /api/tasks/neptun/{id}/done`, ahol `{id}` a task azonosítója.

Válasz:

- `404 Not found`, ha nem létezik a task
- `200 OK` ha sikeres a művelet - a body-ban adja vissza a módosítás utáni task entitást.

### Átsorolás másik státuszba

Egy task egy státuszhoz tartozik (`Task.StatusId`). Készíts egy új http végpontot az alábbiak szerint, amely az `ITasksRepository.MoveToStatus` műveleten keresztül áthelyezi a megadott task-ot egy másik státuszba. Ha az új státusz nem létezik, akkor hozzon létre újat a művelet.

Kérés: `PATCH /api/tasks/neptun/{id}/move?newStatusName=newname`, ahol

- `{id}` a task azonosítója,
- az új státusz **neve** pedig a `newStatusName` query paraméterben érkezik.

Válasz:

- `404 Not found`, ha nem létezik a task
- `200 OK` ha sikeres a művelet - a body-ban adja vissza a módosítás utáni task entitást.

!!! example "BEADANDÓ"
    Készíts egy **képernyőképet** Postman-ből (avagy más, hasonló eszközből, ha nem Postman-t használtál), amely a fentiek közül egy **tetszőleges** kérést és válaszát mutatja. A képet a megoldásban `f3.png` néven add be. A képernyőképen látszódjon a **kimenő kérés és a válasz is minden részletével** (URL, body, válasz kód, válasz body). Ellenőrizd, hogy a **Neptun kódod** az URL-ben szerepel-e! A képernyőkép szükséges feltétele a pontszám megszerzésének.
