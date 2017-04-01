namespace Infrastructure.CommonFrame.Configuration
{
    public static class CommonFrameSettingNames
    {
        public static class UserManagement
        {
            /// <summary>
            /// "Infrastructure.CommonFrame.UserManagement.IsEmailConfirmationRequiredForLogin".
            /// </summary>
            public const string IsEmailConfirmationRequiredForLogin = "Infrastructure.CommonFrame.UserManagement.IsEmailConfirmationRequiredForLogin";

            public static class UserLockOut
            {
                /// <summary>
                /// "Infrastructure.CommonFrame.UserManagement.UserLockOut.IsEnabled".
                /// </summary>
                public const string IsEnabled = "Infrastructure.CommonFrame.UserManagement.UserLockOut.IsEnabled";

                /// <summary>
                /// "Infrastructure.CommonFrame.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout".
                /// </summary>
                public const string MaxFailedAccessAttemptsBeforeLockout = "Infrastructure.CommonFrame.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout";

                /// <summary>
                /// "Infrastructure.CommonFrame.UserManagement.UserLockOut.DefaultAccountLockoutSeconds".
                /// </summary>
                public const string DefaultAccountLockoutSeconds = "Infrastructure.CommonFrame.UserManagement.UserLockOut.DefaultAccountLockoutSeconds";
            }

            public static class TwoFactorLogin
            {
                /// <summary>
                /// "Infrastructure.CommonFrame.UserManagement.TwoFactorLogin.IsEnabled".
                /// </summary>
                public const string IsEnabled = "Infrastructure.CommonFrame.UserManagement.TwoFactorLogin.IsEnabled";

                /// <summary>
                /// "Infrastructure.CommonFrame.UserManagement.TwoFactorLogin.IsEmailProviderEnabled".
                /// </summary>
                public const string IsEmailProviderEnabled = "Infrastructure.CommonFrame.UserManagement.TwoFactorLogin.IsEmailProviderEnabled";

                /// <summary>
                /// "Infrastructure.CommonFrame.UserManagement.TwoFactorLogin.IsSmsProviderEnabled".
                /// </summary>
                public const string IsSmsProviderEnabled = "Infrastructure.CommonFrame.UserManagement.TwoFactorLogin.IsSmsProviderEnabled";

                /// <summary>
                /// "Infrastructure.CommonFrame.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled".
                /// </summary>
                public const string IsRememberBrowserEnabled = "Infrastructure.CommonFrame.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled";
            }
        }

        public static class OrganizationUnits
        {
            /// <summary>
            /// "Infrastructure.CommonFrame.OrganizationUnits.MaxUserMembershipCount".
            /// </summary>
            public const string MaxUserMembershipCount = "Infrastructure.CommonFrame.OrganizationUnits.MaxUserMembershipCount";
        }
    }
}