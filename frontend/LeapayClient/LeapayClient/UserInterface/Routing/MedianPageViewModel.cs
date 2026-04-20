using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels.UiModels;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Routing;

public partial class MedianPageViewModel : BasePageViewModel<DtoBase>
{
    [ObservableProperty]
    public partial ObservableCollection<CardModel> Cards { get; set; } = []; 

    [ObservableProperty]
    public partial string? CurrentCategoryName { get; set; } 
    
    private void LoadCards(IEnumerable<CardModel> items)
    {
        items.Observe(Cards);
    }
    
    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.FirstOrDefault() is IEnumerable<CardModel> items)
        {
            LoadCards(items);
        }

        if (args.ElementAtOrDefault(1) is string catName)
        {
            CurrentCategoryName = catName;
        }
        
        return Task.CompletedTask;
    }
}