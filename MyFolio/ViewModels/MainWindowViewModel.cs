namespace MyFolio.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; set; } = "Welcome to MyFolio!";

    public MainWindowViewModel()
    {
        
    }
    public void Test()
    {
        Greeting = "Let's test this!";
    }
}