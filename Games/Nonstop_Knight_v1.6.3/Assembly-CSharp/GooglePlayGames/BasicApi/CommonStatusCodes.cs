namespace GooglePlayGames.BasicApi
{
    using System;

    public enum CommonStatusCodes
    {
        ApiNotConnected = 0x11,
        AuthApiAccessForbidden = 0xbb9,
        AuthApiClientError = 0xbba,
        AuthApiInvalidCredentials = 0xbb8,
        AuthApiServerError = 0xbbb,
        AuthTokenError = 0xbbc,
        AuthUrlResolution = 0xbbd,
        Canceled = 0x10,
        DeveloperError = 10,
        Error = 13,
        InternalError = 8,
        Interrupted = 14,
        InvalidAccount = 5,
        LicenseCheckFailed = 11,
        NetworkError = 7,
        ResolutionRequired = 6,
        ServiceDisabled = 3,
        ServiceInvalid = 9,
        ServiceMissing = 1,
        ServiceVersionUpdateRequired = 2,
        SignInRequired = 4,
        Success = 0,
        SuccessCached = -1,
        Timeout = 15
    }
}

