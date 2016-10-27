namespace PlayerView
{
    using System;

    public class CrocodileCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_CrocNoise;
        }
    }
}

