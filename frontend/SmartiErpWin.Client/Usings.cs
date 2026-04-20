global using CommunityToolkit.Mvvm.ComponentModel;
global using System;
global using System.Windows;
using Fly.Shared.Modules.PurchaseAndSales.Enums;



static class GlobalConfog
{
    public static bool IsDebug = true;
    public static StorageType StorageType = StorageType.Normal;
    public static PeymentType PeymentType = PeymentType.Deferred;
}