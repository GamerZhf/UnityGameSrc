namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class GameServicesBuilder : BaseReferenceHolder
    {
        private GameServicesBuilder(IntPtr selfPointer) : base(selfPointer)
        {
            InternalHooks.InternalHooks_ConfigureForUnityPlugin(base.SelfPtr());
        }

        internal void AddOauthScope(string scope)
        {
            GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_AddOauthScope(base.SelfPtr(), scope);
        }

        internal GooglePlayGames.Native.PInvoke.GameServices Build(PlatformConfiguration configRef)
        {
            IntPtr selfPointer = GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_Create(base.SelfPtr(), HandleRef.ToIntPtr(configRef.AsHandle()));
            if (selfPointer.Equals(IntPtr.Zero))
            {
                throw new InvalidOperationException("There was an error creating a GameServices object. Check for log errors from GamesNativeSDK");
            }
            return new GooglePlayGames.Native.PInvoke.GameServices(selfPointer);
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_Dispose(selfPointer);
        }

        internal static GameServicesBuilder Create()
        {
            return new GameServicesBuilder(GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_Construct());
        }

        internal void EnableSnapshots()
        {
            GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_EnableSnapshots(base.SelfPtr());
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.Builder.OnAuthActionFinishedCallback))]
        private static void InternalAuthFinishedCallback(Types.AuthOperation op, GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus status, IntPtr data)
        {
            AuthFinishedCallback callback = Callbacks.IntPtrToPermanentCallback<AuthFinishedCallback>(data);
            if (callback != null)
            {
                try
                {
                    callback(op, status);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing InternalAuthFinishedCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.Builder.OnAuthActionStartedCallback))]
        private static void InternalAuthStartedCallback(Types.AuthOperation op, IntPtr data)
        {
            AuthStartedCallback callback = Callbacks.IntPtrToPermanentCallback<AuthStartedCallback>(data);
            try
            {
                if (callback != null)
                {
                    callback(op);
                }
            }
            catch (Exception exception)
            {
                Logger.e("Error encountered executing InternalAuthStartedCallback. Smothering to avoid passing exception into Native: " + exception);
            }
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.Builder.OnMultiplayerInvitationEventCallback))]
        private static void InternalOnMultiplayerInvitationEventCallback(Types.MultiplayerEvent eventType, string matchId, IntPtr match, IntPtr userData)
        {
            Action<Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation> action = Callbacks.IntPtrToPermanentCallback<Action<Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation>>(userData);
            GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation = GooglePlayGames.Native.PInvoke.MultiplayerInvitation.FromPointer(match);
            try
            {
                if (action != null)
                {
                    action(eventType, matchId, invitation);
                }
            }
            catch (Exception exception)
            {
                Logger.e("Error encountered executing InternalOnMultiplayerInvitationEventCallback. Smothering to avoid passing exception into Native: " + exception);
            }
            finally
            {
                if (invitation != null)
                {
                    invitation.Dispose();
                }
            }
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.Builder.OnTurnBasedMatchEventCallback))]
        private static void InternalOnTurnBasedMatchEventCallback(Types.MultiplayerEvent eventType, string matchId, IntPtr match, IntPtr userData)
        {
            Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch> action = Callbacks.IntPtrToPermanentCallback<Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch>>(userData);
            NativeTurnBasedMatch match2 = NativeTurnBasedMatch.FromPointer(match);
            try
            {
                if (action != null)
                {
                    action(eventType, matchId, match2);
                }
            }
            catch (Exception exception)
            {
                Logger.e("Error encountered executing InternalOnTurnBasedMatchEventCallback. Smothering to avoid passing exception into Native: " + exception);
            }
            finally
            {
                if (match2 != null)
                {
                    match2.Dispose();
                }
            }
        }

        internal void RequireGooglePlus()
        {
            GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_RequireGooglePlus(base.SelfPtr());
        }

        internal void SetOnAuthFinishedCallback(AuthFinishedCallback callback)
        {
            GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_SetOnAuthActionFinished(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.Builder.OnAuthActionFinishedCallback(GameServicesBuilder.InternalAuthFinishedCallback), Callbacks.ToIntPtr(callback));
        }

        internal void SetOnAuthStartedCallback(AuthStartedCallback callback)
        {
            GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_SetOnAuthActionStarted(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.Builder.OnAuthActionStartedCallback(GameServicesBuilder.InternalAuthStartedCallback), Callbacks.ToIntPtr(callback));
        }

        internal void SetOnMultiplayerInvitationEventCallback(Action<Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback)
        {
            IntPtr ptr = Callbacks.ToIntPtr(callback);
            GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_SetOnMultiplayerInvitationEvent(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.Builder.OnMultiplayerInvitationEventCallback(GameServicesBuilder.InternalOnMultiplayerInvitationEventCallback), ptr);
        }

        internal void SetOnTurnBasedMatchEventCallback(Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch> callback)
        {
            IntPtr ptr = Callbacks.ToIntPtr(callback);
            GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_SetOnTurnBasedMatchEvent(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.Builder.OnTurnBasedMatchEventCallback(GameServicesBuilder.InternalOnTurnBasedMatchEventCallback), ptr);
        }

        internal delegate void AuthFinishedCallback(Types.AuthOperation operation, GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus status);

        internal delegate void AuthStartedCallback(Types.AuthOperation operation);
    }
}

