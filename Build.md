# Build Instructions

QrCodeDocker supports multiple versions of CorelDRAW.

## CorelDRAW Dependencies

All CorelDRAW SDK references are centralized in the file:

    bonus630.CDRCommon.targets

This file stores the installation paths for the supported CorelDRAW versions and is used by MSBuild to resolve the required libraries (such as VGCore.dll).

CorelDRAW versions with WPF support are supported starting from CorelDRAW X7.

### 1. Configure CorelDRAW Installation Path

Open the file:

    bonus630.CDRCommon.targets

Locate the entry corresponding to the CorelDRAW version you want to compile against and update the installation path to match your local CorelDRAW installation.

Example installation path:

    C:\Program Files\Corel\CorelDRAW Graphics Suite 2020\Programs64

## Build Using MSBuild

This step ensures that MSBuild can find the required CorelDRAW SDK libraries.

### 2. Locate MSBuild

Locate the MSBuild executable. It is usually installed with the .NET Framework.

Typical location:

    C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe

You may optionally add this folder to your system PATH.

### 3. Open Administrator Shell

Open Command Prompt or PowerShell as Administrator.

Administrator privileges are required because during the build process MSBuild automatically copies the compiled files to the CorelDRAW Addons folder, which normally requires elevated permissions.

This folder is where CorelDRAW searches for dockers and addons.

### 4. Locate the Solution File

Find the solution file in the repository:

QrCodeDocker.sln

Copy the full path to this file.

Example:

    D:\Projects\QrCodeDocker\QrCodeDocker.sln
### 5. Run MSBuild

Navigate to the folder where MSBuild.exe is located and run the build command.

PowerShell
    .\msbuild.exe "<path>" /p:Configuration="<configuration>"

Replace:

<path> → full path to QrCodeDocker.sln
<configuration> → desired CorelDRAW target version

Example:

    .\msbuild.exe "D:\Projects\QrCodeDocker\QrCodeDocker.sln" /p:Configuration="2020 Release"
Command Prompt

In Command Prompt remove the .\ prefix:

    msbuild.exe "<path>" /p:Configuration="<configuration>"

Example:

msbuild.exe "D:\Projects\QrCodeDocker\QrCodeDocker.sln" /p:Configuration="2020 Release"
Available Build Configurations

The project includes build configurations for the following CorelDRAW versions:

* X7 Release
* X8 Release
* 2017 Release
* 2018 Release
* 2019 Release
* 2020 Release
* 2021 Release
* 2022 Release
* 2024 Release
* 2025 Release
* 2026 Release

Each configuration links against the corresponding CorelDRAW SDK defined in bonus630.CDRCommon.targets.

Post-Build Behavior

After a successful build, the compiled addon files are automatically copied to the CorelDRAW Addons directory.

This allows the extension to be immediately available when CorelDRAW starts.

Because this folder is typically located inside protected system directories, the build process must be executed with administrator privileges.

## Summary

Build workflow:

Configure CorelDRAW path in bonus630.CDRCommon.targets

Open administrator terminal

Run MSBuild with the desired configuration

Build output is automatically deployed to the CorelDRAW addons folder