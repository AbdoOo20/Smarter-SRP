using Avalonia.Animation.Easings;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;

namespace Leapay.Client.UserInterface.Common.HomePage;

public partial class HomePage : UserControlBase
{
    public HomePage() { } // designer

    public HomePageViewModel ViewModel { get; set; }
    
    public HomePage(HomePageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        _ = AnimationHelper.AnimatePopUpAsync(WelcomeText, 2000, 0.1)
            .ContinueWith(_ => Dispatcher.UIThread.Post(AnimateUi));
    }

    private void AnimateUi()
    {
        ViewModel.LoadCards();

        AnimateCards();
        ViewModel.SynchronisationRequested += (_, _) => AnimateCards(false);
    }

    private void AnimateCards(bool run = true)
    { 
        const double cardWidth = 200;
        const double cardHeight = 300;
        const double favCardWidth = 200;
        const double favCardHeight = 50;
        const double dashboardCardsTopHeight = 680;
        const double favTopHeight = 130;
        bool anim = run && PreferencesService.Get(s => s.BackgroundTransitions);
        
        Dispatcher.UIThread.Post(() =>
        {
            _ = AnimationHelper.AnimateMoveAndScaleAsync(WelcomeText,
                0, -15, 0.65, 2000,
                defaultYTransform: 400,
                easing: new CubicEaseInOut(),
                enable: run);
            SetFavWidths(!ViewModel.FavouritedViews.Any());

            if (anim)
            {
                _ = AnimationHelper.AnimatePopUpAsync(FavouritedViewsText, 1500, 0.1);
            }

            ViewModel.FavouritedItemsRefreshed += (_, _) =>
            {
                SetFavWidths(!ViewModel.FavouritedViews.Any());
            };
            
            if (anim)
            {
                _ = AnimationHelper.AnimatePopUpAsync(FavouritedViewsTextTop, 3000, 0.1);
            }
            
            /*for (var index = 0; index < Favourites.Items.Count; index++)
            {
                var container = Favourites.ContainerFromIndex(index);

                var child = container?.GetVisualDescendants().OfType<Control>().FirstOrDefault();
                
                if (child != null)
                {
                    child.Width = 200;
                    child.Height = 62;

                    if (anim)
                    {
                        _ = AnimationHelper.AnimatePopUpAsync(child, 3000, 0.1);
                    }
                }
            }*/

            DoTotalWidth(CardsTop, dashboardCardsTopHeight);
            if (anim)
            {
                _ = AnimationHelper.AnimatePopUpAsync(CardsTop, 3000, 0.1);
            }
            
            for (var index = 0; index < Cards.Items.Count; index++)
            {
                var container = Cards.ContainerFromIndex(index);

                var child = container?.GetVisualDescendants().OfType<Control>().FirstOrDefault();
                
                if (child != null)
                {
                    DoInstant(child);

                    if (anim)
                    {
                        _ = AnimationHelper.AnimatePopUpAsync(child, 3000, 0.1);
                    }
                }
            }
        }, DispatcherPriority.Loaded); // ensures layout is ready
        return;

        void DoInstant(Control child)
        {
            child.Width = cardWidth;
            child.Height = cardHeight;
        }

        void DoTotalWidth(Control control, double height)
        {
            control.Width = Bounds.Width;
            control.Height = height;
        }
        
        void SetFavWidths(bool invis = false)
        {
            DoTotalWidth(FavouritedViewsTextTop, favTopHeight);

            FavouritedViewsTextTop.IsVisible = true;
            FavouritedViewsText.Width = favCardWidth;
            FavouritedViewsText.Height = favCardHeight;
            FavViewsInfoBar.IsVisible = invis;
        }
    }
}