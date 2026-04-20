using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Leapay.Client.Helpers;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.HomePage;
using SmartErpWin.Core;
using SukiUI.Toasts;

namespace Leapay.Client.ClientModels.Dto.DtoModels.UiModels;

public partial class TabbedViewCardModel: ObservableObject
{
    [ObservableProperty]
    public partial string? ViewName { get; set; }
    
    [ObservableProperty]
    public partial string? CardGroup { get; set; }

    [ObservableProperty]
    public partial string? CardName { get; set; }
    
    [ObservableProperty]
    public partial MaterialIconKind CardSymbol { get; set; } = MaterialIconKind.Card;

    [ObservableProperty]
    public partial object? View { get; set; }
    
    [ObservableProperty]
    public partial bool IsVisible { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StarButtonContent))]
    public partial bool IsFavourited { get; set; }

    public object? StarButtonContent => GetStarButton();

    public bool ListenToFavouriteEvents { get; set; }

    private object GetStarButton()
    {
        const int width = 30;
        const int height = 30;
        
        if (ViewName is "HomeTab" || CardGroup is "GeneralReports")
            return new Control
            {
                Width = width,
                Height = height
            };
        
        var starButton = new ToggleButton
        {
            Width = width,
            Height = height,
            Background = Brushes.Transparent,
            BorderBrush = Brushes.Transparent
        };

        starButton.Content = GetPathIcon(false);

        starButton.Bind(ToggleButton.IsCheckedProperty, new Binding(nameof(IsFavourited)));

        starButton.GetObservable(ToggleButton.IsCheckedProperty).Subscribe(b =>
        {
            starButton.Content = GetPathIcon(b.GetValueOrDefault());
        });

        return starButton;

        PathIcon GetPathIcon(bool check)
        {
            return new PathIcon
            {
                Data = Geometry.Parse("M12,17.27 L18.18,21 L16.54,13.97 L22,9.24 L14.81,8.63 L12,2 L9.19,8.63 L2,9.24 L7.46,13.97 L5.82,21 z"),
                Foreground = check ? Brushes.Gold : Brushes.Gray,
                Width = 17,
                Height = 17,
                RenderTransform = new TranslateTransform(-10, 0)
            };
        }
    }
    
    partial void OnIsFavouritedChanged(bool value)
    {
        var toastManager = App.GetService<ISukiToastManager>();
        var locale = App.GetService<LocalisationService>();

        if (!ListenToFavouriteEvents)
        {
            return;
        }
        
        if (value && PreferencesService.Get(s => s.FavouritedViews).Count >= 10)
        {
            IsFavourited = false;
            _ = DialogueServices.ShowMessageAsync(locale["Error"], locale["FavouritedViewsAreTenAtMax"]);
            return;
        }
        
        PreferencesService.Set(s =>
        {
            if (value && !s.FavouritedViews.Contains(ViewName))
            {
                s.FavouritedViews.Add(ViewName);
                toastManager.CreateSimpleInfoToast().WithTitle(locale["Success"])
                    .WithContent(string.Format(locale["AddFavouriteViewNotif"], CardName))
                    .Queue();
            }
            else if(!value && s.FavouritedViews.Contains(ViewName))
            {
                s.FavouritedViews.Remove(ViewName);
                toastManager.CreateSimpleInfoToast().WithTitle(locale["Success"])
                    .WithContent(string.Format(locale["RemoveFavouriteViewNotif"], CardName))
                    .Queue();
            }
        });
        
        ContentHelper.RefreshFavouritesInMainMenu();
        App.GetService<HomePageViewModel>().OnSynchronisationRequestedAsync(CodeBook.RefData);
    }

    public void SetIsFavouritedFromSettings()
    {
        ListenToFavouriteEvents = false;
        IsFavourited = PreferencesService.Get(s => s.FavouritedViews).Contains(ViewName);
        ListenToFavouriteEvents = true;
    }
}