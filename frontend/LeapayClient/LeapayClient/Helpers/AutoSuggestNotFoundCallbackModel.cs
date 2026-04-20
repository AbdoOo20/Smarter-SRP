using Avalonia.Layout;
using Avalonia.Media;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Sales.Clients;
using SmartErpWin.Core.Enums;
using SukiUI.Controls;
// ReSharper disable AccessToModifiedClosure

namespace Leapay.Client.Helpers;

public class AutoSuggestNotFoundCallbackModel
{
    public event EventHandler? AddUi;
    
    public event EventHandler? RemoveUi;
    
    public event EventHandler<string>? EntityNotFound;
    
    public event EventHandler<DtoBase>? EntityCreated;
    
    public string? CurrentEntityName { get; set; }
    
    public object? UiToShow { get; set; }

    public void OnAddUi()
    {
        AddUi?.Invoke(this, EventArgs.Empty);
    }

    public void OnRemoveUi()
    {
        RemoveUi?.Invoke(this, EventArgs.Empty);
    }

    public void OnEntityNotFound(string name)
    {
        CurrentEntityName = name;
        OnAddUi();
        EntityNotFound?.Invoke(this, name);
    }

    public static AutoSuggestNotFoundCallbackModel? CreateClientNotFoundCallback()
    {
        var locale = App.GetService<LocalisationService>();
        
        AutoSuggestNotFoundCallbackModel? callback = null;
        var judge = App.GetService<SessionService>().JudgeSpecificAccess("Client.Add");

        if (!judge)
            return null;
        
        callback = new AutoSuggestNotFoundCallbackModel
        {
            UiToShow = new GlassCard
            {
                IsOpaque = true,
                Padding = new(-2),
                Content = new StackPanel
                {
                    Spacing = 5,
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        new TextBlock
                        {
                            Text = locale["ClientNotFound"],
                            Margin = new(8, 0, 0, 0),
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 12
                        },
                        new Button
                        {
                            Classes = { "Flat", "Rounded" },
                            FontSize = 11,
                            FontWeight = FontWeight.ExtraBold,
                            Content = locale["AddClient"],
                            Command = new AsyncRelayCommand(ExecuteAsync)
                        }
                    }
                }
            }
        };

        return callback;

        async Task ExecuteAsync()
        {
            var entry = new ClientEntryWindow(new(new()
            {
                Name = callback?.CurrentEntityName ?? locale["GeneralClient"]
            }, EntryType.Add, typeof(ClientsPageViewModel)));
            await entry.ViewModel.SaveCommand.ExecuteAsync(null);
            
            var client = await entry.ViewModel.WaitUntilUserSavesAsync();
            
            callback?.OnEntityCreated(client);
        }
    }

    public void OnEntityCreated(DtoBase e)
    {
        EntityCreated?.Invoke(this, e);
    }
}