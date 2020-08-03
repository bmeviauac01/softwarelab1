# Exercise 4: Optional exercise

**You can earn an additional +3 points with the completion of this exercise.** (In the evaluation you will see the text "imsc" in the exercise title; this is meant for the Hungarian students. Please ignore that.)

The category system is hierarchical currently. We would like to simplify this.

Write T-SQL code block that deletes every category that is not a top-level one (has non-empty `ParentCategoryId`) after moving all products from this category to the parent. Since the category system is a tree with multiple levels you need to repeat this process as long as there are subcategories. You need to start from the bottom of the hierarch and in each iteration handle the categories that have parents, but are not parents themselves,

!!! example "SUBMISSION"
    Submit the code in file `f4.sql`. The file shall contain the T-SQL code. It should not contain any `use` or `go` commands. The solution should be a single T-SQL code block. Do not use stored procedures or triggers here. (There is no need to delete the `ParentCategoryId` column.)

!!! example "SUBMISSION"
    Create a screenshot that shows the `Category` table after executing the script. Save the screenshot as `f4.png` and submit with the other files of the solution. The screenshot shall display the database name in the _Object Explorer_ windows (which should be your **Neptun code**) and the **contents of the `Category` table** too. The screenshot is required to earn the points.
