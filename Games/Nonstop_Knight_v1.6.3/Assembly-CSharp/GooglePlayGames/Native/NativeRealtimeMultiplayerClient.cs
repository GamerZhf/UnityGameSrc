namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class NativeRealtimeMultiplayerClient : IRealTimeMultiplayerClient
    {
        private volatile RoomSession mCurrentSession;
        private readonly NativeClient mNativeClient;
        private readonly RealtimeManager mRealtimeManager;
        private readonly object mSessionLock = new object();

        internal NativeRealtimeMultiplayerClient(NativeClient nativeClient, RealtimeManager manager)
        {
            this.mNativeClient = Misc.CheckNotNull<NativeClient>(nativeClient);
            this.mRealtimeManager = Misc.CheckNotNull<RealtimeManager>(manager);
            this.mCurrentSession = this.GetTerminatedSession();
            PlayGamesHelperObject.AddPauseCallback(new Action<bool>(this.HandleAppPausing));
        }

        public void AcceptFromInbox(RealTimeMultiplayerListener listener)
        {
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                <AcceptFromInbox>c__AnonStorey2A9 storeya = new <AcceptFromInbox>c__AnonStorey2A9();
                storeya.<>f__this = this;
                storeya.newRoom = new RoomSession(this.mRealtimeManager, listener);
                if (this.mCurrentSession.IsActive())
                {
                    Logger.e("Received attempt to accept invitation without cleaning up active session.");
                    storeya.newRoom.LeaveRoom();
                }
                else
                {
                    this.mCurrentSession = storeya.newRoom;
                    this.mCurrentSession.ShowingUI = true;
                    this.mRealtimeManager.ShowRoomInboxUI(new Action<RealtimeManager.RoomInboxUIResponse>(storeya.<>m__DC));
                }
            }
        }

        public void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener)
        {
            <AcceptInvitation>c__AnonStorey2AD storeyad = new <AcceptInvitation>c__AnonStorey2AD();
            storeyad.invitationId = invitationId;
            storeyad.<>f__this = this;
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                <AcceptInvitation>c__AnonStorey2AC storeyac = new <AcceptInvitation>c__AnonStorey2AC();
                storeyac.<>f__ref$685 = storeyad;
                storeyac.<>f__this = this;
                storeyac.newRoom = new RoomSession(this.mRealtimeManager, listener);
                if (this.mCurrentSession.IsActive())
                {
                    Logger.e("Received attempt to accept invitation without cleaning up active session.");
                    storeyac.newRoom.LeaveRoom();
                }
                else
                {
                    this.mCurrentSession = storeyac.newRoom;
                    this.mRealtimeManager.FetchInvitations(new Action<RealtimeManager.FetchInvitationsResponse>(storeyac.<>m__DD));
                }
            }
        }

        public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, RealTimeMultiplayerListener listener)
        {
            this.CreateQuickGame(minOpponents, maxOpponents, variant, 0L, listener);
        }

        public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, RealTimeMultiplayerListener listener)
        {
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                <CreateQuickGame>c__AnonStorey2A2 storeya = new <CreateQuickGame>c__AnonStorey2A2();
                storeya.<>f__this = this;
                storeya.newSession = new RoomSession(this.mRealtimeManager, listener);
                if (this.mCurrentSession.IsActive())
                {
                    Logger.e("Received attempt to create a new room without cleaning up the old one.");
                    storeya.newSession.LeaveRoom();
                }
                else
                {
                    this.mCurrentSession = storeya.newSession;
                    Logger.d("QuickGame: Setting MinPlayersToStart = " + minOpponents);
                    this.mCurrentSession.MinPlayersToStart = minOpponents;
                    using (RealtimeRoomConfigBuilder builder = RealtimeRoomConfigBuilder.Create())
                    {
                        <CreateQuickGame>c__AnonStorey2A0 storeya2 = new <CreateQuickGame>c__AnonStorey2A0();
                        storeya2.<>f__this = this;
                        storeya2.config = builder.SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetVariant(variant).SetExclusiveBitMask(exclusiveBitMask).Build();
                        using (storeya2.config)
                        {
                            <CreateQuickGame>c__AnonStorey2A1 storeya3 = new <CreateQuickGame>c__AnonStorey2A1();
                            storeya3.<>f__ref$674 = storeya;
                            storeya3.<>f__ref$672 = storeya2;
                            storeya3.<>f__this = this;
                            storeya3.helper = HelperForSession(storeya.newSession);
                            try
                            {
                                storeya.newSession.StartRoomCreation(this.mNativeClient.GetUserId(), new Action(storeya3.<>m__D5));
                            }
                            finally
                            {
                                if (storeya3.helper != null)
                                {
                                    storeya3.helper.Dispose();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CreateWithInvitationScreen(uint minOpponents, uint maxOppponents, uint variant, RealTimeMultiplayerListener listener)
        {
            <CreateWithInvitationScreen>c__AnonStorey2A5 storeya = new <CreateWithInvitationScreen>c__AnonStorey2A5();
            storeya.variant = variant;
            storeya.<>f__this = this;
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                <CreateWithInvitationScreen>c__AnonStorey2A4 storeya2 = new <CreateWithInvitationScreen>c__AnonStorey2A4();
                storeya2.<>f__ref$677 = storeya;
                storeya2.<>f__this = this;
                storeya2.newRoom = new RoomSession(this.mRealtimeManager, listener);
                if (this.mCurrentSession.IsActive())
                {
                    Logger.e("Received attempt to create a new room without cleaning up the old one.");
                    storeya2.newRoom.LeaveRoom();
                }
                else
                {
                    this.mCurrentSession = storeya2.newRoom;
                    this.mCurrentSession.ShowingUI = true;
                    this.mRealtimeManager.ShowPlayerSelectUI(minOpponents, maxOppponents, true, new Action<PlayerSelectUIResponse>(storeya2.<>m__DA));
                }
            }
        }

        public void DeclineInvitation(string invitationId)
        {
            <DeclineInvitation>c__AnonStorey2B0 storeyb = new <DeclineInvitation>c__AnonStorey2B0();
            storeyb.invitationId = invitationId;
            storeyb.<>f__this = this;
            this.mRealtimeManager.FetchInvitations(new Action<RealtimeManager.FetchInvitationsResponse>(storeyb.<>m__DE));
        }

        public void GetAllInvitations(Action<Invitation[]> callback)
        {
            <GetAllInvitations>c__AnonStorey2A8 storeya = new <GetAllInvitations>c__AnonStorey2A8();
            storeya.callback = callback;
            this.mRealtimeManager.FetchInvitations(new Action<RealtimeManager.FetchInvitationsResponse>(storeya.<>m__DB));
        }

        public List<Participant> GetConnectedParticipants()
        {
            return this.mCurrentSession.GetConnectedParticipants();
        }

        public Invitation GetInvitation()
        {
            return this.mCurrentSession.GetInvitation();
        }

        public Participant GetParticipant(string participantId)
        {
            return this.mCurrentSession.GetParticipant(participantId);
        }

        public Participant GetSelf()
        {
            return this.mCurrentSession.GetSelf();
        }

        private RoomSession GetTerminatedSession()
        {
            RoomSession session = new RoomSession(this.mRealtimeManager, new NoopListener());
            session.EnterState(new ShutdownState(session), false);
            return session;
        }

        private void HandleAppPausing(bool paused)
        {
            if (paused)
            {
                Logger.d("Application is pausing, which disconnects the RTMP  client.  Leaving room.");
                this.LeaveRoom();
            }
        }

        private static GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper HelperForSession(RoomSession session)
        {
            <HelperForSession>c__AnonStorey2A3 storeya = new <HelperForSession>c__AnonStorey2A3();
            storeya.session = session;
            return GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.Create().SetOnDataReceivedCallback(new Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant, byte[], bool>(storeya.<>m__D6)).SetOnParticipantStatusChangedCallback(new Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant>(storeya.<>m__D7)).SetOnRoomConnectedSetChangedCallback(new Action<NativeRealTimeRoom>(storeya.<>m__D8)).SetOnRoomStatusChangedCallback(new Action<NativeRealTimeRoom>(storeya.<>m__D9));
        }

        public bool IsRoomConnected()
        {
            return this.mCurrentSession.IsRoomConnected();
        }

        public void LeaveRoom()
        {
            this.mCurrentSession.LeaveRoom();
        }

        public void SendMessage(bool reliable, string participantId, byte[] data)
        {
            this.mCurrentSession.SendMessage(reliable, participantId, data);
        }

        public void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
        {
            this.mCurrentSession.SendMessage(reliable, participantId, data, offset, length);
        }

        public void SendMessageToAll(bool reliable, byte[] data)
        {
            this.mCurrentSession.SendMessageToAll(reliable, data);
        }

        public void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
        {
            this.mCurrentSession.SendMessageToAll(reliable, data, offset, length);
        }

        public void ShowWaitingRoomUI()
        {
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                this.mCurrentSession.ShowWaitingRoomUI();
            }
        }

        private static T WithDefault<T>(T presented, T defaultValue) where T: class
        {
            return ((presented == null) ? defaultValue : presented);
        }

        [CompilerGenerated]
        private sealed class <AcceptFromInbox>c__AnonStorey2A9
        {
            internal NativeRealtimeMultiplayerClient <>f__this;
            internal NativeRealtimeMultiplayerClient.RoomSession newRoom;

            internal void <>m__DC(RealtimeManager.RoomInboxUIResponse response)
            {
                <AcceptFromInbox>c__AnonStorey2AA storeyaa = new <AcceptFromInbox>c__AnonStorey2AA();
                storeyaa.<>f__ref$681 = this;
                this.<>f__this.mCurrentSession.ShowingUI = false;
                if (response.ResponseStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
                {
                    Logger.d("User did not complete invitation screen.");
                    this.newRoom.LeaveRoom();
                }
                else
                {
                    storeyaa.invitation = response.Invitation();
                    <AcceptFromInbox>c__AnonStorey2AB storeyab = new <AcceptFromInbox>c__AnonStorey2AB();
                    storeyab.<>f__ref$681 = this;
                    storeyab.<>f__ref$682 = storeyaa;
                    storeyab.helper = NativeRealtimeMultiplayerClient.HelperForSession(this.newRoom);
                    try
                    {
                        Logger.d("About to accept invitation " + storeyaa.invitation.Id());
                        this.newRoom.StartRoomCreation(this.<>f__this.mNativeClient.GetUserId(), new Action(storeyab.<>m__E0));
                    }
                    finally
                    {
                        if (storeyab.helper != null)
                        {
                            storeyab.helper.Dispose();
                        }
                    }
                }
            }

            private sealed class <AcceptFromInbox>c__AnonStorey2AA
            {
                internal NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey2A9 <>f__ref$681;
                internal GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation;
            }

            private sealed class <AcceptFromInbox>c__AnonStorey2AB
            {
                internal NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey2A9 <>f__ref$681;
                internal NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey2A9.<AcceptFromInbox>c__AnonStorey2AA <>f__ref$682;
                internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper;

                internal void <>m__E0()
                {
                    this.<>f__ref$681.<>f__this.mRealtimeManager.AcceptInvitation(this.<>f__ref$682.invitation, this.helper, delegate (RealtimeManager.RealTimeRoomResponse acceptResponse) {
                        using (this.<>f__ref$682.invitation)
                        {
                            this.<>f__ref$681.newRoom.HandleRoomResponse(acceptResponse);
                            this.<>f__ref$681.newRoom.SetInvitation(this.<>f__ref$682.invitation.AsInvitation());
                        }
                    });
                }

                internal void <>m__E1(RealtimeManager.RealTimeRoomResponse acceptResponse)
                {
                    using (this.<>f__ref$682.invitation)
                    {
                        this.<>f__ref$681.newRoom.HandleRoomResponse(acceptResponse);
                        this.<>f__ref$681.newRoom.SetInvitation(this.<>f__ref$682.invitation.AsInvitation());
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <AcceptInvitation>c__AnonStorey2AC
        {
            internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStorey2AD <>f__ref$685;
            internal NativeRealtimeMultiplayerClient <>f__this;
            internal NativeRealtimeMultiplayerClient.RoomSession newRoom;

            internal void <>m__DD(RealtimeManager.FetchInvitationsResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    Logger.e("Couldn't load invitations.");
                    this.newRoom.LeaveRoom();
                }
                else
                {
                    <AcceptInvitation>c__AnonStorey2AE storeyae = new <AcceptInvitation>c__AnonStorey2AE();
                    storeyae.<>f__ref$685 = this.<>f__ref$685;
                    storeyae.<>f__ref$684 = this;
                    IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = response.Invitations().GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            storeyae.invitation = enumerator.Current;
                            using (storeyae.invitation)
                            {
                                if (storeyae.invitation.Id().Equals(this.<>f__ref$685.invitationId))
                                {
                                    this.<>f__this.mCurrentSession.MinPlayersToStart = storeyae.invitation.AutomatchingSlots() + storeyae.invitation.ParticipantCount();
                                    Logger.d("Setting MinPlayersToStart with invitation to : " + this.<>f__this.mCurrentSession.MinPlayersToStart);
                                    <AcceptInvitation>c__AnonStorey2AF storeyaf = new <AcceptInvitation>c__AnonStorey2AF();
                                    storeyaf.<>f__ref$684 = this;
                                    storeyaf.<>f__ref$686 = storeyae;
                                    storeyaf.helper = NativeRealtimeMultiplayerClient.HelperForSession(this.newRoom);
                                    try
                                    {
                                        this.newRoom.StartRoomCreation(this.<>f__this.mNativeClient.GetUserId(), new Action(storeyaf.<>m__E2));
                                        return;
                                    }
                                    finally
                                    {
                                        if (storeyaf.helper != null)
                                        {
                                            storeyaf.helper.Dispose();
                                        }
                                    }
                                }
                                continue;
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator == null)
                        {
                        }
                        enumerator.Dispose();
                    }
                    Logger.e("Room creation failed since we could not find invitation with ID " + this.<>f__ref$685.invitationId);
                    this.newRoom.LeaveRoom();
                }
            }

            private sealed class <AcceptInvitation>c__AnonStorey2AE
            {
                internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStorey2AC <>f__ref$684;
                internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStorey2AD <>f__ref$685;
                internal GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation;
            }

            private sealed class <AcceptInvitation>c__AnonStorey2AF
            {
                internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStorey2AC <>f__ref$684;
                internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStorey2AC.<AcceptInvitation>c__AnonStorey2AE <>f__ref$686;
                internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper;

                internal void <>m__E2()
                {
                    this.<>f__ref$684.<>f__this.mRealtimeManager.AcceptInvitation(this.<>f__ref$686.invitation, this.helper, new Action<RealtimeManager.RealTimeRoomResponse>(this.<>f__ref$684.newRoom.HandleRoomResponse));
                }
            }
        }

        [CompilerGenerated]
        private sealed class <AcceptInvitation>c__AnonStorey2AD
        {
            internal NativeRealtimeMultiplayerClient <>f__this;
            internal string invitationId;
        }

        [CompilerGenerated]
        private sealed class <CreateQuickGame>c__AnonStorey2A0
        {
            internal NativeRealtimeMultiplayerClient <>f__this;
            internal RealtimeRoomConfig config;
        }

        [CompilerGenerated]
        private sealed class <CreateQuickGame>c__AnonStorey2A1
        {
            internal NativeRealtimeMultiplayerClient.<CreateQuickGame>c__AnonStorey2A0 <>f__ref$672;
            internal NativeRealtimeMultiplayerClient.<CreateQuickGame>c__AnonStorey2A2 <>f__ref$674;
            internal NativeRealtimeMultiplayerClient <>f__this;
            internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper;

            internal void <>m__D5()
            {
                this.<>f__this.mRealtimeManager.CreateRoom(this.<>f__ref$672.config, this.helper, new Action<RealtimeManager.RealTimeRoomResponse>(this.<>f__ref$674.newSession.HandleRoomResponse));
            }
        }

        [CompilerGenerated]
        private sealed class <CreateQuickGame>c__AnonStorey2A2
        {
            internal NativeRealtimeMultiplayerClient <>f__this;
            internal NativeRealtimeMultiplayerClient.RoomSession newSession;
        }

        [CompilerGenerated]
        private sealed class <CreateWithInvitationScreen>c__AnonStorey2A4
        {
            internal NativeRealtimeMultiplayerClient.<CreateWithInvitationScreen>c__AnonStorey2A5 <>f__ref$677;
            internal NativeRealtimeMultiplayerClient <>f__this;
            internal NativeRealtimeMultiplayerClient.RoomSession newRoom;

            internal void <>m__DA(PlayerSelectUIResponse response)
            {
                this.<>f__this.mCurrentSession.ShowingUI = false;
                if (response.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
                {
                    Logger.d("User did not complete invitation screen.");
                    this.newRoom.LeaveRoom();
                }
                else
                {
                    this.<>f__this.mCurrentSession.MinPlayersToStart = (uint) ((response.MinimumAutomatchingPlayers() + Enumerable.Count<string>(response)) + 1);
                    using (RealtimeRoomConfigBuilder builder = RealtimeRoomConfigBuilder.Create())
                    {
                        builder.SetVariant(this.<>f__ref$677.variant);
                        builder.PopulateFromUIResponse(response);
                        <CreateWithInvitationScreen>c__AnonStorey2A6 storeya = new <CreateWithInvitationScreen>c__AnonStorey2A6();
                        storeya.<>f__ref$676 = this;
                        storeya.config = builder.Build();
                        try
                        {
                            <CreateWithInvitationScreen>c__AnonStorey2A7 storeya2 = new <CreateWithInvitationScreen>c__AnonStorey2A7();
                            storeya2.<>f__ref$676 = this;
                            storeya2.<>f__ref$678 = storeya;
                            storeya2.helper = NativeRealtimeMultiplayerClient.HelperForSession(this.newRoom);
                            try
                            {
                                this.newRoom.StartRoomCreation(this.<>f__this.mNativeClient.GetUserId(), new Action(storeya2.<>m__DF));
                            }
                            finally
                            {
                                if (storeya2.helper != null)
                                {
                                    storeya2.helper.Dispose();
                                }
                            }
                        }
                        finally
                        {
                            if (storeya.config != null)
                            {
                                storeya.config.Dispose();
                            }
                        }
                    }
                }
            }

            private sealed class <CreateWithInvitationScreen>c__AnonStorey2A6
            {
                internal NativeRealtimeMultiplayerClient.<CreateWithInvitationScreen>c__AnonStorey2A4 <>f__ref$676;
                internal RealtimeRoomConfig config;
            }

            private sealed class <CreateWithInvitationScreen>c__AnonStorey2A7
            {
                internal NativeRealtimeMultiplayerClient.<CreateWithInvitationScreen>c__AnonStorey2A4 <>f__ref$676;
                internal NativeRealtimeMultiplayerClient.<CreateWithInvitationScreen>c__AnonStorey2A4.<CreateWithInvitationScreen>c__AnonStorey2A6 <>f__ref$678;
                internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper;

                internal void <>m__DF()
                {
                    this.<>f__ref$676.<>f__this.mRealtimeManager.CreateRoom(this.<>f__ref$678.config, this.helper, new Action<RealtimeManager.RealTimeRoomResponse>(this.<>f__ref$676.newRoom.HandleRoomResponse));
                }
            }
        }

        [CompilerGenerated]
        private sealed class <CreateWithInvitationScreen>c__AnonStorey2A5
        {
            internal NativeRealtimeMultiplayerClient <>f__this;
            internal uint variant;
        }

        [CompilerGenerated]
        private sealed class <DeclineInvitation>c__AnonStorey2B0
        {
            internal NativeRealtimeMultiplayerClient <>f__this;
            internal string invitationId;

            internal void <>m__DE(RealtimeManager.FetchInvitationsResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    Logger.e("Couldn't load invitations.");
                }
                else
                {
                    IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = response.Invitations().GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            GooglePlayGames.Native.PInvoke.MultiplayerInvitation current = enumerator.Current;
                            using (current)
                            {
                                if (current.Id().Equals(this.invitationId))
                                {
                                    this.<>f__this.mRealtimeManager.DeclineInvitation(current);
                                }
                                continue;
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator == null)
                        {
                        }
                        enumerator.Dispose();
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetAllInvitations>c__AnonStorey2A8
        {
            internal Action<Invitation[]> callback;

            internal void <>m__DB(RealtimeManager.FetchInvitationsResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    Logger.e("Couldn't load invitations.");
                    this.callback(new Invitation[0]);
                }
                else
                {
                    List<Invitation> list = new List<Invitation>();
                    IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = response.Invitations().GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            GooglePlayGames.Native.PInvoke.MultiplayerInvitation current = enumerator.Current;
                            using (current)
                            {
                                list.Add(current.AsInvitation());
                                continue;
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator == null)
                        {
                        }
                        enumerator.Dispose();
                    }
                    this.callback(list.ToArray());
                }
            }
        }

        [CompilerGenerated]
        private sealed class <HelperForSession>c__AnonStorey2A3
        {
            internal NativeRealtimeMultiplayerClient.RoomSession session;

            internal void <>m__D6(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant, byte[] data, bool isReliable)
            {
                this.session.OnDataReceived(room, participant, data, isReliable);
            }

            internal void <>m__D7(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
            {
                this.session.OnParticipantStatusChanged(room, participant);
            }

            internal void <>m__D8(NativeRealTimeRoom room)
            {
                this.session.OnConnectedSetChanged(room);
            }

            internal void <>m__D9(NativeRealTimeRoom room)
            {
                this.session.OnRoomStatusChanged(room);
            }
        }

        private class AbortingRoomCreationState : NativeRealtimeMultiplayerClient.State
        {
            private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

            internal AbortingRoomCreationState(NativeRealtimeMultiplayerClient.RoomSession session)
            {
                this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
            }

            internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
                    this.mSession.OnGameThreadListener().RoomConnected(false);
                }
                else
                {
                    this.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(this.mSession, response.Room(), delegate {
                        this.mSession.OnGameThreadListener().RoomConnected(false);
                    }));
                }
            }

            internal override bool IsActive()
            {
                return false;
            }
        }

        private class ActiveState : NativeRealtimeMultiplayerClient.MessagingEnabledState
        {
            [CompilerGenerated]
            private static Func<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, string> <>f__am$cache0;
            [CompilerGenerated]
            private static Func<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, Participant> <>f__am$cache1;
            [CompilerGenerated]
            private static Func<Participant, string> <>f__am$cache2;
            [CompilerGenerated]
            private static Func<Participant, string> <>f__am$cache3;

            internal ActiveState(NativeRealTimeRoom room, NativeRealtimeMultiplayerClient.RoomSession session) : base(session, room)
            {
            }

            internal override Participant GetParticipant(string participantId)
            {
                if (!base.mParticipants.ContainsKey(participantId))
                {
                    Logger.e("Attempted to retrieve unknown participant " + participantId);
                    return null;
                }
                return base.mParticipants[participantId];
            }

            internal override Participant GetSelf()
            {
                foreach (Participant participant in base.mParticipants.Values)
                {
                    if ((participant.Player != null) && participant.Player.id.Equals(base.mSession.SelfPlayerId()))
                    {
                        return participant;
                    }
                }
                return null;
            }

            internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
            {
                <HandleConnectedSetChanged>c__AnonStorey2B7 storeyb = new <HandleConnectedSetChanged>c__AnonStorey2B7();
                List<string> source = new List<string>();
                List<string> list2 = new List<string>();
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate (GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) {
                        return p.Id();
                    };
                }
                Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> dictionary = Enumerable.ToDictionary<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, string>(room.Participants(), <>f__am$cache0);
                foreach (string str in base.mNativeParticipants.Keys)
                {
                    GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant = dictionary[str];
                    GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant2 = base.mNativeParticipants[str];
                    if (!participant.IsConnectedToRoom())
                    {
                        list2.Add(str);
                    }
                    if (!participant2.IsConnectedToRoom() && participant.IsConnectedToRoom())
                    {
                        source.Add(str);
                    }
                }
                foreach (GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant3 in base.mNativeParticipants.Values)
                {
                    participant3.Dispose();
                }
                base.mNativeParticipants = dictionary;
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate (GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) {
                        return p.AsParticipant();
                    };
                }
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = delegate (Participant p) {
                        return p.ParticipantId;
                    };
                }
                base.mParticipants = Enumerable.ToDictionary<Participant, string>(Enumerable.Select<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, Participant>(base.mNativeParticipants.Values, <>f__am$cache1), <>f__am$cache2);
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = delegate (Participant p) {
                        return p.ToString();
                    };
                }
                Logger.d("Updated participant statuses: " + string.Join(",", Enumerable.ToArray<string>(Enumerable.Select<Participant, string>(base.mParticipants.Values, <>f__am$cache3))));
                if (list2.Contains(this.GetSelf().ParticipantId))
                {
                    Logger.w("Player was disconnected from the multiplayer session.");
                }
                storeyb.selfId = this.GetSelf().ParticipantId;
                source = Enumerable.ToList<string>(Enumerable.Where<string>(source, new Func<string, bool>(storeyb.<>m__F4)));
                list2 = Enumerable.ToList<string>(Enumerable.Where<string>(list2, new Func<string, bool>(storeyb.<>m__F5)));
                if (source.Count > 0)
                {
                    source.Sort();
                    base.mSession.OnGameThreadListener().PeersConnected(Enumerable.ToArray<string>(Enumerable.Where<string>(source, new Func<string, bool>(storeyb.<>m__F6))));
                }
                if (list2.Count > 0)
                {
                    list2.Sort();
                    base.mSession.OnGameThreadListener().PeersDisconnected(Enumerable.ToArray<string>(Enumerable.Where<string>(list2, new Func<string, bool>(storeyb.<>m__F7))));
                }
            }

            internal override bool IsRoomConnected()
            {
                return true;
            }

            internal override void LeaveRoom()
            {
                base.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(base.mSession, base.mRoom, delegate {
                    base.mSession.OnGameThreadListener().LeftRoom();
                }));
            }

            internal override void OnStateEntered()
            {
                if (this.GetSelf() == null)
                {
                    Logger.e("Room reached active state with unknown participant for the player");
                    this.LeaveRoom();
                }
            }

            [CompilerGenerated]
            private sealed class <HandleConnectedSetChanged>c__AnonStorey2B7
            {
                internal string selfId;

                internal bool <>m__F4(string peerId)
                {
                    return !peerId.Equals(this.selfId);
                }

                internal bool <>m__F5(string peerId)
                {
                    return !peerId.Equals(this.selfId);
                }

                internal bool <>m__F6(string peer)
                {
                    return !peer.Equals(this.selfId);
                }

                internal bool <>m__F7(string peer)
                {
                    return !peer.Equals(this.selfId);
                }
            }
        }

        private class BeforeRoomCreateStartedState : NativeRealtimeMultiplayerClient.State
        {
            private readonly NativeRealtimeMultiplayerClient.RoomSession mContainingSession;

            internal BeforeRoomCreateStartedState(NativeRealtimeMultiplayerClient.RoomSession session)
            {
                this.mContainingSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
            }

            internal override void LeaveRoom()
            {
                Logger.d("Session was torn down before room was created.");
                this.mContainingSession.OnGameThreadListener().RoomConnected(false);
                this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mContainingSession));
            }
        }

        private class ConnectingState : NativeRealtimeMultiplayerClient.MessagingEnabledState
        {
            private static readonly HashSet<Types.ParticipantStatus> FailedStatuses;
            private const float InitialPercentComplete = 20f;
            private HashSet<string> mConnectedParticipants;
            private float mPercentComplete;
            private float mPercentPerParticipant;

            static ConnectingState()
            {
                HashSet<Types.ParticipantStatus> set = new HashSet<Types.ParticipantStatus>();
                set.Add(Types.ParticipantStatus.DECLINED);
                set.Add(Types.ParticipantStatus.LEFT);
                FailedStatuses = set;
            }

            internal ConnectingState(NativeRealTimeRoom room, NativeRealtimeMultiplayerClient.RoomSession session) : base(session, room)
            {
                this.mConnectedParticipants = new HashSet<string>();
                this.mPercentComplete = 20f;
                this.mPercentPerParticipant = 80f / ((float) session.MinPlayersToStart);
            }

            internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
            {
                HashSet<string> set = new HashSet<string>();
                if (((room.Status() == Types.RealTimeRoomStatus.AUTO_MATCHING) || (room.Status() == Types.RealTimeRoomStatus.CONNECTING)) && (base.mSession.MinPlayersToStart <= room.ParticipantCount()))
                {
                    base.mSession.MinPlayersToStart += room.ParticipantCount();
                    this.mPercentPerParticipant = 80f / ((float) base.mSession.MinPlayersToStart);
                }
                IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> enumerator = room.Participants().GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        GooglePlayGames.Native.PInvoke.MultiplayerParticipant current = enumerator.Current;
                        using (current)
                        {
                            if (current.IsConnectedToRoom())
                            {
                                set.Add(current.Id());
                            }
                            continue;
                        }
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                if (this.mConnectedParticipants.Equals(set))
                {
                    Logger.w("Received connected set callback with unchanged connected set!");
                }
                else
                {
                    IEnumerable<string> source = Enumerable.Except<string>(this.mConnectedParticipants, set);
                    if (room.Status() == Types.RealTimeRoomStatus.DELETED)
                    {
                        Logger.e("Participants disconnected during room setup, failing. Participants were: " + string.Join(",", Enumerable.ToArray<string>(source)));
                        base.mSession.OnGameThreadListener().RoomConnected(false);
                        base.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(base.mSession));
                    }
                    else
                    {
                        IEnumerable<string> enumerable2 = Enumerable.Except<string>(set, this.mConnectedParticipants);
                        Logger.d("New participants connected: " + string.Join(",", Enumerable.ToArray<string>(enumerable2)));
                        if (room.Status() == Types.RealTimeRoomStatus.ACTIVE)
                        {
                            Logger.d("Fully connected! Transitioning to active state.");
                            base.mSession.EnterState(new NativeRealtimeMultiplayerClient.ActiveState(room, base.mSession));
                            base.mSession.OnGameThreadListener().RoomConnected(true);
                        }
                        else
                        {
                            this.mPercentComplete += this.mPercentPerParticipant * Enumerable.Count<string>(enumerable2);
                            this.mConnectedParticipants = set;
                            base.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
                        }
                    }
                }
            }

            internal override void HandleParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
            {
                if (FailedStatuses.Contains(participant.Status()))
                {
                    base.mSession.OnGameThreadListener().ParticipantLeft(participant.AsParticipant());
                    if ((room.Status() != Types.RealTimeRoomStatus.CONNECTING) && (room.Status() != Types.RealTimeRoomStatus.AUTO_MATCHING))
                    {
                        this.LeaveRoom();
                    }
                }
            }

            internal override void LeaveRoom()
            {
                base.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(base.mSession, base.mRoom, delegate {
                    base.mSession.OnGameThreadListener().RoomConnected(false);
                }));
            }

            internal override void OnStateEntered()
            {
                base.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
            }

            internal override void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
            {
                base.mSession.ShowingUI = true;
                base.mSession.Manager().ShowWaitingRoomUI(base.mRoom, minimumParticipantsBeforeStarting, delegate (RealtimeManager.WaitingRoomUIResponse response) {
                    base.mSession.ShowingUI = false;
                    Logger.d("ShowWaitingRoomUI Response: " + response.ResponseStatus());
                    if (response.ResponseStatus() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
                    {
                        Logger.d(string.Concat(new object[] { "Connecting state ShowWaitingRoomUI: room pcount:", response.Room().ParticipantCount(), " status: ", response.Room().Status() }));
                        if (response.Room().Status() == Types.RealTimeRoomStatus.ACTIVE)
                        {
                            base.mSession.EnterState(new NativeRealtimeMultiplayerClient.ActiveState(response.Room(), base.mSession));
                        }
                    }
                    else if (response.ResponseStatus() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_LEFT_ROOM)
                    {
                        this.LeaveRoom();
                    }
                    else
                    {
                        base.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
                    }
                });
            }
        }

        private class LeavingRoom : NativeRealtimeMultiplayerClient.State
        {
            private readonly Action mLeavingCompleteCallback;
            private readonly NativeRealTimeRoom mRoomToLeave;
            private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

            internal LeavingRoom(NativeRealtimeMultiplayerClient.RoomSession session, NativeRealTimeRoom room, Action leavingCompleteCallback)
            {
                this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
                this.mRoomToLeave = Misc.CheckNotNull<NativeRealTimeRoom>(room);
                this.mLeavingCompleteCallback = Misc.CheckNotNull<Action>(leavingCompleteCallback);
            }

            internal override bool IsActive()
            {
                return false;
            }

            internal override void OnStateEntered()
            {
                this.mSession.Manager().LeaveRoom(this.mRoomToLeave, delegate (GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status) {
                    this.mLeavingCompleteCallback();
                    this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
                });
            }
        }

        private abstract class MessagingEnabledState : NativeRealtimeMultiplayerClient.State
        {
            [CompilerGenerated]
            private static Func<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, string> <>f__am$cache4;
            [CompilerGenerated]
            private static Func<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, Participant> <>f__am$cache5;
            [CompilerGenerated]
            private static Func<Participant, string> <>f__am$cache6;
            [CompilerGenerated]
            private static Func<Participant, bool> <>f__am$cache7;
            protected Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> mNativeParticipants;
            protected Dictionary<string, Participant> mParticipants;
            protected NativeRealTimeRoom mRoom;
            protected readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

            internal MessagingEnabledState(NativeRealtimeMultiplayerClient.RoomSession session, NativeRealTimeRoom room)
            {
                this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
                this.UpdateCurrentRoom(room);
            }

            internal sealed override List<Participant> GetConnectedParticipants()
            {
                if (<>f__am$cache7 == null)
                {
                    <>f__am$cache7 = delegate (Participant p) {
                        return p.IsConnectedToRoom;
                    };
                }
                List<Participant> list = Enumerable.ToList<Participant>(Enumerable.Where<Participant>(this.mParticipants.Values, <>f__am$cache7));
                list.Sort();
                return list;
            }

            internal virtual void HandleConnectedSetChanged(NativeRealTimeRoom room)
            {
            }

            internal virtual void HandleParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
            {
            }

            internal virtual void HandleRoomStatusChanged(NativeRealTimeRoom room)
            {
            }

            internal sealed override void OnConnectedSetChanged(NativeRealTimeRoom room)
            {
                this.HandleConnectedSetChanged(room);
                this.UpdateCurrentRoom(room);
            }

            internal override void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
            {
                this.mSession.OnGameThreadListener().RealTimeMessageReceived(isReliable, sender.Id(), data);
            }

            internal sealed override void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
            {
                this.HandleParticipantStatusChanged(room, participant);
                this.UpdateCurrentRoom(room);
            }

            internal sealed override void OnRoomStatusChanged(NativeRealTimeRoom room)
            {
                this.HandleRoomStatusChanged(room);
                this.UpdateCurrentRoom(room);
            }

            internal override void SendToAll(byte[] data, int offset, int length, bool isReliable)
            {
                byte[] buffer = Misc.GetSubsetBytes(data, offset, length);
                if (isReliable)
                {
                    foreach (string str in this.mNativeParticipants.Keys)
                    {
                        this.SendToSpecificRecipient(str, buffer, 0, buffer.Length, true);
                    }
                }
                else
                {
                    this.mSession.Manager().SendUnreliableMessageToAll(this.mRoom, buffer);
                }
            }

            internal override void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
            {
                if (!this.mNativeParticipants.ContainsKey(recipientId))
                {
                    Logger.e("Attempted to send message to unknown participant " + recipientId);
                }
                else if (isReliable)
                {
                    this.mSession.Manager().SendReliableMessage(this.mRoom, this.mNativeParticipants[recipientId], Misc.GetSubsetBytes(data, offset, length), null);
                }
                else
                {
                    List<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> recipients = new List<GooglePlayGames.Native.PInvoke.MultiplayerParticipant>();
                    recipients.Add(this.mNativeParticipants[recipientId]);
                    this.mSession.Manager().SendUnreliableMessageToSpecificParticipants(this.mRoom, recipients, Misc.GetSubsetBytes(data, offset, length));
                }
            }

            internal void UpdateCurrentRoom(NativeRealTimeRoom room)
            {
                if (this.mRoom != null)
                {
                    this.mRoom.Dispose();
                }
                this.mRoom = Misc.CheckNotNull<NativeRealTimeRoom>(room);
                if (<>f__am$cache4 == null)
                {
                    <>f__am$cache4 = delegate (GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) {
                        return p.Id();
                    };
                }
                this.mNativeParticipants = Enumerable.ToDictionary<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, string>(this.mRoom.Participants(), <>f__am$cache4);
                if (<>f__am$cache5 == null)
                {
                    <>f__am$cache5 = delegate (GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) {
                        return p.AsParticipant();
                    };
                }
                if (<>f__am$cache6 == null)
                {
                    <>f__am$cache6 = delegate (Participant p) {
                        return p.ParticipantId;
                    };
                }
                this.mParticipants = Enumerable.ToDictionary<Participant, string>(Enumerable.Select<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, Participant>(this.mNativeParticipants.Values, <>f__am$cache5), <>f__am$cache6);
            }
        }

        private class NoopListener : RealTimeMultiplayerListener
        {
            public void OnLeftRoom()
            {
            }

            public void OnParticipantLeft(Participant participant)
            {
            }

            public void OnPeersConnected(string[] participantIds)
            {
            }

            public void OnPeersDisconnected(string[] participantIds)
            {
            }

            public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
            {
            }

            public void OnRoomConnected(bool success)
            {
            }

            public void OnRoomSetupProgress(float percent)
            {
            }
        }

        private class OnGameThreadForwardingListener
        {
            private readonly RealTimeMultiplayerListener mListener;

            internal OnGameThreadForwardingListener(RealTimeMultiplayerListener listener)
            {
                this.mListener = Misc.CheckNotNull<RealTimeMultiplayerListener>(listener);
            }

            public void LeftRoom()
            {
                PlayGamesHelperObject.RunOnGameThread(delegate {
                    this.mListener.OnLeftRoom();
                });
            }

            public void ParticipantLeft(Participant participant)
            {
                <ParticipantLeft>c__AnonStorey2B6 storeyb = new <ParticipantLeft>c__AnonStorey2B6();
                storeyb.participant = participant;
                storeyb.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyb.<>m__E9));
            }

            public void PeersConnected(string[] participantIds)
            {
                <PeersConnected>c__AnonStorey2B3 storeyb = new <PeersConnected>c__AnonStorey2B3();
                storeyb.participantIds = participantIds;
                storeyb.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyb.<>m__E6));
            }

            public void PeersDisconnected(string[] participantIds)
            {
                <PeersDisconnected>c__AnonStorey2B4 storeyb = new <PeersDisconnected>c__AnonStorey2B4();
                storeyb.participantIds = participantIds;
                storeyb.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyb.<>m__E7));
            }

            public void RealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
            {
                <RealTimeMessageReceived>c__AnonStorey2B5 storeyb = new <RealTimeMessageReceived>c__AnonStorey2B5();
                storeyb.isReliable = isReliable;
                storeyb.senderId = senderId;
                storeyb.data = data;
                storeyb.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyb.<>m__E8));
            }

            public void RoomConnected(bool success)
            {
                <RoomConnected>c__AnonStorey2B2 storeyb = new <RoomConnected>c__AnonStorey2B2();
                storeyb.success = success;
                storeyb.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyb.<>m__E4));
            }

            public void RoomSetupProgress(float percent)
            {
                <RoomSetupProgress>c__AnonStorey2B1 storeyb = new <RoomSetupProgress>c__AnonStorey2B1();
                storeyb.percent = percent;
                storeyb.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyb.<>m__E3));
            }

            [CompilerGenerated]
            private sealed class <ParticipantLeft>c__AnonStorey2B6
            {
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener <>f__this;
                internal Participant participant;

                internal void <>m__E9()
                {
                    this.<>f__this.mListener.OnParticipantLeft(this.participant);
                }
            }

            [CompilerGenerated]
            private sealed class <PeersConnected>c__AnonStorey2B3
            {
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener <>f__this;
                internal string[] participantIds;

                internal void <>m__E6()
                {
                    this.<>f__this.mListener.OnPeersConnected(this.participantIds);
                }
            }

            [CompilerGenerated]
            private sealed class <PeersDisconnected>c__AnonStorey2B4
            {
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener <>f__this;
                internal string[] participantIds;

                internal void <>m__E7()
                {
                    this.<>f__this.mListener.OnPeersDisconnected(this.participantIds);
                }
            }

            [CompilerGenerated]
            private sealed class <RealTimeMessageReceived>c__AnonStorey2B5
            {
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener <>f__this;
                internal byte[] data;
                internal bool isReliable;
                internal string senderId;

                internal void <>m__E8()
                {
                    this.<>f__this.mListener.OnRealTimeMessageReceived(this.isReliable, this.senderId, this.data);
                }
            }

            [CompilerGenerated]
            private sealed class <RoomConnected>c__AnonStorey2B2
            {
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener <>f__this;
                internal bool success;

                internal void <>m__E4()
                {
                    this.<>f__this.mListener.OnRoomConnected(this.success);
                }
            }

            [CompilerGenerated]
            private sealed class <RoomSetupProgress>c__AnonStorey2B1
            {
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener <>f__this;
                internal float percent;

                internal void <>m__E3()
                {
                    this.<>f__this.mListener.OnRoomSetupProgress(this.percent);
                }
            }
        }

        private class RoomCreationPendingState : NativeRealtimeMultiplayerClient.State
        {
            private readonly NativeRealtimeMultiplayerClient.RoomSession mContainingSession;

            internal RoomCreationPendingState(NativeRealtimeMultiplayerClient.RoomSession session)
            {
                this.mContainingSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
            }

            internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mContainingSession));
                    this.mContainingSession.OnGameThreadListener().RoomConnected(false);
                }
                else
                {
                    this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ConnectingState(response.Room(), this.mContainingSession));
                }
            }

            internal override bool IsActive()
            {
                return true;
            }

            internal override void LeaveRoom()
            {
                Logger.d("Received request to leave room during room creation, aborting creation.");
                this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.AbortingRoomCreationState(this.mContainingSession));
            }
        }

        private class RoomSession
        {
            private volatile string mCurrentPlayerId;
            private Invitation mInvitation;
            private readonly object mLifecycleLock = new object();
            private readonly NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener mListener;
            private readonly RealtimeManager mManager;
            private uint mMinPlayersToStart;
            private volatile bool mShowingUI;
            private volatile NativeRealtimeMultiplayerClient.State mState;
            private volatile bool mStillPreRoomCreation;

            internal RoomSession(RealtimeManager manager, RealTimeMultiplayerListener listener)
            {
                this.mManager = Misc.CheckNotNull<RealtimeManager>(manager);
                this.mListener = new NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener(listener);
                this.EnterState(new NativeRealtimeMultiplayerClient.BeforeRoomCreateStartedState(this), false);
                this.mStillPreRoomCreation = true;
            }

            internal void EnterState(NativeRealtimeMultiplayerClient.State handler)
            {
                this.EnterState(handler, true);
            }

            internal void EnterState(NativeRealtimeMultiplayerClient.State handler, bool fireStateEnteredEvent)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.State>(handler);
                    if (fireStateEnteredEvent)
                    {
                        Logger.d("Entering state: " + handler.GetType().Name);
                        this.mState.OnStateEntered();
                    }
                }
            }

            internal List<Participant> GetConnectedParticipants()
            {
                return this.mState.GetConnectedParticipants();
            }

            public Invitation GetInvitation()
            {
                return this.mInvitation;
            }

            internal virtual Participant GetParticipant(string participantId)
            {
                return this.mState.GetParticipant(participantId);
            }

            internal virtual Participant GetSelf()
            {
                return this.mState.GetSelf();
            }

            internal void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState.HandleRoomResponse(response);
                }
            }

            internal bool IsActive()
            {
                return this.mState.IsActive();
            }

            internal virtual bool IsRoomConnected()
            {
                return this.mState.IsRoomConnected();
            }

            internal void LeaveRoom()
            {
                if (!this.ShowingUI)
                {
                    object mLifecycleLock = this.mLifecycleLock;
                    lock (mLifecycleLock)
                    {
                        this.mState.LeaveRoom();
                    }
                }
                else
                {
                    Logger.d("Not leaving room since showing UI");
                }
            }

            internal RealtimeManager Manager()
            {
                return this.mManager;
            }

            internal void OnConnectedSetChanged(NativeRealTimeRoom room)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState.OnConnectedSetChanged(room);
                }
            }

            internal void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
            {
                this.mState.OnDataReceived(room, sender, data, isReliable);
            }

            internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener OnGameThreadListener()
            {
                return this.mListener;
            }

            internal void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState.OnParticipantStatusChanged(room, participant);
                }
            }

            internal void OnRoomStatusChanged(NativeRealTimeRoom room)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState.OnRoomStatusChanged(room);
                }
            }

            internal string SelfPlayerId()
            {
                return this.mCurrentPlayerId;
            }

            internal void SendMessage(bool reliable, string participantId, byte[] data)
            {
                this.SendMessage(reliable, participantId, data, 0, data.Length);
            }

            internal void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
            {
                this.mState.SendToSpecificRecipient(participantId, data, offset, length, reliable);
            }

            internal void SendMessageToAll(bool reliable, byte[] data)
            {
                this.SendMessageToAll(reliable, data, 0, data.Length);
            }

            internal void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
            {
                this.mState.SendToAll(data, offset, length, reliable);
            }

            public void SetInvitation(Invitation invitation)
            {
                this.mInvitation = invitation;
            }

            internal void ShowWaitingRoomUI()
            {
                this.mState.ShowWaitingRoomUI(this.MinPlayersToStart);
            }

            internal void StartRoomCreation(string currentPlayerId, Action createRoom)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    if (!this.mStillPreRoomCreation)
                    {
                        Logger.e("Room creation started more than once, this shouldn't happen!");
                    }
                    else if (!this.mState.IsActive())
                    {
                        Logger.w("Received an attempt to create a room after the session was already torn down!");
                    }
                    else
                    {
                        this.mCurrentPlayerId = Misc.CheckNotNull<string>(currentPlayerId);
                        this.mStillPreRoomCreation = false;
                        this.EnterState(new NativeRealtimeMultiplayerClient.RoomCreationPendingState(this));
                        createRoom();
                    }
                }
            }

            internal uint MinPlayersToStart
            {
                get
                {
                    return this.mMinPlayersToStart;
                }
                set
                {
                    this.mMinPlayersToStart = value;
                }
            }

            internal bool ShowingUI
            {
                get
                {
                    return this.mShowingUI;
                }
                set
                {
                    this.mShowingUI = value;
                }
            }
        }

        private class ShutdownState : NativeRealtimeMultiplayerClient.State
        {
            private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

            internal ShutdownState(NativeRealtimeMultiplayerClient.RoomSession session)
            {
                this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
            }

            internal override bool IsActive()
            {
                return false;
            }

            internal override void LeaveRoom()
            {
                this.mSession.OnGameThreadListener().LeftRoom();
            }
        }

        internal abstract class State
        {
            protected State()
            {
            }

            internal virtual List<Participant> GetConnectedParticipants()
            {
                Logger.d(base.GetType().Name + ".GetConnectedParticipants: Returning empty connected participants");
                return new List<Participant>();
            }

            internal virtual Participant GetParticipant(string participantId)
            {
                Logger.d(base.GetType().Name + ".GetSelf: Returning null participant.");
                return null;
            }

            internal virtual Participant GetSelf()
            {
                Logger.d(base.GetType().Name + ".GetSelf: Returning null self.");
                return null;
            }

            internal virtual void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
            {
                Logger.d(base.GetType().Name + ".HandleRoomResponse: Defaulting to no-op.");
            }

            internal virtual bool IsActive()
            {
                Logger.d(base.GetType().Name + ".IsNonPreemptable: Is preemptable by default.");
                return true;
            }

            internal virtual bool IsRoomConnected()
            {
                Logger.d(base.GetType().Name + ".IsRoomConnected: Returning room not connected.");
                return false;
            }

            internal virtual void LeaveRoom()
            {
                Logger.d(base.GetType().Name + ".LeaveRoom: Defaulting to no-op.");
            }

            internal virtual void OnConnectedSetChanged(NativeRealTimeRoom room)
            {
                Logger.d(base.GetType().Name + ".OnConnectedSetChanged: Defaulting to no-op.");
            }

            internal virtual void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
            {
                Logger.d(base.GetType().Name + ".OnDataReceived: Defaulting to no-op.");
            }

            internal virtual void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
            {
                Logger.d(base.GetType().Name + ".OnParticipantStatusChanged: Defaulting to no-op.");
            }

            internal virtual void OnRoomStatusChanged(NativeRealTimeRoom room)
            {
                Logger.d(base.GetType().Name + ".OnRoomStatusChanged: Defaulting to no-op.");
            }

            internal virtual void OnStateEntered()
            {
                Logger.d(base.GetType().Name + ".OnStateEntered: Defaulting to no-op.");
            }

            internal virtual void SendToAll(byte[] data, int offset, int length, bool isReliable)
            {
                Logger.d(base.GetType().Name + ".SendToApp: Defaulting to no-op.");
            }

            internal virtual void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
            {
                Logger.d(base.GetType().Name + ".SendToSpecificRecipient: Defaulting to no-op.");
            }

            internal virtual void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
            {
                Logger.d(base.GetType().Name + ".ShowWaitingRoomUI: Defaulting to no-op.");
            }
        }
    }
}

