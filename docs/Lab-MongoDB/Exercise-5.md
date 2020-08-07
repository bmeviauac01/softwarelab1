# Exercise 5: Optional exercise

**You can earn an additional +3 points with the completion of this exercise.** (In the evaluation, you will see the text "imsc" in the exercise title; this is meant for the Hungarian students. Please ignore that.)

We will group the orders in this exercise by date. We would like to see how our company performs by comparing the sales across time. We will use a [`$bucket`](https://docs.mongodb.com/manual/reference/operator/aggregation/bucket/) aggregation.

## Requirements

The method to implement is `OrderGroups GroupOrders(int groupsCount)`. This operation shall group the orders into `groupsCount` equal date ranges. The return value contains two values:

- `IList<DateTime> Thresholds`: The threshold dates of the date ranges.
    - The lower bound of the interval is inclusive, while the upper bound is exclusive.
    - When having `n` intervals, the `Thresholds` list has `n + 1` items
    - _E.g..: Let the `Thresholds` be `a, b, c, d`; the intervals shall then be: `[a, b[`, `[b, c[` and `[c, d[`._
- `IList<OrderGroup> Groups`: The groups that fall into each date range. The properties of `OrderGroup` are:
    - `Date`: The **start** date of the interval. E.g., for the interval `[a, b[` the value is`a`.
    - `Pieces`: The number of orders within the interval.
    - `Total`: The cumulative sum of the values of orders within this interval.

Further requirements:

1. There should be exactly `groupsCount` intervals.
    - The number of items in `Thresholds` thus will be **exactly** `groupsCount + 1`.
    - The number of items in `Groups` is **at most** `groupsCount` — no need for an item for intervals with no orders
1. The lower boundary should be the earliest date in the database
1. The upper boundary should be the latest date in the database + 1 hour
    - This is needed because the upper boundary is exclusive. It ensures that every item in the database falls into one of the intervals.
    - _Tip: add one hour to a date: `date.AddHours(1)`._
1. The intervals should be of equal size
    - _Tip: C# has built-in support for date arithmetic using dates and the `TimeSpan` classes._

You can assume the following:

- All orders in the database have `Date` values even though the type is _nullable_ `DateTime?`.
    - You can use `date.Value` to get the date without having to check `date.HasValue`.
- `groupsCount` is a positive integer **greater than or equal to 1**.

## Draft solution

1. Get the earliest and latest order dates from the database.

    - _Tip: You can execute two queries to get the values or a single aggregation._

1. Calculate the interval boundaries according to the requirements.

    - This will yield the `Thresholds` list for the return value.

1. Execute a `$bucket` aggregation on the orders collection. See the documentation [here](https://docs.mongodb.com/manual/reference/operator/aggregation/bucket/).

    - the `groupBy` expression will be the date of the order
    - `boundaries` expects the values as stated in the requirements; the list assembled in the previous step will work just fine
    - `output` should calculate the count and total value

    !!! tip "Tip"
        If you receive an error message `"Element '...' does not match any field or property of class..."` then in the `output` expression change every property to **lowercase** (e.g., `Pieces` -> `pieces`). It seems that the Mongo C# driver does not perform the required name transformations here.

1. The `$bucket` aggregation will yield the intervals according to the specification. You will only need to transform the results into instances of `OrderGroup` and produce the return value.

1. Use the `Group orders` link of the website to test your solution. A diagram will display the calculated information. Test your solution by changing the number of groups and adding orders in the past using the previously implemented `Add new order` functionality.

!!! example "SUBMISSION"
    Create a **screenshot** of the web page displaying the diagram. Save the screenshot as `f5.png` and submit with the other files of the solution. The screenshot shall show **both diagrams** (you may need to zoom out in the browser to fit them). Verify that your **Neptun code** is visible on the image at the bottom of the page! The screenshot is required to earn the points.
