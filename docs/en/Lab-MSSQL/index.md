# MSSQL

In this lab, we will practice working with the server-side programming features of Microsoft SQL Server.

## Pre-requisites and preparation

Required tools to complete the tasks:

- Windows, Linux, or macOS: All tools are platform-independent, or a platform-independent alternative is available.
- Microsoft SQL Server
    - The free Express version is sufficient, or you may also use _localdb_ installed with Visual Studio
    - A [Linux version](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup) is also available.
    - On macOS, you can use Docker.
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms), or you may also use the platform-independent [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download) is
- Database initialization script: [mssql.sql](../db/mssql.sql)
- GitHub account and a git client

Materials for preparing for this laboratory:

- Using Microsoft SQL Server: [description](https://bmeviauac01.github.io/datadriven/en/db/mssql/)
- The [schema](https://bmeviauac01.github.io/datadriven/en/db/) of the database
- Microsoft SQL Server server-side programming and the SQL language
    - Check the materials of _Data-driven systems_ including the [seminars](https://bmeviauac01.github.io/datadriven/en/)

## Initial steps

Keep in mind that you are expected to follow the [submission process](../GitHub.md).

### Create and check out your Git repository

1. Create your git repository using the invitation link in Moodle. Each lab has a different URL; make sure to use the right one!

1. Wait for the repository creation to complete, then check out the repository.

    !!! tip ""
        If you are not asked for credentials to log in to GitHub in university computer laboratories when checking out the repository, the operation may fail. This is likely due to the machine using someone else's GitHub credentials. Delete these credentials first (see [here](../GitHub-credentials.md)), then retry the checkout.

1. Create a new branch with the name `solution` and work on this branch.

1. Open the checked-out folder and type your Neptun code into the `neptun.txt` file. There should be a single line with the 6 characters of your Neptun code and nothing else in this file.

### Create the database

1. Connect to Microsoft SQL Server using SQL Server Management Studio. Start Management Studio and use the following connection details:

    - Server name: `(localdb)\mssqllocaldb` or `.\sqlexpress` (which is short for: `localhost\sqlexpress`)
    - Authentication: `Windows authentication`

1. Create a new database (if it does not exist yet). The name should be your **Neptun code**: in _Object Explorer_ right-click _Databases_ and choose _Create Database_.

    !!! warning "IMPORTANT"
        The name of the database must be your **Neptun code**. You will need to submit screenshots that display the database name this way!

1. Create the sample database by executing the [initializer script](../db/mssql.sql) Open a new _Query_ window, paste the script into the window, then execute it. Make sure to select the correct database in the toolbar dropdown.

    ![Selecting the database](../images/sql-management-database-dropdown.png)

1. Verify that the tables are created. If the _Tables_ folder was open before, you need to refresh it.

    ![Listing tables](../images/sql-managment-tablak.png).

## Exercise 1: Category view and data insertion

**You can earn 8 points with the completion of this exercise.**

### Create a view

Create a new `CategoryWithParent` view that lists the `Category` table's contents as follows. It should have two columns: the `Name` of the category and the name of the parent category (or null if it does not exist).

Open a new _Query_ window. Make sure to select the correct database. Create the view by executing the T-SQL command below.

```sql
create view CategoryWithParent
as
select c.Name CategoryName, p.Name ParentCategoryName
from Category c
left outer join Category p on c.ParentCategoryId = p.ID
```

Check the contents of the view!

![List the view](../images/sql-management-query-view.png)

### Insert via the view

Create a trigger with the name `InsertCategoryWithParent` that allows inserting a new category through the view (that is, by specifying the category name and the parent category name). It is not necessary to set a parent category. Still, if it is specified and there is no category with the provided name, an error should be raised, and the operation aborted.

You will need an _instead of_ trigger that allows us to define how to insert the data. The skeleton of the trigger is provided below.

```sql
create trigger InsertCategoryWithParent -- name of the trigger
on CategoryWithParent -- name of the view
instead of insert    -- trigger code executed insted of insert
as
begin
  declare @newname nvarchar(255) -- variables used below
  declare @parentname nvarchar(255)

  -- using a cursor to navigate the inserted table
  declare ic cursor for select * from inserted
  open ic
  -- standard way of managing a cursor
  fetch next from ic into @newname, @parentname
  while @@FETCH_STATUS = 0
  begin
    -- check the received values available in the variables
    -- find the id of the parent, if specified
    -- throw error if anything is not right
    -- or insert the record into the Category table
    fetch next from ic into @newname, @parentname
  end

  close ic -- finish cursor usage
  deallocate ic
end
```

1. Finish this trigger by completing the code in the cycle.

    - If a parent category name is provided, check whether any category with the same name as `@parentname` exists.

    - If not, raise an error and abort the trigger.

    - If everything is fine, insert the data into the `Category` table (and not the view... since the view does not store any data, hence this trigger).

    !!! example "SUBMISSION"
        Submit the code of the trigger in file `f1-trigger.sql`. The file should contain a single `create trigger` statement! Do not add `[use]` or `go` commands to the file! The correct behavior earns you 4 points.

1. Verify the correct behavior of the trigger! Write an insert statement that successfully inserts a new category record through the view. Then write an insert statement that fails.

    Suppose that the database is in its initial state: the categories in the table are the ones included in the initializer script. The two tests should _not_ depend on each other. Both shall produce the expected output regardless of whether the other was executed before!

    !!! warning "Use simple names"
        It is recommended to use names (i.e., category names) that contain no special characters. Incorrect encoding of the SQL file might result in incorrect behavior otherwise. E.g., you may use the _LEGO_ category as a known existing category.

    !!! example "SUBMISSION"
        Write the test insert statements into files `f1-test-ok.sql` and `f1-test-error.sql`. Each file shall contain a single `insert` statement! They should not include any `use` or `go` commands. Each file can earn you 2 points.

## Exercise 2: Validating invoices

**You can earn 6 points with the completion of this exercise.**

### Stored procedure

Create a store procedure with the name `CheckInvoice` that expects an `int` input parameter with the name `@invoiceid`.

- The procedure shall check the invoice corresponding to the provided id: check each `InvoiceItem` whether the `Amount` equals the amount on the corresponding `OrderItem`. (`InvoiceItem` directly references the corresponding `OrderItem`.)
- If there are any differences, print the amount values in both, and print the related product name as follows: `Error: Ball (invoice 5 order 6)`
- The procedure should print any message only if an error was found. Do not leave test output in the submitted code!
- The procedure return value shall be an `int` equal to 0 when no discrepancies were found and 1 in case one was identified. This value should be `return`-ed at the end of the procedure (do not use an `output` parameter).

Use the `print` command for output as follows: `PRINT 'Text' + @variable + 'Text'` Any variable you print must be of character type. To convert a number to characters use: `convert(varchar(5), @variable)`, e.g. `PRINT 'Text' + convert(varchar(5), @variable)`

!!! example "SUBMISSION"
    Write the stored procedure code in file `f2-procedure.sql`. The file should contain a single `create proc` statement! The correct behavior earns you 4 points. Partially incorrect behavior earns you partial points.

### Validate all invoices

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
    Submit the code checking all invoices in file `f2-check-all.sql`. The file shall contain the T-SQL code. It should not include the stored procedure nor any `use` or `go` commands. You can earn 2 points with the completion of this task.

!!! example "SUBMISSION"
    Create a screenshot of the output when a **discrepancy was found**. Save the screenshot as `f2.png` and submit it with the other files of the solution. The screenshot shall display the database name (which should be your **Neptun code**) in the _Object Explorer_ window and the **output messages** too.

## Exercise 3: Denormalize invoices

**You can earn 6 points with the completion of this exercise.**

### New column

Update the `Invoice` table by adding a new `ItemCount` integer column that contains the number of items on the invoice (regarding the `InvoiceItems` records associated with each invoice).

!!! example "SUBMISSION"
    The code for adding the column shall be submitted in file `f3-column.sql`. The file shall contain a single `alter table` statement and should not include any `use` or `go` commands. You can earn 1 point with the completion of this task. This task is a prerequisite for the next ones.

Write T-SQL code block to fill this new column with the correct values.

!!! tip ""
    If an `Invoice` has an associated item with 2 red beach balls and another item with 1 tennis racket, then there are **3** items on this invoice. Note that it is invoices (and not orders) you have to consider here!

!!! example "SUBMISSION"
    Submit the code in file `f3-fill.sql`. The file shall contain a single T-SQL code block. Do not use stored procedures or triggers here, and the code should not have any `[use]` or `go` statements either. You can earn 1 point with the completion of this task.

### Maintaining the correct value

Create a trigger with the name `InvoiceItemCountMaintenance` that ensures the value in this new column is updated when an invoice or related items are updated. The trigger must be efficient! Re-calculating the number of items is not an acceptable solution. The trigger must also work correctly when multiple items are updated at the same time.

!!! tip "Tip"
    The trigger shall be on the `InvoiceItem` table despite the new column being in the `Invoice` table.

!!! warning "Important"
    Do not forget that triggers are executed **per statement** and not for each row; that is, your trigger will need to handle multiple changes in the implicit tables! The `inserted` and `deleted` implicit variables are **tables** must be treated as such.

!!! example "SUBMISSION"
    Submit the code of the trigger in file `f3-trigger.sql`. The file shall contain a single `create trigger` statement and should not contain any `use` or `go` commands. The correct behavior earns you 4 points. Partially incorrect behavior earns you partial points.

Verify the correct behavior of the trigger! The test code need not be submitted, but make sure to verify the behavior. Make sure to check the case when multiple records are modified with a single statement, e.g., execute an `update` without a `where` condition).

!!! example "SUBMISSION"
    Create a screenshot displaying the contents of the table `Invoice` with the `ItemCount` column and its correctly filled values. Save the screenshot as `f3.png` and submit it with the other files of the solution. The screenshot shall display the database name (which should be your **Neptun code**) in the _Object Explorer_ window and the **contents of the `Invoice` table**. The screenshot is required to earn the points of this part of the exercise.

## Exercise 4: Optional exercise

**You can earn an additional +3 points with the completion of this exercise.** (In the evaluation, you will see the text "imsc" in the exercise title; this is meant for the Hungarian students. Please ignore that.)

Query the `Categories` so that you get the following outcome:

| Name           | Count | Rank |
| -------------- | ----- | ---- |
| Building items |     3 |    1 |
| Months 0-6     |     2 |    2 |
| DUPLO          |     1 |    3 |
| LEGO           |     1 |    4 |
| Months 18-24   |     1 |    5 |
| Months 6-18    |     1 |    6 |
| Play house     |     1 |    7 |

The first column is the name of the category. The second column contains the number of products in this category. And finally, the third is the rank of the results based on the number of products in the category, descending; if the counts are equal, then the order is based on the name of the category ascending. The ranking should be continuous without gaps, and the final results should be ordered by this rank. The query should be a single statement. The name of the columns in the result set should be the ones you see above.

!!! tip "Tip"
    The fact that the third column is called "rank" should give you an idea.

!!! example "SUBMISSION"
    Submit the query in file `f4.sql`. The file shall contain a single `select` query without any `use` or `go` commands.

!!! example "SUBMISSION"
    Create a screenshot that shows the outcome of the query. Save the screenshot as `f4.png` and submit it with the other files of the solution. The screenshot shall display the database name (which should be your **Neptun code**) in the _Object Explorer_ window and the **query results**. The screenshot is required to earn the points.
