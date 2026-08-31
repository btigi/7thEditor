using System.Reflection;
using System.Windows;

namespace SeventhEditor;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
        ThemeManager.InitializeWindow(this);
        VersionText.Text = $"Version {GetVersion()}";
    }

    private static string GetVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        return version?.ToString() ?? "1.0.0.0";
    }

    private void Ok_Click(object sender, RoutedEventArgs e) => Close();
}