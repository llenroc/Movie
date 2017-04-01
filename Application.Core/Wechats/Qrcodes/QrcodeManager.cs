using Application.Configuration;
using Application.Spread;
using Infrastructure;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.IO;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.Containers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Utility.Net;
using Utility.Web;

namespace Application.Wechats.Qrcodes
{
    public class QrcodeManager:ApplicationDomainServiceBase
    {
        public IRepository<Qrcode> qrcodeRepository { get; set; }
        public AppFolderHelper AppFolderHelper { get; set; }
        public SpreadManager SpreadManager { get; set; }
        private const string qrcodePreUrlBaseFormat = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";

        private string GetQrcodeFolderPathOfUser(long userId)
        {
            string userResourceFolder = AppFolderHelper.GetUserResourcePath(userId);
            string userQrcodeFolderPath = userResourceFolder + "/Qrcode";
            DirectoryHelper.CreateIfNotExists(PathHelper.GetAbsolutePath(userQrcodeFolderPath));
            return userQrcodeFolderPath;
        }

        [UnitOfWork]
        public async Task<Qrcode> CreateQrcodeAsync(UserIdentifier userIdentifier)
        {
            string appId = await SettingManager.GetSettingValueForTenantAsync(WechatSettings.General.AppId, userIdentifier.TenantId.Value);
            string appSecret = await SettingManager.GetSettingValueForTenantAsync(WechatSettings.General.Secret, userIdentifier.TenantId.Value);

            Qrcode maxSceneIdQrcode = qrcodeRepository.GetAll().OrderByDescending(model => model.SceneId).FirstOrDefault();
            int sceneId = maxSceneIdQrcode==null?1: maxSceneIdQrcode.SceneId+1;

            Qrcode qrcode = new Qrcode()
            {
                ExpireSeconds= 604800,
                UserId=userIdentifier.UserId
            };

            //大于10万，生成临时二维码
            if (sceneId > 100000)
            {
                qrcode.Type = QrCode_ActionName.QR_LIMIT_SCENE;
            }
            else
            {
                qrcode.SceneId = sceneId;
                qrcode.Type = QrCode_ActionName.QR_SCENE;
            }
            string accessToken= AccessTokenContainer.TryGetAccessToken(appId, appSecret);

            CreateQrCodeResult createQrCodeResult=QrCodeApi.Create(
                accessToken, 
                qrcode.ExpireSeconds, 
                qrcode.SceneId, 
                qrcode.Type);

            qrcode.Ticket = createQrCodeResult.ticket;
            qrcode.ExpireSeconds = createQrCodeResult.expire_seconds;
            qrcode.Url = createQrCodeResult.url;

            string qrcodePreUrl = String.Format(qrcodePreUrlBaseFormat, qrcode.Ticket);
            qrcode.Path = GetQrcodeFolderPathOfUser(userIdentifier.UserId)+"/"+qrcode.SceneId + ".png";
            Image.GetAndSaveImage(qrcodePreUrl,HttpContext.Current.Server.MapPath(qrcode.Path));

            qrcodeRepository.Insert(qrcode);
            return qrcode;
        }

        public async Task<Qrcode> GetQrcodeAsync(UserIdentifier userIdentifier,bool checkCanSpread=true)
        {
            await SpreadManager.CanSpreadAsync(userIdentifier);
            Qrcode qrcode = qrcodeRepository.GetAll().Where(model => model.UserId == userIdentifier.UserId).FirstOrDefault();

            if (qrcode == null)
            {
                qrcode =await CreateQrcodeAsync(userIdentifier);
            }
            return qrcode;
        }
    }
}
