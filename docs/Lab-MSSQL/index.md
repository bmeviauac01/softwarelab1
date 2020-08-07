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
- Database initialization script: [mssql.sql](https://bmeviauac01.github.io/adatvezerelt/db/mssql.sql)
- GitHub account and a git client

Materials for preparing for this laboratory:

- Using Microsoft SQL Server: [description](https://bmeviauac01.github.io/datadriven-en/db/mssql/) and [video](https://www.youtube.com/watch?v=kK1i_HUujAc)
- The [schema](https://bmeviauac01.github.io/datadriven-en/db/) of the database
- Microsoft SQL Server server-side programming and the SQL language
    - Check the materials of [Data-driven systems](https://www.aut.bme.hu/Course/enviauac01) including the [seminars](https://bmeviauac01.github.io/datadriven-en/)

## Initial steps

Keep in mind that you are expected to follow the [submission process](../GitHub.md).

### Create and check out your Git repository

1. Create your git repository at: <TBD>

1. Wait for the repository creation to complete, then check out the repository.

    !!! tip ""
        If in university computer laboratories you are not asked for credentials to log in to GitHub when checking out the repository, the operation may fail. This is likely due to the machine using someone else's GitHub credentials. Delete these credentials first (see [here](../GitHub-credentials.md)), then retry the checkout.

1. Create a new branch with the name `solution` and work on this branch.

1. Open the checked out folder and type your Neptun code into the `neptun.txt` file. There should be a single line with the 6 characters of your Neptun code and nothing else in this file.

### Create the database

1. Connect to Microsoft SQL Server using SQL Server Management Studio. Start Management Studio and use the following connection details:

    - Server name: `(localdb)\mssqllocaldb` or `.\sqlexpress` (which is short for: `localhost\sqlexpress`)
    - Authentication: `Windows authentication`

1. Create a new database (if it does not exist yet). The name should be your **Neptun code**: in _Object Explorer_ right-click _Databases_ and choose _Create Database_.

    !!! warning "IMPORTANT"
        The name of the database must be your **Neptun code**. You will need to submit screenshots that display the database name this way!

1. Create the sample database by executing the [initializer script](https://bmeviauac01.github.io/adatvezerelt/db/mssql.sql) Open a new _Query_ window, paste the script into the window, then execute it. Make sure to select the right database in the toolbar dropdown.

    ![Selecting the database](../images/sql-management-database-dropdown.png)

1. Verify that the tables are created. If the _Tables_ folder was open before, you need to refresh it.

    ![Listing tables](../images/sql-managment-tablak.png).
