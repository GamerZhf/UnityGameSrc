namespace PlayerView
{
    using System;

    public class GoblinCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_GoblinNoise;
        }
    }
}

