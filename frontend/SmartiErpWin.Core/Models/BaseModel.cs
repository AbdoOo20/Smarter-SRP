using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartErpWin.Core.Models;

// The base class from which all views, models, and view models will inherit.
public partial class BaseModel: ObservableObject
{
    [ObservableProperty]
    private string? _modelName;
}