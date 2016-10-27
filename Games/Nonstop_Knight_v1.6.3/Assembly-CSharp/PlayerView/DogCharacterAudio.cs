namespace PlayerView
{
    using System;

    public class DogCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_PetDogBark;
        }
    }
}

