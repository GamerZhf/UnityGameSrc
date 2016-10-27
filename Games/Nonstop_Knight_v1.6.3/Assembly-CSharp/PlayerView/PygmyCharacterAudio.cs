namespace PlayerView
{
    using System;

    public class PygmyCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_PygmyNoise;
        }
    }
}

