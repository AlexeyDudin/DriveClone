using System.ComponentModel;

namespace Domain
{
    [TypeConverter(typeof(EnumTypeConverter))]
    public enum DriveType
    {
        [Description("Инсталляционный")]
        loader,
        [Description("С исходниками")]
        distributive,
        [Description("Тип 3")]
        type_3,
        [Description("Тип 4")]
        type_4,
        [Description("Тип 5")]
        type_5,
        [Description("Тип 6")]
        type_6
    }

    public static class DriveTypeConverter
    {
        public static string ToConvertedString(this DriveType diskType)
        {
            if (diskType == DriveType.loader)
                return "INST";
            else if (diskType == DriveType.distributive)
                return "DIST";
            else if (diskType == DriveType.type_3)
                return "TYPE3";
            else if (diskType == DriveType.type_4)
                return "TYPE4";
            else if (diskType == DriveType.type_5)
                return "TYPE5";
            else if (diskType == DriveType.type_6)
                return "TYPE6";
            else
                return "";
        }
    }
}
