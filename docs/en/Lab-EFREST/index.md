# Entity Framework and REST

During the lab, we create a REST API (ASP.NET Core Web Api) application based on Entity Framework Core data access.

## Pre-requisites and preparation

Required tools to complete the tasks:

- Windows, Linux, or macOS: All tools are platform-independent, or a platform-independent alternative is available.
- [Postman](https://www.getpostman.com/)
- [DB Browser for SQLite](https://sqlitebrowser.org/) - if you would like to check the database (not necessary)
- GitHub account and a git client
- Microsoft Visual Studio 2022 [with the settings here](../VisualStudio.md)
    - When using Linux or macOS, you can use Visual Studio Code, the .NET Core SDK, and [dotnet CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/).
- [.NET **6** SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

    !!! warning ".NET Core 6.0"
        Mind the version! You need .NET SDK version **6.0** to solve these exercises.

        On Windows, it might already be installed along with Visual Studio (see [here](../VisualStudio.md#check-and-install-net-core-sdk) how to check it); if not, use the link above to install (the SDK and _not_ the runtime). You need to install it manually when using Linux or macOS.

Entity Framework Core, on the other hand, is included in the starting project with version **7.0**, since it is not part of the SDK, but a NuGet package, and is fully compatible with the .NET 6.0 LTS version.

Materials for preparing for this laboratory:

- Entity Framework, REST API, Web API, and using Postman
    - Check the materials of _Data-driven systems_ including the [seminars](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio)

## Exercise overview

In this exercise, we will implement the backend of a simple task management web application. The application handles **two types of entities: statuses and tasks** where a status is associated with multiple tasks (1-\* connection). (In the text of the exercises, will use _tasks_ only for referring to this entity of the application.)

If we had a frontend, the application would be a [Kanban-board](https://en.wikipedia.org/wiki/Kanban_board). We will not create a frontend here, only the REST API and Entity Framework + ASP.NET Core Web API server.

## Initial steps

Keep in mind that you are expected to follow the [submission process](../GitHub.md).

### Create and check out your Git repository

1. Create your git repository using the invitation link in Moodle. Each lab has a different URL; make sure to use the right one!

1. Wait for the repository creation to complete, then check out the repository.

    !!! tip ""
        If you are not asked for credentials to log in to GitHub in university computer laboratories when checking out the repository, the operation may fail. This is likely due to the machine using someone else's GitHub credentials. Delete these credentials first (see [here](../GitHub-credentials.md)), then retry the checkout.

1. Create a new branch with the name `solution` and work on this branch.

1. Open the checked-out folder and type your Neptun code into the `neptun.txt` file. There should be a single line with the 6 characters of your Neptun code and nothing else in this file.

### ~~Creating~~ the database

We will not be using Microsoft SQL Server here, but _Sqlite_. It is a light-weight relational database management system mainly for client-side applications. Although it is not recommended for servers, we will use it for simplicity. **Sqlite requires no installation.**

We will define the database schema with _code first_ using C# code. Therefore, we will not need to create the schema with SQL commands.

## Exercise 1: Managing statuses (8 points)

In this exercise, we will implement the basic management of status entities.

### Open the Visual Studio solution

Open the Visual Studio solution (the `.sln`) file in the checked-out repository. If Visual Studio tells you that the project is not supported, you need to install a missing component (see [here](../VisualStudio.md)).

!!! warning "Do NOT upgrade any version"
    Do not upgrade the project, the .NET Core version, or any Nuget package! If you see such a question, always choose no!

The solution is structured according to a multi-tier architecture:

- The `Controllers` folder contains the Web Api controllers that serve the REST requests.
- The `Song` folder contains the data access layer that contains the Entity Framework Core Code First model.
- The `Services` folder contains the business logic layer (BLL) service classes.
- `Dtos` contains the classes of Data Transfer Objects, which represent the data traveling on the network.

!!! note "DTO in BLL layer?"
    Since the BLL layer does not have a separate so-called Domain data model, in the BLL layer, we use a mixture of DTOs and Entities. Entities are saved and queried by the service, but it expects DTOs and returns them in the method signature.

!!! note "Repository sample"
    The Repository and Unit-of-Work design patterns would provide an abstraction for our data access. If we think about it more, the Entity Framework implements this pattern through DbContext and DbSets. Regardless, it can sometimes be advisable to create your own repository abstraction if you also want to abstract that data access works with EF.

    Now, also for the sake of simplicity, we will not use the Repository pattern.

During your work, work in the `StatusService` and `StatusController` classes! You can modify the contents of these files as you like (provided that the service still conforms to the `IStatusService` interface and of course the code still compiles).

### Start the web app

Check if the web application starts.

1. Compile the code and start in Visual Studio.

1. Open URL <http://localhost:5000/api/ping> in a browser.

!!! success ""
    If everything goes well, you see the response "pong" in the browser, and the incoming request is logged in the console.

### List all statuses (4 points)

Implement the first operation to list all available status entities.

1. Open class `Model.Status`. This is the entity class used by the business layer.

    !!! warning ""
        Do **NOT** make any changes to this class.

    !!! tip "Records in C#"
        The `record` keyword represents a type (by default `class`) that has the constructor defined in the header and the [`init` only setter](https://learn.microsoft.com/en-us/dotnet/csharp/language- reference/proposals/csharp-9.0/init) has properties. This makes a record have immutable behavior, which better matches the behavior of a DTO. Records also have other conveniences ([see more](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record)), but we will not take advantage of here.

1. Open class `DAL.EfDbContext.DbStatus`. This is the Entity Framework and database representation of the same entity. Let us implement this class:

    ```csharp hl_lines="3-4"
    public class DbStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    ```

    The `Id` is the primary key in the database, and `Name` is the name of the status.

1. Open class `DAL.EfDbContext.TasksDbContext`. We need to add a new DbSet property here and configure the C# - database mapping in method `OnModelCreating`:

    ```csharp title="TasksDbContext.cs"
    public DbSet<DbStatus> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbStatus>()
            .ToTable("statuses");
            
        modelBuilder.Entity<DbStatus>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<DbStatus>()
            .Property(s => s.Name)
            .HasMaxLength(50)
            .IsRequired(required: true)
            .IsUnicode(unicode: true);
        }
    ```

    This configuration sets the name of the table in the database, the key (which will generate values automatically), and the constraints related to the name field.

1. Go to the method `StatusService.List()`. Let us list all statuses from the database:

    ```csharp hl_lines="3"
    public IReadOnlyCollection<Dtos.Status> List()
    {
        return _dbContext.Statuses.Select(ToModel).ToList();
    }
    ```

    The variable `_dbContext` represents our database, the DbContext, injected via the framework.

1. The `ToModel` function will be a helper function that will be used several times. This maps the C# class coming from the database to another C# class used as a model. Let's write this here in the service class.

    ```csharp
    private Status ToModel(DbStatus value)
    {
        return new Status(value.Id, value.Name);
    }
    ```

1. After the BLL layer comes the controller. Open the `Controllers.StatusController` class. **Append your Neptun code to the end of the controller's URL**, so the controller handles requests to `/api/status/neptun`, where the last 6 lowercase characters are your own Neptun code.

    ```csharp hl_lines="1"
    [Route("api/[controller]/neptun")]
    [ApiController]
    public class StatusController : ControllerBase
    ```

    !!! warning "Neptun code is important"
        The Neptun code shall appear in screenshots later. You must add it as specified above!

1. Let's write the endpoint responding to the `GET /api/status/neptun` request: Dependency injection is already configured, so the constructor takes over the service interface (_not_ the service class we wrote!).

    ```csharp title="StatusController.cs"
    [HttpGet]
    public IEnumerable<Status> List()
    {
        return _statusService.List();
    }
    ```


1. Compile the code and start the app.

1. Open Postman and send a GET request to URL <http://localhost:5000/api/statuses/neptun> (with your Neptun code in the URL).

    ![Query statuses with Postman](../images/efrest/rest-postman-get-statuses.png)

    !!! success ""
        The query is successful if Postman reports status code 200, and the result is empty. If there is an error, check the Output window in Visual Studio and the running application console window.

1. It is difficult to test with an empty database. Stop the running application, navigate to the function `Dal.TasksDbContext.OnModelCreating` and insert _seed_ data into the database:

    ```csharp title="TasksDbContext.cs" hl_lines="5-10"
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ...

        modelBuilder.Entity<DbStatus>()
            .HasData(new[]
            {
                new DbStatus() { Id = 1, Name = "new" },
                new DbStatus() { Id = 2, Name = "in progress" },
            });
    }
    ```

1. Compile the code again, then start the application and repeat the same GET query. The response shall include the two statuses.

    !!! important "If you do not see the status records"
        If the two _seed_ objects do not appear in the response, it is possible that the DB was not updated and the `HasData` operation did not take effect. Delete the `tasks.db` SQLite file, as a result of which the database file will be created again when the app starts with our test data.

        Schema and data changes of this kind are usually solved in a live environment with migrations. For simplicity, we will avoid this, and if the schema changes, you can simply delete the `tasks.db` file.

### Query and insert operations (4 points)

There are a few other operations we need to implement:

- check existence by specifying name (`HEAD /api/statuses/neptun/{name}`),
- find record by ID (`GET /api/statuses/neptun/{id}`),
- adding a new status record (`POST /api/statuses/neptun`).

Let us implement these.

1. Let's implement the first two first in `StatusService`. Make sure that the name-based search is case-sensitive!

    ```csharp hl_lines="3 8-9"
    public bool ExistsWithName(string statusName)
    {
        return _dbContext.Statuses.Any(s => EF.Functions.Like(s.Name, statusName));
    }

    public Status FindById(int statusId)
    {
        var status = _dbContext.Statuses.SingleOrDefault(s => s.Id == statusId);
        return status == null ? null : ToModel(status);
    }
    ```

    With the `EF.Functions.Like` statement, we "map" an SQL statement to Entity Framework. When the system prepares the SQL statement from this, the `LIKE' operator corresponding to the platform will be generated. This solution is for case-independent comparison, as Contains would not work this way by default in **SQLite**.

2. The controller endpoints for these operations are:

    ```csharp
    [HttpHead("{statusName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsWithName(string statusName)
    {
        return _statusService.ExistsWithName(statusName) ? Ok() : NotFound();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Status> Get(int id)
    {
        var value = _statusService.FindById(id);
        return value != null ? Ok(value) : NotFound();
    }
    ```

    Pay attention to the attributes and return values on the controller methods! If the response has content (body in the http package), then `ActionResult<T>` is the return value, if only a status code is returned, then `ActionResult`. The `Ok` and `NotFound` functions are helper functions for generating the response.

    In terms of URLs, we only had to deal with the end of the URL. We put `/api/status/neptun` on the controller class, so it applies to all of them.

3. To insert the new status, start again from the service side. For creation, we get a DTO class, `CreateStatus`, which has only one name. We want to guarantee the uniqueness of the names so that there are no two statuses with the same name. We will check this during the insertion, even here regardless of lowercase or uppercase letters.

    ```csharp
    public Status Insert(CreateStatus value)
    {
        using var tran = _dbContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        if (_dbContext.Statuses.Any(s => EF.Functions.Like(s.Name, value.Name)))
            throw new ArgumentException("Name must be unique");

        var status = new DbStatus() { Name = value.Name };
        _dbContext.Statuses.Add(status);

        _dbContext.SaveChanges();
        tran.Commit();

        return ToModel(status);
    }
    ```

    !!! important "Managing competition"
        Let's pay attention to the transaction! First we need to check if a similar name already exists. If so, the error is signaled with an exception. If the record can be inserted, we must also commit the transaction after the insertion. And since the ID is generated by the database, the service function returns the created entity with the new ID inside.

1. The POST http request is handled by the following controller endpoint:

    ```csharp
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Status> Create([FromBody]CreateStatus value)
    {
        try
        {
            var created = _statusService.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(nameof(CreateStatus.Name), ex.Message);
            return ValidationProblem(ModelState);
        }
    }
    ```

    Note the successful and unsuccessful answers. If the insertion is successful, the helper function `CreatedAtAction` will return with a response where the body contains the new entity and the _Location_ header is the link where the entity can be queried (hence the reference with `nameof(Get)`).

    If, on the other hand, an exception thrown in the service is received, we report the problem to the caller. In this response, the status code will be 400, and there will also be a body, which follows the format of the Problem Details RFC standard. If we want to return with a 400 error with a body of any format, we could have used the `BadRequest()` function.

    The `[Required]` attribute is also included on the `Name` property in the `CreateStatus` DTO. Because of the `[ApiController]` attribute on the controller, these validation attributes are evaluated before the action is called, and also result in a 400 error in the Problem Details format.

5. Compile and start the app. Test the queries! Produce both successful and erroneous responses too.

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows a **failed insert** request and response. The cause of failure should be that an item with the same name already exists. Save the screenshot as `f1.png` and submit it with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.

## Exercise 2: Task operations (6 points)

In this exercise, we will implement the basic services for _tasks_.

### Preparation with Entity Framework

The task is represented by the DTO class `Dtos.Task`. The task has an identifier (`Id`), a title (`Title`), the `IsDone` flag indicates when it is ready, and `Status` shows the status to which the task is assigned (with a multiplicity of 1-\*) .

The first step is to create the Entity Framework model:

1. Define the properties required for database storage in the `DbTask` class. Make sure that the status link is a real _navigation property_!

2. Add the new `DbSet` type property to `TasksDbContext`.

3. As before, define the exact configuration of the database mapping in `OnModelCreating`. Here too, pay attention to the exact setting of the _navigation property_!

4. It will be advisable to record sample data as seen earlier.

### Operations in the repository

In the `Dal` folder, create a new class called `TaskService` that implements the existing `ITaskService` interface. Perform the following actions:

- `IReadOnlyCollection<Task> List()`: list all tasks
- `Task FindById(int taskId)`: returns the task whose id matches the parameter; or return `null` if there is none
- `Task Insert(CreateTask value)`: add a new task to the database with the given address and assign it to the given status; if there is no status with the specified name, add a new status; its return value is the new task entity with the new identifier
- `Task Delete(int taskId)`: delete the specified task instance; its return value is the deleted task entity (in the state before deletion), or `null` if it does not exist

Do not implement the other operations for now, but they must also have an implementation so that the code will compile. For now, it is enough if their body simply throws an error: `throw new NotImplementedException();`

!!! tip "Tip"
    For the mapping between the C# class used in the database and the model entity class, it will be useful to define a `ToModel` helper function as seen earlier. In order for the database to query the status entity connected to the task (which will be needed for the name), it will be important to use the appropriate `Include`.

### Operations on the REST Api

Create a new `TasksController` in the `Controllers` folder. The controller shall handle REST queries on URL `/api/tasks/neptun` where the last part is your **own Neptun code** lowercase.

Take an instance of `ITaskService` in the controller constructor parameter. In order for the dependency injection framework to solve this at runtime, configuration will also be necessary. This interface must be registered in the `Program' class, just like the other service. (The controller _does_ not have to be registered.)

Implement the following operations using the previously implemented repository methods:

- `GET /api/tasks/neptun`: returns all tasks; response code is always `200 OK`
- `GET /api/tasks/neptun/{id}`: gets a single task; response code is `200 OK` or `404 Not found`
- `POST /api/tasks/neptun`: add a new task based on a `Dto.CreateTask` instance specified in the body; the response code is `201 Created` with the new entity in the body and an appropriate _Location_ header
- `DELETE /api/tasks/neptun/{id}`: deleted a task; response code is `204 No content` or `404 Not found`

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows an **arbitrary** request and response from the list above. Save the screenshot as `f2.png` and submit it with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.

## Exercise 3: Task operations (6 points)

Implement two new endpoints in the controller handling tasks that alter existing tasks as follows.

### Marking a task as done (3 points)

The flag `Task.Done` signals that a task is completed. Create a new http endpoint that uses the `ITasksRepository.MarkDone` method to set this flag on a task instance.

Request: `PATCH /api/tasks/neptun/{id}/done` with `{id}` being the tasks ID.

Response:

- `404 Not found` if no such task exists.
- `200 OK` if the operation was successful - returns the task in the body after the modification is done.

### Move to a new status (3 points)

A task is associated with status through `Task.StatusId` (or similar). Create a new http endpoint that uses the `ITasksRepository.MoveToStatus` method to change the status of the specified tasks to a new one. If the new status with the provided name does not exist, create one.

Request: `PATCH /api/tasks/neptun/{id}/move` with

- `{id}` is the task identifier,
- and the **name** of the new status comes in the body in a `status` property.

Response:

- `404 Not found` if no such task exists.
- `200 OK` if the operation was successful - returns the task in the body after the modification is done.

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows an **arbitrary** request and response from the two above. Save the screenshot as `f3.png` and submit it with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.

## Exercise 4: Optional exercise (3 points)

**You can earn an additional +3 points with the completion of this exercise.** (In the evaluation, you will see the text "imsc" in the exercise title; this is meant for the Hungarian students. Please ignore that.)

If we have lots of tasks listing them should not return all of them at once. Implement a new endpoint to return a subset of the tasks (i.e., "paging"):

- It returns the tasks in a deterministic fashion sorted by ID.
- In the request, an optional `count` query parameter returns an element with the number of pieces specified on each page. by default, the value should be 5 if the client does not send it.
- The next page can be retrieved by declaring an optional `fromId` value. This `fromId` is the **identifier** of the next element in the pagination.
- The two parameters of the http request, `fromId` and `count`, should come as optional query parameters.
- Paging should be available at the existing address `GET /api/task/neptun/paged`.
- During paging, only those entities that are really needed should be queried for the answer (so don't needlessly drag the entire table into memory).
- The paging response should be an instance of the `Dto.PagedTaskList` class. It includes:
    - array of elements on the page (`Items`),
    - the number of elements on the page (`Count`)
    - `fromId` value (`NextId`) required to retrieve the next page,
    - and as a help, the URL with which the next page can be retrieved (`NextUrl`), or `null` if there are no more pages.

        !!! tip ""
            Use the `Url.Action` helper method to assemble this URL. Do not hardcode "localhost:5000" or "/api/tasks/paged" in the source code! You will _not_ need string operations to achieve this.

            `Url.Action` will give you an absolute URL when all parameters (`action`, `controller`, `values`, `protocol`, and `host`) are specified; for the latter ones `this.HttpContext.Request` can provide you the required values.

- The request always returns 200 OK; if there are no items, the result set shall be empty.

The requests-responses shows you the expected behavior:

1. `GET /api/tasks/neptun/paged?count=2`

    This is the first request. There is no `from` value specified to start from the first item.

    Response:

    ```json
    {
      "items": [
        {
          "id": 1,
          "title": "doing homework",
          "done": false,
          "status": "pending"
        },
        {
          "id": 2,
          "title": "doing more homework",
          "done": false,
          "status": "new"
        }
      ],
      "count": 2,
      "nextId": 3,
      "nextUrl": "http://localhost:5000/api/task/neptun/paged?fromId=3&count=2"
    }
    ```

2. `GET /api/tasks/neptun/paged?from=3&count=2`

    This is to query the second page.

    Response:

    ```json
    {
      "items": [
        {
          "id": 3,
          "title": "hosework",
          "done": true,
          "status": "done"
        }
      ],
      "count": 1,
      "nextId": null,
      "nextUrl": null
    }
    ```

    The response indicates no further pages as both `nextId` and `nextUrl` are null.

3. `GET /api/tasks/neptun/paged?from=999&count=999`

    Returns an empty page.

    Response:

    ```json
    {
      "items": [],
      "count": 0,
      "nextId": null,
      "nextUrl": null
    }
    ```

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows an **arbitrary** request and response of fetching a page. Save the screenshot as `f4.png` and submit it with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.
