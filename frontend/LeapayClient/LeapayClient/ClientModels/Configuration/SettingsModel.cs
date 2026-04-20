using System.Diagnostics.CodeAnalysis;
using Fly.Shared.Modules.Global.Dtos;
using SmartErpWin.Core.Models;
using SukiUI.Enums;

namespace Leapay.Client.ClientModels.Configuration;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class SettingsModel
{
    public string? Locale { get; set; }
    
    public string? TemporaryJwt { get; set; }

    public string? Theme { get; set; } = "light";
    
    public string? Colour { get; set; } = "Blue";
    
    public bool BackgroundTransitions { get; set; } = true;
    
    public bool BackgroundAnimations { get; set; } = true;

    public SukiBackgroundStyle BackgroundStyle { get; set; } = SukiBackgroundStyle.Gradient;
    
    public bool AutoLocalise { get; set; }

    public string PaneDisplayType { get; set; } = "left";
    
    public string? BackendUrl { get; set; }
    
    public bool UseWindowsHello { get; set; }
    
    public bool SerialNumberVisible { get; set; }

    public bool FinancialInternalNotesVisible { get; set; }

    public bool DroplistMenu { get; set; } = true;

    public List<string> FavouritedViews { get; set; } = [];

    public List<string?> LastOpenViews { get; set; } = [];
    
    public string? LastOpenView { get; set; }

    public bool ShouldNavigateToLastOpenView { get; set; } = true;
    
    public bool SalesPointExpanderOpen { get; set; }
    
    public bool IsMenuExpanded { get; set; } = true;

    public DtoCompanyInfo CompanyInfo { get; set; } = new();

    public SysConfigModel? SysConfigModel { get; set; } = new();
    
    public List<ConnectionInfo> Connections { get; set; } = new();
    
    public bool IsFrontendOnly { get; set; }
    
    public bool IsBackendFullySetup { get; set; }
    
    public bool HasUserSelectedSystemType { get; set; }
    
    public string? BackendPath { get; set; }
    
    public string? DbConstr { get; set; }
    
    public SystemType SystemType { get; set; }
}

public enum SystemType
{
    Frontend,
    FrontendAndBackend
}