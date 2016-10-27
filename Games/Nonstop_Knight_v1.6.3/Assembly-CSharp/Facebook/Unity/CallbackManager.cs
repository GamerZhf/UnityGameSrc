namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;

    internal class CallbackManager
    {
        private IDictionary<string, object> facebookDelegates = new Dictionary<string, object>();
        private int nextAsyncId;

        public string AddFacebookDelegate<T>(FacebookDelegate<T> callback) where T: IResult
        {
            if (callback == null)
            {
                return null;
            }
            this.nextAsyncId++;
            this.facebookDelegates.Add(this.nextAsyncId.ToString(), callback);
            return this.nextAsyncId.ToString();
        }

        private static void CallCallback(object callback, IResult result)
        {
            if (((callback != null) && (result != null)) && (((!TryCallCallback<IAppRequestResult>(callback, result) && !TryCallCallback<IShareResult>(callback, result)) && (!TryCallCallback<IGroupCreateResult>(callback, result) && !TryCallCallback<IGroupJoinResult>(callback, result))) && ((!TryCallCallback<IPayResult>(callback, result) && !TryCallCallback<IAppInviteResult>(callback, result)) && ((!TryCallCallback<IAppLinkResult>(callback, result) && !TryCallCallback<ILoginResult>(callback, result)) && !TryCallCallback<IAccessTokenRefreshResult>(callback, result)))))
            {
                throw new NotSupportedException("Unexpected result type: " + callback.GetType().FullName);
            }
        }

        public void OnFacebookResponse(IInternalResult result)
        {
            object obj2;
            if (((result != null) && (result.CallbackId != null)) && this.facebookDelegates.TryGetValue(result.CallbackId, out obj2))
            {
                CallCallback(obj2, result);
                this.facebookDelegates.Remove(result.CallbackId);
            }
        }

        private static bool TryCallCallback<T>(object callback, IResult result) where T: IResult
        {
            FacebookDelegate<T> delegate2 = callback as FacebookDelegate<T>;
            if (delegate2 != null)
            {
                delegate2((T) result);
                return true;
            }
            return false;
        }
    }
}

