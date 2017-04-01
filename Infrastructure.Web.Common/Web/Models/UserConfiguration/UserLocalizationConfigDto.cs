using System.Collections.Generic;
using Infrastructure.Localization;

namespace Infrastructure.Web.Models.UserConfiguration
{
    public class UserLocalizationConfigDto
    {
        public UserCurrentCultureConfigDto CurrentCulture { get; set; }

        public List<LanguageInfo> Languages { get; set; }

        public LanguageInfo CurrentLanguage { get; set; }

        public List<LocalizationSourceDto> Sources { get; set; }

        public Dictionary<string, Dictionary<string, string>> Values { get; set; }
    }
}