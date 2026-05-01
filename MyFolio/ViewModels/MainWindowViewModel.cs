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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSetupVisible))]
    [NotifyPropertyChangedFor(nameof(IsNavigationVisible))]
    private ViewModelBase? _currentPage;

    public bool IsSetupVisible => CurrentPage == null;
    public bool IsNavigationVisible => CurrentPage != null;
    #endregion


    #region Constructor
    public MainWindowViewModel()
    {
        Greeting = "Welcome to MyFolio!";
        _ = ChangeText();
        DefaultProfilePicture();
    }
    #endregion

    #region Methods

    private void DefaultProfilePicture()
    {
        var uri = new Uri("avares://MyFolio/Assets/Images/profile.png");
        using var stream = AssetLoader.Open(uri);
        ProfilePicture = new Bitmap(stream);
    }

    private async Task ChangeText()
    {
        await Task.Delay(2000);
        Greeting = "Are you ready? Let's upload your profile picture!";
    }

    partial void OnProfilePictureChanged(Bitmap? value)
    {
        if (CurrentPage is HomeViewModel homeVm)
            homeVm.ProfilePicture = value;
    }

    #endregion


    #region RelayCommands Methods

    [RelayCommand]
    public async Task ContinueCommand()
    {
        await ChangeProfilePicture();
        Greeting = "Profile Picture updated!";
    }


    [RelayCommand]
    public async Task ChangeProfilePicture()
    {
        var window = Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;

        if (window?.StorageProvider is not { } storageProvider)
            return;

        var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Profile Picture",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Image Files")
                {
                    Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp" }
                }
            }
        });

        if (result.Count == 0)
            return;

        try
        {
            await using var stream = await result[0].OpenReadAsync();
            var newBitmap = new Bitmap(stream);
            var oldBitmap = ProfilePicture;

            ProfilePicture = newBitmap;
            oldBitmap?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading profile picture: {ex.Message}");
        }
    }

    [RelayCommand]
    private void NavigateHome()
    {
        CurrentPage = new HomeViewModel(ProfilePicture);
    }

    public void OnMouseEnter(object? sender, Avalonia.Input.PointerEventArgs e) => IsHovering = true;
    public void OnMouseLeave(object? sender, Avalonia.Input.PointerEventArgs e) => IsHovering = false;

    #endregion
}
