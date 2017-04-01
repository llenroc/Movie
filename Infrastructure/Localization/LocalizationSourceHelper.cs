using Infrastructure.Configuration.Startup;
using Infrastructure.Extensions;
using Infrastructure.Logging;
using System;
using System.Globalization;

namespace Infrastructure.Localization
{
    public static class LocalizationSourceHelper
    {
        public static string ReturnGivenNameOrThrowException(ILocalizationConfiguration configuration, string sourceName, string name, CultureInfo culture)
        {
            var exceptionMessage = string.Format("Can not find '{0}' in localization source '{1}'!",name, sourceName);

            if (!configuration.ReturnGivenTextIfNotFound)
            {
                throw new Exception(exceptionMessage);
            }
            LogHelper.Logger.Warn(exceptionMessage);

            var notFoundText = configuration.HumanizeTextIfNotFound? name.ToSentenceCase(culture): name;
            return configuration.WrapGivenTextIfNotFound? string.Format("{0}", notFoundText): notFoundText;
        }
    }
}
