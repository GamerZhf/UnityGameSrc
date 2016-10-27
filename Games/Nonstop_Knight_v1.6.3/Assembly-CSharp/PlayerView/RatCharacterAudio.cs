namespace PlayerView
{
    using System;

    public class RatCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_RatNoise;
        }
    }
}

