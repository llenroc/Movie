namespace Infrastructure.Authorization.Users
{
    public enum LoginResultType : byte
    {
        Success = 1,

        InvalidUserNameOrEmailAddress,

        InvalidPassword,

        UserIsNotActive,

        InvalidTenancyName,

        TenantIsNotActive,

        UserEmailIsNotConfirmed,

        UnknownExternalLogin,

        LockedOut
    }
}
