using System.Windows.Controls;
using Farplane.FFX.Data;
using MahApps.Metro.Controls;

namespace Farplane.FFX.EditorPanels.Aeons;

/// <summary>
/// Interaction logic for AeonsPanel.xaml
/// </summary>
public partial class AeonsPanel : UserControl
{
    public delegate void UpdateTabsDelegate();
    public static event UpdateTabsDelegate UpdateTabsEvent;
    int _currentAeon = 8;
    readonly AeonStats _aeonStats = new();
    readonly AeonAbilities _aeonAbilities = new();

    public AeonsPanel()
    {
        this.InitializeComponent();
        foreach (var item in this.TabAeon.Items)
        {
            ControlsHelper.SetHeaderFontSize((TabItem)item, 14);
        }

        UpdateTabsEvent += this.Refresh;
    }

    public static void UpdateTabs() => UpdateTabsEvent?.Invoke();

    public void Refresh()
    {
        // Refresh names
        for (var i = 0; i < 10; i++)
        {
            var aeonTab = (TabItem)this.TabAeon.Items[i];
            aeonTab.Header = AeonName.GetName(i + 8);
        }
        this._aeonStats.Refresh(this._currentAeon);
        this._aeonAbilities.Refresh(this._currentAeon);
    }

    void TabAeonTab_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var itemIndex = this.TabAeonTab.Items.IndexOf(e.AddedItems[0]);
        this.Refresh();
        switch (itemIndex)
        {
            case 0:
                this.ContentAeon.Content = this._aeonStats;
                break;
            case 1:
                this.ContentAeon.Content = this._aeonAbilities;
                break;
        }
    }

    void TabAeon_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        this._currentAeon = this.TabAeon.Items.IndexOf(e.AddedItems[0]) + 8;
        this.Refresh();
    }
}
