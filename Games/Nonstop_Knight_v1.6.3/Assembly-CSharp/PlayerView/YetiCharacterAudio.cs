namespace PlayerView
{
    using System;

    public class YetiCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_YetiNoise;
        }
    }
}

