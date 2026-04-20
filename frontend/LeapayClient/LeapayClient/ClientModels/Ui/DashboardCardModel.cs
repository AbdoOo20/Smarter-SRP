using Avalonia.Layout;
using Avalonia.Threading;
using SukiUI.Controls;

namespace Leapay.Client.ClientModels.Ui;

public partial class DashboardCardModel: ObservableObject
{
    [ObservableProperty]
    public partial string? Name { get; set; }
    
    [ObservableProperty]
    public partial object? Content { get; set; }

    [ObservableProperty]
    public partial bool  Visible { get; set; }
    
    public async Task LoadDataAsync(Task action, Func<Control> content)
    {
        Content = new Loading
        {
            IsEnabled = true,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        
        await action.ContinueWith(_ =>
        {
            Dispatcher.UIThread.Post(() => Content = content.Invoke());
        });
    }
}

public partial class ItemProfitCardModel: ObservableObject
{
    [ObservableProperty]
    public partial string? Name { get; set; }
    
    [ObservableProperty]
    public partial string ItemSales { get; set; }

    [ObservableProperty]
    public partial decimal ItemSalesNum { get; set; }
    
    [ObservableProperty]
    public partial double  ItemSalesPercentage { get; set; }

    public string ItemSalesPercentageVal => $"{ItemSalesPercentage:F1}%";
}