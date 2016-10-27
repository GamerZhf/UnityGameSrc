namespace PlayerView
{
    using System;

    public class AnubisCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_AnubisNoise;
        }
    }
}

