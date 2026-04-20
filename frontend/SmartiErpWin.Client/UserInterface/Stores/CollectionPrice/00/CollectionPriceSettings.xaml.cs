namespace SmartiErpWin.Client.UserInterface.Stores.CollectionPrice._00;

/// <summary>
/// Interaction logic for CollectionPriceSettings.xaml
/// </summary>
public partial class CollectionPriceSettings
{

    public bool UsePercent { get; set; }
    public decimal? Amount { get; set; }

    public CollectionPriceSettings()
    {
        InitializeComponent();
        DataContext = this;
    }

    [RelayCommand]
    public async Task PressedAsync()
    {
        await Task.CompletedTask;
        var aa = Math.Abs(Amount ?? 0);
        if (UsePercent && (aa < 0 || aa > 100))
        {
            return;
        }

        this.DialogResult = true;
        this.Close();
    }
}