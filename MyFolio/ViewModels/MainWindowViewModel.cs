using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;

namespace MyFolio.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    #region Properties
    [ObservableProperty]
    private string? _greeting;

    [ObservableProperty]
    private Bitmap? _profilePicture;

    [ObservableProperty]
    private bool _isHovering;
    #endregion


    #region Constructor
    public MainWindowViewModel()
    {
        Test();
        Greeting = "Hello, World!";
        DefaultProfilePicture();


    }
    #endregion

    #region Methods

    private void DefaultProfilePicture()
    {
        var uri = new Uri("avares://MyFolio/Assets/Images/profile.png");
        ProfilePicture = new Bitmap(AssetLoader.Open(uri));
    }

    public async void Test()
    {
        Greeting = "Welcome to MyFolio!";
        await ChangeText();
    }

    private async Task ChangeText()
    {
        await Task.Delay(2000);
        Greeting = "Are you ready?";
    }

    #endregion


    #region RelayCommands Methods

    [RelayCommand]
    public void Continue()
    {
        Greeting = "You clicked me!";
    }


    [RelayCommand]
    public async Task ChangeProfilePicture()
    {
        var openDialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Title = "Select Profile Picture",
            Filters =
                {
                    new FileDialogFilter { Name = "Image Files", Extensions = { "png", "jpg", "jpeg", "bmp" } }
                }
        };

        var window = Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;

        var result = await openDialog.ShowAsync(window);
        if (result != null && result.Length > 0)
        {
            var selectedFile = result[0];
            try
            {
                await using var stream = File.OpenRead(selectedFile);
                ProfilePicture = new Bitmap(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile picture: {ex.Message}");
            }
        }

    }

    public void OnMouseEnter(object? sender, Avalonia.Input.PointerEventArgs e) => IsHovering = true;
    public void OnMouseLeave(object? sender, Avalonia.Input.PointerEventArgs e) => IsHovering = false;

    #endregion
}