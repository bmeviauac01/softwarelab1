# Entity Framework and REST

We will build a new REST API (ASP.NET Core Web API) using Entity Framework.

## Pre-requisites and preparation

Required tools to complete the tasks:

- Windows, Linux, or macOS: All tools are platform-independent, or a platform-independent alternative is available.
- Microsoft Visual Studio 2019 [with the settings here](../VisualStudio.md)
    - When using Linux or macOS, you can use Visual Studio Code, the .NET Core SDK, and [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/).
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)
    - Usually installed with Visual Studio; if not, use the link above to install (the SDK and _not_ the runtime).
    - You need to install it manually when using Linux or macOS.
- [Postman](https://www.getpostman.com/)
- [DB Browser for SQLite](https://sqlitebrowser.org/) - if you would like to check the database (not necessary)
- GitHub account and a git client

Materials for preparing for this laboratory:

- Entity Framework, REST API, Web API and using Postman
    - Check the materials of [Data-driven systems](https://www.aut.bme.hu/Course/enviauac01) including the [seminars](https://bmeviauac01.github.io/datadriven-en/)
- Official Microsoft tutorial for [Web API](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio)

## Exercise overview

In this exercise, we will implement the backend of a simple task management web application. The application handles **two types of entities: statuses and tasks** where a status is associated with multiple tasks (1-\* connection). (In the text of the exercises will use _tasks_ only for referring to this entity of the application.)

If we had a frontend, the application would be a [Kanban-board](https://en.wikipedia.org/wiki/Kanban_board). We will not create a frontend here, only the REST API and Entity Framework + ASP.NET Core Web API server.

## Initial steps

Keep in mind that you are expected to follow the [submission process](../GitHub.md).

### Create and check out your Git repository

1. Create your git repository at: <https://classroom.github.com/a/i5FR43u1>

1. Wait for the repository creation to complete, then check out the repository.

    !!! tip ""
        If in university computer laboratories you are not asked for credentials to log in to GitHub when checking out the repository, the operation may fail. This is likely due to the machine using someone else's GitHub credentials. Delete these credentials first (see [here](../GitHub-credentials.md)), then retry the checkout.

1. Create a new branch with the name `solution` and work on this branch.

1. Open the checked out folder and type your Neptun code into the `neptun.txt` file. There should be a single line with the 6 characters of your Neptun code and nothing else in this file.

### ~~Creating~~ the database

We will not be using Microsoft SQL Server here, but _Sqlite_. It is a light-weight relational database management system mainly for client-side applications. Although it is not recommended for servers, we will use it for simplicity. **Sqlite requires no installation.**

We will define the database schema with _code first_ using C# code. Therefore, we will not need to create the schema with SQL commands.
