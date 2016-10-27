namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using UnityEngine;

    internal static class ConversionUtils
    {
        internal static GooglePlayGames.Native.Cwrapper.Types.DataSource AsDataSource(GooglePlayGames.BasicApi.DataSource source)
        {
            GooglePlayGames.BasicApi.DataSource source2 = source;
            if (source2 != GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork)
            {
                if (source2 != GooglePlayGames.BasicApi.DataSource.ReadNetworkOnly)
                {
                    throw new InvalidOperationException("Found unhandled DataSource: " + source);
                }
                return GooglePlayGames.Native.Cwrapper.Types.DataSource.NETWORK_ONLY;
            }
            return GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK;
        }

        internal static GooglePlayGames.BasicApi.ResponseStatus ConvertResponseStatus(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status)
        {
            GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status2 = status;
            switch ((status2 + 5))
            {
                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
                    return GooglePlayGames.BasicApi.ResponseStatus.Timeout;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID:
                    return GooglePlayGames.BasicApi.ResponseStatus.VersionUpdateRequired;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
                    return GooglePlayGames.BasicApi.ResponseStatus.NotAuthorized;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return GooglePlayGames.BasicApi.ResponseStatus.InternalError;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
                    return GooglePlayGames.BasicApi.ResponseStatus.LicenseCheckFailed;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 6):
                    return GooglePlayGames.BasicApi.ResponseStatus.Success;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 7):
                    return GooglePlayGames.BasicApi.ResponseStatus.SuccessWithStale;
            }
            throw new InvalidOperationException("Unknown status: " + status);
        }

        internal static CommonStatusCodes ConvertResponseStatusToCommonStatus(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status)
        {
            GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status2 = status;
            switch ((status2 + 5))
            {
                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
                    return CommonStatusCodes.Timeout;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID:
                    return CommonStatusCodes.ServiceVersionUpdateRequired;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
                    return CommonStatusCodes.AuthApiAccessForbidden;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return CommonStatusCodes.InternalError;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
                    return CommonStatusCodes.LicenseCheckFailed;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 6):
                    return CommonStatusCodes.Success;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 7):
                    return CommonStatusCodes.SuccessCached;
            }
            Debug.LogWarning("Unknown ResponseStatus: " + status + ", defaulting to CommonStatusCodes.Error");
            return CommonStatusCodes.Error;
        }

        internal static GooglePlayGames.BasicApi.UIStatus ConvertUIStatus(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus status)
        {
            GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus status2 = status;
            switch ((status2 + 12))
            {
                case ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_INTERNAL):
                    return GooglePlayGames.BasicApi.UIStatus.UiBusy;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus) 6):
                    return GooglePlayGames.BasicApi.UIStatus.UserClosedUI;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus) 7):
                    return GooglePlayGames.BasicApi.UIStatus.Timeout;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus) 8):
                    return GooglePlayGames.BasicApi.UIStatus.VersionUpdateRequired;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus) 9):
                    return GooglePlayGames.BasicApi.UIStatus.NotAuthorized;

                case ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_UI_BUSY):
                    return GooglePlayGames.BasicApi.UIStatus.InternalError;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus) 13):
                    return GooglePlayGames.BasicApi.UIStatus.Valid;
            }
            throw new InvalidOperationException("Unknown status: " + status);
        }
    }
}

