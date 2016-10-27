namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Quests;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class NativeQuest : BaseReferenceHolder, IQuest
    {
        private volatile NativeQuestMilestone mCachedMilestone;

        internal NativeQuest(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            Quest.Quest_Dispose(selfPointer);
        }

        internal static NativeQuest FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeQuest(pointer);
        }

        public override string ToString()
        {
            if (base.IsDisposed())
            {
                return "[NativeQuest: DELETED]";
            }
            object[] args = new object[] { this.Id, this.Name, this.Description, this.BannerUrl, this.IconUrl, this.State, this.StartTime, this.ExpirationTime, this.AcceptedTime };
            return string.Format("[NativeQuest: Id={0}, Name={1}, Description={2}, BannerUrl={3}, IconUrl={4}, State={5}, StartTime={6}, ExpirationTime={7}, AcceptedTime={8}]", args);
        }

        internal bool Valid()
        {
            return Quest.Quest_Valid(base.SelfPtr());
        }

        public DateTime? AcceptedTime
        {
            get
            {
                long millisSinceEpoch = Quest.Quest_AcceptedTime(base.SelfPtr());
                if (millisSinceEpoch == 0)
                {
                    return null;
                }
                return new DateTime?(PInvokeUtilities.FromMillisSinceUnixEpoch(millisSinceEpoch));
            }
        }

        public string BannerUrl
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return Quest.Quest_BannerUrl(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public string Description
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return Quest.Quest_Description(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public DateTime ExpirationTime
        {
            get
            {
                return PInvokeUtilities.FromMillisSinceUnixEpoch(Quest.Quest_ExpirationTime(base.SelfPtr()));
            }
        }

        public string IconUrl
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return Quest.Quest_IconUrl(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public string Id
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return Quest.Quest_Id(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public IQuestMilestone Milestone
        {
            get
            {
                if (this.mCachedMilestone == null)
                {
                    this.mCachedMilestone = NativeQuestMilestone.FromPointer(Quest.Quest_CurrentMilestone(base.SelfPtr()));
                }
                return this.mCachedMilestone;
            }
        }

        public string Name
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return Quest.Quest_Name(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public DateTime StartTime
        {
            get
            {
                return PInvokeUtilities.FromMillisSinceUnixEpoch(Quest.Quest_StartTime(base.SelfPtr()));
            }
        }

        public GooglePlayGames.BasicApi.Quests.QuestState State
        {
            get
            {
                Types.QuestState state = Quest.Quest_State(base.SelfPtr());
                switch (state)
                {
                    case Types.QuestState.UPCOMING:
                        return GooglePlayGames.BasicApi.Quests.QuestState.Upcoming;

                    case Types.QuestState.OPEN:
                        return GooglePlayGames.BasicApi.Quests.QuestState.Open;

                    case Types.QuestState.ACCEPTED:
                        return GooglePlayGames.BasicApi.Quests.QuestState.Accepted;

                    case Types.QuestState.COMPLETED:
                        return GooglePlayGames.BasicApi.Quests.QuestState.Completed;

                    case Types.QuestState.EXPIRED:
                        return GooglePlayGames.BasicApi.Quests.QuestState.Expired;

                    case Types.QuestState.FAILED:
                        return GooglePlayGames.BasicApi.Quests.QuestState.Failed;
                }
                throw new InvalidOperationException("Unknown state: " + state);
            }
        }
    }
}

