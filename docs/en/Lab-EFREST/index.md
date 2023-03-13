# Entity Framework and REST

!!! danger "!!! MATERIAL IS WORK IN PROGRESS !!!"
    !!! DO NOT START THIS LAB YET !!!

    !!! MATERIAL IS WORK IN PROGRESS !!!

    !!! DO NOT START THIS LAB YET !!!

We will build a new REST API (ASP.NET Core Web API) using Entity Framework.

## Pre-requisites and preparation

Required tools to complete the tasks:

- Windows, Linux, or macOS: All tools are platform-independent, or a platform-independent alternative is available.
- [Postman](https://www.getpostman.com/)
- [DB Browser for SQLite](https://sqlitebrowser.org/) - if you would like to check the database (not necessary)
- GitHub account and a git client
- Microsoft Visual Studio 2019/2022 [with the settings here](../VisualStudio.md)
    - When using Linux or macOS, you can use Visual Studio Code, the .NET Core SDK, and [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/).
- [.NET Core **3.1** SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)

    !!! warning ".NET Core 3.1"
        Mind the version! You need .NET Core SDK version **3.1** to solve these exercises.

        On Windows, it might already be installed along with Visual Studio (see [here](../VisualStudio.md#check-and-install-net-core-sdk) how to check it); if not, use the link above to install (the SDK and _not_ the runtime). You need to install it manually when using Linux or macOS.

Materials for preparing for this laboratory:

- Entity Framework, REST API, Web API, and using Postman
    - Check the materials of _Data-driven systems_ including the [seminars](https://bmeviauac01.github.io/datadriven/en/)
- Official Microsoft tutorial for [Web API](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio)

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

## Exercise 1: Managing statuses

In this exercise, we will implement the basic management of status entities.

**You can earn 8 points with the completion of this exercise.**

### Open the Visual Studio solution

Open the Visual Studio solution (the `.sln`) file in the checked-out repository. If Visual Studio tells you that the project is not supported, you need to install a missing component (see [here](../VisualStudio.md)).

!!! warning "Do NOT upgrade any version"
    Do not upgrade the project, the .NET Core version, or any Nuget package! If you see such a question, always choose no!

The solution is structured according to a multi-tier architecture:

- The `Controllers` folder has the Web Api controllers handling the REST requests.
- The `DAL` folder implements the data access; it contains a repository layer and an Entity Framework Code First data model.
- The `Model` folder contains the shared entities.

In this exercise, you will need to work in classes `DAL.StatusesRepository` and `Controllers.StatusesController`. You can make changes to these classes as long as the source code complies (and the repository implements interface `IStatusesRepository`).

### Start the web app

Check if the web application starts.

1. Compile the code and start in Visual Studio.

1. Open URL <http://localhost:5000/api/ping> in a browser.

!!! success ""
    If everything goes well, you see the response "pong" in the browser, and the incoming request is logged in the console.

### List all statuses (4p)

Implement the first operation to list all available status entities.

1. Open class `Model.Status`. This is the entity class used by the business layer.

    !!! warning ""
        Do **NOT** make any changes to this class.

1. Open class `DAL.EfDbContext.DbStatus`. This is the Entity Framework and database representation of the same entity. Let us implement this class:

    ```csharp
    public class DbStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    ```

    The `Id` is the primary key in the database, and `Name` is the name of the status.

1. Open class `DAL.EfDbContext.TasksDbContext`. We need to add a new DbSet property here and configure the C# - database mapping in method `OnModelCreating`:

    ```csharp
    public class TasksDbContext : DbContext
    {
        public DbSet<DbStatus> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbStatus>()
                .ToTable("statuses");
            modelBuilder.Entity<DbStatus>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<DbStatus>()
                .Property(s => s.Name).HasMaxLength(50)
                .IsRequired(required: true).IsUnicode(unicode: true);
        }
    }
    ```

    This configuration sets the name of the table in the database, the key (which will generate values automatically), and the constraints related to the name field.

1. Go to the method `DAL.StatusesRepository.List()`. This is the repository layer that interacts with the database using Entity Framework. Let us list all statuses from the database:

    ```csharp
    public IReadOnlyCollection<Model.Status> List()
    {
        return db.Statuses.Select(ToModel).ToList();
    }
    ```

    The variable `db` represents our database, the DbContext, injected via the framework.

1. We will use the helper method `ToModel` to translate the EF representation to the business layer representation. Let us implement this method (in the repository class).

    ```csharp
    private static Model.Status ToModel(DbStatus value)
    {
        return new Model.Status(value.Id, value.Name);
    }
    ```

1. Once the repository is ready, let us move to the controller. Open class `Controllers.StatusesController`. **Add your Neptun code into the controller's URL**: this controller shall respond to queries that arrive at URL  `/api/statuses/neptun` where the last 6 characters are your Neptun code, all lowercase.

    ```csharp hl_lines="1"
    [Route("api/statuses/neptun")]
    [ApiController]
    public class StatusesController : ControllerBase
    ```

    !!! warning "Neptun code is important"
        The Neptun code shall appear in screenshots later. You must add it as specified above!

1. Let us implement an endpoint for handling the `GET /api/statuses/neptun` request: The dependency injection is configured already; thus, the constructor accepts the repository interface (_not_ the implementation but its interface).

    ```csharp
    public class StatusesController : ControllerBase
    {
        // ...

        [HttpGet]
        public IEnumerable<Status> List()
        {
            return repository.List();
        }
    }
    ```

1. Compile the code and start the app.

1. Open Postman and send a GET request to URL <http://localhost:5000/api/statuses/neptun> (with your Neptun code in the URL).

    ![Query statuses with Postman](../images/efrest/rest-postman-get-statuses.png)

    !!! success ""
        The query is successful if Postman reports status code 200, and the result is empty. If there is an error, check the Output window in Visual Studio and the running application console window.

1. Add some test data. Stop the application. Go to method `DAL.EfDbContext.TasksDbContext.OnModelCreating` and insert a few test records (also called _seed data_):

    ```csharp
    public class TasksDbContext : DbContext
    {
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
    }
    ```

1. Compile the code again, then start the application and repeat the same GET query. The response shall include the two statuses.

    !!! important "If you do not see the status records"
        If the _seed_ records do not appear in the response, it is possible that the database was not updated with the `HasData` method. Delete the SQLite database file `tasks.db`; this will re-create the database when the application starts again.

        Generally, schema and data modification in live environments are handled using [migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli). To simplify things, we will not use migrations. If the database schema is changed, delete `tasks.db`.

### Query and insert operations (4p)

There are a few other operations we need to implement:

- check existence by specifying name (`HEAD /api/statuses/neptun/{name}`),
- find record by ID (`GET /api/statuses/neptun/{id}`),
- adding a new status record (`POST /api/statuses/neptun`).

Let us implement these.

1. Let us start with the implementation of the first two in the repository. When looking for an item based on a name, we will perform a case-insensitive comparison.

    ```csharp
    public bool ExistsWithName(string statusName)
    {
        return db.Statuses.Any(s => EF.Functions.Like(s.Name, statusName));
    }

    public Model.Status FindById(int statusId)
    {
        var dbRecord = db.Statuses.FirstOrDefault(s => s.Id == statusId);
        if (dbRecord == null)
            return null;
        else
            return ToModel(dbRecord);
    }
    ```

    `EF.Functions.Like` helps us to write a SQL `like` query in Entity Framework. The command will translate into the proper `LIKE` operator when executed, allowing us to perform a case insensitive comparison.

1. The controller endpoints for these operations are:

    ```csharp
    [HttpHead("{statusName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsWithName(string statusName)
    {
        var exists = repository.ExistsWithName(statusName);
        if (exists)
            return Ok();
        else
            return NotFound();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Status> Get(int id)
    {
        var value = repository.FindById(id);
        if (value == null)
            return NotFound();
        else
            return Ok(value);
    }
    ```

    Note the controller method attributes and return values! If the response contains data (in the http body, the return type is `ActionResult<T>`; if there is no body and only a status code is returned, the return type is `ActionResult`. Methods `Ok` and `NotFound` help us create the correct responses.

    !!! note ""
        The URL is defined on two "levels." We defined `/api/statuses/neptun` on the controller; it applies to all endpoints. It is the "last part" of the URL that is defined on each endpoint separately.

1. To implement the insertion, let us start with the repository again. Insert is triggered by receiving a `CreateStatus` model class with a name. We want to ensure unique names. Thus, we need to verify it before insertion.

    ```csharp
    public Model.Status Insert(CreateStatus value)
    {
        using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
        {
            if (db.Statuses.Any(s => EF.Functions.Like(s.Name, value.Name)))
                throw new ArgumentException("name must be unique");

            var toInsert = new DbStatus() { Name = value.Name };
            db.Statuses.Add(toInsert);

            db.SaveChanges();
            tran.Commit();

            return new Model.Status(toInsert.Id, toInsert.Name);
        }
    }
    ```

    !!! important ""
        Mind the transaction! First, we check if an identical name exists. If so, an error is raised. After inserting the record, the transaction also has to be committed. Since the database assigns the ID, the repository needs to return the created entity with this assigned ID.

1. The POST http request is handled by the following controller endpoint:

    ```csharp
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Status> Create([FromBody] Dto.CreateStatus value)
    {
        try
        {
            var created = repository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    ```

    Note both the successful and the failed responses. If insertion succeeded, the `CreatedAtAction` helper method prepares a response that contains the entity in the body and adds a _Location_ header with the URL to fetch the record (thus the reference to `nameof(Get)`). In case of failure, the exception is handled by reporting the error to the caller. The status code will be 400, and the body will indicate an explanation as text (as REST does not have other means to report errors besides status code).

1. Compile and start the app. Test the queries! Produce both successful and erroneous responses too.

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows a **failed insert** request and response. The cause of failure should be that an item with the same name already exists. Save the screenshot as `f1.png` and submit it with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.

## Exercise 2: Task operations

In this exercise, we will implement the basic services for _tasks_.

**You can earn 6 points with the completion of this exercise.**

### Preparation with Entity Framework

The task entity is represented by the class `Model.Task`. It has a unique `ID`, a text `Title`, a `Done` flag to signal completion, and a `Status` field referencing the status of this task (with 1-\* multiplicity).

Define the Entity Framework model first:

1. Define the Entity Framework data model of this entity in class `DAL.EfDbContext.DbTask`. The referenced status should be a proper _navigation property_!

1. Add the new `DbSet` field to class `TasksDbContext`.

1. Specify the configuration of the mapping of this entity in `OnModelCreating`. Make sure to configure the _navigation property_ here correctly!

1. Add some initial ("seed") data, as seen previously.

### Operations in the repository

Create a new class, `TasksRepository`, in the `DAL` folder that implements the existing `ITasksRepository` interface. Implement the following operations:

- `IReadOnlyCollection<Task> List()`: lists all available tasks
- `Task FindById(int taskId)`: returns the single task with the specified ID if it exists; returns null otherwise
- `Task Insert(CreateTask value)`: adds a new task to the database with the specified title and associates it with the specified status; if no status with the provided name exists, create a new status record; the return value is the new task entity as created in the database with its assigned ID
- `Task Delete(int taskId)`: deletes the specified task; return value is the task (state before deletion), or null if the task does not exist

You don't need to implement the other operations yet; however, an implementation needs to be provided so that the code compiles. You may use `throw new NotImplementedException();` as a placeholder for now.

!!! tip "Tip"
    You will need to map the database entity to the model class in the repository. It is recommended to create a `ToModel` helper method, as seen previously. When querying the tasks, you will need the associated status record too (to get the name). You will need to use an `.Include()`.

### Operations on the REST Api

Create a new `TasksController` in the `Controllers` folder. The controller shall handle REST queries on URL `/api/tasks/neptun` where the last part is your **own Neptun code** lowercase.

The controller shall take an `ITasksRepository` as a parameter. For the dependency injection framework to resolve this in runtime, further configuration is needed. In the `Startup` class, register this interface and its corresponding implementation in the method `ConfigureServices` similarly to how the other repository is registered. (The controller does _not_ need registration.)

Implement the following operations using the previously implemented repository methods:

- `GET /api/tasks/neptun`: returns all tasks; response code is always `200 OK`
- `GET /api/tasks/neptun/{id}`: gets a single task; response code is `200 OK` or `404 Not found`
- `POST /api/tasks/neptun`: add a new task based on a `Dto.CreateTask` instance specified in the body; the response code is `201 Created` with the new entity in the body and an appropriate _Location_ header
- `DELETE /api/tasks/neptun/{id}`: deleted a task; response code is `204 No content` or `404 Not found`

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows an **arbitrary** request and response from the list above. Save the screenshot as `f2.png` and submit it with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.

## Exercise 3: Task operations

Implement two new endpoints in the controller handling tasks that alter existing tasks as follows.

**You can earn 3-3 points with the completion of these exercises.**

### Marking a task as done

The flag `Task.Done` signals that a task is completed. Create a new http endpoint that uses the `ITasksRepository.MarkDone` method to set this flag on a task instance.

Request: `PATCH /api/tasks/neptun/{id}/done` with `{id}` being the tasks ID.

Response:

- `404 Not found` if no such task exists.
- `200 OK` if the operation was successful - returns the task in the body after the modification is done.

### Move to a new status

A task is associated with status through `Task.StatusId` (or similar). Create a new http endpoint that uses the `ITasksRepository.MoveToStatus` method to change the status of the specified tasks to a new one. If the new status with the provided name does not exist, create one.

Request: `PATCH /api/tasks/neptun/{id}/move?newStatusName=newname` with

- `{id}` is the task's ID,
- and the **name** of the new status is received in the `newStatusName` query parameter.

Response:

- `404 Not found` if no such task exists.
- `200 OK` if the operation was successful - returns the task in the body after the modification is done.

!!! example "SUBMISSION"
    Create a **screenshot** in Postman (or an alternative tool you used) that shows an **arbitrary** request and response from the two above. Save the screenshot as `f3.png` and submit it with the other files of the solution. The screenshot shall include both the **request and the response with all details** (URL, body, response code, response body). Verify that your **Neptun code** is visible in the URL! The screenshot is required to earn the points.

## Exercise 4: Optional exercise

**You can earn an additional +3 points with the completion of this exercise.** (In the evaluation, you will see the text "imsc" in the exercise title; this is meant for the Hungarian students. Please ignore that.)

If we have lots of tasks listing them should not return all of them at once. Implement a new endpoint to return a subset of the tasks (i.e., "paging"):

- It returns the tasks in a deterministic fashion sorted by ID.
- The query accepts a `count` parameter that specifies how many tasks to return.
- Specifying the next page is performed via a `from` parameter. This `from` is the ID of the first item to return on this page.
- Both the `from` and `count` are specified as query parameters.
- The new paging endpoint should be available on URL `GET /api/tasks/neptun/paged` (the `/paged` part is necessary so that the previous listing endpoint also remains functional).
- The response should return an instance of class `Controllers.Dto.PagedTaskList`. This includes:
    - `items`: an array containing the tasks on the current page,
    - `count`: specifying the number of items on the current page,
    - `nextId`: id of the next task - to fetch the next page (to be used in `from`),
    - `nextUrl`: a helper URL that fetches the next page, or `null` if there are no further pages.

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
          ID: 2,
          "title": "doing more homework",
          "done": false,
          "status": "new"
        }
      ],
      "count": 2,
      "nextId": 3,
      "nextUrl": "http://localhost:5000/api/tasks/neptun/paged?from=3&count=2"
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
