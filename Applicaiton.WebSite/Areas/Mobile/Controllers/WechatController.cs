using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP;
using System.Threading.Tasks;
using Application.WebSite.Wechat.MessageHandles;
using Senparc.Weixin.MP.MvcExtension;
using Infrastructure.Configuration;
using Application.Configuration;

namespace Application.WebSite.Areas.Mobile.Controllers
{
    public class WechatController : AnonymousMobileControllerBase
    {
        [HttpGet]
        [ActionName("Index")]
        public Task<ActionResult> Get(string signature, string timestamp, string nonce, string echostr)
        {
            string Token = SettingManager.GetSettingValue(WechatSettings.General.Token);

            return Task.Factory.StartNew(() =>
            {
                if (CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    return echostr; //返回随机字符串则表示验证通过
                }
                else
                {
                    return "failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。" +
                        "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。";
                }
            }).ContinueWith<ActionResult>(task => Content(task.Result));
        }

        /// <summary>
        /// 最简化的处理流程
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            string Token= SettingManager.GetSettingValue(WechatSettings.General.Token);
            string AppId = SettingManager.GetSettingValue(WechatSettings.General.AppId);
            string EncodingAESKey = SettingManager.GetSettingValue(WechatSettings.General.EncodingAESKey);

            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                //return new WeixinResult("参数错误！");
            }
            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAESKey;
            postModel.AppId = AppId;

            //执行微信处理过程
            var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);
            messageHandler.OmitRepeatedMessage = true;//启用消息去重功能
            messageHandler.Execute();
            return new FixWeixinBugWeixinResult(messageHandler);
        }
    }
}