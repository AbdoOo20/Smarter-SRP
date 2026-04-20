using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.Prefabs;

public partial class PagerPrefab : UserControl
{
    public PagerPrefab()
    {
        foreach (var item in _availableLimits)
        {
            Limits.Add(item);
        }
        
        InitializeComponent();
    }

    public ObservableCollection<int> Limits { get; set; } = new();

    private List<int> _availableLimits = new List<int>
    {
        5, 10, 20, 30, 50, 100
    };
    
    public static readonly DependencyProperty LimitProperty =
        DependencyProperty.Register("Limit", typeof(object), typeof(PagerPrefab), new PropertyMetadata(null));

    public object Limit
    {
        get { return GetValue(LimitProperty); }
        set { SetValue(LimitProperty, value); }
    }

    public static readonly DependencyProperty PreviousButtonProperty =
        DependencyProperty.Register("PreviousButton", typeof(string), typeof(PagerPrefab), new PropertyMetadata(null));

    public string PreviousButton
    {
        get { return (string)GetValue(PreviousButtonProperty); }
        set { SetValue(PreviousButtonProperty, value); }
    }

    public static readonly DependencyProperty MoveOneDownCommandProperty =
        DependencyProperty.Register("MoveOneDownCommand", typeof(ICommand), typeof(PagerPrefab),
            new PropertyMetadata(null));

    public ICommand MoveOneDownCommand
    {
        get { return (ICommand)GetValue(MoveOneDownCommandProperty); }
        set { SetValue(MoveOneDownCommandProperty, value); }
    }

    public static readonly DependencyProperty PagesTextProperty =
        DependencyProperty.Register("PagesText", typeof(string), typeof(PagerPrefab), new PropertyMetadata(null));

    public string PagesText
    {
        get { return (string)GetValue(PagesTextProperty); }
        set { SetValue(PagesTextProperty, value); }
    }

    public static readonly DependencyProperty NextButtonProperty =
        DependencyProperty.Register("NextButton", typeof(string), typeof(PagerPrefab), new PropertyMetadata(null));

    public string NextButton
    {
        get { return (string)GetValue(NextButtonProperty); }
        set { SetValue(NextButtonProperty, value); }
    }

    public static readonly DependencyProperty MoveOneUpCommandProperty =
        DependencyProperty.Register("MoveOneUpCommand", typeof(ICommand), typeof(PagerPrefab),
            new PropertyMetadata(null));

    public ICommand MoveOneUpCommand
    {
        get { return (ICommand)GetValue(MoveOneUpCommandProperty); }
        set { SetValue(MoveOneUpCommandProperty, value); }
    }
}