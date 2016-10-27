namespace PlayerView
{
    using System;

    public class MooseCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_PetMooseVoice;
        }
    }
}

