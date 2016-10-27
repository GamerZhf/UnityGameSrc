namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class MultiplayerParticipant
    {
        [DllImport("gpg")]
        internal static extern UIntPtr MultiplayerParticipant_AvatarUrl(HandleRef self, Types.ImageResolution resolution, StringBuilder out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern UIntPtr MultiplayerParticipant_DisplayName(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern void MultiplayerParticipant_Dispose(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool MultiplayerParticipant_HasMatchResult(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool MultiplayerParticipant_HasPlayer(HandleRef self);
        [DllImport("gpg")]
        internal static extern UIntPtr MultiplayerParticipant_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool MultiplayerParticipant_IsConnectedToRoom(HandleRef self);
        [DllImport("gpg")]
        internal static extern uint MultiplayerParticipant_MatchRank(HandleRef self);
        [DllImport("gpg")]
        internal static extern Types.MatchResult MultiplayerParticipant_MatchResult(HandleRef self);
        [DllImport("gpg")]
        internal static extern IntPtr MultiplayerParticipant_Player(HandleRef self);
        [DllImport("gpg")]
        internal static extern Types.ParticipantStatus MultiplayerParticipant_Status(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool MultiplayerParticipant_Valid(HandleRef self);
    }
}

