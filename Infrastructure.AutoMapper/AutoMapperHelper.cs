using AutoMapper;
using System;
using System.Reflection;

namespace Infrastructure.AutoMapper
{
    internal static class AutoMapperHelper
    {
        public static void CreateAutoAttributeMaps(this IMapperConfigurationExpression configuration, Type type)
        {
            foreach (var autoMapAttribute in type.GetCustomAttributes<AutoMapAttributeBase>())
            {
                autoMapAttribute.CreateMap(configuration, type);
            }
        }
    }
}
