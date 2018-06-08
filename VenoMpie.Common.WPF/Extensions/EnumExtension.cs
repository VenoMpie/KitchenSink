using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.WPF.Extensions
{
    public static class EnumExtension
    {
        public static string GetEnumDescription(this Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            var descriptionAttr = fieldInfo
                                    .GetCustomAttributes(false)
                                    .OfType<DescriptionAttribute>()
                                    .Cast<DescriptionAttribute>()
                                    .SingleOrDefault();
            if (descriptionAttr == null)
            {
                return enumObj.ToString();
            }
            else
            {
                return descriptionAttr.Description;
            }
        }
    }
}
