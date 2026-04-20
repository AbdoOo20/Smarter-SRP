using Fly.Framework.Shared.Common.Dtos;
using Material.Icons.Avalonia;

namespace Leapay.Client.Polymorphism.Bases;

public class ConcreteViewModel: BaseViewModel<DtoBase>
{
    public string DisplayName { get; set; }
    
    public MaterialIcon Icon { get; set; }
    
    public int Index { get; set; }
}