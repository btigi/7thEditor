using System.Windows;
using SeventhEditor.ViewModels;

namespace SeventhEditor;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
