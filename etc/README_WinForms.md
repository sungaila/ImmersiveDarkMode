# ![ImmersiveDarkMode Logo](https://raw.githubusercontent.com/sungaila/ImmersiveDarkMode/master/etc/Icon_64.png) Sungaila.ImmersiveDarkMode

[![GitHub Workflow Build Status](https://img.shields.io/github/actions/workflow/status/sungaila/ImmersiveDarkMode/dotnet.yml?event=push&style=flat-square&logo=github&logoColor=white)](https://github.com/sungaila/ImmersiveDarkMode/actions/workflows/dotnet.yml)
[![SonarCloud Quality Gate](https://img.shields.io/sonar/quality_gate/sungaila_ImmersiveDarkMode?server=https%3A%2F%2Fsonarcloud.io&style=flat-square&logo=sonarcloud&logoColor=white)](https://sonarcloud.io/dashboard?id=sungaila_ImmersiveDarkMode)
[![NuGet version](https://img.shields.io/nuget/v/Sungaila.ImmersiveDarkMode.WinForms.svg?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/Sungaila.ImmersiveDarkMode.WinForms/)
[![NuGet downloads](https://img.shields.io/nuget/dt/Sungaila.ImmersiveDarkMode.WinForms.svg?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/Sungaila.ImmersiveDarkMode.WinForms/)
[![GitHub license](https://img.shields.io/github/license/sungaila/ImmersiveDarkMode?style=flat-square)](https://github.com/sungaila/ImmersiveDarkMode/blob/master/LICENSE)

Applies a dark theme to the titlebar of Win32 windows. Can also be toggled automatically whenever the system-wide application theme changes.

Works on Windows 11 (Build 22000) and newer.

## Getting started
Call `WindowExtensions.SetTitlebarTheme()` in the constructor of your `System.Windows.Forms.Form` and override `WndProc` like this:
```csharp
public Form1()
{
    InitializeComponent();
    this.SetTitlebarTheme();
}

protected override void WndProc(ref Message m)
{
    base.WndProc(ref m);
    WindowExtensions.CheckAppsThemeChanged(m);
}
```