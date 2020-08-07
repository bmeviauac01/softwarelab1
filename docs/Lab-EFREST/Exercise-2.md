# Exercise 2: Task operations

In this exercise, we will implement the basic services for _tasks_.

**You can earn 6 points with the completion of this exercise.**

## Preparation with Entity Framework

The task entity is represented by class `Model.Task`. It has a unique `ID`, a text `Title`, a `Done` flag to signal completion, and a `Status` field referencing the status of this task (with 1-\* multiplicity).

Define the Entity Framework model first:

1. Define the Entity Framework data model of this entity in class `DAL.EfDbContext.DbTask`. The referenced status should be a proper _navigation property_!

1. Add the new `DbSet` field to class `TasksDbContext`.

1. Specify the configuration of the mapping of this entity in `OnModelCreating`. Make sure to configure the _navigation property_ here correctly!

1. Add some initial ("seed") data, as seen previously.

## Operations in the repository

Create a new class `TasksRepository` in the `DAL` folder that implements the existing `ITasksRepository` interface. Implement the following operations:

- `IReadOnlyCollection<Task> List()`: lists all available tasks
- `Task FindById(int taskId)`: returns the single task with the specified ID if it exists; returns null otherwise
- `Task Insert(CreateTask value)`: adds a new task to the database with the specified title and associates it with the specified status; if no status with the provided name exists, create a new status record; the return value is the new task entity as created in the database with its assigned ID
- `Task Delete(int taskId)`: deletes the specified task; return value is the task (state before deletion), or null if the task is not found

You don't need to implement the other operations yet; however, an implementation needs to be provided so that the code compiles. You may use `throw new NotImplementedException();` as a placeholder for now.

!!! tip "Tip"
    You will need to map the database entity to the model class in the repository. It is recommended to create a `ToModel` helper method, as seen previously. When querying the tasks, you will need the associated status record too (to get the name). You will need to use an `.Include()`.

## Operations on the REST Api

Create a new `TasksController` in the `Controllers` folder. The controller shall handle REST queries on URL `/api/tasks/neptun` where the last part is your **own Neptun code** lowercase.

The controller shall take an `ITasksRepository` as a parameter. For the dependency injection framework to resolve this in runtime, further configuration is needed. In the `Startup` class, register this interface and its corresponding implementation in the method `ConfigureServices` similarly to how the other repository is registered. (The controller does _not_ need registration.)

Implement the following operations using the previously implemented repository methods:

- `GET /api/tasks/neptun`: returns all tasks; response code is always `200 OK`
- `GET /api/tasks/neptun/{id}`: gets a single task; response code is `200 OK` or `404 Not found`
- `POST /api/tasks/neptun`: add a new task based on a `Dto.CreateTask` instance specified in the body; the response code is `201 Created` with the new entity in the body and an appropriate _Location_ header
- `DELETE /api/tasks/neptun/{id}`: deleted a task; response code is `204 No content` or `404 Not found`

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows an **arbitrary** request and response from the list above. Save the screenshot as `f2.png` and submit with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.
