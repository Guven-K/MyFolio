namespace MyFolio.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; set; } 

    public MainWindowViewModel()
    {
        Test();
    }
    public void Test()
    {
        Greeting = "Welcome to MyFolio!";
    }
}