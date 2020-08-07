# Optional exercises

**You can earn an additional +3 points with the completion of this exercise.**

## Exercise 16

Compare the following `Invoice`-`InvoiceItem` query: for each invoice item get the customer name.

```sql
SELECT CustomerName, Name
FROM Invoice JOIN InvoiceItem ON Invoice.ID = InvoiceItem.InvoiceID
```

Which _join_ strategy was chosen? Explain why the system chose it!

## Exercise 17

Compare the various JOIN strategies when querying all `Product`-`Category` record pairs.

!!! tip "Tip"
    Use [query hints](https://docs.microsoft.com/en-us/sql/t-sql/queries/hints-transact-sql-join) or the [option command](https://docs.microsoft.com/en-us/sql/t-sql/queries/option-clause-transact-sql) to explicitly specify the join strategy.

    Put the 3 queries (each with a different join strategy) into one execution unit (execute them together). This will give you the relative cost of each option.

Document the SQL commands you used and explain the actual query execution plan!
