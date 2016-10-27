namespace PlayerView
{
    using System;

    public class MummyCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_MummyNoise;
        }
    }
}

