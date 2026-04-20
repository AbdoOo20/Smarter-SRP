using System.ComponentModel;
using Avalonia.Collections;
using Avalonia.Media;
using Leapay.Client.Helpers;

namespace Leapay.Client.ClientModels.Dto;

public partial class ColumnImportModel: ObservableObject
{
    [ObservableProperty]
    public partial string? Name { get; set; }
    
    [ObservableProperty]
    public partial string? DbName { get; set; }
    
    [ObservableProperty]
    public partial string? DefaultValue { get; set; }
    
    [ObservableProperty]
    public partial bool HasForeignKey { get; set; }
    
    [ObservableProperty]
    public partial int ProcessOrder { get; set; }

    [ObservableProperty]
    public partial bool ShowInUi { get; set; } = true;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ColumnIcon), nameof(ColumnColour), nameof(IsReady))]
    public partial bool IsAvailable { get; set; }
   
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ColumnIcon), nameof(ColumnColour), nameof(IsReady))]
    public partial bool IsRequired { get; set; }

    public bool IsReady => IsAvailable || !IsRequired && !IsAvailable;

    public MaterialIconKind ColumnIcon => IsRequired && !IsAvailable ? MaterialIconKind.Warning :
        IsAvailable ? MaterialIconKind.Check : MaterialIconKind.Information;
    
    public LinearGradientBrush? ColumnColour => IsRequired && !IsAvailable ? ContentHelper.GetRedGradient() :
        IsAvailable ? ContentHelper.GetGreenGradient() : ContentHelper.GetWarningYellowGradient();

    [ObservableProperty]
    public partial AvaloniaList<string> CurrentValues { get; set; } = [];
    
    public Func<string, bool>? ForeignKeySatisfactionMechanism { get; set; }

    [ObservableProperty]
    public partial Func<string?, Task>? ItemAdditionMechanism { get; set; }
    
    [ObservableProperty]
    public partial bool AddVisibility { get; set; }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ItemAdditionMechanism) or nameof(IsAvailable))
        {
            AddVisibility = ItemAdditionMechanism is not null && !IsAvailable;
        }
        
        base.OnPropertyChanged(e);
    }

    public Action<(object?, string?)>? ForeignKeyMatchingMechanism { get; set; }

    [RelayCommand]
    private void OnAdd()
    {
        ItemAdditionMechanism?.Invoke(Name);
    }
}