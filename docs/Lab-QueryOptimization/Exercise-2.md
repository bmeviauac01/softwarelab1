# Individual exercises

!!! example "SUBMISSION"
    Continue documenting the results the same way.

## Exercise 6 (1p)

Repeat the queries on the `Product` table, but instead of fetching the entire record, only get the `ID` and the `Price` columns. How do the execution plans change? Explain the differences!

## Exercise 7 (1p)

Analyze the following queries executed on the `Product` table:

- k) query records where the primary key is between two values (use the `BETWEEN` operator)
- k) query records where the primary key is between two values (use the `BETWEEN` operator), **or** it is equal to a value that falls outside of this range

Document the SQL commands you used and explain the actual query execution plan!

## Exercise 8 (1p)

In Exercises 6 `WHERE Price=` compared an integer and a floating-point number. Let us experiment with other comparisons: query records from the `Product` table with the following filter criteria.

- m) `where cast(Price as int) = integer number`
- n) `where Price BETWEEN integer number-0.0001 AND integer number+0.0001`

Choose a random integer number in these queries and fetch **only the primary key**. Analyze the execution plans.

## Exercise 9 (1p)

Analyze the following queries execute on the `Product` table:

- o) query entire records where `Price` is less than a constant value (the filter should yield few matches), ordered by ID descending
- p) the same, but fetch only `ID` and `Price`
- q) query entire records where `Price` is greater than a constant value (the filter should yield many matches), ordered by ID descending
- r) the same, but fetch only `ID` and `Price`

Document the SQL commands you used and explain the actual query execution plan!

## Exercise 10 (1p)

Create a new index for the `Name` column and analyze the following queries executed on the `Product` table:

- s) query names and IDs where the name begins with Z - use function [`SUBSTRING`](https://docs.microsoft.com/en-us/sql/t-sql/functions/substring-transact-sql)
- t) the same, but now using [`LIKE`](https://docs.microsoft.com/en-us/sql/t-sql/language-elements/like-transact-sql)
- u) query names and IDs where the name contains a Z (LIKE)
- v) query the ID of a product where the name equals (=) a string
- w) the same, but now compare case-insensitively using [`UPPER`](https://docs.microsoft.com/en-us/sql/t-sql/functions/upper-transact-sql?view=sql-server-ver15)

Document the SQL commands you used and explain the actual query execution plan!

## Exercise 11 (1p)

Analyze the following queries executed on the `Product` table:

- x) get the maximum of `Id`
- y) get the minimum of `Price`

Document the SQL commands you used and explain the actual query execution plan!

## Exercise 12 (1p)

Query the number of products per category (`CategoryId`).

Document the SQL commands you used and explain the actual query execution plan!

## Exercise 13 (1p)

How could we improve on the performance of the previous query? Explain and implement the solution and repeat the previous query.

!!! tip "Tip"
    You need to add a new index. The question is: to which column?

## Exercise 14 (1p)

List the `Name` of each `Product` where `CategoryId` equals 2.

Document the SQL commands you used and explain the actual query execution plan! Explain whether the index added in the previous exercises helps the performance.

## Exercise 15 (1p)

Improve the performance of the previous query. Extend the index added before by including the name: right-click the index -> _Properties_ -> and add `Name` to _Included columns_.

Repeat the previous query and analyze the plan.
