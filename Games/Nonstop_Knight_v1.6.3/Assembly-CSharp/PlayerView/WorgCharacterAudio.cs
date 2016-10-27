namespace PlayerView
{
    using System;
    using System.Collections.Generic;

    public class WorgCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            List<AudioSourceType> list = new List<AudioSourceType>();
            list.Add(AudioSourceType.SfxGameplay_WorgNoise1);
            list.Add(AudioSourceType.SfxGameplay_WorgNoise2);
            base.AggroAudioSourceTypes = list;
            base.AggroAudioGroupType = AudioGroupType.UNSPECIFIED;
        }
    }
}

