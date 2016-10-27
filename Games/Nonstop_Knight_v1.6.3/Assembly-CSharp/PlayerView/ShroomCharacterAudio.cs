namespace PlayerView
{
    using System;

    public class ShroomCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_ShroomNoise;
        }
    }
}

