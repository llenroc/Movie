using System;
using Infrastructure.Authorization.Users;
using Infrastructure.Extensions;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Application.Channel.ChannelAgencies;

namespace Application.Authorization.Users
{
    public enum PasswordFormat
    {
        /// <summary>
        /// 不加密
        /// </summary>
        Clear = 0,
        /// <summary>
        /// Hashed加密法
        /// </summary>
        Hashed = 1,
        /// <summary>
        /// Encrypted加密法
        /// </summary>
        Encrypted = 2,
    }

    public enum UserSource
    {
        System,
        WebPageRegist,
        WeixinInteraction,
        WeixinExternalLogin,
        OtherExternalLogin
    }

    public class User : CommonFrameUser<User>
    {
        public const string DefaultPassword = "123456";
        public const string DefaultAvatar = "/Content/Images/avatar.png";
        public const int MinPlainPasswordLength = 6;
        public const int MaxNickNameLength = 32;

        public string Avatar { get; set; }

        public string NickName { get; set; }

        public bool IsSpreader { get; set; } = false;

        public long? ParentUserId { get; set; }

        [ForeignKey("ParentUserId")]
        public virtual User ParentUser { get; set; }

        [ForeignKey("ParentUserId")]
        public virtual ICollection<User> Children { get; set; }

        public int? ChannelAgencyId { get; set; }

        public int ChannelAgentDepth { get; set; }

        public bool IsChannelAgency { get; set; }

        public int? UserChannelAgencyId { get; set; }

        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        public int ChildCountOfDepth1{ get;set; }

        public int ChildCountOfDepth2 { get; set; }

        public int ChildCountOfDepth3 { get; set; }

        public int GroupCount { get; set; }

        public decimal Sales { get; set; }

        public bool IsHide { get; set; }

        public UserSource Source { get; set; }

        public User()
        {
            Avatar = DefaultAvatar;
            Source = UserSource.WebPageRegist;
        }

        public int GetChildCount()
        {
            return Children.Count;
        }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static string CreateRandomUserName()
        {
            return Guid.NewGuid().ToString("N").Truncate(MaxUserNameLength);
        }

        public static string PreProcessUserName(string userName)
        {
            userName = userName.Replace("-", "");
            userName = userName.Replace("_", "");
            return userName;
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                NickName = "Administrator",
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };
        }
    }
}