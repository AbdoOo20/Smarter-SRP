using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Common.BarcodePrintDialogue;

public partial class BarcodePrintDialogueWindow: UserControlBase
{
    public BarcodePrintDialogueViewModel ViewModel { get; }
    
    public BarcodePrintDialogueWindow(BarcodePrintDialogueViewModel viewModel)
    {
        ViewModel = viewModel;
        SynchronisationModel = viewModel;
        
        InitializeComponent();
        DataContext = this;
    }
}