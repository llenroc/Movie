namespace Infrastructure.Net.Mail
{
    /// <summary>
    /// Declares names of the settings defined by <see cref="EmailSettingProvider"/>.
    /// </summary>
    public static class EmailSettingNames
    {
        /// <summary>
        /// Infrastructure.Net.Mail.DefaultFromAddress
        /// </summary>
        public const string DefaultFromAddress = "Infrastructure.Net.Mail.DefaultFromAddress";

        /// <summary>
        /// Infrastructure.Net.Mail.DefaultFromDisplayName
        /// </summary>
        public const string DefaultFromDisplayName = "Infrastructure.Net.Mail.DefaultFromDisplayName";

        /// <summary>
        /// SMTP related email settings.
        /// </summary>
        public static class Smtp
        {
            /// <summary>
            /// Infrastructure.Net.Mail.Smtp.Host
            /// </summary>
            public const string Host = "Infrastructure.Net.Mail.Smtp.Host";

            /// <summary>
            /// Infrastructure.Net.Mail.Smtp.Port
            /// </summary>
            public const string Port = "Infrastructure.Net.Mail.Smtp.Port";

            /// <summary>
            /// Infrastructure.Net.Mail.Smtp.UserName
            /// </summary>
            public const string UserName = "Infrastructure.Net.Mail.Smtp.UserName";

            /// <summary>
            /// Infrastructure.Net.Mail.Smtp.Password
            /// </summary>
            public const string Password = "Infrastructure.Net.Mail.Smtp.Password";

            /// <summary>
            /// Infrastructure.Net.Mail.Smtp.Domain
            /// </summary>
            public const string Domain = "Infrastructure.Net.Mail.Smtp.Domain";

            /// <summary>
            /// Infrastructure.Net.Mail.Smtp.EnableSsl
            /// </summary>
            public const string EnableSsl = "Infrastructure.Net.Mail.Smtp.EnableSsl";

            /// <summary>
            /// Infrastructure.Net.Mail.Smtp.UseDefaultCredentials
            /// </summary>
            public const string UseDefaultCredentials = "Infrastructure.Net.Mail.Smtp.UseDefaultCredentials";
        }
    }
}
