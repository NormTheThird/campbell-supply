using CampbellSupply.Common.Enums;
using System;
using System.ComponentModel;
using System.Reflection;

namespace CampbellSupply.Common.Helpers
{
    public static class DateTimeConvert
    {

        public static DateTime GetTimeZoneDateTime(TimeZoneInfoId infoId)
        {
            try
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(GetEnumDescription(infoId));
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
            }
            catch 
            {
                return DateTime.Now;
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
