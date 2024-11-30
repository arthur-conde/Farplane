using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Farplane.Memory;

namespace Farplane.FFX.EditorPanels.Debug
{
    public partial class DebugPanel : UserControl
    {
        private readonly int _debugOffset = OffsetScanner.GetOffset(GameOffset.FFX_DebugFlags);

        private List<int> known;

        private List<int> unknown;

        public DebugPanel()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            byte[] array = GameMemory.Read<byte>(_debugOffset, 32, isRelative: false);
            string[] names = Enum.GetNames(typeof(DebugFlags));
            Array values = Enum.GetValues(typeof(DebugFlags));
            known = new List<int>();
            unknown = new List<int>();
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].StartsWith("Unknown"))
                {
                    unknown.Add((int)values.GetValue(i));
                }
                else
                {
                    known.Add((int)values.GetValue(i));
                }
            }
            for (int j = 0; j < known.Count; j++)
            {
                (StackDebugOptions.Children[j] as CheckBox).IsChecked = array[known[j]] == 1;
            }
            for (int k = 0; k < unknown.Count; k++)
            {
                (StackUnknown.Children[k] as CheckBox).IsChecked = array[unknown[k]] == 1;
            }
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < known.Count; i++)
            {
                GameMemory.Write(_debugOffset + known[i], (byte)((StackDebugOptions.Children[i] as CheckBox).IsChecked.Value ? 1 : 0), isRelative: false);
            }
            for (int j = 0; j < unknown.Count; j++)
            {
                GameMemory.Write(_debugOffset + unknown[j], (byte)((StackUnknown.Children[j] as CheckBox).IsChecked.Value ? 1 : 0), isRelative: false);
            }
            Refresh();
        }

        private void CheckShowUnknownFlags_OnChecked(object sender, RoutedEventArgs e)
        {
            StackUnknown.Visibility = ((!CheckShowUnknownFlags.IsChecked.Value) ? Visibility.Hidden : Visibility.Visible);
            TextUnknownWarning.Visibility = ((!CheckShowUnknownFlags.IsChecked.Value) ? Visibility.Collapsed : Visibility.Visible);
        }
    }
}
