namespace App
{
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections.Generic;

    public static class ConfigBoosts
    {
        public static List<BoostType> ALL_BOOSTS = LangUtil.GetEnumValuesWithException<BoostType>(BoostType.UNSPECIFIED);
        public static Dictionary<BoostType, SharedData> SHARED_DATA;

        static ConfigBoosts()
        {
            Dictionary<BoostType, SharedData> dictionary = new Dictionary<BoostType, SharedData>(new BoostTypeBoxAvoidanceComparer());
            SharedData data = new SharedData();
            data.Name = "Midas";
            data.DurationSeconds = 60f;
            data.Sprite = new SpriteAtlasEntry("Menu", "sprite_skill017");
            data.EffectType = EffectType.Boost_Midas;
            data.Modifier = 3f;
            dictionary.Add(BoostType.Midas, data);
            data = new SharedData();
            data.Name = "Shield";
            data.DurationSeconds = 180f;
            data.Sprite = new SpriteAtlasEntry("Menu", "sprite_skill005");
            data.EffectType = EffectType.Boost_Shield;
            data.BaseStat1 = BaseStatProperty.UniversalArmorBonus;
            data.Modifier = 1f;
            dictionary.Add(BoostType.Shield, data);
            data = new SharedData();
            data.Name = "Xp";
            data.DurationSeconds = 300f;
            data.Sprite = new SpriteAtlasEntry("Menu", "uiz_sprite_rankup");
            data.EffectType = EffectType.Boost_Xp;
            data.BaseStat1 = BaseStatProperty.UniversalXpBonus;
            data.Modifier = 1f;
            dictionary.Add(BoostType.Xp, data);
            data = new SharedData();
            data.Name = "Damage";
            data.DurationSeconds = 180f;
            data.Sprite = new SpriteAtlasEntry("Menu", "sprite_icon_frenzy");
            data.EffectType = EffectType.Boost_Damage;
            data.BaseStat1 = BaseStatProperty.DamagePerHit;
            data.BaseStat2 = BaseStatProperty.SkillDamage;
            data.Modifier = 0.25f;
            dictionary.Add(BoostType.Damage, data);
            data = new SharedData();
            data.Name = "Speed";
            data.DurationSeconds = 180f;
            data.Sprite = new SpriteAtlasEntry("Menu", "uiz_sprite_ascend");
            data.EffectType = EffectType.Boost_Speed;
            data.BaseStat1 = BaseStatProperty.MovementSpeed;
            data.BaseStat2 = BaseStatProperty.AttacksPerSecond;
            data.Modifier = 1f;
            dictionary.Add(BoostType.Speed, data);
            SHARED_DATA = dictionary;
        }

        public class SharedData
        {
            public BaseStatProperty BaseStat1;
            public BaseStatProperty BaseStat2;
            public float DurationSeconds;
            public PlayerView.EffectType EffectType;
            public float Modifier;
            public string Name = "CHANGE ME";
            public SpriteAtlasEntry Sprite;
        }
    }
}

