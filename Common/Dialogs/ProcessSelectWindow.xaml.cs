using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Farplane.Memory;
using Farplane.Properties;
using MahApps.Metro.Controls;

namespace Farplane.Common.Dialogs;

/// <summary>
/// Interaction logic for ProcessSelectWindow.xaml
/// </summary>
public partial class ProcessSelectWindow : MetroWindow
{
    readonly string _moduleName = string.Empty;
    Process _selectedProcess;
    public Process ResultProcess => this.DialogResult == false ? null : this._selectedProcess;
    bool _ready;

    public ProcessSelectWindow(string moduleName)
    {
        this.InitializeComponent();
        this._moduleName = moduleName;

        if (Settings.Default.ShowAllProcesses)
        {
            this.ButtonShowAll.IsChecked = true;
        }

        this.PopulateProcessList(this._moduleName);
    }

    void PopulateProcessList(string moduleName)
    {
        this._ready = false;
        this.ProcessList.Items.Clear();

        var processes = Process.GetProcesses();

        foreach (var process in processes)
        {
            if (string.Compare(process.ProcessName, moduleName, StringComparison.CurrentCultureIgnoreCase) != 0 && !this.ButtonShowAll.IsChecked.Value)
            {
                continue;
            }

            try
            {
                var processFile = process.MainModule.FileName;
                var icon = System.Drawing.Icon.ExtractAssociatedIcon(processFile);

                ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
        icon.Handle,
        Int32Rect.Empty,
        BitmapSizeOptions.FromEmptyOptions());

                var processItem = new ProcessListItem
                {
                    ProcessIcon = imageSource,
                    ProcessID = process.Id,
                    ProcessName = process.ProcessName,
                    Process = process
                };

                this.ProcessList.Items.Add(processItem);
            }
            catch
            {
                continue;
            }

        }
        this._ready = true;
    }

    void SelectProcess_Click(object sender, RoutedEventArgs e)
    {
        if (!this._ready || this.ProcessList.SelectedItems.Count != 1)
        {
            return;
        }

        var selectedProcess = ((ProcessListItem)this.ProcessList.SelectedItem).Process;
        var attachResult = GameMemory.Attach(selectedProcess);
        if (!attachResult)
        {
            return;
        }

        LegacyMemoryReader.Attach(selectedProcess);
        this._selectedProcess = selectedProcess;
        this.DialogResult = true;
        this.Close();
    }

    void Refresh_Click(object sender, RoutedEventArgs e) => this.RefreshList();

    void RefreshList() => this.PopulateProcessList(this.ButtonShowAll.IsChecked != null && this.ButtonShowAll.IsChecked.Value ? string.Empty : this._moduleName);

    void ShowAll_Click(object sender, RoutedEventArgs e)
    {
        if (!this._ready)
        {
            return;
        }

        this.RefreshList();
    }

    void ButtonFileMode_OnClick(object sender, RoutedEventArgs e)
    {
        this._selectedProcess = null;
        this.DialogResult = true;
        this.Close();
    }
}

class ProcessListItem
{
    public ImageSource ProcessIcon { get; set; }
    public int ProcessID { get; set; }
    public string ProcessName { get; set; }
    public Process Process { get; set; }
}
