# Query optimization

We will examine the query optimization behavior of Microsoft SQL Server. To properly understand the optimizer's behavior, in the **first 5 exercises**, we will explain the queries and the behavior too. The **rest of the exercises is individual work** where it is your task to infer the reason for a specific plan. Your task is to document the behavior and submit the documentation of all exercises.

## Pre-requisites and preparation

Required tools to complete the tasks:

- Windows, Linux, or macOS: All tools are platform-independent, or a platform-independent alternative is available.
- Microsoft SQL Server
    - The free Express version is sufficient, or you may also use _localdb_ installed with Visual Studio
    - A [Linux version](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup) is also available.
    - On macOS, you can use Docker.
- [Visual Studio Code](https://code.visualstudio.com/) or any other tool for writing markdown
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms), or you may also use the platform-independent [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download) is
- Database initialization script: [mssql.sql](https://raw.githubusercontent.com/bmeviauac01/adatvezerelt/master/docs/db/mssql.sql)
- GitHub account and a git client

Materials for preparing for this laboratory:

- Markdown [introduction](https://guides.github.com/features/mastering-markdown/) and [detailed documentation](https://help.github.com/en/github/writing-on-github/basic-writing-and-formatting-syntax)
- Using Microsoft SQL Server: [description](https://bmeviauac01.github.io/datadriven-en/db/mssql/) and [video](https://web.microsoftstream.com/video/98a6697d-daec-4a5f-82b6-8e96f06302e8)
- The [schema](https://bmeviauac01.github.io/datadriven-en/db/) of the database
- Microsoft SQL Server query optimization
    - Check the materials of [Data-driven systems](https://www.aut.bme.hu/Course/enviauac01)

## Initial steps

Keep in mind that you are expected to follow the [submission process](../GitHub.md).

### Create and check out your Git repository

1. Create your git repository at <https://classroom.github.com/a/gE7FwteL>

1. Wait for the repository creation to complete, then check out the repository.

    !!! tip ""
        If you are not asked for credentials to log in to GitHub in university computer laboratories when checking out the repository, the operation may fail. This is likely due to the machine using someone else's GitHub credentials. Delete these credentials first (see [here](../GitHub-credentials.md)), then retry the checkout.

1. Create a new branch with the name `solution` and work on this branch.

1. Open the checked-out folder and type your Neptun code into the `neptun.txt` file. There should be a single line with the 6 characters of your Neptun code and nothing else in this file.

### Open the markdown file

Create the documentation in a markdown file. Open the checked-out git repository with a markdown editor. We recommend using Visual Studio Code:

1. Start VS Code.

1. Use _File > Open Folder..._ to open the git repository folder.

1. In the folder structure on the left, find `README.md` and double click to open.

   - Edit this file.
   - When you create a screenshot, put the file in this directory next to the other files. This will enable you to use the file name to include the image.

    !!! warning "Use simple file names"
        You should avoid using special characters in the file names. Best if you use the English alphabet and no spaces either.

1. For convenient editing open the [preview](https://code.visualstudio.com/docs/languages/markdown#_markdown-preview) (_Ctrl-K + V_).

!!! note "Alternative editor"
    If you do not like VS code, you can also use the [GitHub web interface](https://help.github.com/en/github/managing-files-in-a-repository/editing-files-in-your-repository) to edit the markdown; you also have a preview here. [File upload](https://help.github.com/en/github/managing-files-in-a-repository/adding-a-file-to-a-repository) will be trickier.

### Create the database

1. Connect to Microsoft SQL Server using SQL Server Management Studio. Start Management Studio and use the following connection details:

    - Server name: `(localdb)\mssqllocaldb` or `.\sqlexpress` (which is short for: `localhost\sqlexpress`)
    - Authentication: `Windows authentication`

1. Create a new database (if it does not exist yet). The name should be your Neptun code: in _Object Explorer_ right-click _Databases_ and choose _Create Database_.

1. Create the sample database by executing the [initializer script](https://raw.githubusercontent.com/bmeviauac01/adatvezerelt/master/docs/db/mssql.sql) Open a new _Query_ window, paste the script into the window, then execute it. Make sure to select the correct database in the toolbar dropdown.

    ![Selecting the database](../images/sql-management-database-dropdown.png)

1. Verify that the tables are created. If the _Tables_ folder was open before, you need to refresh it.

    ![Listing tables](../images/sql-managment-tablak.png).

### Getting the actual execution plan

!!! tip "If you are not using Windows"
    We are primarily using SQL Server Management Studio to get the execution plans. If you are not using Windows, you can also use Azure Data Studio-t to [obtain the query plan](https://richbenner.com/2019/02/azure-data-studio-execution-plans/).

We will check the query plan the optimizer chose and the server executed in the following exercises. In SQL Server Management Studio, open the _Query_ menu and check [_Include Actual Execution Plan_](https://docs.microsoft.com/en-us/sql/relational-databases/performance/display-an-actual-execution-plan).

![Enable query plan](../images/queryopt/queryopt-include-plan.png)

The plan will be displayed after the query is completed at the bottom of the window on the _Execution plan_ pane.

![View query plan](../images/queryopt/queryopt-plan-result.png)

The plan is a data flow diagram where the query execution is the flow of the data. The items are the individual steps, and the percentages are the relative cost of each step with regards to the whole query.
