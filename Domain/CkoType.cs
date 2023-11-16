using System.ComponentModel;

namespace Domain
{
    [TypeConverter(typeof(EnumTypeConverter))]
    public enum CkoType
    {
        [Description("СКО")]
        cko,
        [Description("УПО")]
        upo,
        [Description("УМ")]
        um
    }

    public static class CkoTypeConverter
    {
        public static string ToConvertedString(this CkoType diskType)
        {
            if (diskType == CkoType.cko)
                return "CKO";
            else if (diskType == CkoType.upo)
                return "UPO";
            else if (diskType == CkoType.um)
                return "UM";
            return "";
        }
    }
}
