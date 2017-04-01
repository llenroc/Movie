using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Util;
using Infrastructure.Configuration;
using Infrastructure.Dependency;
using Infrastructure.Extensions;

namespace Application.IO
{
    public class QiniuFileHelper:ISingletonDependency
    {
        public SettingManager SettingManager { get; set; }

        public string GetAuthorizedDownloadPath(string path)
        {
            // AK = "ACCESS_KEY"
            // SK = "SECRET_KEY"
            // 加上过期参数，使用?e=<UnixTimestamp>
            // rawURL = "RAW_URL" + "?e=1482207600"; 
            Mac mac = new Mac(SettingManager.GetSettingValue("Qiniu.accessKey"), SettingManager.GetSettingValue("Qiniu.secretKey"));
            var e = DateTime.Now.AddDays(2).ToStamp();
            path = path + "?e=" + e;
            string token = Auth.createDownloadToken(path, mac);
            string signedURL = path + "&token=" + token;
            return signedURL;
        }
    }
}
