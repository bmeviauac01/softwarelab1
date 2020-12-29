# Exercise 3: Querying and modifying orders

**You can earn 5 points with the completion of this exercise.**

In this exercise, we will implement CRUD (create, retrieve, update, delete) operations for `Order` entities. This exercise is similar to the previous one; feel free to look back to the solutions of that exercise.

The properties of `Model.Order` are:

- `ID`: the `ID` of the database serialized using `ToString`
- `Date`, `Deadline`, `Status`: taken from the database directly
- `PaymentMethod`: taken from the `Method` field of the `PaymentMethod` complex entity
- `Total`: the cumulative sum of the product of `Amount` and `Price` for all items associated with this order (`OrderItems`)

You will need to implement the management methods related to orders: `ListOrders`, `FindOrder`, `InsertOrder`, `DeleteOrder` and `UpdateOrder`.

Before starting the tasks below, do not forget to add and initialize an `orderCollection` in the repository class similar to the other one.

## Listing

1. Method `ListOrders` receives a `string status` parameter. If this value is empty or `null` (see: `string.IsNullOrEmpty`) list all orders. Otherwise, list orders where the `Status` field is identical to the `status` received as parameter.

1. Method `FindOrder` returns the data of a single order identified by `string id`. If no record with the same `ID` exists, this method shall return `null`.

## Creation

1. Implement the method `InsertOrder`. The following information is provided to create the new order: `Order order`, `Product product`, and `int amount`.

1. You need the set the following information in the database entity:

    - `CustomerID`, `SiteID`: find a chosen `Customer` in the database and copy the values from this record from fields `_id` and `mainSiteId`. Hard-wire these values in code.
    - `Date`, `Deadline`, `Status`: take these values from the value received as `order` parameter
    - `PaymentMethod`: create a new instance of `PaymentMethod`. The `Method` should be `PaymentMethod` from the object received through the `order` parameter. Leave `Deadline` as `null`.
    - `OrderItems`: create a single item here with the following data:
        - `ProductID` and `Price`: take the values from the parameter `product`
        - `Amount`: copy value from the method parameter `amount`
        - `Status`: equals to the `Status` field of parameter `order`
    - other fields (related to invoicing) should be left as `null`!

## Delete

`DeleteOrder` should delete the record specified by the `ID`.

## Modification

When updating the record in `UpdateOrder`, only update the information that is present in `Models.Order`: `Date`, `Deadline`, `Status` and `PaymentMethod`. Ignore the value `Total`; it does not need to be considered in this context.

!!! tip ""
    You can combine multiple updates using `Builders<Entities.Order>.Update.Combine`.

Keep in mind that the `IsUpsert` property should be set to `false` in the update!

The method should return `true` if there was a record with a matching `ID`.

## Testing

You can test the functionalities using the `Orders` link in the test web app. Verify the behavior of `Filter`, `Add new order`, `Edit`, `Details`, and `Delete` too!

!!! example "SUBMISSION"
    Create a **screenshot** of the web page listing the orders **after successfully adding at least one new order**. Save the screenshot as `f3.png` and submit with the other files of the solution. The screenshot shall display the **list of orders**. Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.
