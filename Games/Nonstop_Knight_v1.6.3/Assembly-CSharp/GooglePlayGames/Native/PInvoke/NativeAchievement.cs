namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeAchievement : BaseReferenceHolder
    {
        private const ulong MinusOne = ulong.MaxValue;

        internal NativeAchievement(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal GooglePlayGames.BasicApi.Achievement AsAchievement()
        {
            GooglePlayGames.BasicApi.Achievement achievement = new GooglePlayGames.BasicApi.Achievement();
            achievement.Id = this.Id();
            achievement.Name = this.Name();
            achievement.Description = this.Description();
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            ulong num = this.LastModifiedTime();
            if (num == ulong.MaxValue)
            {
                num = 0L;
            }
            achievement.LastModifiedTime = time.AddMilliseconds((double) num);
            achievement.Points = this.getXP();
            achievement.RevealedImageUrl = this.getRevealedImageUrl();
            achievement.UnlockedImageUrl = this.getUnlockedImageUrl();
            if (this.Type() == Types.AchievementType.INCREMENTAL)
            {
                achievement.IsIncremental = true;
                achievement.CurrentSteps = (int) this.CurrentSteps();
                achievement.TotalSteps = (int) this.TotalSteps();
            }
            achievement.IsRevealed = (this.State() == Types.AchievementState.REVEALED) || (this.State() == Types.AchievementState.UNLOCKED);
            achievement.IsUnlocked = this.State() == Types.AchievementState.UNLOCKED;
            return achievement;
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Dispose(selfPointer);
        }

        internal uint CurrentSteps()
        {
            return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_CurrentSteps(base.SelfPtr());
        }

        internal string Description()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Description(base.SelfPtr(), out_string, out_size);
            });
        }

        internal string getRevealedImageUrl()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_RevealedIconUrl(base.SelfPtr(), out_string, out_size);
            });
        }

        internal string getUnlockedImageUrl()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_UnlockedIconUrl(base.SelfPtr(), out_string, out_size);
            });
        }

        internal ulong getXP()
        {
            return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_XP(base.SelfPtr());
        }

        internal string Id()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Id(base.SelfPtr(), out_string, out_size);
            });
        }

        internal ulong LastModifiedTime()
        {
            if (GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Valid(base.SelfPtr()))
            {
                return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_LastModifiedTime(base.SelfPtr());
            }
            return 0L;
        }

        internal string Name()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Name(base.SelfPtr(), out_string, out_size);
            });
        }

        internal Types.AchievementState State()
        {
            return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_State(base.SelfPtr());
        }

        internal uint TotalSteps()
        {
            return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_TotalSteps(base.SelfPtr());
        }

        internal Types.AchievementType Type()
        {
            return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Type(base.SelfPtr());
        }
    }
}

