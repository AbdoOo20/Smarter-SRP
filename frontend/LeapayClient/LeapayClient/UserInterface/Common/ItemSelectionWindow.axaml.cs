using Avalonia.Input;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Common;

public partial class ItemSelectionWindow: UserControlBase
{
    public ItemSelectionWindowViewModel ViewModel { get; }
    
    public ItemSelectionWindow(ItemSelectionWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        SynchronisationModel = viewModel;

        InitializeComponent();
        DataContext = this;

        ViewModel.ItemsFetched += (_, _) => FocusList();
    }
    
    public void FocusList()
    {
        FocusTarget.Focus();

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

    private void ListBoxDoubleTapped(object? sender, TappedEventArgs tappedEventArgs)
    {
        ViewModel.SelectCommand.Execute(null);
    }
}