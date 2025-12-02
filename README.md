# LIMS - Lightweight Image Manipulation Software

LIMS is a lightweight image-editing application written in C#.
It features a clean and simple user interface built with the WPF framework.
Users can import multiple images simultaneously, making it easy to edit several files in one session.
The core idea behind LIMS is to let users apply the same tool or effect to a batch of images with minimal effort.
Currently, the application supports applying watermarks to imported files, with additional tools and features planned as development continues.

## Project structure
```
.
├── Logic/
│   ├── Core/
│   │   ├── DataStorage.cs
│   │   └── ToolProcessor.cs
│   ├── ImageLoading/
│   │   ├── ImageDataContainer
│   │   └── ...
│   ├── Tools/
│   │   ├── ToolBase.cs
│   │   ├── ToolsManager.cs
│   │   └── ...
│   └── TabContext.cs
├── Debugging/
│   └── Logger.cs
├── UI/
│   ├── Panels/
│   │   ├── ActionPanel.xaml
│   │   └── ...
│   └── MainWindow.xaml
├── Docs/
│   ├── TechDocs.xml
│   └── ...
├── Propeties/
│   └── AssemblyInfo.cs
└── Vendor/
    └── BitmapLoader.cs
```
* Logic/ – Contains the core application logic, including data handling, image processing, and tool management.
    * Core/ – Core classes for data storage and processing of tools.
    * ImageLoading/ – Classes and structures for loading and storing image data.
    * Tools/ – Base classes and manager for image processing tools.
* Debugging/ – Debugging utilities and logging functionality.
* UI/ – Contains all user interface components.
    * Panels/ –  UI panels (UserControl) the user interacts with.
* Docs/ – Technical and project documentation.
* Propeties/ – Project properties and metadata.
* Vendor/ – Third-party or external code used in the project.

## Requirements
- **C#** (version 11 or higher recommended)  
- **.NET 8** or higher  
- **WPF** (Windows Presentation Foundation) compatibility  
- Optional (for code editing): Visual Studio 2022 or later for building from source

## Running the program

You can download the latest release of LIMS here: [v1.0.0](https://github.com/LupusLudit/LIMS/releases/tag/v.1.0.0)

**To run the program:**  
1. Download the compressed release files.  
2. Extract the contents to a folder of your choice.  
3. Locate and run the `LIMS.exe` file.  

No installation is required. The program should run on any Windows system that meets the requirements.

## Notes
**This project is still in development!**  
LIMS is currently a work-in-progress and may contain bugs or incomplete features.  
This program was developed as a school project.
LICENSE:  GNU GENERAL PUBLIC LICENSE Version 2

