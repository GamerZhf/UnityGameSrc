namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class TurnBasedMatchConfig : BaseReferenceHolder
    {
        internal TurnBasedMatchConfig(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_Dispose(selfPointer);
        }

        internal long ExclusiveBitMask()
        {
            return GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_ExclusiveBitMask(base.SelfPtr());
        }

        internal uint MaximumAutomatchingPlayers()
        {
            return GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_MaximumAutomatchingPlayers(base.SelfPtr());
        }

        internal uint MinimumAutomatchingPlayers()
        {
            return GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_MinimumAutomatchingPlayers(base.SelfPtr());
        }

        private string PlayerIdAtIndex(UIntPtr index)
        {
            <PlayerIdAtIndex>c__AnonStorey2E4 storeye = new <PlayerIdAtIndex>c__AnonStorey2E4();
            storeye.index = index;
            storeye.<>f__this = this;
            return PInvokeUtilities.OutParamsToString(new PInvokeUtilities.OutStringMethod(storeye.<>m__17B));
        }

        internal IEnumerator<string> PlayerIdsToInvite()
        {
            return PInvokeUtilities.ToEnumerator<string>(GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_Length(base.SelfPtr()), new Func<UIntPtr, string>(this.PlayerIdAtIndex));
        }

        internal uint Variant()
        {
            return GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_Variant(base.SelfPtr());
        }

        [CompilerGenerated]
        private sealed class <PlayerIdAtIndex>c__AnonStorey2E4
        {
            internal GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig <>f__this;
            internal UIntPtr index;

            internal UIntPtr <>m__17B(StringBuilder out_string, UIntPtr size)
            {
                return GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_GetElement(this.<>f__this.SelfPtr(), this.index, out_string, size);
            }
        }
    }
}

