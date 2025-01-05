namespace MyFolio.ViewModels;

public class MainWindowViewModel : ViewModelBase
{

    #region Properties
    public string? Greeting { get; set; }
    #endregion

    #region Constructor
    public MainWindowViewModel()
    {
        Test();
    }
    #endregion

    #region Methods
    public void Test()
    {
        Greeting = "Welcome to MyFolio!";
    }
    #endregion
}