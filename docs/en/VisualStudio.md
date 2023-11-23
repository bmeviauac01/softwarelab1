# Install Visual Studio & .NET SDK

In some of the exercises require Microsoft Visual Studio **version 2022 17.8 or newer**. The free [Community edition](https://visualstudio.microsoft.com/vs/community/) is sufficient for solving these exercises.

You can check the version by starting the _Visual Studio Installer_:

![Visual Studio version](images/visual-studio/vs-verzio.png)

!!! info "VS Code"
    The exercises can also be solved using the platform-independent **Visual Studio Code**. The skeletons of the exercises are prepared for Visual Studio. If you are working with VS Code, you need to configure your environment.

## Visual Studio workloads

When installing Visual Studio, the following [workloads](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2022#step-4---choose-workloads) have to be selected:

- ASP.NET and web development
- Data Storage and Processing

![Visual Studio workloads](images/visual-studio/vs-workload.png)

An existing installation can be [_modified_](https://docs.microsoft.com/en-us/visualstudio/install/modify-visual-studio?view=vs-2022) using the _Visual Studio Installer_.

![Visual Studio install components](images/visual-studio/vs-installer-modify.png)

## Check and install .NET SDK

Visual Studio might install certain versions of the .NET SDK. To check if you have the right version, use the `dotnet` CLI: in a console, execute the `dotnet --list-sdks` command. This command works on Linux and Mac too. It will print something similar:

```hl_lines="2"
PS C:\Users\toth.tibor> dotnet --list-sdks
8.0.100 [C:\Program Files\dotnet\sdk]
```

If you see version **8.0.x** in this list, then you are good to go. Otherwise, install the SDK [from here](https://dotnet.microsoft.com/download/dotnet-core/8.0).
