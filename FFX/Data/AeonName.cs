using Farplane.Common;
using Farplane.Memory;

namespace Farplane.FFX.Data;

public class AeonName
{
    static readonly int _offsetAeonNames = OffsetScanner.GetOffset(GameOffset.FFX_AeonNames);

    public static void SetName(int partyIndex, string name)
    {
        if (partyIndex < 8)
        {
            return;
        }

        var offset = _offsetAeonNames + 0xA0 + AeonNames[partyIndex - 8];
        var aeonName = StringConverter.ToFFX(name);
        var nameBytes = new byte[aeonName.Length + 1];
        aeonName.CopyTo(nameBytes, 0);
        GameMemory.Write(offset, nameBytes, false);
    }

    public static string GetName(int partyIndex)
    {
        if (partyIndex < 8)
        {
            return null;
        }

        var offset = _offsetAeonNames + 0xA0 + AeonNames[partyIndex - 8];
        var nameBytes = GameMemory.Read<byte>(offset, 8, false);
        return StringConverter.ToASCII(nameBytes);
    }

    public static int[] AeonNames =>
    [
        0x00,
        0x14,
        0x28,
        0x3C,
        0x50,
        0x64,
        0x78,
        0x8C,
        0xA0,
        0xB4,
    ];
}
