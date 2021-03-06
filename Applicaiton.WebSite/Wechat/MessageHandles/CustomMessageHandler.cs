﻿using Application.Authorization.Users;
using Application.MultiTenancy;
using Application.Spread.SpreadPosters;
using Application.WebSite.MultiTenancy;
using Application.Wechats;
using Application.Wechats.AutoReplys;
using Infrastructure;
using Infrastructure.Dependency;
using Infrastructure.Localization;
using Infrastructure.Localization.Sources;
using Infrastructure.Threading;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Application.WebSite.Wechat.MessageHandles
{
    public class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        protected User _user;
        protected Tenant _tenant;
        protected HttpContext _context;
        protected WechatUserManager _wechatUserManager;
        protected AutoReplyManager _autoReplyManager;
        protected WechatCommonManager _wechatCommonManager;
        protected CustomerServiceMessageHelper _customerServiceMessageHelper;
        protected string _accessToken;
        protected ILocalizationManager LocalizationManager { get; set; }
        protected string LocalizationSourceName { get; set; }

        protected ILocalizationSource LocalizationSource
        {
            get
            {
                if (LocalizationSourceName == null)
                {
                    throw new InfrastructureException("Must set LocalizationSourceName before, in order to get LocalizationSource");
                }

                if (_localizationSource == null || _localizationSource.Name != LocalizationSourceName)
                {
                    _localizationSource = LocalizationManager.GetSource(LocalizationSourceName);
                }

                return _localizationSource;
            }
        }
        private ILocalizationSource _localizationSource;

        protected virtual string L(string name)
        {
            return LocalizationSource.GetString(name);
        }

        /// <summary>  
        /// 模板消息集合（Key：checkCode，Value：OpenId）  
        /// </summary>  
        public static Dictionary<string, string> TemplateMessageCollection = new Dictionary<string, string>();

        public CustomMessageHandler(Stream inputStream, PostModel postModel)
            : base(inputStream, postModel)
        {
            LocalizationManager = IocManager.Instance.Resolve<ILocalizationManager>();
            LocalizationSourceName = ApplicationConsts.LocalizationSourceName;
            _wechatUserManager = IocManager.Instance.Resolve<WechatUserManager>();
            _autoReplyManager = IocManager.Instance.Resolve<AutoReplyManager>();
            _wechatCommonManager = IocManager.Instance.Resolve<WechatCommonManager>();
            _customerServiceMessageHelper = IocManager.Instance.Resolve<CustomerServiceMessageHelper>();
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "DefaultMessage";
            return responseMessage;
        }

        public override void OnExecuting()
        {
            _tenant = TenantHelper.GetTenant();
            _user = _wechatUserManager.GetUserFromOpenId(_tenant.Id, RequestMessage.FromUserName);
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
        }

        private void SendSpreadPoster(string openid)
        {
            if (_user == null)
            {
                return;
            }
            AsyncHelper.RunSync(async() =>
            {
                _accessToken = await _wechatCommonManager.GetAccessTokenAsync(_tenant.Id);

                try
                {
                    SpreadPosterManager spreadPosterManager = IocManager.Instance.Resolve<SpreadPosterManager>();
                    string path = await spreadPosterManager.GetDefaultSpreadPosterAsync(_user.ToUserIdentifier());
                    string serverPath = HttpContext.Current.Server.MapPath(path);
                    UploadTemporaryMediaResult uploadTemporaryMediaResult = await MediaApi.UploadTemporaryMediaAsync(
                        _accessToken,
                        UploadMediaFileType.image,
                        serverPath);
                    _customerServiceMessageHelper.SendImage(_accessToken, openid, uploadTemporaryMediaResult.media_id);
                }
                catch (Exception exception)
                {
                    _customerServiceMessageHelper.SendText(_accessToken, openid, exception.Message);
                }
            });
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            if (requestMessage.EventKey == "spreadPoster")
            {
                SendSpreadPoster(requestMessage.FromUserName);
            }
            SendAutoReplyMessages(RequestType.Event_ClickRequest,requestMessage.EventKey);
            return ResponseMessage;
        }

        public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            SendAutoReplyMessages(RequestType.Event_ViewRequest,requestMessage.EventKey);
            return ResponseMessage;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            SendAutoReplyMessages(RequestType.TextRequest, requestMessage.Content);
            return ResponseMessage;
        }

        private void SendAutoReplyMessages(RequestType requestType,string key=null)
        {
            List<AutoReply> autoReplys = _autoReplyManager.GetAutoReplysOfRequestMsgType(_tenant.Id,requestType, key);

            if (autoReplys != null)
            {
                Task.Run(async () =>
                {
                    _accessToken = await _wechatCommonManager.GetAccessTokenAsync(_tenant.Id);

                    foreach (AutoReply autoReply in autoReplys)
                    {
                        switch (autoReply.MsgType)
                        {
                            case ResponseMsgType.Text:

                                if (_user != null)
                                {
                                    autoReply.Content = autoReply.Content.Replace("@nickName", _user.NickName);
                                }
                                _customerServiceMessageHelper.SendText(_accessToken, RequestMessage.FromUserName, autoReply.Content);
                                break;
                            case ResponseMsgType.Image:
                                _customerServiceMessageHelper.SendImage(_accessToken, RequestMessage.FromUserName, autoReply.MediaId);
                                break;
                            case ResponseMsgType.MultipleNews:
                                _customerServiceMessageHelper.SendNews(_accessToken, RequestMessage.FromUserName, autoReply.Articles.ToList());
                                break;
                            case ResponseMsgType.News:
                                _customerServiceMessageHelper.SendMpNews(_accessToken, RequestMessage.FromUserName, autoReply.MediaId);
                                break;
                        }
                    }
                });
            }  
        }

        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            AsyncHelper.RunSync(async () =>
            {
                _accessToken = await _wechatCommonManager.GetAccessTokenAsync(_tenant.Id);

                if (_user == null)
                {
                    UserInfoJson userInfo = UserApi.Info(_accessToken, RequestMessage.FromUserName);
                    _user = await _wechatUserManager.CreateUserWhenSubscribeAsync(_tenant.Id, userInfo);
                }

                if (!String.IsNullOrEmpty(requestMessage.EventKey))
                {
                    int sceneId = int.Parse(requestMessage.EventKey);
                    await _wechatUserManager.ProcessSceneId(sceneId, _user.Id, _tenant.Id);
                }
            });
            SendAutoReplyMessages(RequestType.Event_ScanRequest);
            return ResponseMessage;
        }

        /// <summary>  
        /// 订阅（关注）事件  
        /// </summary>  
        /// <returns></returns>  
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            AsyncHelper.RunSync(async () =>
            {
                _accessToken = await _wechatCommonManager.GetAccessTokenAsync(_tenant.Id);

                if (_user == null)
                {
                    UserInfoJson userInfo = UserApi.Info(_accessToken, RequestMessage.FromUserName);
                    _user = await _wechatUserManager.CreateUserWhenSubscribeAsync(_tenant.Id, userInfo);
                }

                if (requestMessage.EventKey.Contains("qrscene_"))
                {
                    int sceneId = int.Parse(requestMessage.EventKey.Substring(8));
                    await _wechatUserManager.ProcessSceneId(sceneId, _user.Id, _tenant.Id);
                }
            });
            SendAutoReplyMessages(RequestType.Event_SubscribeRequest);
            return ResponseMessage;
        }
    }
}