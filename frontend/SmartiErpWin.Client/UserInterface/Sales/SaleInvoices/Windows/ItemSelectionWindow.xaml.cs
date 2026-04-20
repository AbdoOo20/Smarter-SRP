using System.Windows.Controls;
using System.Windows.Input;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace SmartiErpWin.Client.UserInterface.Sales.Lookup;

public partial class ItemSelectionWindow
{
    public ItemSelectionWindowViewModel ViewModel { get; }

    public ItemSelectionWindow(ItemSelectionWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        ViewModel.Window = this;
        
        DataContext = this;

        InitializeComponent();

        ViewModel.ItemsFetched += (_, _) => FocusList();
    }

    public void FocusList()
    {
        FocusTarget.Focus();
        Keyboard.Focus(FocusTarget);
        var clickArgs = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
        {
            RoutedEvent = MouseLeftButtonDownEvent,
            Source = FocusTarget
        };
        
        FocusTarget.RaiseEvent(clickArgs);
        
        // Not wanted by customer
        /*ListView.Focus();
        
        if (ListView.Items.Count > 0)
        {
            var index = 0;

            if (ListView.SelectedItem != null)
                index = ListView.SelectedIndex;
            
            ListView.UpdateLayout();

            var item = (ListViewItem)ListView.ItemContainerGenerator.ContainerFromIndex(index);
            item?.Focus();
            
            Keyboard.Focus(item);
        }*/
    }
    
    private async void CategorySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await ViewModel.SetItemsByCategoryAsync();
    }

    private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.OnSearch((sender as TextBox)?.Text ?? "");
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.Enter && ViewModel.SelectedItem != null)
        {
            ViewModel.SelectCommand.Execute(null);
        }
    }

    private void ListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        ViewModel.SelectCommand.Execute(null);
    }
}