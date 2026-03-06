# QrCodeDocker
CorelDraw Addon X7 or Higher, provides a creation of qr codes in offline mode, in batch and customization, uses the zxing library to codify and decodify the qrcode. Dont require internet or autentication. Please see link below to features demonstration

<table class="tg">
<thead>
  <tr>
    <td class="tg-0pky"><img src="Screenshots/QrcodeDocker01.PNG" ></td>
<td class="tg-0pky"><img src="Screenshots/QrcodeDocker02.PNG" ></td>  
<td class="tg-0pky"><img src="Screenshots/QrcodeDocker03.PNG" ></td>  </tr>
</thead>
</table>

## About the Project

QrCodeDocker is an open source project.

The source code is freely available and can be compiled by anyone.

To support development, official precompiled builds are distributed commercially.

## How add a new language
Go to folder "Lang" in any project, copy a language xml e replace language code in file name for target language.
In language xml, changes author and translate the tags values, dont change tags names.
Is required make this in all projects.
if doesnt know language code value follow to coreldraw installation folder in language folder and get the folder name, for exemple, <C:\Program Files\Corel\CorelDRAW Graphics Suite X8\Languages>

## Build From Source

Anyone can compile the project locally using Visual Studio.

Follow the instructions in the Build section below.

## Build Instructions

[Build](Build.md)
````
1. Open the file "bonus630.CDRCommon.targets" and change the installation path of the desired version to your installation path.

2. Locate the MSBuild.exe, usually located at `C:\Windows\Microsoft.NET\Framework64\v4.0.30319`.

3. Run the command prompt or PowerShell in the folder as an administrator.

4. Copy the path to the "QrCodeDocker.sln" file located in the project folder.

5. In PowerShell, type the command `.\msbuild.exe "<path>" /p:Configuration="<configuration>"`, replacing `<path>` with the copied path and `<configuration>` with the desired installation. The available configurations are:
   - X7 Release
   - X8 Release
   - 2017 Release
   - 2018 Release
   - 2019 Release
   - 2020 Release
   - 2021 Release
   - 2022 Release
   - 2024 Release

6. In the command prompt, remove the initial `.\` from the command.
````
## Distribution Model

QrCodeDocker is open source.

You can compile the project yourself for free.

However, precompiled binaries and packaged releases are distributed commercially to support the development of the project.

Options:

1. Build it yourself (free)
2. Purchase the ready-to-use compiled version

## Download

Precompiled releases are available here:

[https://www.corelnaveia.com/2017/02/Quer-Ganhar-Dinheiro-Obtenha-guia-de-atalhos-coreldraw-x7-x8-2017.html]

By purchasing the compiled version you support the development of this project.

## How to use it in your project - Hello World

Draw a simple Qrcode code contains "Hello World!"

Adds reference

- ImageRender
- QrCodeDocker

## Third Party Libraries

This project uses the following libraries:

 -ZXing.Net  
    Licensed under the Apache License 2.0
    [https://github.com/micjahn/ZXing.Net]
 -Tesseract-ocr
    Licensed under the Apache License 2.0
    [https://github.com/tesseract-ocr/tesseract]

```csharp
 var codeGen = new br.corp.bonus630.QrCodeDocker.QrCodeGenerator(CorelApplicationObject);
 var imageRender = new br.corp.bonus630.ImageRender.ZXingImageRender();
 codeGen.SetRender(imageRender);
 Corel.Interop.VGCore.Shape code = codeGen.CreateVetorLocal(CorelApplicationObject.ActiveLayer, "Hello World!", 100);
````   