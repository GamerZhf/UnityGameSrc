namespace PlayerView
{
    using System;

    public class StumpyCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_PetTreeRustle;
        }
    }
}

