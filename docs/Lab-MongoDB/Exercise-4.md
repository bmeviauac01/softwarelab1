# Exercise 4: Listing customers

**You can earn 4 points with the completion of this exercise.**

We will list the customers in this exercise, along with the cumulative value of their orders. This will be similar to exercise 2: we will use aggregation and merging in C# code.

The method to implement is `IList<Customer> ListCustomers()`. The method shall return every customer. The properties of `Model.Customer` are:

- `Name`: the name of the customer
- `ZipCode`, `City`, `Street`: the address fields of the main site of the customer
- `TotalOrders`: the cumulative total of all orders of the customer. You have to aggregate the price\*amount for each order of a customer to get this total. If a particular customer has no orders, this value shall be `null`.

Follow these steps to solve this exercise:

1. Create and initialize the `customerCollection`.

1. List all customers. The customer entity has the list of `Sites`; the main site is the item `MainSiteID` points to. Use this value to find the main in among the list.

1. In the collection of the orders, use an aggregation pipeline to calculate the total of all orders for each `CustomerID`.

1. Finally, you need the "merge" the two result sets. Every customer has a main site; however, not all of them have orders (in which case `TotalOrders` shall be `null`).

1. Use the `Customers` link of the website to test your solution. This will list the data provided by your code in a tabular format. You can use the `Add new order` functionality from before to create new orders. This must result in an increase in the total for one of the customers.

!!! example "SUBMISSION"
    Create a **screenshot** of the web page listing the customers. Save the screenshot as `f4.png` and submit it with the other files of the solution. The screenshot shall display the **list of customers**. Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.
