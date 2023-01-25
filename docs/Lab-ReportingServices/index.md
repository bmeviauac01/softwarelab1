# Reporting Services

In this lab, we will work with _Microsoft SQL Server Reporting Services_, a tool we have not seen before. We will start working together, then some of the exercises will be individual work. You shall submit the solution to all exercises.

## Pre-requisites and preparation

Required tools to complete the tasks:

- A PC with Windows.
- Microsoft SQL Server: The free Express version is sufficient, or you may also use _localdb_ installed with Visual Studio
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Database initialization script: [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip)
- Microsoft Visual Studio 2019 (2022 does **not** work here): The free Community edition is sufficient
- Report Server Project support for Visual Studio: [Microsoft Reporting Services Projects extension](https://marketplace.visualstudio.com/items?itemName=ProBITools.MicrosoftReportProjectsforVisualStudio) (Keep the extension [up to date](https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-update-a-visual-studio-extension?view=vs-2019).)
- GitHub account and a git client

Materials for preparing for this laboratory:

- Using Microsoft SQL Server: [description](https://bmeviauac01.github.io/datadriven-en/db/mssql/) and [video](https://web.microsoftstream.com/video/98a6697d-daec-4a5f-82b6-8e96f06302e8)
- SQL Reporting Services [official tutorial](https://docs.microsoft.com/en-us/sql/reporting-services/create-a-basic-table-report-ssrs-tutorial)

## Initial steps

Keep in mind that you are expected to follow the [submission process](../GitHub.md).

### Create and check out your Git repository

1. Create your git repository using the invitation link in Moodle. Each lab has a different URL; make sure to use the right one!

1. Wait for the repository creation to complete, then check out the repository.

    !!! tip ""
        If you are not asked for credentials to log in to GitHub in university computer laboratories when checking out the repository, the operation may fail. This is likely due to the machine using someone else's GitHub credentials. Delete these credentials first (see [here](../GitHub-credentials.md)), then retry the checkout.

1. Create a new branch with the name `solution` and work on this branch.

1. Open the checked-out folder and type your Neptun code into the `neptun.txt` file. There should be a single line with the 6 characters of your Neptun code and nothing else in this file.

### Create the Adventure Works 2014 database

We will work with the _Adventure Works_ sample database. This database contains the operational information of a fictional retail company. Instead of understanding the database contents, we will use a few predefined queries that list product purchases.

1. Download [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip) and extract it to folder `C:\work\Adventure Works 2014 OLTP Script` (create the folder if it does not exist yet).

    !!! important ""
        The folder name should be as above; otherwise, you need to change the path in the sql script:

        ```sql
        -- NOTE: Change this path if you copied the script source to another path
        :setvar SqlSamplesSourceDataPath "C:\work\Adventure Works 2014 OLTP Script\"
        ```

        If you need to edit the path, make sure to keep the trailing slash!

1. Connect to Microsoft SQL Server using SQL Server Management Studio. Use the following connection details.

    - Server name: `(localdb)\mssqllocaldb` when using LocalDB, or `localhost\sqlexpress` when using SQL Express
    - Authentication: `Windows authentication`

1. Use _File / Open / File..._ to open `instawdb.sql` from the folder created above. **Do not execute it yet!** First, you should turn on SQLCMD mode: in the _Query_ menu click _SQLCMD Mode_; then click _Execute_.

    ![SQLCMD mode](../images/sql-management-sqlcmd-mode.png)

1. Verify whether the database and its contents are created. Select _Databases_ in the _Object explorer_ on the left and click _Refresh_. The _AdventureWorks2014_ database shall appear with a number of tables inside.

    ![AdventureWorks database tables](../images/reportingservices/rs-adventureworks-tablak.png).

1. Open a new SQL Query window on this database (right-click the database and choose _New query_), and execute the following script **with your own Neptun code** substituted:

    ```sql
    update Production.Product set Name='NEPTUN'+Name
    ```

    Check the contents of the table `Production.Product` and verify if it has your Neptun code in the product names: right-click the table and choose _Select top 1000 rows_.

    !!! warning "IMPORTANT"
        Your Neptun code must be listed in the names. You will need to create screenshots in the following exercises, and your Neptun code **must** appear on these images.

## Exercise 1: Table report

This exercise is **solved together** with the lab instructor.

In the checked-out repository, locate file `reportserver.sln` and open it with Visual Studio. This is an empty _Report Server_ project.

The Report Server project consists mainly of _Report Definition_ (.rdl) files, that define the data sources (queries) and a template, which, when rendered, produces the final result: a report. These reports can be installed to a _Report Server_ and executed there, providing the users with up-to-date data.

!!! note ""
    In this lab, we will not use the Report Server. This is mainly because the configuration would require administrative privileges that we do not have in the labs. Therefore, we will preview the reports in Visual Studio.

### Create the first Report Definition file

1. In the _Solution Explorer_ right-click _Reports_ and choose _Add_ > _New Item_.

    ![Adding a new report](../images/reportingservices/rs-add-new-report.png)

1. Choose the _Report_ type from among the listed templates. Call it _Sales Orders.rdl_, then click Add.

    ??? fail "If adding the new report file fails"
        In certain Visual Studio and Report Server project versions adding this new report file might fail. If this happens, follow these steps instead.

        1. Download [this empty rdl file](empty.rdl).
        1. Save the file with the correct name to the `reportserver` folder of your repository (this folder already exist).
        1. In Visual Studio right-click _Reports_ then choose _Add_ > _Existing Item_, and browse for this file.

1. Open the report file to get the Report Designer view. Here, the new .rdl file is displayed in the Design view.

    ![Report Desinger](../images/reportingservices/rs-report-designer.png)

    This is our development view. The Report Designer has two views: _Design_ and _Preview_. A panel called Report Data (on the left) is also opened. Here, we can define data sources. If the data sources are set, we can create the report on the _Design_ tab and check how it would look like on the _Preview_ tab.

### Configuring the data source

A data source defines where our data comes from. This will be the SQL Server database created before.

1. Using the _Report Data_ pane, click _New_ > _Data Source_. The name shall be "AdventureWorks2014".

    ![Add datasource](../images/reportingservices/rs-add-datasource.png)

1. Choose _Microsoft SQL Server_ as the connection type and click the button to the right of _connection string_ to configure the database access

    - Server name: `(localdb)\mssqllocaldb`
    - Authentication: `Windows Authentication`
    - Select or enter database name: `AdventureWorks2014`

1. Click OK to close the dialog. Then **re-open** the Data Source settings from the Report Data panel by right-clicking on the newly created data source, opening its properties, and then going to the _Credentials_ page. The following checkbox has to be checked:

    ![Data source credentials configuration](../images/reportingservices/rs-data-source-properties.png)

### Configuring a data set

The next step is the configuration of a dataset. Practically, this means executing a query in the database.

1. Using the _Report Data_ pane, click _New_ > _Data Set_. Call the dataset "AdventureWorksDataset". Select the data source created before from the dropdown, then apply the following settings:

    ![Data set properties](../images/reportingservices/rs-data-set-properties.png)

1. Copy the following query.

    ```sql
    SELECT
    soh.OrderDate AS [Date],
    soh.SalesOrderNumber AS [Order],
    pps.Name AS Subcat, pp.Name as Product,
    SUM(sd.OrderQty) AS Qty,
    SUM(sd.LineTotal) AS LineTotal
    FROM Sales.SalesPerson sp
    INNER JOIN Sales.SalesOrderHeader AS soh
          ON sp.BusinessEntityID = soh.SalesPersonID
    INNER JOIN Sales.SalesOrderDetail AS sd
          ON sd.SalesOrderID = soh.SalesOrderID
    INNER JOIN Production.Product AS pp
          ON sd.ProductID = pp.ProductID
    INNER JOIN Production.ProductSubcategory AS pps
          ON pp.ProductSubcategoryID = pps.ProductSubcategoryID
    INNER JOIN Production.ProductCategory AS ppc
          ON ppc.ProductCategoryID = pps.ProductCategoryID
    GROUP BY ppc.Name, soh.OrderDate, soh.SalesOrderNumber,
             pps.Name, pp.Name, soh.SalesPersonID
    HAVING ppc.Name = 'Clothing'
    ```

    Click _Refresh fields_ when ready.

    !!! note ""
        We have a Query Designer where the query can be created with visual aids. We will not use it, but it is available.

    Click OK to close the dialog.

### Table report (5p)

Now that we have our connection to the database and the query that will supply the data, let us create a report. A report is basically data from the database displayed in a table or with diagrams.

1. Open the _Toolbox_ pane. You can do this from the _View_ menu.

1. From the _Toolbox_ choose _Table_ and draw a table on the big, empty and white canvas on the Design tab:

    ![Adding a table](../images/reportingservices/rs-add-table.png)

1. Switch back to the _Report Data_ pane and expand the AdventureWorksDataset:

    ![Dataset fields](../images/reportingservices/rs-dataset-fields.png)

    !!! info ""
        If the node is empty or cannot be opened, you need to re-open the data set properties using right-click, then clicking the _Refresh Fields_ button.

1. Drag the _Date_ field to the first column of the table. It should look like this:

    ![Added the date column](../images/reportingservices/rs-table-add-date-col.png)

    !!! note ""
        The `[Date]` in the second row shows the expression to evaluate, while "Date" in the first row is the literal header label - we can change it too.

1. Similarly, add _Order_ and _Product_ to the second and third columns. Add _Qty_ as well: drag it to the right side of the last column; the cursor icon will change to + sign, and a blue line at the end of the table will appear. This will add a new fourth column. Add _LineTotal_ similarly into the fifth column.

    ![Additional columns](../images/reportingservices/rs-table-add-order-product-qty-col.png)

1. The first report is almost ready. Let us check how it looks like using the Preview tab. Note that it might take a while for it to open the first time. It will be faster the second time. Verify that your Neptun code appears in the table content! (If not, you forgot a preparation step. Go back, and repeat the steps!)

    ![Report preview](../images/reportingservices/rs-table-preview-1.png)

    We can print or export the report into various formats (e.g., Word, Excel, PDF). However, this report is not very pretty, e.g., the currency is not displayed, and the Qty and date columns are not formatted, etc. 

1. Go back to _Design_ tab, right-click the `[Date]` expression, and select Text Box Properties. Navigate to the _Number_ page, select the _Date category_, and choose a date format you like.

    ![Date formatting](../images/reportingservices/rs-table-date-col-properties.png)

1. Right-click `[LineTotal]`, use Text Box Properties again, and select the _Currency_ option in _Number_.

    ![Line total formatting](../images/reportingservices/rs-table-linetotal-col-properties.png)

1. By moving the mouse over the gray boxes at the top of the table header, the cursor changes to resize mode. (Just like you would resize a table in Word.) Use this to resize the entire table, and the columns (_Qty_ and _Line Total_ can be narrower, while the others might need more space).

    Finally, emphasize the header row. Select the whole row (by clicking the gray rectangle on the left end of the row) and click _Bold_ on the ribbon.

    ![Format the header row](../images/reportingservices/rs-table-bolt-header-row.png)

    If you check the preview, it should look like this:

    ![Riport preview](../images/reportingservices/rs-table-preview-2.png)

!!! example "SUBMISSION"
    _If you are continuing with the next exercise, you may omit to create the screenshot here._

    Create a screenshot of the **report preview** page. Save the screenshot as `f1.png` and submit it with the other files of the solution. The screenshot shall include Visual Studio and the report preview. Verify that your **Neptun code** is visible!

    Upload the changed Visual Studio project and its corresponding files too.

### Grouping and total value (5p)

The report we created is very long, and it contains everything without structure. These are retail sales information: the amount of products sold each day. Let us group the data.

1. Go back to _Design_ tab. Make sure that we see the _Row Groups_ pane below the table. If it is not there, right-click the design area and select _Grouping_ in the _View_ menu.

1. Drag the _Date_ field from _Report Data_ to the _Row Groups_ pane above the _(Details)_ row.

    ![Grouping by date](../images/reportingservices/rs-group-by-date.png)

    The table will look like this:

    ![Grouping preview](../images/reportingservices/rs-group-by-date-table-designer.png)

1. Drag field _Order_ into _Row Groups_ between _Date_ and _(Details)_.

    ![Grouping by order](../images/reportingservices/rs-group-by-order.png)

1. Now there are duplicate columns in the table. Let us delete these. Select the rightmost _Date_ and _Order_ columns by clicking on the gray boxes above them. Delete them by right click and _Delete Columns_.

    ![Duplicate columns](../images/reportingservices/rs-group-by-duplicated-columns.png)

    Unfortunately, the new _Date_ column format is now lost, but you can set it again as previously.

    Check the _Preview_ now, and see that the table is now ordered and grouped as we specified it.

    ![Table with grouping](../images/reportingservices/rs-table-preview-3.png)

1. Go back to the _Design_ view. Right-click the `[LineTotal]` cell and click _Add Total_. This will add a total for each _Order_ (which we used for grouping). There will be no label added to this line. Add one by left-clicking the cell and typing: "Order Total".

    ![Order total](../images/reportingservices/rs-add-total-order.png)

1. Holding the CTRL key pressed down, click _Order Total_, and the two cells to the right to select them all. Then set a background color by choosing one from the _Format_ menu.

    ![Background color for the total](../images/reportingservices/rs-add-total-order-color.png)

1. Check the preview of the report now.

    ![Preview](../images/reportingservices/rs-table-preview-4.png)

1. Let us create a daily total as well!

    - Go back to the _Design_ view.
    - Right-click on the `[Order]` cell and click _Add Total_ > _After_.
    - A new cell (Total) appears below `[Order]`. Click in it and change the label to "Daily Total".
    - Select the cell and the three right next to it (e.g., by using CTRL and clicking them) and change their background color (_Format_ > _Background color_).

1. Since there are quite a few orders per day, you may need to scroll down 4-5 pages to check the result in the preview:

   ![Daily total](../images/reportingservices/rs-table-preview-5.png)

!!! example "SUBMISSION"
    Create a screenshot of the **report preview** page. Save the screenshot as `f1.png` and submit it with the other files of the solution. The screenshot shall include Visual Studio and the report preview, including the **lines showing the totals** (a turn a few pages if needed to see one). Verify that your **Neptun code** is visible!

    Upload the changed Visual Studio project and its corresponding files too.

## Exercise 2: Visualization

**This exercise is to be completed individually for 5 points.**

The table report shows all sales. That is not useful to get a quick overview. A diagram would be more helpful. Create a diagram that displays the sales per category.

### Inserting a diagram

1. Switch to _Design_ tab and drag a _Chart_ from the _Toolbox_ to the canvas to the table's right side.  You might have to wait a while for the diagram wizard to open. Choose a column diagram.

1. Drag _LineTotal_ from the _Report Data_ pane to the diagram. **Do not release the left mouse button.** A window will appear beside the diagram. The window is titled _Chart Data_. Go to the "∑values" field with your mouse. Release the left mouse button now.

    ![Adding a chart](../images/reportingservices/rs-chart-data.png)

    This makes the totals be displayed on the vertical axis.

1. Next, drag _Subcat_ field into _Category Groups_ and _Date_ into _Series Groups_.

    ![Diagram configuration](../images/reportingservices/rs-chart-values.png)

    The horizontal axis displays the categories, and we get separate columns per date series.

1. Right, click label `[Date]` and select _Series Groups Properties_. Click on the **_fx_** button in _Group Expressions_.

    ![Set Expression](../images/reportingservices/rs-chart-group-expression.png)

    The following expression: `=Year(Fields!Date.Value)`

    ![Expression value](../images/reportingservices/rs-chart-group-expression2.png)

    This will produce a yearly breakdown per category.

1. Press OK in both windows. Before checking the _Preview_, increase the height of the chart; otherwise, the labels at the bottom will remain hidden:

    ![Resize diagram](../images/reportingservices/rs-chart-resize.png)

1. Check the preview now: it shows the sales per category for each year separately.

    ![Diagram preview](../images/reportingservices/rs-chart-preview-1.png)

### Format the diagram

There are a few changes to make.

1. Click _Chart title_ and replace the title: "Revenue by category NEPTUN" **with your own Neptun code**.

1. Right-click `<<Expr>>` in _Series Groups_ and choose _Series Groups Properties_. Click on the **_fx_** button next to _Label_. Type: `=Year(Fields!Date.Value)`. Now the label will be the year part of the date.

1. Right-click the vertical axis and select _Vertical Axis Properties_.

    ![Axis properties](../images/reportingservices/rs-y-axis-properties.png)

    Select _Currency_ from the _Number_ group and fill out as previously:

    ![Axis formatting](../images/reportingservices/rs-y-axis-properties-currency.png)

1. Check the preview now: the diagram will look much better (and also has your Neptun code):

    ![Preview](../images/reportingservices/rs-chart-preview-2.png)

!!! example "SUBMISSION"
    Create a screenshot of the **report preview** page. Save the screenshot as `f2.png` and submit it with the other files of the solution. The screenshot shall include Visual Studio and the report preview. Verify that your **Neptun code** is visible in the diagram title!

    Upload the changed Visual Studio project and its corresponding files too.

## Exercise 3: Sales personnel report

**This exercise is to be completed individually for 5 points.**

Let us create a new report about the effectiveness of the sales personnel.

### Extending the data set

The dataset we created in the Report Data panel has to be extended with further information.

1. Open the data set properties by right-clicking on _AdventureWorksDataSet_ in the _Report Data_ pane, click _Dataset properties_ then change the SQL command to:

    ```sql hl_lines="7 9 17"
    SELECT
      soh.OrderDate AS [Date],
      soh.SalesOrderNumber AS [Order],
      pps.Name AS Subcat, pp.Name as Product,
      SUM(sd.OrderQty) AS Qty,
      SUM(sd.LineTotal) AS LineTotal
     , CONCAT(pepe.FirstName, ' ', pepe.LastName) AS SalesPersonName
    FROM Sales.SalesPerson sp
      INNER JOIN Person.Person as pepe ON sp.BusinessEntityID = pepe.BusinessEntityID
      INNER JOIN Sales.SalesOrderHeader AS soh ON sp.BusinessEntityID = soh.SalesPersonID
      INNER JOIN Sales.SalesOrderDetail AS sd ON sd.SalesOrderID = soh.SalesOrderID
      INNER JOIN Production.Product AS pp ON sd.ProductID = pp.ProductID
      INNER JOIN Production.ProductSubcategory AS pps ON pp.ProductSubcategoryID = pps.ProductSubcategoryID
      INNER JOIN Production.ProductCategory AS ppc ON ppc.ProductCategoryID = pps.ProductCategoryID
    GROUP BY ppc.Name, soh.OrderDate, soh.SalesOrderNumber,
             pps.Name, pp.Name, soh.SalesPersonID
            , pepe.FirstName, pepe.LastName
    HAVING ppc.Name = 'Clothing'
    ```

    Don't forget to press _Refresh fields_ before closing the dialog.  Close the dialog.

1. In the _Report data_ expand _AdventureWorksDataset_ (if already expanded, close then re-expand). A new field _SalesPersonName_ will appear.

1. Right-click the data source _AdventureWorks2014_ and choose _Convert to shared Data Source_, and then repeat this with the _AdventureWorksDataSet_ too. These will enable us to use them in a new report.

### New report and data sources

We will use the converted and shared data source and data set in a new report.

1. In the _Solution Explorer_ right-click _Reports_ and choose _Add_ > _New Item_ > _Report_. The name of this new report should be "Sales People".

1. Open the new report. There are no data sources associated with the report. Use the _Report Data_ pane to add the existing one:

    - Right-click _Data Source_ and choose _Add Data Source_

    - Click the _Use shared data source reference_ option and select "AdventureWorks2014".

        ![Shared data source](../images/reportingservices/rs-add-datasource-shared.png)

    - Right lick _Datasets_ > _Add Dataset_

    - Click _Use a shared dataset_ and select the existing AdventureWorksDataset

        ![Shared dataset](../images/reportingservices/rs-add-dataset-shared.png)

### Contents of the report

Create a tabular report containing the sales persons and their activity. Group by product category and sales person. Add a total line for each person. Make sure to set appropriate formatting of numbers.

The key is creating the table and grouping as below. The category is the field _Subcat_.

![Recommended groups](../images/reportingservices/rs-sales-person-groups.png)

The final report should look like this:

![Preview](../images/reportingservices/rs-sales-person-total.png)

!!! tip "Tip"
    You should use the _Add total_ > _After_ just like before. But you should do this by clicking on `[Subcat]` and **not**`[SalesPersonName]`! (If you click SalesPersonName to add the total, it will be a "grand total" adding up all persons.)

!!! example "SUBMISSION"
    Create a screenshot of the **report preview** page. Save the screenshot as `f3.png` and submit it with the other files of the solution. The screenshot shall include Visual Studio and the report preview. Verify that your **Neptun code** is visible in the table!

    Upload the changed Visual Studio project and its corresponding files too.

## Exercise 4: Optional exercise

**You can earn an additional +3 points with the completion of this exercise.**

Create a pie chart to compare the sales persons' performance according to the total value of purchases achieved by them!  **Put your Neptun code into the pie chart title!** The goal is to get a diagram similar to the one below.

![Expected chart](../images/reportingservices/rs-sales-person-pie-chart.png)

!!! tip "Tip"
    The pie chart is similar to the column diagram. The _∑ Values_ should be the _LineTotal_, and _Category Groups_ is the _SalesPersonName_. (_Series Groups_ will remain empty this time.)

    Make sure that the legend shows the names. You might need to adjust the size of the diagram to have enough space.

    ![Values to be used in the pie chart](../images/reportingservices/rs-sales-person-pie-char-valuest.png)

!!! example "SUBMISSION"
    Create a screenshot of the **report preview** page. Save the screenshot as `f4.png` and submit it with the other files of the solution. The screenshot shall include Visual Studio and the report preview. Verify that your **Neptun code** is visible in the diagram title!

    Upload the changed Visual Studio project and its corresponding files too.
