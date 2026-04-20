using System.Windows.Controls;
using System.Windows.Media;
using Fly.Shared.Modules.Accounting.Dtos;
using Wpf.Ui;
using Wpf.Ui.Controls;
using MenuItem = Wpf.Ui.Controls.MenuItem;
using ProgressRing = MahApps.Metro.Controls.ProgressRing;
using TextBlock = Wpf.Ui.Controls.TextBlock;
using TreeViewItem = System.Windows.Controls.TreeViewItem;

namespace SmartiErpWin.Client.UserInterface.Accounting.AccountingTree;

public sealed class AccountingTreePageViewModel : BaseRefreshablePageViewModel<DtoAccount>
{
    protected override string Endpoint => "Account";

    private readonly List<string> _stateRestoredObjects = [];
    
    public AccountingTreePageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
    }
    
    public override void ShowData(StackPanel panel)
    {
        DataGridPanel = panel;
        RefreshDataGrid();
    }

    protected override async void RefreshDataGrid()
    {
        try
        {
            SetState(true);
            SetRefreshState(true);
            DataGridPanel?.Children.Clear();
        
            var parentAccounts = (await GetAllAsync()).Data.ToList();
            var data = parentAccounts.Where(p => p.ParentId is null);
        
            var treeView = new TreeView
            {
                MinWidth = 600
            };

            foreach (var item in data)
            {
                var treeViewItem = new TreeViewItem
                {
                    BorderThickness = new Thickness(15),
                    Padding = new Thickness(3),
                    Margin = new Thickness(5),
                    Header = CreateParentHeader(item),
                    Tag = item
                };
            
                // we need to show a null item so that the expanded arrow
                // is shown. otherwise, no event will be fired
                treeViewItem.Items.Add(null);

                CreateContextMenuForParentAccount(treeViewItem,
                    item, false, false);
            
                treeViewItem.Expanded += OnTreeViewItemExpanded;
            
                treeView.Items.Add(treeViewItem);
                RestoreStateIfPossible(treeViewItem, item.Id.ToString());
            }
            
            DataGridPanel?.Children.Add(treeView);
        }
        catch (Exception exception)
        {
            ShowErrorResult(exception);
        }
        finally
        {
            SetState(false);
            SetRefreshState(false);
        }
    }

    private async void OnTreeViewItemExpanded(
        object sender,
        RoutedEventArgs args)
    {
        if (args.Source is not TreeViewItem sentTreeViewItem)
            return;

        // just to make sure we have no data, especially the 'null' item
        sentTreeViewItem.Items.Clear();

        if (sentTreeViewItem.Tag is not DtoAccount tagData)
            return;
        
        var ring = Find<ProgressRing>(sentTreeViewItem.Header);
        var card = Find<Card>(sentTreeViewItem.Header);

        if (ring is null || card is null)
        {
            // if the ring is null, this is not a parent tree view item,
            // and this event was not fired by an item of interest,
            // so let's just return
            
            return;
        }
        
        card.Visibility = Visibility.Hidden;
        ring.IsActive = true;
        
        var childAccounts = await GetAllInnerAsync("tree",
            Guid.Parse(tagData.Id.ToString()));

        if (childAccounts.Data.ToList() is not { } childrenData || childrenData.Count == 0)
        {
            card.Visibility = Visibility.Visible;
            ring.IsActive = false;
            
            return;
        }
        
        foreach (var account in childrenData)
        {
            var innerTreeViewItem = new TreeViewItem
            {
                Header = CreateParentHeader(account),
                Margin = new Thickness(2),
                Tag = account
            };
            innerTreeViewItem.Items.Add(null);
            
            CreateContextMenuForParentAccount(innerTreeViewItem, account);
            
            sentTreeViewItem.Items.Add(innerTreeViewItem);
            RestoreStateIfPossible(innerTreeViewItem, account.Id.ToString());
        }

        ring.IsActive = false;
        RegisterStateRestore(sentTreeViewItem, tagData.Id.ToString());
    }

    private void RestoreStateIfPossible(TreeViewItem item, string account)
    {
        if(_stateRestoredObjects.Contains(account))
            item.ExpandSubtree();
    }
    
    private void RegisterStateRestore(TreeViewItem item,
        string id,
        bool addNow = true)
    {
        if(addNow)
            Add();
        
        item.Expanded += (_, _) =>
        {
            Add();
        };
        
        item.Collapsed += (_, _) =>
        {
            _stateRestoredObjects.Clear();

            foreach (var treeItem in DataGridPanel?.Children
                         .OfType<TreeView>()
                         .FirstOrDefault()?.Items
                         .OfType<TreeViewItem>()
                         .ToList() ?? [])
            {
                LoopAndCheck(treeItem);
            }
        };
        
        return;

        void LoopAndCheck(TreeViewItem treeViewItem)
        {
            if (treeViewItem.IsExpanded)
            {
                _stateRestoredObjects.Add((treeViewItem.Tag as DtoAccount)
                    ?.Id.ToString() ?? "");
            }

            foreach (var innerItem in treeViewItem.Items.OfType<TreeViewItem>())
            {
                LoopAndCheck(innerItem);
            }
        }
        
        void Add()
        {
            if (!_stateRestoredObjects.Contains(id))
            {
                _stateRestoredObjects.Add(id);
            }
        }
    }
    
    private void CreateContextMenuForParentAccount(
        TreeViewItem innerTreeViewItem,
        DtoAccount account,
        bool canEdit = true,
        bool canDelete = true)
    {
        List<Tuple<string, Action?>> menuActions =
        [
            new(LocaleService["NewButton"], ExecuteAdd),
            new(LocaleService["EditButton"], canEdit ? ExecuteEdit : null),
            new(LocaleService["DeleteButton"], canDelete ? ExecuteDelete : null),
        ];
        
        CreateContextMenuGlobal(menuActions,
            LocaleService.DetermineFlowDirection(),
            innerTreeViewItem);
        
        // loop bug:
        // innerTreeViewItem.MouseDoubleClick += (_, _) => ExecuteEdit();

        return;

        void ExecuteAdd()
        {
            var entry = new AccountingTreeEntryWindow(
                LocaleService,
                new DtoAccount
                {
                    ParentId = account.Id
                },
                EntryType.Add,
                HttpClient, GetType());

            entry.ShowDialog();
        }
        
        void ExecuteEdit()
        {
            var entry = new AccountingTreeEntryWindow(
                LocaleService,
                account,
                EntryType.Edit,
                HttpClient, GetType());
            
            entry.ShowDialog();
        }
        
        async void ExecuteDelete()
        {
            if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
                return;
            
            await DeleteRemoveAsync(account.Id);
            RefreshDataGrid();
        }
    }
    
    private static void CreateContextMenuGlobal(
        List<Tuple<string, Action?>> menuActions,
        FlowDirection flowDirection,
        TreeViewItem innerTreeViewItem)
    {
        var contextMenu = new ContextMenu
        {
            FlowDirection = flowDirection
        };

        foreach (var (item, action) in menuActions)
        {
            if (action != null)
            {
                AddItem(item, action, contextMenu);
            }
        }
        
        innerTreeViewItem.ContextMenu = contextMenu;
    }

    private static void AddItem(string? @string, Action? action, ContextMenu contextMenu)
    {
        if (action == null)
            return;
        
        var editItem = new MenuItem
        {
            Header = @string,
            Command = new RelayCommand(action)
        };
            
        contextMenu.Items.Add(editItem);
    }

    private DockPanel CreateParentHeader(DtoAccount account) =>
        new()
        {
            LastChildFill = true,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Children =
            {
                new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Text = account.Name,
                    Padding = new Thickness(5),
                    FontSize = 18,
                    FontWeight = FontWeights.Regular
                },
                new ProgressRing
                {
                    Margin = new Thickness(15, 0, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = 25,
                    Height = 25,
                    IsActive = false,
                },
                new FrameworkElement
                {
                    Width = 300
                },
                new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(10, 0, 0, 0),
                    MaxHeight = 30,
                    Orientation = Orientation.Horizontal,
                    //Children =
                    //{
                    //    new Card
                    //    {
                    //        Padding = new Thickness(7),
                    //        Content = new TextBlock
                    //        {
                    //            Margin = new Thickness(0),
                    //            FontSize = 10,
                    //            Text = string.Format(LocaleService["TotalBalance"], account.TotalBalance)
                    //        }
                    //    },
                    //    new Card
                    //    {
                    //        Padding = new Thickness(7),
                    //        Margin = new Thickness(3, 0, 0, 0),
                    //        Content = new TextBlock
                    //        {
                    //            FontSize = 10,
                    //            Text = string.Format(LocaleService["TotalCredit"], account.TotalCredit)
                    //        },
                    //        Background = new SolidColorBrush(Colors.ForestGreen)
                    //    },
                    //    new Card
                    //    {
                    //        Padding = new Thickness(7),
                    //        Margin = new Thickness(3, 0, 0, 0),
                    //        Content = new TextBlock
                    //        {
                    //            Foreground = new SolidColorBrush(Colors.Black),
                    //            FontSize = 10,
                    //            Text = string.Format(LocaleService["TotalDebt"], account.TotalDebit)
                    //        },
                    //        Background = new SolidColorBrush(Colors.DarkOrange)
                    //    },
                    //}
                },
                new Card
                {
                    Visibility = Visibility.Hidden,
                    Padding = new Thickness(7),
                    Margin = new Thickness(3, 0, 0, 0),
                    Content = new TextBlock
                    {
                        FontSize = 12,
                        Text = LocaleService["NoAccountsFound"]
                    },
                    Background = new SolidColorBrush(Colors.DarkRed)
                },
            }
        };

    private static T? Find<T>(object? stackPanel)
    {
        return (stackPanel as DockPanel ?? new()).Children
            .OfType<T>()
            .FirstOrDefault();
    }

    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (!IsRefreshing)
        {
            RefreshDataGrid();
        }
        
        return Task.CompletedTask;
    }
}
