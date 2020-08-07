# Exercise 2: Listing categories

**You can earn 4 points with the completion of this exercise.**

We will be listing available categories here with the number of products in each category. We will need an aggregation pipeline here. (Continue working in class `MongoLabor.DAL.Repository`.)

The method you should implement is `IList<Category> ListCategories()`. The method shall return all categories. Class `Models.Category` has 3 members.

- `Name`: the name of the category
- `ParentCategoryName`: the name of the parent category. If there is no parent, the value should be `null`.
- `NumberOfProducts`: number of products in this category. If there are no products, the value should be **0**.

The outline of the solution is as follows.

1. Create and initialize a new `productCollection` similar to how `categoryCollection` is initialized. The name of the collection is `categories` - you can verify this using _Robo3T_.

1. `ListCategories()` should first query all categories. Perform this similarly to how it was done in the previous exercise. Store the result set in variable `dbCategories`.

1. Query the number of products associated with each category (`Product.CategoryID`). Use an aggregation pipeline and a [`$group`](https://docs.mongodb.com/manual/reference/operator/aggregation/group/) step as follows.

    ```csharp
    var productCounts = productCollection
        .Aggregate()
        .Group(t => t.CategoryID, g => new { CategoryID = g.Key, NumberOfProducts = g.Count() })
        .ToList();
    ```

    This query yields a list where each item has a `CategoryID` and the number of associated products.

1. We have all information we need: all categories (including the parents) and the number of products for each. The final step is to "merge" the results in C# code.

    ```csharp
    return dbCategories
    .Select(c =>
    {
        string parentCategoryName = null;
        if (c.ParentCategoryID.HasValue)
            parentCategoryName = dbCategories.Single(p => p.ID == c.ParentCategoryID.Value).Name;

        var numProd = productCounts.SingleOrDefault(pc => pc.CategoryID == c.ID)?.NumberOfProducts ?? 0;
        return new Category { Name = c.Name, ParentCategoryName = parentCategoryName, NumberOfProducts = numProd };
    })
    .ToList();
    ```

    As seen above, this is performed using LINQ. 

    !!! note ""
        This is not the only solution to "join" collections in MongoDB. Although there is no `join` operation, there are ways to query data across collections. Instead of doing this in MongoDB, we do the merging in C# as above. This would not be good if the data set was large. Also, if there was filtering involved, the code above would be much more complicated.

1. Use the `Categories` link of the website to test your solution. This will list the data provided by your code in a tabular format. You can use the `Add new product` functionality from before to create new products. This must result in an increase in the number of products in one of the categories. (Remember that inserting the product hard-coded a category ID.)

!!! example "SUBMISSION"
    Create a **screenshot** of the web page listing the categories. Save the screenshot as `f2.png` and submit with the other files of the solution. The screenshot shall display the **list of categories**. Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.
