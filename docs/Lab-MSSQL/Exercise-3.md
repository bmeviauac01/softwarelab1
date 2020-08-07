# Exercise 3: Denormalize invoices

**You can earn 6 points with the completion of this exercise.**

## New column

Update the `Invoice` table by adding a new `ItemCount` integer column that contains the number of items on the invoice (regarding the `InvoiceItems` records associated with each invoice).

!!! example "SUBMISSION"
    The code for adding the column shall be submitted in file `f3-column.sql`. The file shall contain a single `alter table` statement and should not include any `use` or `go` commands. You can earn 1 point with the completion of this task. This task is a prerequisite for the next ones.

Write T-SQL code block to fill this new column with the correct values.

!!! tip ""
    If an `Invoice` has an associated item with 2 red beach balls and another item with 1 tennis racket, then there are **3** items on this invoice. Note that it is invoices (and not orders) you have to consider here!

!!! example "SUBMISSION"
    Submit the code in file `f3-fill.sql`. The file shall contain a single T-SQL code block. Do not use stored procedures or triggers here, and the code should not have any `[use]` or `go` statements either. You can earn 1 point with the completion of this task.

## Maintaining the correct value

Create a trigger with the name `InvoiceItemCountMaintenance` that ensures the value in this new column is updated when an invoice or related items are updated. The trigger must be efficient! Re-calculating the number of items is not an acceptable solution.

!!! tip "Tip"
    The trigger shall be on the `InvoiceItem` table despite the new column being in the `Invoice` table.

!!! warning "IMPORTANT"
    Do not forget that triggers are executed **per statement** and not for each row, that is, your trigger will need to handle multiple changes in the implicit tables! The `inserted` and `deleted` **tables** must be treated as such.

!!! example "SUBMISSION"
    Submit the code of the trigger in file `f3-trigger.sql`. The file shall contain a single `create trigger` statement and should not contain any `use` or `go` commands. The correct behavior earns you 4 points. Partially incorrect behavior earns you partial points.

Verify the correct behavior of the trigger! The test code need not be submitted, but make sure to verify the behavior. Make sure to check the behavior when multiple records are modified with a single statement, e.g., execute an `update` without a `where` condition).

!!! example "SUBMISSION"
    Create a screenshot displaying the contents of the table `Invoice` with the `ItemCount` column and its correctly filled values. Save the screenshot as `f3.png` and submit with the other files of the solution. The screenshot shall display the database name in the _Object Explorer_ windows (which should be your **Neptun code**) and the **contents of the `Invoice` table**. The screenshot is required to earn the points.
