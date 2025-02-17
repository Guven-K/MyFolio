using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyFolio.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    #region Properties
    [ObservableProperty]
    private string? _greeting;

    [ObservableProperty]
    private Bitmap? _profilePicture;
    #endregion

    #region Constructor
    public MainWindowViewModel()
    {
        Test();
        Greeting = "Hello, World!";
    }
    #endregion

    #region Methods

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
    #endregion
}