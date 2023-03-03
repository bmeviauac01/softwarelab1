# MongoDB

In this lab, we will work with the MongoDB NoSQL database and the Mongo C# driver.

## Pre-requisites and preparation

Required tools to complete the tasks:

- Windows, Linux, or macOS: All tools are platform-independent, or a platform-independent alternative is available.
- MongoDB Community Server ([download](https://www.mongodb.com/download-center/community))
- Robo 3T ([download](https://robomongo.org/download))
    - Without installing you can run the server with the following command using Docker:
    
          ```cmd
        docker run --name swlab1-mongo -p 27017:27017 -d mongo
        ```
        
- Sample database initialization script: ([mongo.js](https://bmeviauac01.github.io/adatvezerelt/db/mongo.js))
- GitHub account and a git client
- Microsoft Visual Studio 2022 [with the settings here](../VisualStudio.md)
    - When using Linux or macOS, you can use Visual Studio Code, the .NET SDK, and [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/).
- [.NET Core **6.0** SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

    !!! warning ".NET 6.0"
        Mind the version! You need .NET SDK version **6.0** to solve these exercises.

        On Windows it might already be installed along with Visual Studio (see [here](../VisualStudio.md#check-and-install-net-core-sdk) how to check it); if not, use the link above to install (the SDK and _not_ the runtime). You need to install it manually when using Linux or macOS.

Materials for preparing for this laboratory:

- MongoDB database system and the C# driver
    - Check the materials of _Data-driven systems_ including the [seminars](https://bmeviauac01.github.io/datadriven/en/)
- Official Microsoft tutorial for [WebApi using MongoDB](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-6.0&tabs=visual-studio)
    - We will not be creating a WebApi in this lab, but the Mongo part is the same.

## Initial steps

Keep in mind that you are expected to follow the [submission process](../GitHub.md).

### Create and check out your Git repository

1. Create your git repository using the invitation link in Moodle. Each lab has a different URL; make sure to use the right one!

1. Wait for the repository creation to complete, then check out the repository.

    !!! warning "Password in the labs"
        If you are not asked for credentials to log in to GitHub in university computer laboratories when checking out the repository, the operation may fail. This is likely due to the machine using someone else's GitHub credentials. Delete these credentials first (see [here](../GitHub-credentials.md)), then retry the checkout.

1. Create a new branch with the name `solution` and work on this branch.

1. Open the checked-out folder and type your Neptun code into the `neptun.txt` file. There should be a single line with the 6 characters of your Neptun code and nothing else in this file.

### Create the database

Follow the steps in the [seminar material](https://bmeviauac01.github.io/datadriven/en/seminar/mongodb/#exercise-0-create-database-open-starter-code) to start the database server and initialize the database.

## Exercise 1: Listing and modifying products

This exercise will implement CRUD (create, retrieve, update, delete) operations for `Product` entities.

### Open the Visual Studio solution

Open the Visual Studio solution (the `.sln`) file in the checked-out repository. If Visual Studio tells you that the project is not supported, you need to install a missing component (see [here](../VisualStudio.md)).

!!! warning "Do NOT upgrade any version"
    Do not upgrade the project, the .NET version, or any NuGet package! If you see such a question, always choose no!

You will need to work in class `Dal.Repository`! You can make changes to this class as long as the source code complies, the repository implements interface `mongolab.DAL.IRepository`, and the constructor accepts a single `IMongoDatabase` parameter.

The database access is configured in class `Dal.MongoConnectionConfig`. If needed, you can change the database name in this file.

Other parts of the application should **NOT** be modified!

!!! info "Razor Pages"
    The web application is a so-called [_Razor Pages_](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/) ASP.NET Core project. It includes a presentation layer rendered on the server using C# code and the Razor template. (You do not need to concern yourself with the UI.)

### Start the web app

Check if the web application starts.

1. Compile the code and start in Visual Studio.

1. Open URL <http://localhost:5000/> in a browser.

!!! success ""
    If everything was successful, you should see a page with links where you will be able to test your code. (The links will not work as the data access layer is not implemented yet.)

### Display the Neptun code on the web page

You will need to create screenshots that display your Neptun code.

1. Open file `Pages\Shared\_Layout.cshtml`. In the middle of the file, find the following section, and edit your Neptun code.

    ```csharp hl_lines="5"
    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>@ViewData["Title"] - NEPTUN</p>
        </footer>
    </div>
    ```

1. Compile the code and start the app again, then check the starting page. You should see the Neptune code at the bottom of the page.

    ![Neptun code in the footer](../images/mongo/mongo-neptun-footer.png)

!!! warning "IMPORTANT"
    The Neptun code is a mandatory requirement in the footer!

### Listing

1. First, you will need a way to access the `products` collection from C#. Create and initialize a new variable that represents the collection in class `Repository`. Use the injected `IMongoDatabase` variable to get the collection:

    ```csharp
    private readonly IMongoCollection<Entities.Product> _productCollection;

    public Repository(IMongoDatabase database)
    {
        this._productCollection = database.GetCollection<Entities.Product>("products");
    }
    ```

1. You can use `_productCollection` to access the database's product records from now on. Let us start by implementing `ListProducts`. This will require two steps: first, to query the data from the database, then transform each record to an instance of `Models.Product`.

    The query is as follows:

    ```csharp
    var dbProducts = _productCollection
        .Find(_ => true) // listing all products hence an empty filter
        .ToList();
    ```

    All items are then transformed.

    ```csharp
    return dbProducts
        .Select(t => new Product
        {
            Id = t.Id.ToString(),
            Name = t.Name,
            Price = t.Price,
            Stock = t.Stock
        })
        .ToList();
    ```

1. The implementation of `FindProduct(string id)` is similar, except for querying a single record by matching the `Id`. Pay attention to the fact that the `Id` is received as a string, but it needs converting to `ObjectId`.

    The transformation to the model remains identical. However, we should also handle when there is no matching record found and return a `null` value in this case (without converting anything to a model).

    The query is as follows:

    ```csharp
    var dbProduct = _productCollection
        .Find(t => t.Id == ObjectId.Parse(id))
        .SingleOrDefault();
    // ... model conversion
    ```

    Note how the filter expression looks like! Also, note how the `ToList` is replaced with a `SingleOrDefault` call. This returns either the first (and single) element in the result set or `null` when there is none. This is a generic way of querying a single record from the database. You will need to write a similar code in further exercises.

    The conversion/transformation code is already given; however, we should prepare to handle when `dbProduct` is `null`. Instead of conversion, we should return `null` then.

1. Test the behavior of these queries! Start the web application and go to <http://localhost:5000> in a browser. Click `Products` to list the data from the database. If you click on `Details` it will show the details of the selected product.

!!! failure "If you do not see any product"
    If you see no items on this page, but there is no error, it is most likely due to a misconfigured database access. MongoDB will not return an error if the specified database does not exist. See the instructions for changing the connection details above.

### Creation

1. Implement the method `InsertProduct(Product product)`. The input is an instance of `Models.Product` that collects the information specified on the UI.

1. To create a new product, we will first create a new database entity (in memory first). This is an instance of class `Entities.Product`. There is no need to set the `Id` - the database will generate it. `Name`, `Price` and `Stock` are provided by the user. What is left is `Vat` and `CategoryId`. We should hard-code values here: create a new VAT entity and find a random category using _Studio 3T_ and copy the `_id` value.

    ```csharp
    var dbProduct = new Entities.Product
    {
        Name = product.Name,
        Price = product.Price,
        Stock = product.Stock,
        Vat = new Entities.Vat
        {
            Name = "General",
            Percentage = 20
        },
        CategoryId = ObjectId.Parse("5d7e4370cffa8e1030fd2d99"),
    };
    _productCollection.InsertOne(dbProduct);
    ```

    Once the database entity is ready, use `InsertOne` to add it to the database.

3. To test your code, start the application and click the `Add new product` link on the products page. You will need to fill in the necessary data, and then the presentation layer will call your code.

### Delete

1. Implement method `DeleteProduct(string id)`. Use `DeleteOne` on the collection to delete the record. You will need a filter expression here to find the matching record similarly to how it was done in `FindProduct(string id)`.

1. Test the functionality using the web application by clicking the `Delete` link next to a product.

### Modification

1. We will implement the method `bool SellProduct(string id, int amount)` as a modification operation. The method shall return `true` if a record with a matching `id` is found, and there are at least `amount` pieces in stock. If the product is not found or there is not enough in stock return `false`.

1. Using the atomicity guarantees of MongoDB, we will perform the changes in a single step. A filter will be used to find both the `id` and check if there are enough items in stock. A modification will decrease the stock only if the filter is matched.

    ```csharp
    var result = _productCollection.UpdateOne(
        filter: t => t.Id == ObjectId.Parse(id) && t.Stock >= amount,
        update: Builders<Entities.Product>.Update.Inc(t => t.Stock, -amount),
        options: new UpdateOptions { IsUpsert = false });
    ```

    Note that the `UpdateOptions` is used to signal that we do **NOT** want as upsert operation; instead, we want the operation to do nothing when the filter is not matched.

    The modification is assembled using `Update` in `Builders`. Here we want to decrease the stock value with `amount` (which is, effectively, an increase with `-amount`).

    We can determine what happened based on the `result` returned by the update operation. If the result indicates that the filter matched a record and the modification was performed, return `true`. Otherwise, return `false`.

    ```csharp
    return result.MatchedCount > 0;
    ```

1. Test the functionality using the web application by clicking the `Buy` link next to a product. Verify the behavior when you enter a too large amount!

!!! example "SUBMISSION"
    Create a **screenshot** of the web page listing the products **after successfully adding at least one new product**. Save the screenshot as `f1.png` and submit it with the other files of the solution. The screenshot shall display the **list of products**. Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.

## Exercise 2: Listing categories

We will be listing available categories here with the number of products in each category. We will need an aggregation pipeline here. Continue working in class `Dal.Repository`.

The method you should implement is `IList<Category> ListCategories()`. The method shall return all categories. Class `Models.Category` has 3 members.

- `Name`: the name of the category
- `ParentCategoryName`: the name of the parent category. If there is no parent, the value should be `null`.
- `NumberOfProducts`: number of products in this category. If there are no products, the value should be **0**.

The outline of the solution is as follows.

1. Create and initialize a new `_productCollection` similar to how `_categoryCollection` is initialized. The name of the collection is `categories` - you can verify this using _Studio 3T_.

1. `ListCategories()` should first query all categories. Perform this similarly to how it was done in the previous exercise. Store the result set in variable `dbCategories`.

1. Query the number of products associated with each category (`Product.CategoryId`). Use an aggregation pipeline and a [`$group`](https://docs.mongodb.com/manual/reference/operator/aggregation/group/) step as follows.

    ```csharp
    var productCounts = _productCollection
        .Aggregate()
        .Group(t => t.CategoryID, g => new { CategoryID = g.Key, NumberOfProducts = g.Count() })
        .ToList();
    ```

    This query yields a list where each item has a `CategoryID` and the number of associated products.

1. We have all information we need: all categories (including the parents) and the number of products for each. The final step is to "merge" the results in C# code.

    ```csharp
    return dbCategories
    .Select(k => new Category
       {
           Name = k.Name,
           ParentCategoryName = k.ParentCategoryId.HasValue
               ? dbCategories.Single(p => p.Id == k.ParentCategoryId.Value).Name
               : null,
           NumberOfProducts = productCounts.SingleOrDefault(pc => pc.CategoryID == k.Id)?.NumberOfProducts ?? 0
       })
       .ToList();
    ```

    As seen above, this is performed using LINQ.

    !!! tip "Join with MongoDB"
        This is not the only solution to "join" collections in MongoDB. Although there is no `join` operation, there are ways to query data across collections. Instead of doing this in MongoDB, we do the merging in C# as above. This would not be good if the data set were large. Also, if there were filtering involved, the code above would be much more complicated.

1. Use the `Categories` link of the website to test your solution. This will list the data provided by your code in a tabular format. You can use the `Add new product` functionality from before to create new products. This must result in an increase in the number of products in one of the categories. (Remember that inserting the product hard-coded a category ID.)

!!! example "SUBMISSION"
    Create a **screenshot** of the web page listing the categories. Save the screenshot as `f2.png` and submit it with the other files of the solution. The screenshot shall display the **list of categories**. Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.

## Exercise 3: Querying and modifying orders

In this exercise, we will implement CRUD (create, retrieve, update, delete) operations for `Order` entities. This exercise is similar to the previous one; feel free to look back to the solutions of that exercise.

The properties of `Model.Order` are:

- `Id`: the `Id` of the database serialized using `ToString`
- `Date`, `Deadline`, `Status`: taken from the database directly
- `PaymentMethod`: taken from the `Method` field of the `PaymentMethod` complex entity
- `Total`: the cumulative sum of the product of `Amount` and `Price` for all items associated with this order (`OrderItems`)

You will need to implement the management methods related to orders: `ListOrders`, `FindOrder`, `InsertOrder`, `DeleteOrder`, and `UpdateOrder`.

Before starting the tasks below, do not forget to add and initialize an `_orderCollection` in the repository class similar to the other one.

### Listing

1. Method `ListOrders` receives a `string status` parameter. If this value is empty or `null` (see: `string.IsNullOrEmpty`) list all orders. Otherwise, list orders where the `Status` field is identical to the `status` received as a parameter.

1. Method `FindOrder` returns the data of a single order identified by `string id`. If no record with the same `ID` exists, this method shall return `null`.

### Creation

1. Implement the method `InsertOrder`. The following information is provided to create the new order: `Order order`, `Product product`, and `int amount`.

1. You need the set the following information in the database entity:

    - `CustomerId`, `SiteId`: find a chosen `Customer` in the database and copy the values from this record from fields `_id` and `mainSiteId`. Hard-wire these values in code.
    - `Date`, `Deadline`, `Status`: take these values from the value received as `order` parameter
    - `PaymentMethod`: create a new instance of `PaymentMethod`. The `Method` should be `PaymentMethod` from the object received through the `order` parameter. Leave `Deadline` as `null`.
    - `OrderItems`: create a single item here with the following data:
        - `ProductId` and `Price`: take the values from the parameter `product`
        - `Amount`: copy value from the method parameter `amount`
        - `Status`: equals to the `Status` field of parameter `order`
    - other fields (related to invoicing) should be left as `null`!

### Delete

`DeleteOrder` should delete the record specified by the `Id`.

### Modification

When updating the record in `UpdateOrder`, only update the information present in `Models.Order`: `Date`, `Deadline`, `Status`, and `PaymentMethod`. Ignore the value `Total`; it does not need to be considered in this context.

!!! tip "Hint"
You can combine multiple updates using `Builders<Entities.Order>.Update.Combine`.

Keep in mind that the `IsUpsert` property should be set to `false` in the update!

The method should return `true` if there were a record with a matching `ID`.

### Testing

You can test the functionalities using the `Orders` link in the test web app. Verify the behavior of `Filter`, `Add new order`, `Edit`, `Details`, and `Delete` too!

!!! example "SUBMISSION"
    Create a **screenshot** of the web page listing the orders **after successfully adding at least one new order**. Save the screenshot as `f3.png` and submit it with the other files of the solution. The screenshot shall display the **list of orders**. Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.

## Exercise 4: Listing customers

We will list the customers in this exercise, along with the cumulative value of their orders. This will be similar to exercise 2: we will use aggregation and merging in C# code.

The method to implement is `IList<Customer> ListCustomers()`. The method shall return every customer. The properties of `Model.Customer` are:

- `Name`: the name of the customer
- `ZipCode`, `City`, `Street`: the address fields of the main site of the customer
- `TotalOrders`: the cumulative total of all orders of the customer. You have to aggregate the price\*amount for each order of a customer to get this total. If a particular customer has no orders, this value shall be `null`.

Follow these steps to solve this exercise:

1. Create and initialize the `_customerCollection`.

1. List all customers. The customer entity has the list of `Sites`; the main site is the item `MainSiteId` points to. Use this value to find the main in among the list.

1. In the collection of the orders, use an aggregation pipeline to calculate the total of all orders for each `CustomerId`.

1. Finally, you need the "merge" the two result sets. Every customer has a main site; however, not all of them have orders (in which case `TotalOrders` shall be `null`).

1. Use the `Customers` link of the website to test your solution. This will list the data provided by your code in a tabular format. You can use the `Add new order` functionality from before to create new orders. This must result in an increase in the total for one of the customers.

!!! example "SUBMISSION"
    Create a **screenshot** of the web page listing the customers. Save the screenshot as `f4.png` and submit it with the other files of the solution. The screenshot shall display the **list of customers**. Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.

## Exercise 5: Optional exercise

(In the evaluation, you will see the text "imsc" in the exercise title; this is meant for the Hungarian students. Please ignore that.)

We will group the orders in this exercise by date. We would like to see how our company performs by comparing the sales across time. We will use a [`$bucket`](https://docs.mongodb.com/manual/reference/operator/aggregation/bucket/) aggregation.

### Requirements

The method to implement is `OrderGroups GroupOrders(int groupsCount)`. This operation shall group the orders into `groupsCount` equal date ranges. The return value contains two values:

- `IList<DateTime> Thresholds`: The threshold dates of the date ranges.
    - The lower bound of the interval is inclusive, while the upper bound is exclusive.
    - When having `n` intervals, the `Thresholds` list has `n + 1` items
    - _E.g.: Let the `Thresholds` be `a, b, c, d`; the intervals shall then be: `[a, b[`, `[b, c[` and `[c, d[`._
- `IList<OrderGroup> Groups`: The groups that fall into each date range. The properties of `OrderGroup` are:
    - `Date`: The _start_ date of the interval. E.g., for the interval `[a, b[` the value is`a`.
    - `Pieces`: The number of orders within the interval.
    - `Total`: The cumulative sum of the values of orders within this interval.

Further requirements:

1. There should be exactly `groupsCount` intervals.
    - The number of items in `Thresholds` will be **exactly** `groupsCount + 1`.
    - The number of items in `Groups` is **at most** `groupsCount` — no need for an item for intervals with no orders
1. The lower boundary should be the earliest date in the database
1. The upper boundary should be the latest date in the database + 1 hour
    - This is needed because the upper boundary is exclusive. It ensures that every item in the database falls into one of the intervals.
    - _Tip: add one hour to a date: `date.AddHours(1)`._
1. The intervals should be of equal size
    - _Tip: C# has built-in support for date arithmetic using dates (`DateTime`) and duration (`TimeSpan`) classes._

You can assume the following:

- All orders in the database have `Date` values even though the type is _nullable_ (`DateTime?`).
    - You can use `date.Value` to get the date without checking `date.HasValue`.
- `groupsCount` is a positive integer **greater than or equal to 1**.

### Draft solution

1. Get the earliest and latest order dates from the database.

    - _Tip: You can execute two queries to get the values or a single aggregation._

1. Calculate the interval boundaries according to the requirements.

    - This will yield the `Thresholds` list for the return value.

1. Execute a `$bucket` aggregation on the orders collection. See the documentation [here](https://docs.mongodb.com/manual/reference/operator/aggregation/bucket/).

    - the `groupBy` expression will be the date of the order
    - `boundaries` expects the values as stated in the requirements; the list assembled in the previous step will work just fine
    - `output` should calculate the count and total value

    !!! tip "Tip"
        If you receive an error message `"Element '...' does not match any field or property of class..."` then in the `output` expression, change every property to **lowercase** (e.g., `Pieces` -> `pieces`). It seems that the Mongo C# driver does not perform the required name transformations here.

1. The `$bucket` aggregation will yield the intervals according to the specification. You will only need to transform the results into instances of `OrderGroup` and produce the return value.

1. Use the `Group orders` link of the website to test your solution. A diagram will display the calculated information. Test your solution by changing the number of groups and adding orders in the past using the previously implemented `Add new order` functionality.

!!! example "SUBMISSION"
    Create a **screenshot** of the web page displaying the diagram. Save the screenshot as `f5.png` and submit it with the other files of the solution. The screenshot shall show **both diagrams** (you may need to zoom out in the browser to fit them). Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.
