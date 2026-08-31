using System.Windows;
using SeventhEditor.ViewModels;

namespace SeventhEditor;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ThemeManager.InitializeWindow(this);
        DataContext = new MainViewModel();
    }
}
