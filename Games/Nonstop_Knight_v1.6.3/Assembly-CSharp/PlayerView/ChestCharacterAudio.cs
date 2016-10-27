namespace PlayerView
{
    using System;

    public class ChestCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_PetChesterVoice;
        }
    }
}

