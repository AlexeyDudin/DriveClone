using System.ComponentModel;

namespace Domain
{
    [TypeConverter(typeof(EnumTypeConverter))]
    public enum VidType
    {
        [Description("Тип1")]
        type1,
        [Description("Тип2")]
        type2,
        [Description("Тип3")]
        type3
    }

    public static class CkoTypeConverter
    {
        public static string ToConvertedString(this VidType diskType)
        {
            if (diskType == VidType.type1)
                return "TYPE_1";
            else if (diskType == VidType.type2)
                return "TYPE_2";
            else if (diskType == VidType.type3)
                return "TYPE_3";
            return "";
        }
    }
}
