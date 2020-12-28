# MongoDB

In this lab, we will work with the MongoDB NoSQL database and the Mongo C# driver.

## Pre-requisites and preparation

Required tools to complete the tasks:

- Windows, Linux, or macOS: All tools are platform-independent, or a platform-independent alternative is available.
- MongoDB Community Server ([download](https://www.mongodb.com/download-center/community))
- Robo 3T ([download](https://robomongo.org/download))
- Sample database initialization script: ([mongo.js](https://bmeviauac01.github.io/adatvezerelt/db/mongo.js))
- Microsoft Visual Studio 2019 [with the settings here](../VisualStudio.md)
    - When using Linux or macOS, you can use Visual Studio Code, the .NET Core SDK, and [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/).
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)
    - Usually installed with Visual Studio; if not, use the link above to install (the SDK and _not_ the runtime).
    - You need to install it manually when using Linux or macOS.
- GitHub account and a git client

Materials for preparing for this laboratory:

- MongoDB database system and the C# driver
    - Check the materials of [Data-driven systems](https://www.aut.bme.hu/Course/enviauac01) including the [seminars](https://bmeviauac01.github.io/datadriven-en/)
- Official Microsoft tutorial for [WebApi using MongoDB](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio)
    - We will not be creating a WebApi in this lab, but the Mongo part is the same.

## Initial steps

Keep in mind that you are expected to follow the [submission process](../GitHub.md).

### Create and check out your Git repository

1. Create your git repository at: <https://classroom.github.com/a/LQu7BuEO>

1. Wait for the repository creation to complete, then check out the repository.

    !!! tip ""
        If in university computer laboratories you are not asked for credentials to log in to GitHub when checking out the repository, the operation may fail. This is likely due to the machine using someone else's GitHub credentials. Delete these credentials first (see [here](../GitHub-credentials.md)), then retry the checkout.

1. Create a new branch with the name `solution` and work on this branch.

1. Open the checked out folder and type your Neptun code into the `neptun.txt` file. There should be a single line with the 6 characters of your Neptun code and nothing else in this file.

### Create the database

Follow the steps in the [seminar material](https://bmeviauac01.github.io/datadriven-en/seminar/mongodb/#exercise-0-create-database-open-starter-code) to start the database server and initialize the database.
