# Power BI

During the lab, we get acquainted with a new tool called _Microsoft Power BI_. We will start working together, then some of the exercises will be individual work. You shall submit the solution to all exercises.

## Prerequisites, preparation

Tools needed to perform the lab:

- Windows
- Power BI Desktop
- GitHub account and a git client
- Microsoft365 account (***@edu.bme.hu)

Auxiliary materials and preparation materials that can be used to perform the laboratory:

- Power BI platform: [documentation](https://learn.microsoft.com/en-us/power-bi/)

## Preparation

When solving tasks, do not forget to follow the[task submission process](../GitHub.md).

### Create and Download Git Repository

1. In Moodle, find the URL of the invitation to the lab and use it to create your own repository.

1. Wait until the repository is ready, and then checkout-old out.

    !!! tip ""
    In university labs, if the system does not ask for a username and password during the checkout, and the checkout fails, then the system probably tried using a previously memorized username on the machine. First, delete the saved login data [see here](../GitHub-credentials.md) and try again.

1. Create a new branch called `solution` and work on it.

1. Enter your Neptun code in the `neptun.txt` file. The file should contain nothing but 6 characters of the Neptun code in a single line.

1. Enter your ***@edu.bme.hu email address in the `eduid.txt` file. Do not include anything else in the file except the email address in a single line.

### Free Power BI registration

1. We will use the free subscription level of Power BI. Use of this is subject to pre-registration. Please visit [Power BI's homepage](https://powerbi.microsoft.com/) login with your ***@edu.bme.hu email address and click on the free registration button. Upon successful registration, you will receive the following message:

    ![Registration successful](../images/powerbi/pb-register-licenseassigned.png)

1. Registration takes us to [Power BI's Website](https://app.powerbi.com/). Let's try it out.

### Install PowerBI Desktop

We will use the Power BI Desktop application to create Power BI reports. It is pre-installed on lab machines, it probably needs to be installed on your own device.

1. Make sure that you have the Power BI Desktop app installed on your computer, and if you do, you don't need to follow the steps below.

    Power BI Desktop is the most convenient to install from the Microsoft Store. This is also practical in laboratory machines because it does not require administrative privileges. You can also choose [the downloadable installer](https://www.microsoft.com/en-us/download/details.aspx?id=58494) for your machine.

1. Open [Power Bi Desktop App in the Microsoft Store](https://aka.ms/pbidesktopstore) and choose installation! You do not need to log in to the Store to do the installation.

    ![Setup](../images/powerbi/pb-install-store.png)

## 1. Task: Spreadsheet Report (5p)

This exercise is solved **together** with the lab instructor.

Generating Power BI reports usually follows a typical workflow with the following steps:

![Power BI Workflow](../images/powerbi/pb-intro-workflow.png)

In the process, we will use the **Power BI Desktop** application to design the reports, while publishing and sharing the reports will be done with **Power BI Service**.

### Create the first dataset

In Power BI, we can import data from about 150 different data sources, either for a single load or for continuous queries over a live connection. For the sake of simplicity, the data to be processed in this lab will be obtained from a version of the relational database exported to Excel, which will be registered in the next steps.

1. Download the database from[AdventureWorksSales.xlsx](AdventureWorksSales.xlsx). Open the file, get acquainted with the data in it!

1. Launch the Power BI desktop application. Log in with your own ***@edu.bme.hu user.

    ![Belépés](../images/powerbi/pb-install-signin.png)

!!! tip "Hint"
Power BI Desktop can be used without logging in, but publishing will require a logged in account. If you work on a shared (lab) computer, don't forget to log out at the end of the lab!

1. Close the pop-up dialogs and save the project (File/Save) in any directory.**Project name should be your Neptun code**!

!!! tip "Hint"
Everything we do in Power BI Desktop will go into this project file (NEPTUN.pbix). The project file must also be submitted together with screenshots of the solution.

1. Load the previously downloaded _AdventureWorksSales.xlsx_ file. (_Get data_ /_Excel workbook_)

    ![Loading the database](../images/powerbi/pb-load-excel.png)

1. Select all the data tables (no _data_ postfix in the name) and press the _Load_ button.

    ![Select tables](../images/powerbi/pb-load-tables.png)

1. You can check the results in the _Data view_ and _Model view_ views. Note that based on the naming conventions, the loader immediately recognized some of the foreign key relationships.

    ![Model](../images/powerbi/pb-load-model.png)

1. Note in the model that for dates, the relationships are not recognized by the loader. Let's create them by hand. Drag and drop the _OrderDateKey_, _ShipDateKey_  and _DueDateKey_ fields from the _Sales_ table one by one onto the _DateKey_ column of the _Date_ table.

    ![Date relationships](../images/powerbi/pb-load-datekey.png)

### Let's create the first report

1. Let's switch to _Report view_.

1. Add a new table to the current page.

    ![Add New Table](../images/powerbi/pb-1streport-addtable.png)

1. Drag the following columns from the _Data_ toolbar onto the table:

- _Product_/_Category_,
- _Product_/_Model_
- _Product_/_Product_
- _Sales_/_Sales Amount_

    ![Add New Table](../images/powerbi/pb-1streport-addcolumns.png)

1. Note that the _Product_ and _Sales_ tables are in a one-to-many relationships, therefore, results from the _Sales_ table are automatically summed together.

1. Let's format the table. To do this, we use the _Format your visual_ page of the _Visualizations_ toolbar (make sure the table is constantly selected).

    1. Choose the _Alternating rows_ option for _Style presets_. This will give the table a default formatting.

    1. In the _Values_ block, set the font size to 14, and set a custom _Text color_ and _Alternate text color_.

    1. Resize the table so that the content fits nicely with the larger font size.

        ![Resize Table](../images/powerbi/pb-1streport-resize.png)

    1. Increase the font size of the header (_column headers_)

    1. Turn off the summary (set the switch in the top right corner of _Totals_/_Values_ to off)

    1. Let's Highlight the amount column. To do this, select _Sum of Extended Amount_ column in the _Specific column_ block, set the value of _Text color_ to white, and set _Background color_ to a darker color.

    1. At this point, our table looks something like this:

    ![Formatted Table](../images/powerbi/pb-1streport-formattedtable.png)

### Creating and publishing filters

Below we define custom filters for the report and then publish our work.

1. Click the filter icon below the table to bring up the _Filters_ toolbar. You can see that filters have already been created for the 4 columns.

    ![Filters](../images/powerbi/pb-1streport-filters.png)

1. Drag the _SalesTerritory_/_Country_ column to the filters section. Now we can filter by country as needed.

1. Hide the sales price filter. To do this, click the small eye icon inside the _Sum of Extended Amount_ filter. Although the filter will still be visible to us, it will no longer appear as such in the published report.

    ![Hide filter](../images/powerbi/pb-1streport-hidefilter.png)

1. Click on a section of the page where there is no table to select the page itself. The formatting in the _Visualizations_ toolbar will then include settings for the entire page (_Format page_). Within this, we customize the appearance of the filters in the next steps.

1. Set the _Filter pane_/_Search_ property to a different bright color.

1. Set the _Filter pane_/_Background_ property to a darker color.

1. Set the _Filter_ **cards**/_Background_ property to the same color as the previous one. The end result could be something like this:

![Formatted Filters](../images/powerbi/pb-1streport-formattedfilters.png)

1. Save the changes and publish the completed report to the online service using the _Publish_ button on the _Home_ page.

    ![Publish button](../images/powerbi/pb-1streport-publishbutton.png)

1. When publishing, mark the default workspace as the destination (_My Workspace_)

    ![Publishing dialog](../images/powerbi/pb-1streport-publishdialog.png)

1. After successful publication, click on the link in the dialog to open the report.

    ![Publish Complete](../images/powerbi/pb-1streport-publishready.png)

1. Let's experiment with our final report to see what we've done.

    ![Published report](../images/powerbi/pb-1streport-online.png)

!!! example "SUBMISSION"
Take a screenshot of the published report. Save the image as f1.png and submit it with the other files of the solution. The image should show the entire screen (browser window, system tray, etc.). Check again that your Neptun code is visible (in the upper gray line)!

Also upload the updated NEPTUN.pbix file.

## 2. Task: Chart (5p)

**This exercise is to be completed individually for 5 points.**

### Create chart

The tabular display shows the sales data in detail. However, a chart can be interpreted more quickly. Make a chart showing the sales volumes for each product category.

1. For this chart, create a new page in the report.

    ![Add Page](../images/powerbi/pb-diagram-addpage.png)

1. Let's use the opportunity to give meaningful names to the pages. Double-click the page names to rename them. Call the first page 'Table' and the new page 'Chart'

1. Add a _clustered column chart_ to the new page.

    ![Bar Chart](../images/powerbi/pb-diagram-statechart.png)

1. In the chart, we want to summarize the number of sales by product category. Here's how we can achieve this.

1. Select the chart.

1. You can specify the parameters on the _Visualizations_ toolbars _Build visual_ page. Drag the _Product_/_Category_ column onto the _X axis_ field and the _Sales_/_Order quantity_ column onto the _Y axis_ field.

    ![Column chart with data](../images/powerbi/pb-diagram-quantities.png)

### Annual breakdown

As a next step, we would like to look at the previous data in an annual breakdown. Since there are three types of dates in the _Sales_ table (_OrderDate_,_DueDate_,_ShipDate_), we need to decide which one to use. For us, the order date will now be relevant, so we need to make sure that if we filter or group by _Date_ table, the system takes this into account.

1. Open the _Model view_. Notice that there are three connections between the _Sales_ and the _Date_ table, but only one of them is highlighted by a continuous line, and the other two are dashed. The continuous line is the active link and will be the basis for subsequent grouping and filtering. Move the mouse cursor over each link to see which columns are connected. There can be no more than one active relationship between two tables.

![Dates Connection](../images/powerbi/pb-diagram-orderdate.png)

1. If the _OrderDateKey_-_DateKey_ connection is not the active one, we must first terminate the existing active connection. To do this, select it, and turn off the _Make this relationship active_ option, then click _Apply changes_.

![Active Connection](../images/powerbi/pb-diagram-relationship.png)

1. Similarly to the previous step sequence, select the line for the _OrderDateKey_-_DateKey_ connection and make it active. Do not miss the _Apply changes_ step.

!!! tip "Hint"
There could be a situation where we want to categorize based on two types of dates at the same time. In this case, we can bypass the one-relation-one-active-connection limit by duplicating the _Date_ table.

1. To return to the report view, drag the _Date_/_Fiscal Year_ column to the _Legend_ property of the chart.

![Annual grouping](../images/powerbi/pb-diagram-year.png)

1. Publish the report in a similar way to the previous task. In the course of publishing, we will see that our previous report with this name is already in the cloud. Feel free to select the **Replace** option if you are asked to overwrite it.

!!! example "SUBMISSION"
Take a screenshot of the published report. Save the image as f2.png and submit it with the other files of the solution. The image should show the entire screen (browser window, system tray, etc.). Check again that your Neptun code is visible (in the upper gray line)!

## 3. Task: Map (5p)

**This exercise is to be completed individually for 5 points.**

Power BI has a number of spectacular and intelligent diagram models. The following is a world map showing each country's sales by category.

1. Add a new page with the name _Map_ and fill in its properties as follows

1. _Location_ should be set to _SalesTerritory_/_country_

1. _Bubble size_ should be set to _Sales_/_order quantity_

1. Resize the map to fill in the page

At this point, you can already see the bubbles proportional to the national sales data on the map. In the last step, we will  break down the data by categories, which we can do by using in the _Legend_ field of the bar chart.

1. _Legend_ should be set to _Product_/_category_

    ![Map](../images/powerbi/pb-diagram-map.png)

1. Publish the report in a similar way to the previous task.

!!! example "SUBMISSION"
Take a screenshot of the published report. Save the image as f3.png and submit it with the other files of the solution. The image should show the entire screen (browser window, system tray, etc.). Check again that your Neptun code is visible (in the upper gray line)!

## 4. Task: Sales Reports (5p)

**This exercise is to be completed individually for 5 points.**

In this exercise, you will learn about complex filters, line diagrams, and complex data

### Content of the report

Provide a report on how product sales for each category and subcategory have progressed year-on-year, including monthly breakdowns. Also visualize the results in a line chart.

The final report should be similar to the following:

![Suggested groups](../images/powerbi/pb-categories-full.png)

The solution steps are described below:

### Slicer

1. Create a new page with the name _Categories_. A report similar to the sample will be achieved by using 3 different _Visuals_

1. Put a _Slicer_ on the report. It will function as a filter, by displaying the filter categories on the UI.

1. Drag the _Product_/_Category_ and _Product_/_Subcategory_ columns on the _Slicers_ _Field_ property. (The order matters!). Try out the _Slicer_!

![Configure Slicer](../images/powerbi/pb-categories-slicer.png)

### Table

1. Place a _Table_ component next to the _Slicer_. If necessary, resize both of them so that their heights are aligned.

1. In the table, order times will be grouped by year and month. Here, we take advantage of the system's "understanding" of how dates work so that the _Date_/_date_ column can be expanded and the _Year_ and _Month_ fields can be used separately. Let's do this.

1. Add the columns _Sales_/_Sales Amount_ and _Sales_/_Order Quantity_ to the table.

1. Let's try out how selecting certain categories in the _Slicer_ affects your current data.

![Configure Table](../images/powerbi/pb-categories-table.png)

### Line chart

1. Place a _Line Chart_ component under the _Slicer_ and the _Table_. Resize them as needed so that the width of the chart is the same as the elements above it.

1. In the chart, we will plot the monthly sales data grouped by years.

1. Set the _X-Axis_ field of the chart. Its value should be _Date_/_Date_/_Month_ while _Y-Axis_ should be set to _Sales_/_Sales amount_.

1. Use the _Legend_ field to categorize the results by year, drag and drop the _Date_/_Date_/_Year_ column onto it.

![Configure a chart](../images/powerbi/pb-categories-linechart.png)

1. Finally, set the title of the graph. You can do this using the _General_ tab on the _Format your visual_ page in the _Visualizations_ toolbar. Set the value of the _Title_ field to your Neptun code, and in the same block, increase the font size, set the _Text color_ field to red, and center the text (_Horizontal alignment_:_Center_)

![Chart title](../images/powerbi/pb-categories-charttitle.png)

!!! example "SUBMISSION"
Take a screenshot of the published report. Save the image as f4.png and submit it with the other files of the solution. The image should show the entire screen (browser window, system tray, etc.). Check again that your Neptun code is visible (in the upper gray line)!

## 5. Task: Optional task

**You can earn 3 IMSc points by completing this task.**

Create a pie chart about the number of transactions in each product category. **Enter your Neptune code in the title of the diagram.**.

- Colors of the categories (as shown below) should be red, yellow, green and blue

- Data labels should be inside the pie chart

- Upload the completed report to the online Power BI service as well!

- The title of the chart should be your Neptun code and should be displayed centered and in bold

- The goal is to create a similar chart to the one below

![Expected Pie Chart](../images/powerbi/pb-diagram-piechart.png)

!!! example "SUBMISSION"
Take a screenshot of the published report. Save the image as f5.png and submit it with the other files of the solution. The image should show the entire screen (browser window, system tray, etc.). Check again that your Neptun code is visible (in the upper gray line)!
