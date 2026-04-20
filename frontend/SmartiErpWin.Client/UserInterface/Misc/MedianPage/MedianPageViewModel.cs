using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common.Dtos;
using SmartErpWin.Core.Extensions;
using SmartiErpWin.Client.DtoModels.UiModels;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Misc.MedianPage;

public partial class MedianPageViewModel : BaseViewModel<DtoBase>
{
    [ObservableProperty]
    private ObservableCollection<CardModel> _cards = [];
    
    public MedianPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
    }

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
        
        return base.OnSynchronisationRequestedAsync(args);
    }
}