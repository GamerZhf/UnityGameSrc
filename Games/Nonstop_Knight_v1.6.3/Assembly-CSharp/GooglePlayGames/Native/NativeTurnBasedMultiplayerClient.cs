namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class NativeTurnBasedMultiplayerClient : ITurnBasedMultiplayerClient
    {
        private volatile Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> mMatchDelegate;
        private readonly NativeClient mNativeClient;
        private readonly TurnBasedManager mTurnBasedManager;

        internal NativeTurnBasedMultiplayerClient(NativeClient nativeClient, TurnBasedManager manager)
        {
            this.mTurnBasedManager = manager;
            this.mNativeClient = nativeClient;
        }

        public void AcceptFromInbox(Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
        {
            <AcceptFromInbox>c__AnonStorey2C9 storeyc = new <AcceptFromInbox>c__AnonStorey2C9();
            storeyc.callback = callback;
            storeyc.<>f__this = this;
            storeyc.callback = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(storeyc.callback);
            this.mTurnBasedManager.ShowInboxUI(new Action<TurnBasedManager.MatchInboxUIResponse>(storeyc.<>m__110));
        }

        public void AcceptInvitation(string invitationId, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
        {
            <AcceptInvitation>c__AnonStorey2CA storeyca = new <AcceptInvitation>c__AnonStorey2CA();
            storeyca.invitationId = invitationId;
            storeyca.callback = callback;
            storeyca.<>f__this = this;
            storeyca.callback = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(storeyca.callback);
            this.FindInvitationWithId(storeyca.invitationId, new Action<GooglePlayGames.Native.PInvoke.MultiplayerInvitation>(storeyca.<>m__111));
        }

        public void AcknowledgeFinished(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
        {
            <AcknowledgeFinished>c__AnonStorey2D2 storeyd = new <AcknowledgeFinished>c__AnonStorey2D2();
            storeyd.callback = callback;
            storeyd.<>f__this = this;
            storeyd.callback = Callbacks.AsOnGameThreadCallback<bool>(storeyd.callback);
            this.FindEqualVersionMatch(match, storeyd.callback, new Action<NativeTurnBasedMatch>(storeyd.<>m__119));
        }

        private Action<TurnBasedManager.TurnBasedMatchResponse> BridgeMatchToUserCallback(Action<GooglePlayGames.BasicApi.UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> userCallback)
        {
            <BridgeMatchToUserCallback>c__AnonStorey2C8 storeyc = new <BridgeMatchToUserCallback>c__AnonStorey2C8();
            storeyc.userCallback = userCallback;
            storeyc.<>f__this = this;
            return new Action<TurnBasedManager.TurnBasedMatchResponse>(storeyc.<>m__10F);
        }

        public void Cancel(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
        {
            <Cancel>c__AnonStorey2D5 storeyd = new <Cancel>c__AnonStorey2D5();
            storeyd.callback = callback;
            storeyd.<>f__this = this;
            storeyd.callback = Callbacks.AsOnGameThreadCallback<bool>(storeyd.callback);
            this.FindEqualVersionMatch(match, storeyd.callback, new Action<NativeTurnBasedMatch>(storeyd.<>m__11C));
        }

        public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
        {
            this.CreateQuickMatch(minOpponents, maxOpponents, variant, 0L, callback);
        }

        public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitmask, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
        {
            <CreateQuickMatch>c__AnonStorey2C3 storeyc = new <CreateQuickMatch>c__AnonStorey2C3();
            storeyc.callback = callback;
            storeyc.callback = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(storeyc.callback);
            using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder builder = GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
            {
                builder.SetVariant(variant).SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetExclusiveBitMask(exclusiveBitmask);
                using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig config = builder.Build())
                {
                    this.mTurnBasedManager.CreateMatch(config, this.BridgeMatchToUserCallback(new Action<GooglePlayGames.BasicApi.UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(storeyc.<>m__10A)));
                }
            }
        }

        public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<GooglePlayGames.BasicApi.UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
        {
            <CreateWithInvitationScreen>c__AnonStorey2C5 storeyc = new <CreateWithInvitationScreen>c__AnonStorey2C5();
            storeyc.callback = callback;
            storeyc.variant = variant;
            storeyc.<>f__this = this;
            storeyc.callback = Callbacks.AsOnGameThreadCallback<GooglePlayGames.BasicApi.UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(storeyc.callback);
            this.mTurnBasedManager.ShowPlayerSelectUI(minOpponents, maxOpponents, true, new Action<PlayerSelectUIResponse>(storeyc.<>m__10C));
        }

        public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
        {
            <CreateWithInvitationScreen>c__AnonStorey2C4 storeyc = new <CreateWithInvitationScreen>c__AnonStorey2C4();
            storeyc.callback = callback;
            this.CreateWithInvitationScreen(minOpponents, maxOpponents, variant, new Action<GooglePlayGames.BasicApi.UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(storeyc.<>m__10B));
        }

        public void DeclineInvitation(string invitationId)
        {
            this.FindInvitationWithId(invitationId, delegate (GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation) {
                if (invitation != null)
                {
                    this.mTurnBasedManager.DeclineInvitation(invitation);
                }
            });
        }

        private void FindEqualVersionMatch(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> onFailure, Action<NativeTurnBasedMatch> onVersionMatch)
        {
            <FindEqualVersionMatch>c__AnonStorey2CF storeycf = new <FindEqualVersionMatch>c__AnonStorey2CF();
            storeycf.match = match;
            storeycf.onFailure = onFailure;
            storeycf.onVersionMatch = onVersionMatch;
            this.mTurnBasedManager.GetMatch(storeycf.match.MatchId, new Action<TurnBasedManager.TurnBasedMatchResponse>(storeycf.<>m__116));
        }

        private void FindEqualVersionMatchWithParticipant(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string participantId, Action<bool> onFailure, Action<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, NativeTurnBasedMatch> onFoundParticipantAndMatch)
        {
            <FindEqualVersionMatchWithParticipant>c__AnonStorey2D0 storeyd = new <FindEqualVersionMatchWithParticipant>c__AnonStorey2D0();
            storeyd.participantId = participantId;
            storeyd.onFoundParticipantAndMatch = onFoundParticipantAndMatch;
            storeyd.match = match;
            storeyd.onFailure = onFailure;
            this.FindEqualVersionMatch(storeyd.match, storeyd.onFailure, new Action<NativeTurnBasedMatch>(storeyd.<>m__117));
        }

        private void FindInvitationWithId(string invitationId, Action<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback)
        {
            <FindInvitationWithId>c__AnonStorey2CB storeycb = new <FindInvitationWithId>c__AnonStorey2CB();
            storeycb.callback = callback;
            storeycb.invitationId = invitationId;
            this.mTurnBasedManager.GetAllTurnbasedMatches(new Action<TurnBasedManager.TurnBasedMatchesResponse>(storeycb.<>m__112));
        }

        public void Finish(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, MatchOutcome outcome, Action<bool> callback)
        {
            <Finish>c__AnonStorey2D1 storeyd = new <Finish>c__AnonStorey2D1();
            storeyd.outcome = outcome;
            storeyd.callback = callback;
            storeyd.data = data;
            storeyd.<>f__this = this;
            storeyd.callback = Callbacks.AsOnGameThreadCallback<bool>(storeyd.callback);
            this.FindEqualVersionMatch(match, storeyd.callback, new Action<NativeTurnBasedMatch>(storeyd.<>m__118));
        }

        public void GetAllInvitations(Action<Invitation[]> callback)
        {
            <GetAllInvitations>c__AnonStorey2C6 storeyc = new <GetAllInvitations>c__AnonStorey2C6();
            storeyc.callback = callback;
            this.mTurnBasedManager.GetAllTurnbasedMatches(new Action<TurnBasedManager.TurnBasedMatchesResponse>(storeyc.<>m__10D));
        }

        public void GetAllMatches(Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[]> callback)
        {
            <GetAllMatches>c__AnonStorey2C7 storeyc = new <GetAllMatches>c__AnonStorey2C7();
            storeyc.callback = callback;
            storeyc.<>f__this = this;
            this.mTurnBasedManager.GetAllTurnbasedMatches(new Action<TurnBasedManager.TurnBasedMatchesResponse>(storeyc.<>m__10E));
        }

        public int GetMaxMatchDataSize()
        {
            throw new NotImplementedException();
        }

        internal void HandleMatchEvent(Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match)
        {
            <HandleMatchEvent>c__AnonStorey2CD storeycd = new <HandleMatchEvent>c__AnonStorey2CD();
            storeycd.match = match;
            storeycd.<>f__this = this;
            storeycd.currentDelegate = this.mMatchDelegate;
            if (storeycd.currentDelegate != null)
            {
                if (eventType == Types.MultiplayerEvent.REMOVED)
                {
                    Logger.d("Ignoring REMOVE event for match " + matchId);
                }
                else
                {
                    storeycd.shouldAutolaunch = eventType == Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
                    storeycd.match.ReferToMe();
                    Callbacks.AsCoroutine(this.WaitForLogin(new Action(storeycd.<>m__114)));
                }
            }
        }

        public void Leave(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
        {
            <Leave>c__AnonStorey2D3 storeyd = new <Leave>c__AnonStorey2D3();
            storeyd.callback = callback;
            storeyd.<>f__this = this;
            storeyd.callback = Callbacks.AsOnGameThreadCallback<bool>(storeyd.callback);
            this.FindEqualVersionMatch(match, storeyd.callback, new Action<NativeTurnBasedMatch>(storeyd.<>m__11A));
        }

        public void LeaveDuringTurn(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string pendingParticipantId, Action<bool> callback)
        {
            <LeaveDuringTurn>c__AnonStorey2D4 storeyd = new <LeaveDuringTurn>c__AnonStorey2D4();
            storeyd.callback = callback;
            storeyd.<>f__this = this;
            storeyd.callback = Callbacks.AsOnGameThreadCallback<bool>(storeyd.callback);
            this.FindEqualVersionMatchWithParticipant(match, pendingParticipantId, storeyd.callback, new Action<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, NativeTurnBasedMatch>(storeyd.<>m__11B));
        }

        public void RegisterMatchDelegate(MatchDelegate del)
        {
            <RegisterMatchDelegate>c__AnonStorey2CC storeycc = new <RegisterMatchDelegate>c__AnonStorey2CC();
            storeycc.del = del;
            if (storeycc.del == null)
            {
                this.mMatchDelegate = null;
            }
            else
            {
                this.mMatchDelegate = Callbacks.AsOnGameThreadCallback<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool>(new Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool>(storeycc.<>m__113));
            }
        }

        public void Rematch(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
        {
            <Rematch>c__AnonStorey2D6 storeyd = new <Rematch>c__AnonStorey2D6();
            storeyd.callback = callback;
            storeyd.<>f__this = this;
            storeyd.callback = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(storeyd.callback);
            this.FindEqualVersionMatch(match, new Action<bool>(storeyd.<>m__11D), new Action<NativeTurnBasedMatch>(storeyd.<>m__11E));
        }

        private static Types.MatchResult ResultToMatchResult(MatchOutcome.ParticipantResult result)
        {
            switch (result)
            {
                case MatchOutcome.ParticipantResult.None:
                    return Types.MatchResult.NONE;

                case MatchOutcome.ParticipantResult.Win:
                    return Types.MatchResult.WIN;

                case MatchOutcome.ParticipantResult.Loss:
                    return Types.MatchResult.LOSS;

                case MatchOutcome.ParticipantResult.Tie:
                    return Types.MatchResult.TIE;
            }
            Logger.e("Received unknown ParticipantResult " + result);
            return Types.MatchResult.NONE;
        }

        public void TakeTurn(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, string pendingParticipantId, Action<bool> callback)
        {
            <TakeTurn>c__AnonStorey2CE storeyce = new <TakeTurn>c__AnonStorey2CE();
            storeyce.data = data;
            storeyce.callback = callback;
            storeyce.<>f__this = this;
            Logger.describe(storeyce.data);
            storeyce.callback = Callbacks.AsOnGameThreadCallback<bool>(storeyce.callback);
            this.FindEqualVersionMatchWithParticipant(match, pendingParticipantId, storeyce.callback, new Action<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, NativeTurnBasedMatch>(storeyce.<>m__115));
        }

        [DebuggerHidden]
        private IEnumerator WaitForLogin(Action method)
        {
            <WaitForLogin>c__Iterator26 iterator = new <WaitForLogin>c__Iterator26();
            iterator.method = method;
            iterator.<$>method = method;
            iterator.<>f__this = this;
            return iterator;
        }

        [CompilerGenerated]
        private sealed class <AcceptFromInbox>c__AnonStorey2C9
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

            internal void <>m__110(TurnBasedManager.MatchInboxUIResponse callbackResult)
            {
                using (NativeTurnBasedMatch match = callbackResult.Match())
                {
                    if (match == null)
                    {
                        this.callback(false, null);
                    }
                    else
                    {
                        GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match2 = match.AsTurnBasedMatch(this.<>f__this.mNativeClient.GetUserId());
                        Logger.d("Passing converted match to user callback:" + match2);
                        this.callback(true, match2);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <AcceptInvitation>c__AnonStorey2CA
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;
            internal string invitationId;

            internal void <>m__111(GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
            {
                if (invitation == null)
                {
                    Logger.e("Could not find invitation with id " + this.invitationId);
                    this.callback(false, null);
                }
                else
                {
                    this.<>f__this.mTurnBasedManager.AcceptInvitation(invitation, this.<>f__this.BridgeMatchToUserCallback(delegate (GooglePlayGames.BasicApi.UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match) {
                        this.callback(status == GooglePlayGames.BasicApi.UIStatus.Valid, match);
                    }));
                }
            }

            internal void <>m__120(GooglePlayGames.BasicApi.UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
            {
                this.callback(status == GooglePlayGames.BasicApi.UIStatus.Valid, match);
            }
        }

        [CompilerGenerated]
        private sealed class <AcknowledgeFinished>c__AnonStorey2D2
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<bool> callback;

            internal void <>m__119(NativeTurnBasedMatch foundMatch)
            {
                this.<>f__this.mTurnBasedManager.ConfirmPendingCompletion(foundMatch, delegate (TurnBasedManager.TurnBasedMatchResponse response) {
                    this.callback(response.RequestSucceeded());
                });
            }

            internal void <>m__123(TurnBasedManager.TurnBasedMatchResponse response)
            {
                this.callback(response.RequestSucceeded());
            }
        }

        [CompilerGenerated]
        private sealed class <BridgeMatchToUserCallback>c__AnonStorey2C8
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<GooglePlayGames.BasicApi.UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> userCallback;

            internal void <>m__10F(TurnBasedManager.TurnBasedMatchResponse callbackResult)
            {
                using (NativeTurnBasedMatch match = callbackResult.Match())
                {
                    if (match == null)
                    {
                        GooglePlayGames.BasicApi.UIStatus internalError = GooglePlayGames.BasicApi.UIStatus.InternalError;
                        switch ((callbackResult.ResponseStatus() + 5))
                        {
                            case ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED):
                                internalError = GooglePlayGames.BasicApi.UIStatus.Timeout;
                                break;

                            case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID:
                                internalError = GooglePlayGames.BasicApi.UIStatus.VersionUpdateRequired;
                                break;

                            case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE:
                                internalError = GooglePlayGames.BasicApi.UIStatus.NotAuthorized;
                                break;

                            case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED:
                                internalError = GooglePlayGames.BasicApi.UIStatus.InternalError;
                                break;

                            case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_MATCH_ALREADY_REMATCHED:
                                internalError = GooglePlayGames.BasicApi.UIStatus.Valid;
                                break;

                            case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_INACTIVE_MATCH:
                                internalError = GooglePlayGames.BasicApi.UIStatus.Valid;
                                break;
                        }
                        this.userCallback(internalError, null);
                    }
                    else
                    {
                        GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match2 = match.AsTurnBasedMatch(this.<>f__this.mNativeClient.GetUserId());
                        Logger.d("Passing converted match to user callback:" + match2);
                        this.userCallback(GooglePlayGames.BasicApi.UIStatus.Valid, match2);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Cancel>c__AnonStorey2D5
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<bool> callback;

            internal void <>m__11C(NativeTurnBasedMatch foundMatch)
            {
                this.<>f__this.mTurnBasedManager.CancelMatch(foundMatch, delegate (GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status) {
                    this.callback(status > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));
                });
            }

            internal void <>m__126(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status)
            {
                this.callback(status > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));
            }
        }

        [CompilerGenerated]
        private sealed class <CreateQuickMatch>c__AnonStorey2C3
        {
            internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

            internal void <>m__10A(GooglePlayGames.BasicApi.UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
            {
                this.callback(status == GooglePlayGames.BasicApi.UIStatus.Valid, match);
            }
        }

        [CompilerGenerated]
        private sealed class <CreateWithInvitationScreen>c__AnonStorey2C4
        {
            internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

            internal void <>m__10B(GooglePlayGames.BasicApi.UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
            {
                this.callback(status == GooglePlayGames.BasicApi.UIStatus.Valid, match);
            }
        }

        [CompilerGenerated]
        private sealed class <CreateWithInvitationScreen>c__AnonStorey2C5
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<GooglePlayGames.BasicApi.UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;
            internal uint variant;

            internal void <>m__10C(PlayerSelectUIResponse result)
            {
                if (result.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
                {
                    this.callback((GooglePlayGames.BasicApi.UIStatus) result.Status(), null);
                }
                else
                {
                    using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder builder = GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
                    {
                        builder.PopulateFromUIResponse(result).SetVariant(this.variant);
                        using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig config = builder.Build())
                        {
                            this.<>f__this.mTurnBasedManager.CreateMatch(config, this.<>f__this.BridgeMatchToUserCallback(this.callback));
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FindEqualVersionMatch>c__AnonStorey2CF
        {
            internal GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match;
            internal Action<bool> onFailure;
            internal Action<NativeTurnBasedMatch> onVersionMatch;

            internal void <>m__116(TurnBasedManager.TurnBasedMatchResponse response)
            {
                using (NativeTurnBasedMatch match = response.Match())
                {
                    if (match == null)
                    {
                        Logger.e(string.Format("Could not find match {0}", this.match.MatchId));
                        this.onFailure(false);
                    }
                    else if (match.Version() != this.match.Version)
                    {
                        Logger.e(string.Format("Attempted to update a stale version of the match. Expected version was {0} but current version is {1}.", this.match.Version, match.Version()));
                        this.onFailure(false);
                    }
                    else
                    {
                        this.onVersionMatch(match);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FindEqualVersionMatchWithParticipant>c__AnonStorey2D0
        {
            internal GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match;
            internal Action<bool> onFailure;
            internal Action<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, NativeTurnBasedMatch> onFoundParticipantAndMatch;
            internal string participantId;

            internal void <>m__117(NativeTurnBasedMatch foundMatch)
            {
                if (this.participantId == null)
                {
                    using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant = GooglePlayGames.Native.PInvoke.MultiplayerParticipant.AutomatchingSentinel())
                    {
                        this.onFoundParticipantAndMatch(participant, foundMatch);
                        return;
                    }
                }
                using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant2 = foundMatch.ParticipantWithId(this.participantId))
                {
                    if (participant2 == null)
                    {
                        Logger.e(string.Format("Located match {0} but desired participant with ID {1} could not be found", this.match.MatchId, this.participantId));
                        this.onFailure(false);
                    }
                    else
                    {
                        this.onFoundParticipantAndMatch(participant2, foundMatch);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FindInvitationWithId>c__AnonStorey2CB
        {
            internal Action<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback;
            internal string invitationId;

            internal void <>m__112(TurnBasedManager.TurnBasedMatchesResponse allMatches)
            {
                if (allMatches.Status() <= ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED))
                {
                    this.callback(null);
                }
                else
                {
                    IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = allMatches.Invitations().GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            GooglePlayGames.Native.PInvoke.MultiplayerInvitation current = enumerator.Current;
                            using (current)
                            {
                                if (current.Id().Equals(this.invitationId))
                                {
                                    this.callback(current);
                                    return;
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
                    this.callback(null);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Finish>c__AnonStorey2D1
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<bool> callback;
            internal byte[] data;
            internal MatchOutcome outcome;

            internal void <>m__118(NativeTurnBasedMatch foundMatch)
            {
                GooglePlayGames.Native.PInvoke.ParticipantResults results = foundMatch.Results();
                foreach (string str in this.outcome.ParticipantIds)
                {
                    Types.MatchResult result = NativeTurnBasedMultiplayerClient.ResultToMatchResult(this.outcome.GetResultFor(str));
                    uint placementFor = this.outcome.GetPlacementFor(str);
                    if (results.HasResultsForParticipant(str))
                    {
                        Types.MatchResult result2 = results.ResultsForParticipant(str);
                        uint num2 = results.PlacingForParticipant(str);
                        if ((result == result2) && (placementFor == num2))
                        {
                            continue;
                        }
                        Logger.e(string.Format("Attempted to override existing results for participant {0}: Placing {1}, Result {2}", str, num2, result2));
                        this.callback(false);
                        return;
                    }
                    GooglePlayGames.Native.PInvoke.ParticipantResults results2 = results;
                    results = results2.WithResult(str, placementFor, result);
                    results2.Dispose();
                }
                this.<>f__this.mTurnBasedManager.FinishMatchDuringMyTurn(foundMatch, this.data, results, delegate (TurnBasedManager.TurnBasedMatchResponse response) {
                    this.callback(response.RequestSucceeded());
                });
            }

            internal void <>m__122(TurnBasedManager.TurnBasedMatchResponse response)
            {
                this.callback(response.RequestSucceeded());
            }
        }

        [CompilerGenerated]
        private sealed class <GetAllInvitations>c__AnonStorey2C6
        {
            internal Action<Invitation[]> callback;

            internal void <>m__10D(TurnBasedManager.TurnBasedMatchesResponse allMatches)
            {
                Invitation[] invitationArray = new Invitation[allMatches.InvitationCount()];
                int num = 0;
                IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = allMatches.Invitations().GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        invitationArray[num++] = enumerator.Current.AsInvitation();
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                this.callback(invitationArray);
            }
        }

        [CompilerGenerated]
        private sealed class <GetAllMatches>c__AnonStorey2C7
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[]> callback;

            internal void <>m__10E(TurnBasedManager.TurnBasedMatchesResponse allMatches)
            {
                int num = (allMatches.MyTurnMatchesCount() + allMatches.TheirTurnMatchesCount()) + allMatches.CompletedMatchesCount();
                GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[] matchArray = new GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[num];
                int num2 = 0;
                IEnumerator<NativeTurnBasedMatch> enumerator = allMatches.MyTurnMatches().GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        matchArray[num2++] = enumerator.Current.AsTurnBasedMatch(this.<>f__this.mNativeClient.GetUserId());
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                IEnumerator<NativeTurnBasedMatch> enumerator2 = allMatches.TheirTurnMatches().GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        matchArray[num2++] = enumerator2.Current.AsTurnBasedMatch(this.<>f__this.mNativeClient.GetUserId());
                    }
                }
                finally
                {
                    if (enumerator2 == null)
                    {
                    }
                    enumerator2.Dispose();
                }
                IEnumerator<NativeTurnBasedMatch> enumerator3 = allMatches.CompletedMatches().GetEnumerator();
                try
                {
                    while (enumerator3.MoveNext())
                    {
                        matchArray[num2++] = enumerator3.Current.AsTurnBasedMatch(this.<>f__this.mNativeClient.GetUserId());
                    }
                }
                finally
                {
                    if (enumerator3 == null)
                    {
                    }
                    enumerator3.Dispose();
                }
                this.callback(matchArray);
            }
        }

        [CompilerGenerated]
        private sealed class <HandleMatchEvent>c__AnonStorey2CD
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> currentDelegate;
            internal NativeTurnBasedMatch match;
            internal bool shouldAutolaunch;

            internal void <>m__114()
            {
                this.currentDelegate(this.match.AsTurnBasedMatch(this.<>f__this.mNativeClient.GetUserId()), this.shouldAutolaunch);
                this.match.ForgetMe();
            }
        }

        [CompilerGenerated]
        private sealed class <Leave>c__AnonStorey2D3
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<bool> callback;

            internal void <>m__11A(NativeTurnBasedMatch foundMatch)
            {
                this.<>f__this.mTurnBasedManager.LeaveMatchDuringTheirTurn(foundMatch, delegate (GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status) {
                    this.callback(status > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));
                });
            }

            internal void <>m__124(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status)
            {
                this.callback(status > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));
            }
        }

        [CompilerGenerated]
        private sealed class <LeaveDuringTurn>c__AnonStorey2D4
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<bool> callback;

            internal void <>m__11B(GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch)
            {
                this.<>f__this.mTurnBasedManager.LeaveDuringMyTurn(foundMatch, pendingParticipant, delegate (GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status) {
                    this.callback(status > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));
                });
            }

            internal void <>m__125(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status)
            {
                this.callback(status > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterMatchDelegate>c__AnonStorey2CC
        {
            internal MatchDelegate del;

            internal void <>m__113(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, bool autoLaunch)
            {
                this.del(match, autoLaunch);
            }
        }

        [CompilerGenerated]
        private sealed class <Rematch>c__AnonStorey2D6
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

            internal void <>m__11D(bool failed)
            {
                this.callback(false, null);
            }

            internal void <>m__11E(NativeTurnBasedMatch foundMatch)
            {
                this.<>f__this.mTurnBasedManager.Rematch(foundMatch, this.<>f__this.BridgeMatchToUserCallback(delegate (GooglePlayGames.BasicApi.UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch m) {
                    this.callback(status == GooglePlayGames.BasicApi.UIStatus.Valid, m);
                }));
            }

            internal void <>m__127(GooglePlayGames.BasicApi.UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch m)
            {
                this.callback(status == GooglePlayGames.BasicApi.UIStatus.Valid, m);
            }
        }

        [CompilerGenerated]
        private sealed class <TakeTurn>c__AnonStorey2CE
        {
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action<bool> callback;
            internal byte[] data;

            internal void <>m__115(GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch)
            {
                this.<>f__this.mTurnBasedManager.TakeTurn(foundMatch, this.data, pendingParticipant, delegate (TurnBasedManager.TurnBasedMatchResponse result) {
                    if (result.RequestSucceeded())
                    {
                        this.callback(true);
                    }
                    else
                    {
                        Logger.d("Taking turn failed: " + result.ResponseStatus());
                        this.callback(false);
                    }
                });
            }

            internal void <>m__121(TurnBasedManager.TurnBasedMatchResponse result)
            {
                if (result.RequestSucceeded())
                {
                    this.callback(true);
                }
                else
                {
                    Logger.d("Taking turn failed: " + result.ResponseStatus());
                    this.callback(false);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <WaitForLogin>c__Iterator26 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Action <$>method;
            internal NativeTurnBasedMultiplayerClient <>f__this;
            internal Action method;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(this.<>f__this.mNativeClient.GetUserId()))
                        {
                            break;
                        }
                        this.$current = null;
                        this.$PC = 1;
                        return true;

                    case 1:
                        break;

                    default:
                        goto Label_0060;
                }
                this.method();
                this.$PC = -1;
            Label_0060:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

