using System.ComponentModel;

namespace Domain
{
    [TypeConverter(typeof(EnumTypeConverter))]
    public enum DriveType
    {
        [Description("Загрузочный")]
        loader,
        [Description("Дистрибутивный")]
        distributive,
        [Description("Шлюзовой")]
        shluz,
        [Description("С шаблонами")]
        shablon,
        [Description("Диск разработчика")]
        developer,
        [Description("Диск с дополнениями")]
        dopPo
    }

    public static class DriveTypeConverter
    {
        public static string ToConvertedString(this DriveType diskType)
        {
            if (diskType == DriveType.loader)
                return "LOAD";
            else if (diskType == DriveType.distributive)
                return "DIST";
            else if (diskType == DriveType.shluz)
                return "SHLUZ";
            else if (diskType == DriveType.shablon)
                return "SHB";
            else if (diskType == DriveType.developer)
                return "DEV";
            else if (diskType == DriveType.dopPo)
                return "DOP";
            else
                return "";
        }
    }
}
