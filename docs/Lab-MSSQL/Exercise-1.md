# Exercise 1: Category view and data insertion

**You can earn 8 points with the completion of this exercise.**

## Create a view

Create a new `CategoryWithParent` view that lists the contents of the `Category` table as follows. It should have two columns: the `Name` of the category and the name of the parent category (or null if it does not exist).

Open a new _Query_ window. Make sure to select the right database. Create the view by executing the T-SQL command below.

```sql
create view CategoryWithParent
as
select c.Name CategoryName, p.Name ParentCategoryName
from Category c
left outer join Category p on c.ParentCategoryId = p.ID
```

Check the contents of the view!

![List the view](../images/sql-management-query-view.png)

## Insert via the view

Create a trigger with the name `InsertCategoryWithParent` that allows inserting a new category through the view (that is, by specifying the category name and the parent category name). It is not necessary to set a parent category, but if it is specified and there is no category with the provided name, an error should be raised and the operation aborted.

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

    - If a parent category name is provided, check whether any category with the same name as `@parentname` exist.

    - If not, raise and error and abort the trigger.

    - If everything is fine, insert the data into the `Category` table (and not the view... since the view does not store any data, hence this trigger).

    !!! example "SUBMISSION"
        Submit the code of the trigger in file `f1-trigger.sql`. The file should contain a single `create trigger` statement! Do not add `[use]` or `go` commands to the file! The correct behavior earns you 4 points.

1. Verify the correct behavior of the trigger! Write an insert statement that successfully inserts a new category record through the view. Then write an insert statement that fails.

    Suppose that the database is in its initial state: the categories in the table are the ones included in the initializer script. The two tests should _not_ depend on each other. Both shall produce the expected output regardless whether the other was executed before!

    !!! warning "Use simple names"
        It is recommended to use names (i.e. category names) that contain no special characters. Incorrect encoding of the SQL file might result in incorrect behavior otherwise. E.g. you may use the _LEGO_ category as a known existing category.

    !!! example "SUBMISSION"
        Write the test insert statements into files `f1-ok.sql` and `f1-error.sql`. Each file shall contain a single `insert` statement! They should not contain any `use` or `go` commands. Each file can earn you 2 points.
