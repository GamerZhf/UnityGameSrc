namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Pets : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public List<PetInstance> Instances = new List<PetInstance>();
        public int SelectedPet = -1;

        public bool doNotify()
        {
            for (int i = 0; i < this.Instances.Count; i++)
            {
                PetInstance instance = this.Instances[i];
                if ((!instance.InspectedByPlayer && (instance.Level > 0)) && App.Binder.ConfigMeta.IsActivePetId(instance.CharacterId))
                {
                    return true;
                }
            }
            return false;
        }

        public PetInstance getFirstLevelUppablePet()
        {
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (this.Instances[i].canLevelUp())
                {
                    return this.Instances[i];
                }
            }
            return null;
        }

        public int getNumLevelUppablePets()
        {
            int num = 0;
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (this.Instances[i].canLevelUp())
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumPetsOwned()
        {
            return this.Instances.Count;
        }

        public int getPetIndex(PetInstance pet)
        {
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (this.Instances[i] == pet)
                {
                    return i;
                }
            }
            return -1;
        }

        public PetInstance getPetInstance(Character character)
        {
            if (character == null)
            {
                return null;
            }
            return this.getPetInstance(character.Id);
        }

        public PetInstance getPetInstance(string characterId)
        {
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (this.Instances[i].CharacterId == characterId)
                {
                    return this.Instances[i];
                }
            }
            return null;
        }

        public PetInstance getSelectedPetInstance()
        {
            if (this.SelectedPet < 0)
            {
                return null;
            }
            return this.Instances[this.SelectedPet];
        }

        public int getTotalNumOfLevelUps()
        {
            int num = 0;
            for (int i = 0; i < this.Instances.Count; i++)
            {
                num += this.Instances[i].getNumLevelUpsPossible();
            }
            return num;
        }

        public bool isPetSelected()
        {
            return (this.SelectedPet > -1);
        }

        public bool isPetSelected(Character character)
        {
            if (character == null)
            {
                return false;
            }
            return this.isPetSelected(character.Id);
        }

        public bool isPetSelected(string characterId)
        {
            return ((this.SelectedPet != -1) && (this.Instances[this.SelectedPet].CharacterId == characterId));
        }

        public bool ownsPet(string characterId)
        {
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (this.Instances[i].CharacterId == characterId)
                {
                    return true;
                }
            }
            return false;
        }

        public void postDeserializeInitialization()
        {
            for (int i = 0; i < this.Instances.Count; i++)
            {
                this.Instances[i].postDeserializeInitialization();
            }
            this.SelectedPet = Mathf.Clamp(this.SelectedPet, -1, this.Instances.Count - 1);
        }

        [JsonIgnore]
        public GameLogic.Player Player
        {
            [CompilerGenerated]
            get
            {
                return this.<Player>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Player>k__BackingField = value;
            }
        }
    }
}

