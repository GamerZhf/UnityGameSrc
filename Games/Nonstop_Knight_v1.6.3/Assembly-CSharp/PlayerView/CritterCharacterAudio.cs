namespace PlayerView
{
    using System;

    public class CritterCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_CritterVoice;
        }
    }
}

