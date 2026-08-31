namespace SeventhEditor;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(System.Windows.StartupEventArgs e)
    {
        ThemeManager.ApplyTheme(AppSettings.Instance.Theme);
        base.OnStartup(e);
    }
}
