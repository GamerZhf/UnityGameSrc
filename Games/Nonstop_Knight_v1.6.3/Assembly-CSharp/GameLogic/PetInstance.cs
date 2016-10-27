namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using UnityEngine;

    public class PetInstance : IJsonData
    {
        [JsonIgnore]
        public GameLogic.Character Character;
        public string CharacterId = string.Empty;
        public int Duplicates;
        public bool InspectedByPlayer;
        public int Level;
        [JsonIgnore]
        public CharacterInstance SpawnedCharacterInstance;

        public bool canLevelUp()
        {
            return (this.Duplicates >= App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(this.Level + 1));
        }

        public float getNormalizedProgressTowardsLevelUp()
        {
            if (this.isAtMaxLevel())
            {
                return 1f;
            }
            int num = App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(this.Level + 1);
            return Mathf.Clamp01(((float) this.Duplicates) / ((float) num));
        }

        public int getNumLevelUpsPossible()
        {
            int num = 0;
            int duplicates = this.Duplicates;
            int level = this.Level + 1;
            while (true)
            {
                int num4 = App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(level);
                if (duplicates < num4)
                {
                    return num;
                }
                duplicates -= num4;
                level++;
                num++;
            }
        }

        public bool isAtMaxLevel()
        {
            return (this.Level >= ConfigGameplay.PET_MAX_LEVEL);
        }

        public void postDeserializeInitialization()
        {
            this.Character = GameLogic.Binder.CharacterResources.getResource(this.CharacterId);
            Assert.IsTrue_Release(this.Character != null, "Cannot link Character to Pet with id: " + this.CharacterId);
        }
    }
}

