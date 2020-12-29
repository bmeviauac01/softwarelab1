# Exercise 1: Listing and modifying products

**You can earn 7 points with the completion of this exercise.**

This exercise will implement CRUD (create, retrieve, update, delete) operations for `Product` entities.

## Open the Visual Studio solution

Open the Visual Studio solution (the `.sln`) file in the checked out repository. If Visual Studio tells you that the project is not supported, you need to install a missing component (see [here](../VisualStudio.md)).

!!! warning "Do NOT upgrade any version"
    Do not upgrade the project, the .NET Core version, or any Nuget package! If you see such a question, always choose no!

You will need to work in class `monolab.DAL.Repository`! You can make changes to this class as long as the source code complies, the repository implements interface `mongolab.DAL.IRepository`, and the constructor accepts a single `IMongoDatabase` parameter.

The database access is configured in class  `mongolab.DAL.MongoConnectionConfig`. If needed, you can change the database name in this file.

Other parts of the application should **NOT** be modified!

!!! info ""
    The web application is a so-called [_Razor Pages_](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/) ASP.NET Core project. This includes a presentation layer that is rendered on the server using C# code and the Razor engine. (You do not need to concern yourself with the UI.)

## Start the web app

Check if the web application starts.

1. Compile the code and start in Visual Studio.

1. Open URL <http://localhost:5000/> in a browser.

!!! success ""
    If everything was successful, you should see a page with links where you will be able to test your code. (The links will not work as the data access layer is not implemented yet.)

## Display the Neptun code on the web page

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

## Listing

1. First, you will need a way to access the `products` collection from C#. Create and initialize a new variable that represents the collection in class `Repository`. Use the injected `IMongoDatabase` variable to get the collection:

    ```csharp
    private readonly IMongoCollection<Enites.Product> productCollection;

    public Repository(IMongoDatabase database)
    {
        this.productCollection = database.GetCollection<Enites.Product>("products");
    }
    ```

1. You can use `productCollection` to access the product records of the database from now on. Let us start by implementing `ListProducts`. This will require two steps: first, to query the data from the database, then transform each record to an instance of `Models.Product`.

    The query is as follows:

    ```csharp
    var dbProducts = productCollection
        .Find(_ => true) // listing all products hence an empty filter
        .ToList();
    ```

    All items are then transformed.

    ```csharp
    return dbProducts
        .Select(t => new Product
        {
            ID = t.ID.ToString(),
            Name = t.Name,
            Price = t.Price,
            InStock = t.InStock
        })
        .ToList();
    ```

1. The implementation of `FindProduct(string id)` is similar, except for querying a single record by matching the `ID`. Pay attention to that fact that the `ID` is received as string, but it needs converting to `ObjectId`.

    The transformation to the model remains identical. However, we should also handle when there is no matching record found and return a `null` value in this case (without converting anything to a model).

    The query is as follows:

    ```csharp
    var dbProduct = productCollection
        .Find(t => t.ID == ObjectId.Parse(id))
        .SingleOrDefault();
    // ... model conversion
    ```

    Note how the filter expression looks like! Also, note how the `ToList` is replaced with a `SingleOrDefault` call. This returns either the first (and single) element in the result set or `null` when there is none. This is a generic way of querying a single record from the database. You will need to write a similar code in further exercises.

    The conversion/transformation code is already given; however, we should prepare to handle when `dbProduct` is `null`. Instead of conversion, we should just return `null` then.

1. Test the behavior of these queries! Start the web application and go to <http://localhost:5000> in a browser. Click `Products` to list the data from the database. If you click on `Details` it will show the details of the selected product.

!!! error "If you do not see any product"
    If you see no items on this page, but there is no error, it is most likely due to a misconfigured database access. MongoDB will not return an error if the specified database does not exist. See the instructions for changing the connection details above.

## Creation

1. Implement the method `InsertProduct(Product product)`. The input is an instance of `Models.Product` that collects the information specified on the UI.

1. To create a new product, we will first create a new database entity (in memory first). This is an instance of class `Entities.Product`. There is no need to set the `ID` - the database will generate it. `Name`, `Price` and `Stock` are provided by the user. What is left is `VAT` and `CategoryID`. We should just hard-code values here: create a new VAT entity and find a random category using _Robo3T_ and copy the `_id` value.

    ```csharp
    var dbProduct = new Enites.Product
    {
        Name = product.Name,
        Price = product.Price,
        Stock = product.Stock,
        VAT = new Entities.VAT { Name = "General", Percentage = 20 },
        CategoryID = ObjectId.Parse("5d7e4370cffa8e1030fd2d99"),
    };
    // ... insertion
    ```

    Once the database entity is ready use `InsertOne` to add it to the database.

1. To test your code, start the application and click the `Add new product` link on the products page. You will need to fill the necessary data, and then the presentation layer will call your code.

## Delete

1. Implement method `DeleteProduct(string id)`. Use `DeleteOne` on the collection to delete the record. You will need a filter expression here to find the matching record similarly to how it was done in `FindProduct(string id)`.

1. Test the functionality using the web application by clicking the `Delete` link next to a product.

## Modification

1. We will implement the method `bool SellProduct(string id, int amount)` as a modification operation. The method shall return `true` if a record with a matching `id` is found, and there are at least `amount` pieces in stock. If the product is not found or there is not enough in stock return `false`.

1. Using the atomicity guarantees of MongoDB, we will perform the changes in a single step. A filter will be used to find both the `id` and check if there are enough items in stock. A modification will decrease the stock only if the filter is matched.

    ```csharp
    var result = productCollection.UpdateOne(
        filter: t => t.ID == ObjectId.Parse(id) && t.Stock >= amount,
        update: Builders<Enites.Product>.Update.Inc(t => t.Stock, -amount),
        options: new UpdateOptions { IsUpsert = false });
    ```

    Note that the `UpdateOptions` is used to signal that we do **NOT** want as upsert operation; instead, we want the operation to do nothing when the filter is not matched.

    The modification is assembled using `Update` in `Builders`. Here we want to decrease the stock value with `amount` (which is effectively and increase with `-amount`).

    We can determine what happened based on the `result` returned by the update operation. If the result indicates that the filter matched a record and the modification was performed, return `true`. Otherwise, return `false`.

    ```csharp
    return result.MatchedCount > 0;
    ```

1. Test the functionality using the web application by clicking the `Buy` link next to a product. Verify the behavior when you enter a too large amount!

!!! example "SUBMISSION"
    Create a **screenshot** of the web page listing the products **after successfully adding at least one new product**. Save the screenshot as `f1.png` and submit with the other files of the solution. The screenshot shall display the **list of products**. Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.
