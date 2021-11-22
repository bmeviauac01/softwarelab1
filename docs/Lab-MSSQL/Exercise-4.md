# Exercise 4: Optional exercise

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
