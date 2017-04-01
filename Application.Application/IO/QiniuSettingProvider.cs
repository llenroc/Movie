using Infrastructure.Configuration;
using System.Collections.Generic;

namespace Application.IO
{
    public class QiniuSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                    {
                    new SettingDefinition(
                        "Qiniu.accessKey",
                        "FfK3PZzkR9XTByJmVAZDIK5CJANY1flCQI_mcVJj"
                        ),
                    new SettingDefinition(
                        "Qiniu.secretKey",
                        "PftjjwWk6v5Wg8oeg_yd-gNLEple2bAgQlmtguiB"
                        ),
                };
        }
    }
}
