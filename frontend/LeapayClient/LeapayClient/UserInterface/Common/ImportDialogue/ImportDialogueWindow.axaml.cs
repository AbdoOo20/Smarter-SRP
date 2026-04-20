using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;

namespace Leapay.Client.UserInterface.Common.ImportDialogue;

public partial class ImportDialogueWindow: UserControlBase
{
    public required object? ViewModel
    {
        get;
        set
        {
            field = value;
            SynchronisationModel = value as ISynchronisationAware;
            InitializeComponent();
            DataContext = this;
        }
    }

    public ImportDialogueWindow()
    {
        
    }
}