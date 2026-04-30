using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFolio.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private Bitmap? _profilePicture;

    public HomeViewModel() { }

    public HomeViewModel(Bitmap? profilePicture)
    {
        ProfilePicture = profilePicture;
    }
}
