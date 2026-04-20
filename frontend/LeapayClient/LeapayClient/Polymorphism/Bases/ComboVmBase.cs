using System.Collections.ObjectModel;

namespace Leapay.Client.Polymorphism.Bases;

public partial class ComboVmBase<T>: ObservableObject where T: class
{
    [ObservableProperty]
    public partial ObservableCollection<T> Items { get; set; } = [];

    [ObservableProperty]
    public partial T? SelectedItem { get; set; }
}