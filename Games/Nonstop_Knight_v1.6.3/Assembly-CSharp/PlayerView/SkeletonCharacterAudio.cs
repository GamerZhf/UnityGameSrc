namespace PlayerView
{
    using System;

    public class SkeletonCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_SkeletonRattle;
        }
    }
}

