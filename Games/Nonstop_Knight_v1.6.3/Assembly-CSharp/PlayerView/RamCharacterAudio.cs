namespace PlayerView
{
    using System;

    public class RamCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_PetRamVoice;
        }
    }
}

