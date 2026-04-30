# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MyFolio is a cross-platform desktop portfolio application built with **Avalonia UI** (v11.3.10), targeting .NET 10.0. It follows the **MVVM pattern** using CommunityToolkit.Mvvm for source-generated observable properties and relay commands.

## Commands

```bash
# Build
dotnet build MyFolio.sln

# Run
dotnet run --project MyFolio/MyFolio.csproj

# Build release
dotnet build MyFolio.sln -c Release
```

There are no test projects currently.

## Architecture

```
MyFolio/
├── App.axaml / App.axaml.cs       # Application entry, theme setup, DI disabled for DataAnnotations
├── ViewLocator.cs                  # Convention-based ViewModel→View resolution via reflection
├── ViewModels/                     # Business logic; extend ViewModelBase (ObservableObject)
├── Views/                          # Avalonia XAML UI + code-behind (.axaml + .axaml.cs)
├── models/                         # Empty — reserved for data models
└── Assets/Images/                  # Embedded image assets
```

### Key Patterns

**ViewLocator** maps `*ViewModel` → `*View` by name convention (e.g., `MainWindowViewModel` → `MainWindow`). It implements `IDataTemplate`, so `ContentControl` with a ViewModel as its `Content` automatically renders the matching view.

**ViewModels** use CommunityToolkit.Mvvm source generators:
- `[ObservableProperty]` on private fields generates public properties with `INotifyPropertyChanged`
- `[RelayCommand]` on methods generates `ICommand` properties
- Base class is `ViewModelBase : ObservableObject`

**Views** use Avalonia compiled bindings (`x:CompileBindings="True"` is the project default). Binding paths are verified at compile time.

### Current Views

- **MainWindow** — primary shell: profile picture upload (with hover overlay, circular crop, shadow), animated greeting text, navigation header buttons, Continue button
- **HomeView / ProfileView** — placeholder UserControls
- **SettingsPage** — checkbox UI for Native title bar / Dark mode / Notifications (not yet wired to logic)

### Profile Picture Flow

`MainWindowViewModel` loads the default image from `avares://MyFolio/Assets/Images/profile.png` at startup. The `UploadProfilePicture` command opens a file dialog (PNG/JPG/JPEG/BMP), loads the selected file into a `Bitmap`, and disposes the previous one to avoid memory leaks. The `ContinueCommand` is async and updates the greeting text.

## Dependencies

| Package | Purpose |
|---|---|
| Avalonia 11.3.10 | Cross-platform UI framework |
| Avalonia.Themes.Fluent | Fluent design theme |
| Avalonia.Xaml.Behaviors | Interaction/event behaviors in XAML |
| CommunityToolkit.Mvvm 8.4.0 | MVVM source generators |
| System.Text.Json 10.0.1 | JSON serialization |
| Avalonia.Fonts.Inter | Inter typeface |

## Notes

- Output type is `WinExe` but Avalonia enables Linux/macOS support — use `OperatingSystem.IsWindows()` for platform-specific code.
- `DataAnnotationsValidationPlugin` is explicitly disabled in `App.axaml.cs`; validation is handled via CommunityToolkit.
- The `app.manifest` enables Windows 10+ styling, COM interop (`supportedOS`), and visual transparency (needed for AcrylicBlur).
