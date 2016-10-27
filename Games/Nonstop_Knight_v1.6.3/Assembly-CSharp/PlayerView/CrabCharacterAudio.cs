namespace PlayerView
{
    using System;

    public class CrabCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_PetCrabVoice;
        }
    }
}

