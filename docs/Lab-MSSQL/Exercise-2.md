# Exercise 2: Validating invoices

**You can earn 6 points with the completion of this exercise.**

## Stored procedure

Create a store procedure with the name `CheckInvoice` that expects an `int` input parameter with the name `@invoiceid`.

- The procedure shall check the invoice corresponding to the provided id: check each `InvoiceItem` whether the `Amount` equals the amount on the corresponding `OrderItem`. (`InvoiceItem` directly references the corresponding `OrderItem`.)
- If there are any differences, print the amount values in both, and print the related product name as follows: `Difference: Ball (invoice 5 order 6)`
- The procedure should print any message only if an error was found. Do not leave test output in the submitted code!
- The procedure return value shall be an `int` equal to 0 when no discrepancies were found and 1 in case one was identified. This value should be `return`-ed at the end of the procedure (do not use an `output` parameter).

Use the `print` command for output as follows: `PRINT 'Text' + @variable + 'Text'` Any variable you print must be of character type. To convert a number to characters use: `convert(varchar(5), @variable)`, e.g. `PRINT 'Text' + convert(varchar(5), @variable)`

!!! example "SUBMISSION"
    Write the stored procedure code in file `f2-proc.sql`. The file should contain a single `create proc` statement! The correct behavior earns you 4 points. Partially incorrect behavior earns you partial points.

## Validate all invoices

Write T-SQL code that calls the procedure on all invoices. You should use a cursor to iterate all invoices.

The code shall print the ID of the invoice (e.g., `InvoiceID: 12`) before checking an invoice. If an invoice contains no discrepancies, print 'Invoice ok' before moving on to the next one. Check the _Messages_ pane under the query window for the messages.

!!! tip "Invoking a stored procedure"
    Invoking a stored procedure is performed with the `exec` statement:

    ```sql
    declare @checkresult int
    exec @checkresult = CheckInvoice 123
    ```

Verify the correct behavior of this code. You might need to alter a few records in the database to have any discrepancies. (The testing code does not need to be submitted.)

!!! example "SUBMISSION"
    Submit the code checking all invoices in file `f2-exec.sql`. The file shall contain the T-SQL code. It should not include the stored procedure nor any `use` or `go` commands. You can earn 2 points with the completion of this task.

!!! example "SUBMISSION"
    Create a screenshot of the output when a **discrepancy was found**. Save the screenshot as `f2.png` and submit it with the other files of the solution. The screenshot shall display the database name (which should be your **Neptun code**) in the _Object Explorer_ window and the **output messages** too.
