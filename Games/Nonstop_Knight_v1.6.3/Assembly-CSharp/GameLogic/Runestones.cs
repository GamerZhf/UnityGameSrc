namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Runestones : IJsonData, ICharacterStatModifier
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public List<RunestoneInstance> RunestoneInstances = new List<RunestoneInstance>();
        public List<string> SelectedRunestoneIds = new List<string>();
        public List<RunestoneSelection> SelectedRunestones = new List<RunestoneSelection>();
        private static List<RunestoneSelection> sm_tempList = new List<RunestoneSelection>(Enum.GetValues(typeof(RunestoneSelectionSource)).Length);

        public float getBaseStatModifier(BaseStatProperty baseStat)
        {
            float num = 0f;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            PerkInstance perkInstance = runestoneData.PerkInstance;
                            num += perkInstance.getBaseStatModifier(baseStat);
                        }
                    }
                }
            }
            return num;
        }

        public float getCharacterTypeArmorModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            PerkInstance perkInstance = runestoneData.PerkInstance;
                            num += perkInstance.getCharacterTypeArmorModifier(characterType);
                        }
                    }
                }
            }
            return num;
        }

        public float getCharacterTypeCoinModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            PerkInstance perkInstance = runestoneData.PerkInstance;
                            num += perkInstance.getCharacterTypeCoinModifier(characterType);
                        }
                    }
                }
            }
            return num;
        }

        public float getCharacterTypeDamageModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            PerkInstance perkInstance = runestoneData.PerkInstance;
                            num += perkInstance.getCharacterTypeDamageModifier(characterType);
                        }
                    }
                }
            }
            return num;
        }

        public float getCharacterTypeXpModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            PerkInstance perkInstance = runestoneData.PerkInstance;
                            num += perkInstance.getCharacterTypeXpModifier(characterType);
                        }
                    }
                }
            }
            return num;
        }

        public float getGenericModifierForPerkType(PerkType perkType)
        {
            float num = 0f;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            num += runestoneData.PerkInstance.getGenericModifierForPerkType(perkType);
                        }
                    }
                }
            }
            return num;
        }

        public int getNumberOfRunestonesAffectingSkill(SkillType skillType)
        {
            if (skillType == SkillType.NONE)
            {
                return 0;
            }
            int num = 0;
            for (int i = 0; i < this.RunestoneInstances.Count; i++)
            {
                if (ConfigRunestones.GetSkillTypeForRunestone(this.RunestoneInstances[i].Id) == skillType)
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfUninspectedRunestones()
        {
            int num = 0;
            for (int i = 0; i < this.RunestoneInstances.Count; i++)
            {
                if (!this.RunestoneInstances[i].InspectedByPlayer)
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfUninspectedRunestonesAffectingSkill(SkillType skillType)
        {
            if (skillType == SkillType.NONE)
            {
                return 0;
            }
            int num = 0;
            for (int i = 0; i < this.RunestoneInstances.Count; i++)
            {
                RunestoneInstance instance = this.RunestoneInstances[i];
                if ((ConfigRunestones.GetSkillTypeForRunestone(instance.Id) == skillType) && !instance.InspectedByPlayer)
                {
                    num++;
                }
            }
            return num;
        }

        public int getPerkInstanceCount(PerkType perkType)
        {
            int num = 0;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            PerkInstance perkInstance = runestoneData.PerkInstance;
                            num += perkInstance.getPerkInstanceCount(perkType);
                        }
                    }
                }
            }
            return num;
        }

        public void getPerkInstancesOfType(PerkType perkType, IBuffIconProvider iconProvider, ref List<KeyValuePair<PerkInstance, BuffSource>> outPerkInstances)
        {
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            runestoneData.PerkInstance.getPerkInstancesOfType(perkType, runestoneData, ref outPerkInstances);
                        }
                    }
                }
            }
        }

        public RunestoneInstance getRunestoneInstance(string runestoneId)
        {
            for (int i = 0; i < this.RunestoneInstances.Count; i++)
            {
                if (this.RunestoneInstances[i].Id == runestoneId)
                {
                    return this.RunestoneInstances[i];
                }
            }
            return null;
        }

        public RunestoneSelection getRunestoneSelection(string runestoneId)
        {
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Id == runestoneId)
                {
                    return this.SelectedRunestones[i];
                }
            }
            return null;
        }

        public List<RunestoneSelection> getRunestoneSelectionsForSkillType(SkillType skillType, [Optional, DefaultParameterValue(0)] RunestoneSelectionSource source)
        {
            sm_tempList.Clear();
            if (skillType != SkillType.NONE)
            {
                for (int i = 0; i < this.SelectedRunestones.Count; i++)
                {
                    RunestoneSelection item = this.SelectedRunestones[i];
                    if (((source == RunestoneSelectionSource.None) || (item.Source == source)) && (skillType == ConfigRunestones.GetSkillTypeForRunestone(item.Id)))
                    {
                        sm_tempList.Add(item);
                    }
                }
            }
            return sm_tempList;
        }

        public RunestoneSelectionSource getRunestoneSelectionSource(string runestoneId)
        {
            RunestoneSelection selection = this.getRunestoneSelection(runestoneId);
            return ((selection == null) ? RunestoneSelectionSource.None : selection.Source);
        }

        public int getRunestoneSkillGroupIndex(string runestoneId)
        {
            ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(runestoneId);
            ConfigPerks.SharedData data2 = ConfigPerks.SHARED_DATA[runestoneData.PerkInstance.Type];
            ConfigSkills.SharedData data3 = ConfigSkills.SHARED_DATA[data2.LinkedToSkill];
            return data3.Group;
        }

        public string getSelectedRunestoneId(SkillType skillType, RunestoneSelectionSource source)
        {
            if (skillType != SkillType.NONE)
            {
                for (int i = 0; i < this.SelectedRunestones.Count; i++)
                {
                    RunestoneSelection selection = this.SelectedRunestones[i];
                    if ((skillType == ConfigRunestones.GetSkillTypeForRunestone(selection.Id)) && (source == selection.Source))
                    {
                        return selection.Id;
                    }
                }
            }
            return null;
        }

        public float getSkillCooldownModifier(SkillType skillType)
        {
            float num = 0f;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            PerkInstance perkInstance = runestoneData.PerkInstance;
                            num += perkInstance.getSkillCooldownModifier(skillType);
                        }
                    }
                }
            }
            return num;
        }

        public float getSkillDamageModifier(SkillType skillType)
        {
            float num = 0f;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            PerkInstance perkInstance = runestoneData.PerkInstance;
                            num += perkInstance.getSkillDamageModifier(skillType);
                        }
                    }
                }
            }
            return num;
        }

        public int getSkillExtraCharges(SkillType skillType)
        {
            int num = 0;
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if (runestoneData.PerkInstance != null)
                        {
                            PerkInstance perkInstance = runestoneData.PerkInstance;
                            num += perkInstance.getSkillExtraCharges(skillType);
                        }
                    }
                }
            }
            return num;
        }

        public bool hasSkillInvulnerability(SkillType skillType)
        {
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Player)
                {
                    string id = this.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    if ((skillTypeForRunestone != SkillType.NONE) && this.Player.ActiveCharacter.isSkillActive(skillTypeForRunestone))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                        if ((runestoneData.PerkInstance != null) && runestoneData.PerkInstance.hasSkillInvulnerability(skillType))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool isBasicRunestoneSelected(SkillType skillType)
        {
            return (this.getSelectedRunestoneId(skillType, RunestoneSelectionSource.Player) == null);
        }

        public bool isRunestoneSelected(string runestoneId)
        {
            return (this.getRunestoneSelection(runestoneId) != null);
        }

        public bool isRunestoneSelected(string runestoneId, RunestoneSelectionSource source)
        {
            for (int i = 0; i < this.SelectedRunestones.Count; i++)
            {
                RunestoneSelection selection = this.SelectedRunestones[i];
                if ((selection.Id == runestoneId) && (selection.Source == source))
                {
                    return true;
                }
            }
            return false;
        }

        public float normalizedPctAllRunestonesOwned()
        {
            return Mathf.Clamp01(((float) this.numRunestonesOwned()) / ((float) ConfigRunestones.RUNESTONES.Length));
        }

        public int numRunestonesOwned()
        {
            return this.RunestoneInstances.Count;
        }

        public bool ownsAllRunestonesWithRarity(int rarity)
        {
            for (int i = 0; i < ConfigRunestones.RUNESTONES.Length; i++)
            {
                ConfigRunestones.SharedData data = ConfigRunestones.RUNESTONES[i];
                if ((data.Rarity == rarity) && !this.ownsRunestone(data.Id))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ownsRunestone(string runestoneId)
        {
            if (this.getRunestoneInstance(runestoneId) != null)
            {
                return true;
            }
            for (int i = 0; i < this.Player.UnclaimedRewards.Count; i++)
            {
                if (this.Player.UnclaimedRewards[i].RunestoneDrops.Contains(runestoneId))
                {
                    return true;
                }
            }
            return this.Player.PendingRankUpRunestoneUnlocks.Contains(runestoneId);
        }

        public void postDeserializeInitialization()
        {
            for (int i = this.RunestoneInstances.Count - 1; i >= 0; i--)
            {
                if (((this.RunestoneInstances[i] == null) || string.IsNullOrEmpty(this.RunestoneInstances[i].Id)) || (ConfigRunestones.GetRunestoneData(this.RunestoneInstances[i].Id) == null))
                {
                    this.RunestoneInstances.RemoveAt(i);
                }
            }
            IEnumerator enumerator = Enum.GetValues(typeof(RunestoneSelectionSource)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    RunestoneSelectionSource current = (RunestoneSelectionSource) ((int) enumerator.Current);
                    if (current != RunestoneSelectionSource.None)
                    {
                        for (int m = 0; m < ConfigSkills.ACTIVE_HERO_SKILLS.Count; m++)
                        {
                            bool flag = false;
                            SkillType type = ConfigSkills.ACTIVE_HERO_SKILLS[m];
                            for (int n = this.SelectedRunestones.Count - 1; n >= 0; n--)
                            {
                                RunestoneSelection selection = this.SelectedRunestones[n];
                                if ((type == ConfigRunestones.GetSkillTypeForRunestone(selection.Id)) && (current == selection.Source))
                                {
                                    if (flag)
                                    {
                                        this.SelectedRunestones.RemoveAt(n);
                                    }
                                    flag = true;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            this.refreshRunestoneSelections();
            for (int j = 0; j < this.RunestoneInstances.Count; j++)
            {
                this.RunestoneInstances[j].postDeserializeInitialization();
            }
            for (int k = 0; k < this.SelectedRunestones.Count; k++)
            {
                this.SelectedRunestones[k].postDeserializeInitialization();
            }
        }

        public void refreshRunestoneSelections()
        {
            PetInstance instance = this.Player.Pets.getSelectedPetInstance();
            if (instance == null)
            {
                for (int i = this.SelectedRunestones.Count - 1; i >= 0; i--)
                {
                    if (this.SelectedRunestones[i].Source == RunestoneSelectionSource.Pet)
                    {
                        this.SelectedRunestones.RemoveAt(i);
                    }
                }
            }
            else
            {
                GatedPerkContainer fixedPerks = GameLogic.Binder.CharacterResources.getResource(instance.CharacterId).FixedPerks;
                for (int j = 0; j < ConfigSkills.ACTIVE_HERO_SKILLS.Count; j++)
                {
                    SkillType skillType = ConfigSkills.ACTIVE_HERO_SKILLS[j];
                    string runestoneId = this.getSelectedRunestoneId(skillType, RunestoneSelectionSource.Pet);
                    string linkedToRunestone = null;
                    for (int m = 0; m < fixedPerks.Entries.Count; m++)
                    {
                        if (fixedPerks.isPerkUnlocked(instance.Level, m))
                        {
                            PerkType type = fixedPerks.getPerkInstanceAtIndex(m).Type;
                            ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[type];
                            if ((data.LinkedToSkill == skillType) && (data.LinkedToRunestone != null))
                            {
                                linkedToRunestone = data.LinkedToRunestone;
                                break;
                            }
                        }
                    }
                    if (runestoneId != linkedToRunestone)
                    {
                        this.SelectedRunestones.Remove(this.getRunestoneSelection(runestoneId));
                        if (linkedToRunestone != null)
                        {
                            RunestoneSelection item = new RunestoneSelection();
                            item.Id = linkedToRunestone;
                            item.Source = RunestoneSelectionSource.Pet;
                            this.SelectedRunestones.Add(item);
                        }
                    }
                }
                for (int k = 0; k < ConfigSkills.ACTIVE_HERO_SKILLS.Count; k++)
                {
                    SkillType type3 = ConfigSkills.ACTIVE_HERO_SKILLS[k];
                    string str3 = this.getSelectedRunestoneId(type3, RunestoneSelectionSource.Player);
                    if (str3 != null)
                    {
                        string str4 = this.getSelectedRunestoneId(type3, RunestoneSelectionSource.Pet);
                        if (str3 == str4)
                        {
                            RunestoneSelection selection = this.getRunestoneSelection(str3);
                            int runestoneOrderNumberForSkillType = ConfigRunestones.GetRunestoneOrderNumberForSkillType(str3, type3);
                            selection.Id = ConfigRunestones.GetRunestoneId(type3, runestoneOrderNumberForSkillType + 1);
                        }
                    }
                }
            }
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

