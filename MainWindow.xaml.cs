using System;
using System.Windows;
using System.Windows.Input;
using Farplane.Common.Controls;
using Farplane.Common.Dialogs;
using Farplane.FFX;
using Farplane.FFX2;
using Farplane.Memory;
using Farplane.Properties;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace Farplane;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    readonly ConfigFlyout _configFlyout = new();


    int _splashCounter = 10;

    public MainWindow()
    {
        var exeVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        if (exeVersion > new Version(Settings.Default.SettingsVersion))
        {
            Settings.Default.Upgrade();
        }

        Settings.Default.SettingsVersion = exeVersion.ToString();
        Settings.Default.Save();

        try
        {
            // Load app theme and accent
            var currentTheme = ThemeManager.GetAppTheme(Settings.Default.AppTheme);
            var currentAccent = ThemeManager.GetAccent(Settings.Default.AppAccent);

            ThemeManager.ChangeAppStyle(Application.Current, currentAccent, currentTheme);
        }
        catch
        {
            // Theme error, revert to default
            Settings.Default.AppTheme = "BaseLight";
            Settings.Default.AppAccent = "Blue";

            Settings.Default.Save();

            var currentTheme = ThemeManager.GetAppTheme(Settings.Default.AppTheme);
            var currentAccent = ThemeManager.GetAccent(Settings.Default.AppAccent);

            ThemeManager.ChangeAppStyle(Application.Current, currentAccent, currentTheme);
        }

        this.InitializeComponent();
        var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        this.Title = string.Format(this.Title, $"{version.Major}.{version.Minor}.{version.Build}");

        this.Flyouts = new FlyoutsControl();
        this.Flyouts.Items.Add(this._configFlyout);
    }

    void FFX2_Click(object sender, RoutedEventArgs e)
    {
        this._configFlyout.IsOpen = false;
        var processSelect = new ProcessSelectWindow("FFX-2") { Owner = this };
        processSelect.ShowDialog();

        if (processSelect.DialogResult == true)
        {
            this.Hide();
            var FFX2Editor = new FFX2Editor();
            FFX2Editor.ShowDialog();
            GameMemory.Detach();
            if (Settings.Default.CloseWithGame)
            {
                Environment.Exit(0);
            }

            this.Show();
            this.Topmost = true;
            this.Topmost = false;
        }
    }

    void FFX_Click(object sender, RoutedEventArgs e)
    {
        this._configFlyout.IsOpen = false;
        var processSelect = new ProcessSelectWindow("FFX") { Owner = this };
        processSelect.ShowDialog();

        if (processSelect.DialogResult == true)
        {
            this.Hide();

            if (processSelect.ResultProcess == null)
            {
                var FFXEditor = new FFXEditor(true);
                FFXEditor.ShowDialog();
            }
            else
            {
                var FFXEditor = new FFXEditor();
                FFXEditor.ShowDialog();
            }
            GameMemory.Detach();
            if (Settings.Default.CloseWithGame)
            {
                Environment.Exit(0);
            }

            this.Show();
            this.Topmost = true;
            this.Topmost = false;
        }
    }

    void ButtonConfig_Click(object sender, RoutedEventArgs e) => this._configFlyout.IsOpen = !this._configFlyout.IsOpen;

    void SplashLogo_MouseDown(object sender, MouseButtonEventArgs e)
    {
        this._splashCounter--;
        if (this._splashCounter > 0)
        {
            return;
        }

        this._splashCounter = 10;
        var credits = new CreditsWindow() { Owner = this, ShowInTaskbar = false, WindowStartupLocation = WindowStartupLocation.CenterOwner };
        credits.ShowDialog();
    }
}
