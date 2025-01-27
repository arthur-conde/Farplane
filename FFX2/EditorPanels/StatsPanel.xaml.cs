using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Farplane.FFX.Data;

namespace Farplane.FFX2.EditorPanels;

/// <summary>
/// Interaction logic for StatsPanel.xaml
/// </summary>
public partial class StatsPanel : UserControl
{
    int _statsOffset;

    public StatsPanel() => this.InitializeComponent();

    void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        if (sender.GetType() != typeof(TextBox))
        {
            return;
        }

        if (sender is not TextBox senderTextBox)
        {
            return;
        }

        try
        {
            switch (senderTextBox.Name)
            {
                case "TextCurrentExperience":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.CurrentExperience,
                        BitConverter.GetBytes(uint.Parse(senderTextBox.Text)));
                    break;
                case "ModHP":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.HPModifier,
                        BitConverter.GetBytes(uint.Parse(senderTextBox.Text)));
                    break;
                case "ModMP":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.MPModifier,
                        BitConverter.GetBytes(uint.Parse(senderTextBox.Text)));
                    break;
                case "TextStrength":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModStrength,
                        [byte.Parse(senderTextBox.Text)]);
                    break;
                case "TextDefense":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModDefense,
                        [byte.Parse(senderTextBox.Text)]);
                    break;
                case "TextMagic":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModMagic,
                        [byte.Parse(senderTextBox.Text)]);
                    break;
                case "TextMagicDefense":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModMagicDefense,
                        [byte.Parse(senderTextBox.Text)]);
                    break;
                case "TextAgility":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModAgility,
                        [byte.Parse(senderTextBox.Text)]);
                    break;
                case "TextAccuracy":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModAccuracy,
                        [byte.Parse(senderTextBox.Text)]);
                    break;
                case "TextEvasion":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModEvasion,
                        [byte.Parse(senderTextBox.Text)]);
                    break;
                case "TextLuck":
                    LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModLuck,
                        [byte.Parse(senderTextBox.Text)]);
                    break;
                case "TextName":
                    break;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    public void Refresh(int partyIndex)
    {
        this._statsOffset = (int)OffsetType.PartyStatBase + (0x80 * partyIndex);
        var statsBytes = LegacyMemoryReader.ReadBytes(this._statsOffset, 0x80);

        this.TextCurrentExperience.Text = BitConverter.ToUInt32(statsBytes, (int)Offsets.StatOffsets.CurrentExperience).ToString();

        this.CurrentHP.Content = BitConverter.ToUInt32(statsBytes, (int)Offsets.StatOffsets.CurrentHP).ToString();
        this.MaxHP.Content = BitConverter.ToUInt32(statsBytes, (int)Offsets.StatOffsets.MaxHP).ToString();
        this.ModHP.Text = BitConverter.ToUInt32(statsBytes, (int)Offsets.StatOffsets.HPModifier).ToString();

        this.CurrentMP.Content = BitConverter.ToUInt32(statsBytes, (int)Offsets.StatOffsets.CurrentMP).ToString();
        this.MaxMP.Content = BitConverter.ToUInt32(statsBytes, (int)Offsets.StatOffsets.MaxMP).ToString();
        this.ModMP.Text = BitConverter.ToUInt32(statsBytes, (int)Offsets.StatOffsets.MPModifier).ToString();

        this.LabelStrength.Content = statsBytes[(int)Offsets.StatOffsets.Strength].ToString();
        this.LabelDefense.Content = statsBytes[(int)Offsets.StatOffsets.Defense].ToString();
        this.LabelMagic.Content = statsBytes[(int)Offsets.StatOffsets.Magic].ToString();
        this.LabelMagicDefense.Content = statsBytes[(int)Offsets.StatOffsets.MagicDefense].ToString();
        this.LabelAgility.Content = statsBytes[(int)Offsets.StatOffsets.Agility].ToString();
        this.LabelAccuracy.Content = statsBytes[(int)Offsets.StatOffsets.Accuracy].ToString();
        this.LabelEvasion.Content = statsBytes[(int)Offsets.StatOffsets.Evasion].ToString();
        this.LabelLuck.Content = statsBytes[(int)Offsets.StatOffsets.Luck].ToString();

        this.TextStrength.Text = statsBytes[(int)Offsets.StatOffsets.ModStrength].ToString();
        this.TextDefense.Text = statsBytes[(int)Offsets.StatOffsets.ModDefense].ToString();
        this.TextMagic.Text = statsBytes[(int)Offsets.StatOffsets.ModMagic].ToString();
        this.TextMagicDefense.Text = statsBytes[(int)Offsets.StatOffsets.ModMagicDefense].ToString();
        this.TextAgility.Text = statsBytes[(int)Offsets.StatOffsets.ModAgility].ToString();
        this.TextAccuracy.Text = statsBytes[(int)Offsets.StatOffsets.ModAccuracy].ToString();
        this.TextEvasion.Text = statsBytes[(int)Offsets.StatOffsets.ModEvasion].ToString();
        this.TextLuck.Text = statsBytes[(int)Offsets.StatOffsets.ModLuck].ToString();
    }

    void MaxAll_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.HPModifier, BitConverter.GetBytes(99999U));
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.MPModifier, BitConverter.GetBytes(99999U));
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModStrength, [0xFF]);
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModDefense, [0xFF]);
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModMagic, [0xFF]);
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModMagicDefense, [0xFF]);
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModAgility, [0xFF]);
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModAccuracy, [0xFF]);
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModEvasion, [0xFF]);
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModLuck, [0xFF]);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxHP_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.HPModifier, BitConverter.GetBytes(99999U));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxMP_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.MPModifier, BitConverter.GetBytes(99999U));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxStrength_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModStrength, [0xFF]);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxDefense_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModDefense, [0xFF]);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxMagic_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModMagic, [0xFF]);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxMagicDefense_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModMagicDefense, BitConverter.GetBytes(99999U));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxAgility_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModAgility, [0xFF]);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxLuck_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModLuck, [0xFF]);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxEvasion_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModEvasion, [0xFF]);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }

    void MaxAccuracy_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LegacyMemoryReader.WriteBytes(this._statsOffset + (int)Offsets.StatOffsets.ModAccuracy, [0xFF]);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error updating value", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        this.Refresh(0);
    }
}
