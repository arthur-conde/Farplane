using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Farplane.FFX.Data;
using Farplane.Memory;

namespace Farplane.FFX.EditorPanels.Aeons;

/// <summary>
/// Interaction logic for PartyStats.xaml
/// </summary>
public partial class AeonStats : UserControl
{
    int _characterIndex;

    public AeonStats() => this.InitializeComponent();

    public void Refresh(int characterIndex)
    {
        this._characterIndex = characterIndex;
        var partyMember = Party.ReadPartyMember(this._characterIndex);

        this.TextAeonName.Text = AeonName.GetName(characterIndex);

        this.TextBaseHP.Text = partyMember.BaseHp.ToString();
        this.TextBaseMP.Text = partyMember.BaseMp.ToString();

        this.TextOverdrive.Text = partyMember.OverdriveLevel.ToString();
        this.TextOverdriveMax.Text = partyMember.OverdriveMax.ToString();

        this.TextBaseStrength.Text = partyMember.BaseStrength.ToString();
        this.TextBaseDefense.Text = partyMember.BaseDefense.ToString();
        this.TextBaseMagic.Text = partyMember.BaseMagic.ToString();
        this.TextBaseMagicDef.Text = partyMember.BaseMagicDefense.ToString();
        this.TextBaseAgility.Text = partyMember.BaseAgility.ToString();
        this.TextBaseLuck.Text = partyMember.BaseLuck.ToString();
        this.TextBaseEvasion.Text = partyMember.BaseEvasion.ToString();
        this.TextBaseAccuracy.Text = partyMember.BaseAccuracy.ToString();
    }

    void ButtonMaxOverdrive_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        this.TextOverdrive.Text = partyMember.OverdriveMax.ToString();
        Party.SetPartyMemberAttribute(this._characterIndex, "OverdriveLevel", partyMember.OverdriveMax);
        this.Refresh(this._characterIndex);
    }

    void TextBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        try
        {
            var offset = 0;
            var textBox = sender as TextBox;
            switch (textBox.Name)
            {

                case "TextOverdrive":
                    Party.SetPartyMemberAttribute(this._characterIndex, "OverdriveLevel", byte.Parse(this.TextOverdrive.Text));
                    break;
                case "TextOverdriveMax":
                    Party.SetPartyMemberAttribute(this._characterIndex, "OverdriveMax", byte.Parse(this.TextOverdriveMax.Text));
                    break;
                case "TextAeonName":
                    AeonName.SetName(this._characterIndex, this.TextAeonName.Text);
                    break;
                case "TextBaseHP":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseHp") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, uint.Parse(this.TextBaseHP.Text), false);
                    break;
                case "TextBaseMP":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseMp") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, uint.Parse(this.TextBaseMP.Text), false);
                    break;
                case "TextBaseStrength":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseStrength") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, byte.Parse(this.TextBaseStrength.Text), false);
                    break;
                case "TextBaseDefense":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseDefense") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, byte.Parse(this.TextBaseDefense.Text), false);
                    break;
                case "TextBaseMagic":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseMagic") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, byte.Parse(this.TextBaseMagic.Text), false);
                    break;
                case "TextBaseMagicDef":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseMagicDefense") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, byte.Parse(this.TextBaseMagicDef.Text), false);
                    break;
                case "TextBaseAgility":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseAgility") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, byte.Parse(this.TextBaseAgility.Text), false);
                    break;
                case "TextBaseLuck":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseLuck") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, byte.Parse(this.TextBaseLuck.Text), false);
                    break;
                case "TextBaseEvasion":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseEvasion") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, byte.Parse(this.TextBaseEvasion.Text), false);
                    break;
                case "TextBaseAccuracy":
                    offset = (int)Marshal.OffsetOf<PartyMember>("BaseAccuracy") + Party.GetMemoryOffset(this._characterIndex);
                    GameMemory.Write(offset, byte.Parse(this.TextBaseAccuracy.Text), false);
                    break;
            }
            AeonsPanel.UpdateTabs();
            textBox.SelectAll();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error parsing input", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }
    }

    void MaxAll_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute(this._characterIndex, nameof(PartyMember.BaseHp), 99999);
        Party.SetPartyMemberAttribute(this._characterIndex, nameof(PartyMember.BaseMp), 9999);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseStrength), 255);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseDefense), 255);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseMagic), 255);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseMagicDefense), 255);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseAgility), 255);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseLuck), 255);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseEvasion), 255);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseAccuracy), 255);
        this.Refresh(this._characterIndex);
    }

    void MaxHP_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute(this._characterIndex, nameof(PartyMember.BaseHp), 99999);
        this.Refresh(this._characterIndex);
    }

    void MaxMP_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute(this._characterIndex, nameof(PartyMember.BaseMp), 9999);
        this.Refresh(this._characterIndex);
    }

    void MaxStrength_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseStrength), 255);
        this.Refresh(this._characterIndex);
    }

    void MaxDefense_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseDefense), 255);
        this.Refresh(this._characterIndex);
    }

    void MaxMagic_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseMagic), 255);
        this.Refresh(this._characterIndex);
    }

    void MaxMagicDefense_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseMagicDefense), 255);
        this.Refresh(this._characterIndex);
    }

    void MaxAgility_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseAgility), 255);
        this.Refresh(this._characterIndex);
    }

    void MaxLuck_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseLuck), 255);
        this.Refresh(this._characterIndex);
    }

    void MaxEvasion_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseEvasion), 255);
        this.Refresh(this._characterIndex);
    }

    void MaxAccuracy_Click(object sender, RoutedEventArgs e)
    {
        var partyMember = Party.ReadPartyMember(this._characterIndex);
        Party.SetPartyMemberAttribute<byte>(this._characterIndex, nameof(PartyMember.BaseAccuracy), 255);
        this.Refresh(this._characterIndex);
    }
}
