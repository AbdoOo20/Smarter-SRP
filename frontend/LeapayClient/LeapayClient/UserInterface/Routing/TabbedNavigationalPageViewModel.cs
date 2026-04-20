using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels.UiModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core;

namespace Leapay.Client.UserInterface.Routing;

public partial class TabbedNavigationalPageViewModel : BasePageViewModel<DtoBase>
{
    [ObservableProperty]
    public partial ObservableCollection<TabbedViewCardModel> Cards { get; set; } = []; 

    [ObservableProperty]
    public partial TabbedViewCardModel? SelectedTab { get; set; }
    
    private static string? _lastTrueTab;

    private void AddOrSelectCard(TabbedViewCardModel item)
    {
        if (Cards.All(c => c.ViewName != item.ViewName))
        {
            Cards.Add(item);
        }

        SelectedTab = Cards.FirstOrDefault(c => c.ViewName == item.ViewName);
        SelectedTab?.SetIsFavouritedFromSettings();

        SyncLastOpenWithSettings();
    }

    [RelayCommand]
    private void OnRemoveCard(TabbedViewCardModel item)
    {
        var selectedWasRemoved = SelectedTab?.ViewName == item.ViewName;
        var indexOfItem = Cards.IndexOf(item);

        if (Cards.Any(c => c.ViewName == item.ViewName))
        {
            Cards.Remove(item);
        }

        if (selectedWasRemoved)
        {
            SelectedTab = Cards.ElementAtOrDefault(indexOfItem - 1) ?? Cards.LastOrDefault();
        }

        if (item.View is UserControlBase userControl)
        {
            userControl.Dispose();
        }
        
        SyncLastOpenWithSettings();
    }
    
    partial void OnSelectedTabChanged(TabbedViewCardModel? value)
    {
        if(SelectedTab?.View is UserControlBase @base)
        {
            _ = @base.SynchronisationModel?.OnSynchronisationRequestedAsync(value.ViewName);
        }
        
        PreferencesService.Set(s => s.LastOpenView = SelectedTab?.ViewName);
    }

    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.FirstOrDefault() is TabbedViewCardModel item)
        {
            AddOrSelectCard(item);
        }
        
        if (args.FirstOrDefault() is CodeBook.LimVis && args.ElementAtOrDefault(1) is TabbedViewCardModel card)
        {
            OnRemoveCard(card);
        }
        
        if (args.FirstOrDefault() is CodeBook.AddNew)
        {
            ImportViewsFromSettings();
        }
        
        if (args.FirstOrDefault() is CodeBook.RefCom && args.ElementAtOrDefault(1) is string str)
        {
            _lastTrueTab = str;
        }
        
        return Task.CompletedTask;
    }

    private void SyncLastOpenWithSettings()
    {
        PreferencesService.Set(s 
            => s.LastOpenViews = Cards.Where(g => g.CardGroup is not "GeneralReports")
                .Select(f => f.ViewName).ToList());
    }

    private void ImportViewsFromSettings()
    {
        var views = PreferencesService.Get(s
            => s.LastOpenViews);

        if (!views.Any())
        {
            SelectedTab = Cards.FirstOrDefault();
            return;
        }
        
        Cards.Clear();
        
        foreach (var view in views.Select(ContentHelper.GetView))
        {
            var tab = view.GetTabCardModel();
            tab.SetIsFavouritedFromSettings();
            
            Cards.Add(tab);
        }

        if (PreferencesService.Get(y => y.ShouldNavigateToLastOpenView))
        {
            if (!string.IsNullOrEmpty(_lastTrueTab) && Cards.Any(v => v.ViewName == _lastTrueTab))
            {
                SelectedTab = Cards.FirstOrDefault(c => c.ViewName == _lastTrueTab);
            }
        }
    }
}