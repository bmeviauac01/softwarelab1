# Exercise 1: Managing statuses

In this exercise, we will implement the basic management of status entities.

**You can earn 8 points with the completion of this exercise.**

## Open the Visual Studio solution

Open the Visual Studio solution (the `.sln`) file in the checked-out repository. If Visual Studio tells you that the project is not supported, you need to install a missing component (see [here](../VisualStudio.md)).

!!! warning "Do NOT upgrade any version"
    Do not upgrade the project, the .NET Core version, or any Nuget package! If you see such a question, always choose no!

The solution is structured according to a multi-tier architecture:

- The `Controllers` folder has the Web Api controllers handling the REST requests.
- The `DAL` folder implements the data access; it contains a repository layer and an Entity Framework Code First data model.
- The `Model` folder contains the shared entities.

In this exercise, you will need to work in classes `DAL.StatusesRepository` and `Controllers.StatusesController`. You can make changes to these classes as long as the source code complies (and the repository implements interface `IStatusesRepository`).

## Start the web app

Check if the web application starts.

1. Compile the code and start in Visual Studio.

1. Open URL <http://localhost:5000/api/ping> in a browser.

!!! success ""
    If everything goes well, you see the response "pong" in the browser, and the incoming request is logged in the console.

## List all statuses (4p)

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

## Query and insert operations (4p)

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
