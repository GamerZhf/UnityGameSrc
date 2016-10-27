namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class BuffSystem : MonoBehaviour, IBuffSystem
    {
        private List<Buff> m_allBuffs = new List<Buff>(0x80);
        private Dictionary<CharacterInstance, List<Buff>> m_characterBuffLists = new Dictionary<CharacterInstance, List<Buff>>();
        private static List<CharacterInstance> sm_tempCandidateList = new List<CharacterInstance>();

        protected void Awake()
        {
        }

        public void endBuff(Buff buff)
        {
            GameLogic.Binder.EventBus.BuffPreEnd(buff.Character, buff);
            CharacterInstance character = buff.Character;
            for (int i = this.m_characterBuffLists[character].Count - 1; i >= 0; i--)
            {
                if (this.m_characterBuffLists[character][i] == buff)
                {
                    this.m_characterBuffLists[character].Remove(buff);
                }
            }
            for (int j = this.m_allBuffs.Count - 1; j >= 0; j--)
            {
                if (this.m_allBuffs[j] == buff)
                {
                    this.m_allBuffs.Remove(buff);
                }
            }
            if (buff.Stuns)
            {
                bool flag = false;
                for (int k = this.m_characterBuffLists[character].Count - 1; k >= 0; k--)
                {
                    if (this.m_characterBuffLists[character][k].Stuns)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    CmdSetStunCondition.ExecuteStatic(character, false);
                }
            }
            if (buff.Charms)
            {
                bool flag2 = false;
                for (int m = this.m_characterBuffLists[character].Count - 1; m >= 0; m--)
                {
                    if (this.m_characterBuffLists[character][m].Charms)
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (!flag2)
                {
                    CmdSetCharmCondition.ExecuteStatic(character, false);
                }
            }
            if (buff.Confuses)
            {
                bool flag3 = false;
                for (int n = this.m_characterBuffLists[character].Count - 1; n >= 0; n--)
                {
                    if (this.m_characterBuffLists[character][n].Confuses)
                    {
                        flag3 = true;
                        break;
                    }
                }
                if (!flag3)
                {
                    CmdSetConfusedCondition.ExecuteStatic(character, false);
                }
            }
            buff.Ended = true;
            GameLogic.Binder.EventBus.BuffEnded(buff.Character, buff);
        }

        public void endBuffsForCharacter(CharacterInstance c)
        {
            for (int i = this.m_characterBuffLists[c].Count - 1; i >= 0; i--)
            {
                Buff buff = this.m_characterBuffLists[c][i];
                this.endBuff(buff);
            }
        }

        public void endBuffsFromCharacter(CharacterInstance c)
        {
            for (int i = this.m_allBuffs.Count - 1; i >= 0; i--)
            {
                Buff buff = this.m_allBuffs[i];
                if (buff.SourceCharacter == c)
                {
                    this.endBuff(buff);
                }
            }
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                this.refreshAuraBuffs();
                this.refreshBuildupBuffs();
                this.refreshTickBuffs();
                for (int i = this.m_allBuffs.Count - 1; i >= 0; i--)
                {
                    Buff buff = this.m_allBuffs[i];
                    buff.TimeRemaining = Mathf.Max((float) (buff.TimeRemaining - (Time.deltaTime * Time.timeScale)), (float) 0f);
                    if (buff.TimeRemaining <= 0f)
                    {
                        this.endBuff(buff);
                    }
                }
                if (GameLogic.Binder.FrenzySystem.isFrenzyActive() || activeDungeon.hasDungeonModifier(DungeonModifierType.HeroMaxSpeed))
                {
                    CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
                    for (int j = GameLogic.Binder.BuffSystem.getNumberOfBuffsWithId(primaryPlayerCharacter, ConfigGameplay.SPURT_BUFF_ID); j <= ConfigGameplay.SPURTING_MAX_NUM_BUFFS; j++)
                    {
                        this.grantSpurtBuff(primaryPlayerCharacter);
                    }
                }
            }
        }

        public float getBaseStatModifierSumFromActiveBuffs(CharacterInstance c, BaseStatProperty baseStat)
        {
            float a = 0f;
            List<Buff> list = this.m_characterBuffLists[c];
            for (int i = 0; i < list.Count; i++)
            {
                Buff buff = list[i];
                if (!buff.Ended && ((buff.BaseStat1 == baseStat) || (buff.BaseStat2 == baseStat)))
                {
                    a += buff.TotalModifier;
                }
            }
            if (baseStat == BaseStatProperty.MovementSpeed)
            {
                return Mathf.Max(a, ConfigGameplay.BUFFS_MOVEMENT_DEBUFF_TOTAL_MODIFIER_CAP);
            }
            if (baseStat == BaseStatProperty.AttacksPerSecond)
            {
                a = Mathf.Max(a, ConfigGameplay.BUFFS_ATTACKS_PER_SECOND_DEBUFF_TOTAL_MODIFIER_CAP);
            }
            return a;
        }

        public Buff getBuffFromBoost(CharacterInstance c, BoostType boostType)
        {
            List<Buff> list = this.m_characterBuffLists[c];
            for (int i = 0; i < list.Count; i++)
            {
                Buff buff = list[i];
                if (buff.FromBoost == boostType)
                {
                    return buff;
                }
            }
            return null;
        }

        public Buff getBuffFromPerk(CharacterInstance c, PerkType perkType)
        {
            List<Buff> list = this.m_characterBuffLists[c];
            for (int i = 0; i < list.Count; i++)
            {
                Buff buff = list[i];
                if (buff.FromPerk == perkType)
                {
                    return buff;
                }
            }
            return null;
        }

        public Buff getBuffFromSource(CharacterInstance c, BuffSource source)
        {
            List<Buff> list = this.m_characterBuffLists[c];
            for (int i = 0; i < list.Count; i++)
            {
                Buff buff = list[i];
                if (buff.Source.Object == source.Object)
                {
                    return buff;
                }
            }
            return null;
        }

        public int getNumberOfBuffsFromSource(CharacterInstance c, BuffSource source)
        {
            if (source.Object == null)
            {
                return 0;
            }
            int num = 0;
            List<Buff> list = this.m_characterBuffLists[c];
            for (int i = 0; i < list.Count; i++)
            {
                Buff buff = list[i];
                if (buff.Source.Object == source.Object)
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfBuffsWithId(CharacterInstance c, string id)
        {
            int num = 0;
            List<Buff> list = this.m_characterBuffLists[c];
            for (int i = 0; i < list.Count; i++)
            {
                Buff buff = list[i];
                if (!buff.Ended && (buff.Id == id))
                {
                    num++;
                }
            }
            return num;
        }

        public float getSkillCooldownModifierSumFromActiveBuffs(CharacterInstance c, SkillType skillType)
        {
            List<Buff> list = this.m_characterBuffLists[c];
            float num = 0f;
            for (int i = 0; i < list.Count; i++)
            {
                Buff buff = list[i];
                if (!buff.Ended)
                {
                    num += buff.AllSkillsCooldownBonus;
                }
            }
            return num;
        }

        public float getTargetViewScaleForCharacter(CharacterInstance c)
        {
            float num = 0f;
            int num2 = 0;
            for (int i = 0; i < this.m_characterBuffLists[c].Count; i++)
            {
                Buff buff = this.m_characterBuffLists[c][i];
                if (buff.ViewScaleModifier != 0f)
                {
                    num += buff.ViewScaleModifier;
                    num2++;
                }
            }
            return ((num2 != 0) ? (1f + (num / ((float) num2))) : 1f);
        }

        public float getTotalBuffModifierFromSource(CharacterInstance c, BuffSource source)
        {
            if (source.Object == null)
            {
                return 0f;
            }
            float num = 0f;
            List<Buff> list = this.m_characterBuffLists[c];
            for (int i = 0; i < list.Count; i++)
            {
                Buff buff = list[i];
                if (buff.Source.Object == source.Object)
                {
                    num += buff.TotalModifier;
                }
            }
            return num;
        }

        public void grantSpurtBuff(CharacterInstance c)
        {
            if ((ConfigGameplay.SPURTING_ENABLED && !GameLogic.Binder.GameState.ActiveDungeon.isTutorialDungeon()) && (this.getNumberOfBuffsWithId(c, ConfigGameplay.SPURT_BUFF_ID) < ConfigGameplay.SPURTING_MAX_NUM_BUFFS))
            {
                Buff buff2 = new Buff();
                buff2.Id = ConfigGameplay.SPURT_BUFF_ID;
                buff2.BaseStat1 = BaseStatProperty.MovementSpeed;
                buff2.DurationSeconds = ConfigGameplay.SPURTING_BUFF_DURATION_SECONDS;
                buff2.Modifier = ConfigGameplay.SPURTING_BUFF_MOVEMENT_BONUS;
                Buff buff = buff2;
                this.startBuff(c, buff);
                if (c.IsPrimaryPlayerCharacter)
                {
                    PetInstance instance = c.OwningPlayer.Pets.getSelectedPetInstance();
                    if ((instance != null) && (instance.SpawnedCharacterInstance != null))
                    {
                        buff2 = new Buff();
                        buff2.BaseStat1 = BaseStatProperty.MovementSpeed;
                        buff2.DurationSeconds = ConfigGameplay.SPURTING_BUFF_DURATION_SECONDS;
                        buff2.Modifier = ConfigGameplay.SPURTING_BUFF_MOVEMENT_BONUS;
                        buff = buff2;
                        this.startBuff(instance.SpawnedCharacterInstance, buff);
                    }
                }
            }
        }

        public bool hasBuffFromBoost(CharacterInstance c, BoostType boostType)
        {
            return (this.getBuffFromBoost(c, boostType) != null);
        }

        public bool hasBuffFromPerk(CharacterInstance c, PerkType perkType)
        {
            return (this.getBuffFromPerk(c, perkType) != null);
        }

        private void onCharacterKilled(CharacterInstance c, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            this.endBuffsForCharacter(c);
            if (c.IsBoss || (c.IsSupport && (c.Prefab == CharacterPrefab.KnightClone)))
            {
                this.endBuffsFromCharacter(c);
            }
            if ((killer != null) && killer.IsPrimaryPlayerCharacter)
            {
                this.grantSpurtBuff(killer);
            }
        }

        private void onCharacterPreDestroyed(CharacterInstance c)
        {
            this.endBuffsForCharacter(c);
            this.m_characterBuffLists.Remove(c);
        }

        private void onCharacterSpawnStarted(CharacterInstance character)
        {
            this.m_characterBuffLists.Add(character, new List<Buff>());
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnCharacterSpawnStarted -= new GameLogic.Events.CharacterSpawnStarted(this.onCharacterSpawnStarted);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterPreDestroyed -= new GameLogic.Events.CharacterPreDestroyed(this.onCharacterPreDestroyed);
            GameLogic.Binder.EventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            GameLogic.Binder.EventBus.OnMultikillBonusGranted -= new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            GameLogic.Binder.EventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            GameLogic.Binder.EventBus.OnItemEquipped -= new GameLogic.Events.ItemEquipped(this.onItemEquipped);
            GameLogic.Binder.EventBus.OnDungeonBoostActivated -= new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
        }

        private void onDungeonBoostActivated(DungeonBoost dungeonBoost, SkillType fromSkill)
        {
            Buff buff4;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (activeDungeon.hasDungeonModifier(DungeonModifierType.DungeonBoostBoxBonusSkillDamage))
            {
                buff4 = new Buff();
                buff4.BaseStat1 = BaseStatProperty.SkillDamage;
                buff4.Modifier = ConfigDungeonModifiers.DungeonBoostBoxBonusSkillDamage.SkillDamageModifier;
                buff4.DurationSeconds = ConfigDungeonModifiers.DungeonBoostBoxBonusSkillDamage.BuffDurationSeconds;
                Buff buff = buff4;
                this.startBuff(activeDungeon.PrimaryPlayerCharacter, buff);
            }
            if (activeDungeon.hasDungeonModifier(DungeonModifierType.DungeonBoostBoxBonusWeaponDamage))
            {
                buff4 = new Buff();
                buff4.BaseStat1 = BaseStatProperty.DamagePerHit;
                buff4.Modifier = ConfigDungeonModifiers.DungeonBoostBoxBonusWeaponDamage.DamagePerHitModifier;
                buff4.DurationSeconds = ConfigDungeonModifiers.DungeonBoostBoxBonusWeaponDamage.BuffDurationSeconds;
                Buff buff2 = buff4;
                this.startBuff(activeDungeon.PrimaryPlayerCharacter, buff2);
            }
            if (activeDungeon.hasDungeonModifier(DungeonModifierType.DungeonBoostBoxBonusUniversalDamage))
            {
                buff4 = new Buff();
                buff4.BaseStat1 = BaseStatProperty.DamagePerHit;
                buff4.BaseStat2 = BaseStatProperty.SkillDamage;
                buff4.Modifier = ConfigDungeonModifiers.DungeonBoostBoxBonusUniversalDamage.Modifier;
                buff4.DurationSeconds = ConfigDungeonModifiers.DungeonBoostBoxBonusUniversalDamage.BuffDurationSeconds;
                Buff buff3 = buff4;
                this.startBuff(activeDungeon.PrimaryPlayerCharacter, buff3);
            }
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterSpawnStarted += new GameLogic.Events.CharacterSpawnStarted(this.onCharacterSpawnStarted);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterPreDestroyed += new GameLogic.Events.CharacterPreDestroyed(this.onCharacterPreDestroyed);
            GameLogic.Binder.EventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            GameLogic.Binder.EventBus.OnMultikillBonusGranted += new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            GameLogic.Binder.EventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            GameLogic.Binder.EventBus.OnItemEquipped += new GameLogic.Events.ItemEquipped(this.onItemEquipped);
            GameLogic.Binder.EventBus.OnDungeonBoostActivated += new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
        }

        private void onItemEquipped(CharacterInstance character, ItemInstance itemInstance, ItemInstance replacedItemInstance)
        {
            if (replacedItemInstance != null)
            {
                List<Buff> list = this.m_characterBuffLists[character];
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    Buff buff = list[i];
                    if (buff.Source.IconProvider == replacedItemInstance.Item)
                    {
                        this.endBuff(buff);
                    }
                }
            }
        }

        private void onMultikillBonusGranted(Player player, int killCount, double coinAmount)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.MultikillShield);
            for (int i = 0; i < perkInstancesOfType.Count; i++)
            {
                KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[i];
                KeyValuePair<PerkInstance, BuffSource> pair2 = perkInstancesOfType[i];
                this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.MultikillShield, ConfigPerks.SHARED_DATA[PerkType.MultikillShield].DurationSeconds, (double) pair.Key.Modifier, pair2.Value, null);
            }
        }

        private void onPlayerRetired(Player player, int retirementFloor)
        {
            this.endBuffsForCharacter(player.ActiveCharacter);
        }

        private void refreshAuraBuffs()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Room activeRoom = activeDungeon.ActiveRoom;
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AuraIce);
            if (perkInstancesOfType.Count > 0)
            {
                List<CharacterInstance> list2 = activeDungeon.ActiveRoom.getEnemyCharactersWithinRadius(activeCharacter.PhysicsBody.Transform.position, ConfigPerks.AuraIce.AuraRadius, activeCharacter);
                for (int num2 = 0; num2 < list2.Count; num2++)
                {
                    CharacterInstance c = list2[num2];
                    for (int num3 = 0; num3 < perkInstancesOfType.Count; num3++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[num3];
                        KeyValuePair<PerkInstance, BuffSource> pair2 = perkInstancesOfType[num3];
                        this.startOrRefreshBuffFromPerk(c, PerkType.AuraIce, 1f, (double) pair.Key.Modifier, pair2.Value, null);
                    }
                }
            }
            perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AuraDamageBonus);
            if (perkInstancesOfType.Count > 0)
            {
                List<CharacterInstance> list3 = activeDungeon.ActiveRoom.getEnemyCharactersWithinRadius(activeCharacter.PhysicsBody.Transform.position, ConfigPerks.AuraDamageBonus.AuraRadius, activeCharacter);
                if (list3.Count > 0)
                {
                    for (int num4 = 0; num4 < perkInstancesOfType.Count; num4++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair3 = perkInstancesOfType[num4];
                        KeyValuePair<PerkInstance, BuffSource> pair4 = perkInstancesOfType[num4];
                        this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.AuraDamageBonus, 1f, (double) (pair3.Key.Modifier * list3.Count), pair4.Value, null);
                    }
                }
            }
            if (activeCharacter.CurrentHpNormalized <= ConfigPerks.SHARED_DATA[PerkType.AuraLowHpArmor].Threshold)
            {
                perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AuraLowHpArmor);
                for (int num5 = 0; num5 < perkInstancesOfType.Count; num5++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair5 = perkInstancesOfType[num5];
                    KeyValuePair<PerkInstance, BuffSource> pair6 = perkInstancesOfType[num5];
                    this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.AuraLowHpArmor, 1f, (double) pair5.Key.Modifier, pair6.Value, null);
                }
            }
            if (activeCharacter.CurrentHpNormalized <= ConfigPerks.SHARED_DATA[PerkType.AuraLowHpAttackSpeed].Threshold)
            {
                perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AuraLowHpAttackSpeed);
                for (int num6 = 0; num6 < perkInstancesOfType.Count; num6++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair7 = perkInstancesOfType[num6];
                    KeyValuePair<PerkInstance, BuffSource> pair8 = perkInstancesOfType[num6];
                    this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.AuraLowHpAttackSpeed, 1f, (double) pair7.Key.Modifier, pair8.Value, null);
                }
            }
            if (activeCharacter.CurrentHpNormalized <= ConfigPerks.SHARED_DATA[PerkType.AuraLowHpDamage].Threshold)
            {
                perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AuraLowHpDamage);
                for (int num7 = 0; num7 < perkInstancesOfType.Count; num7++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair9 = perkInstancesOfType[num7];
                    KeyValuePair<PerkInstance, BuffSource> pair10 = perkInstancesOfType[num7];
                    this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.AuraLowHpDamage, 1f, (double) pair9.Key.Modifier, pair10.Value, null);
                }
            }
            if (activeCharacter.CurrentHpNormalized <= ConfigPerks.SHARED_DATA[PerkType.AuraLowHpDodge].Threshold)
            {
                perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AuraLowHpDodge);
                for (int num8 = 0; num8 < perkInstancesOfType.Count; num8++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair11 = perkInstancesOfType[num8];
                    KeyValuePair<PerkInstance, BuffSource> pair12 = perkInstancesOfType[num8];
                    this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.AuraLowHpDodge, 1f, (double) pair11.Key.Modifier, pair12.Value, null);
                }
            }
            if (activeCharacter.CurrentHpNormalized == 1f)
            {
                perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AuraFullHpDamage);
                for (int num9 = 0; num9 < perkInstancesOfType.Count; num9++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair13 = perkInstancesOfType[num9];
                    KeyValuePair<PerkInstance, BuffSource> pair14 = perkInstancesOfType[num9];
                    this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.AuraFullHpDamage, 1f, (double) pair13.Key.Modifier, pair14.Value, null);
                }
            }
            perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AllyBonusAttacksPerSecond);
            for (int i = 0; i < perkInstancesOfType.Count; i++)
            {
                for (int num11 = 0; num11 < activeDungeon.ActiveRoom.ActiveCharacters.Count; num11++)
                {
                    CharacterInstance instance3 = activeDungeon.ActiveRoom.ActiveCharacters[num11];
                    if (instance3.IsSupport && !instance3.IsDead)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair15 = perkInstancesOfType[i];
                        KeyValuePair<PerkInstance, BuffSource> pair16 = perkInstancesOfType[i];
                        this.startOrRefreshBuffFromPerk(instance3, PerkType.AllyBonusAttacksPerSecond, 1f, (double) pair15.Key.Modifier, pair16.Value, null);
                    }
                }
            }
            perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AllyBonusDamage);
            for (int j = 0; j < perkInstancesOfType.Count; j++)
            {
                for (int num13 = 0; num13 < activeDungeon.ActiveRoom.ActiveCharacters.Count; num13++)
                {
                    CharacterInstance instance4 = activeDungeon.ActiveRoom.ActiveCharacters[num13];
                    if (instance4.IsSupport && !instance4.IsDead)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair17 = perkInstancesOfType[j];
                        KeyValuePair<PerkInstance, BuffSource> pair18 = perkInstancesOfType[j];
                        this.startOrRefreshBuffFromPerk(instance4, PerkType.AllyBonusDamage, 1f, (double) pair17.Key.Modifier, pair18.Value, null);
                    }
                }
            }
            perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.AllyBonusSpeed);
            for (int k = 0; k < perkInstancesOfType.Count; k++)
            {
                for (int num15 = 0; num15 < activeDungeon.ActiveRoom.ActiveCharacters.Count; num15++)
                {
                    CharacterInstance instance5 = activeDungeon.ActiveRoom.ActiveCharacters[num15];
                    if (instance5.IsSupport && !instance5.IsDead)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair19 = perkInstancesOfType[k];
                        KeyValuePair<PerkInstance, BuffSource> pair20 = perkInstancesOfType[k];
                        this.startOrRefreshBuffFromPerk(instance5, PerkType.AllyBonusSpeed, 1f, (double) pair19.Key.Modifier, pair20.Value, null);
                    }
                }
            }
            perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(activeCharacter, PerkType.PetBonusDamage);
            for (int m = 0; m < perkInstancesOfType.Count; m++)
            {
                for (int num17 = 0; num17 < activeDungeon.ActiveRoom.ActiveCharacters.Count; num17++)
                {
                    CharacterInstance instance6 = activeDungeon.ActiveRoom.ActiveCharacters[num17];
                    if (instance6.IsPet && !instance6.IsDead)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair21 = perkInstancesOfType[m];
                        KeyValuePair<PerkInstance, BuffSource> pair22 = perkInstancesOfType[m];
                        this.startOrRefreshBuffFromPerk(instance6, PerkType.PetBonusDamage, 1f, (double) pair21.Key.Modifier, pair22.Value, null);
                    }
                }
            }
            for (int n = 0; n < activeRoom.ActiveCharacters.Count; n++)
            {
                CharacterInstance source = activeRoom.ActiveCharacters[n];
                if (!source.IsDead && (source.IsBoss && (Vector3.Distance(Vector3Extensions.ToXzVector3(source.PhysicsBody.Transform.position), Vector3Extensions.ToXzVector3(activeCharacter.PhysicsBody.Transform.position)) <= ((ConfigPerks.BossAuras.Radius + activeCharacter.Radius) + source.Radius))))
                {
                    perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(source, PerkType.BossAuraIce);
                    for (int num19 = 0; num19 < perkInstancesOfType.Count; num19++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair23 = perkInstancesOfType[num19];
                        KeyValuePair<PerkInstance, BuffSource> pair24 = perkInstancesOfType[num19];
                        this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.BossAuraIce, 1f, (double) pair23.Key.Modifier, pair24.Value, source);
                    }
                    perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(source, PerkType.BossAuraCooldownSlow);
                    for (int num20 = 0; num20 < perkInstancesOfType.Count; num20++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair25 = perkInstancesOfType[num20];
                        KeyValuePair<PerkInstance, BuffSource> pair26 = perkInstancesOfType[num20];
                        this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.BossAuraCooldownSlow, 1f, (double) pair25.Key.Modifier, pair26.Value, source);
                    }
                    perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(source, PerkType.BossAuraDamageOverTime);
                    for (int num21 = 0; num21 < perkInstancesOfType.Count; num21++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair27 = perkInstancesOfType[num21];
                        KeyValuePair<PerkInstance, BuffSource> pair28 = perkInstancesOfType[num21];
                        this.startOrRefreshBuffFromPerk(source, PerkType.BossAuraDamageOverTime, 1f, (double) pair27.Key.Modifier, pair28.Value, source);
                    }
                }
            }
            sm_tempCandidateList.Clear();
            PetInstance instance8 = player.Pets.getSelectedPetInstance();
            if (((instance8 != null) && (instance8.SpawnedCharacterInstance != null)) && !instance8.SpawnedCharacterInstance.IsDead)
            {
                sm_tempCandidateList.Add(instance8.SpawnedCharacterInstance);
            }
            for (int num22 = 0; num22 < sm_tempCandidateList.Count; num22++)
            {
                CharacterInstance instance9 = sm_tempCandidateList[num22];
                perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(instance9, PerkType.AuraDamageBonus);
                for (int num23 = 0; num23 < perkInstancesOfType.Count; num23++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair29 = perkInstancesOfType[num23];
                    KeyValuePair<PerkInstance, BuffSource> pair30 = perkInstancesOfType[num23];
                    this.startOrRefreshBuffFromPerk(instance9, PerkType.AuraDamageBonus, 1f, (double) pair29.Key.Modifier, pair30.Value, null);
                    KeyValuePair<PerkInstance, BuffSource> pair31 = perkInstancesOfType[num23];
                    KeyValuePair<PerkInstance, BuffSource> pair32 = perkInstancesOfType[num23];
                    this.startOrRefreshBuffFromPerk(activeCharacter, PerkType.AuraDamageBonus, 1f, (double) pair31.Key.Modifier, pair32.Value, null);
                }
            }
        }

        public void refreshBuff(Buff buff, float duration)
        {
            if (!buff.Character.IsDead)
            {
                buff.TimeRemaining = duration;
                if (buff.TickTimer != null)
                {
                    buff.TickTimer.reset();
                }
                GameLogic.Binder.EventBus.BuffRefreshed(buff.Character, buff);
            }
        }

        public void refreshBuff(Buff buff, double modifier, float duration)
        {
            if (!buff.Character.IsDead)
            {
                buff.Modifier = (float) modifier;
                buff.TimeRemaining = duration;
                GameLogic.Binder.EventBus.BuffRefreshed(buff.Character, buff);
            }
        }

        private void refreshBuildupBuffs()
        {
            for (int i = 0; i < this.m_allBuffs.Count; i++)
            {
                Buff buff = this.m_allBuffs[i];
                if (!buff.Ended && (buff.ModifierBuildupTick != 0f))
                {
                    float dt = Time.deltaTime * Time.timeScale;
                    if (buff.TickTimer.tick(dt))
                    {
                        buff.ModifierFactor++;
                        buff.TickTimer.set(buff.ModifierBuildupTick);
                    }
                }
            }
        }

        private void refreshTickBuffs()
        {
            for (int i = 0; i < this.m_allBuffs.Count; i++)
            {
                Buff buff = this.m_allBuffs[i];
                if ((!buff.Ended && (buff.DurationSeconds > 0f)) && ((buff.DamagePerSecond > 0.0) || (buff.HealingPerSecond > 0.0)))
                {
                    float dt = Time.deltaTime * Time.timeScale;
                    if (buff.TickTimer.tick(dt))
                    {
                        if (buff.HealingPerSecond > 0.0)
                        {
                            double num3 = buff.HealingPerSecond * buff.DurationSeconds;
                            double amount = num3 * (ConfigGameplay.BUFFS_TICK_INTERVAL / buff.DurationSeconds);
                            CmdGainHp.ExecuteStatic(buff.Character, amount, false);
                        }
                        if (buff.DamagePerSecond > 0.0)
                        {
                            double num5 = buff.DamagePerSecond * buff.DurationSeconds;
                            double baseAmount = num5 * (ConfigGameplay.BUFFS_TICK_INTERVAL / buff.DurationSeconds);
                            CmdDealDamageToCharacter.ExecuteStatic(buff.SourceCharacter, buff.Character, baseAmount, false, DamageType.Magic, SkillType.NONE);
                        }
                        float num7 = buff.TickTimer.timeRemaining() - dt;
                        buff.TickTimer.set(ConfigGameplay.BUFFS_TICK_INTERVAL + num7);
                    }
                }
            }
        }

        public void startBuff(CharacterInstance c, Buff buff)
        {
            if (!c.IsDead)
            {
                if (buff.DurationSeconds <= 0f)
                {
                    buff.DurationSeconds = 0.1f;
                }
                if (buff.Stuns)
                {
                    CmdSetStunCondition.ExecuteStatic(c, true);
                    if (!c.IsPlayerCharacter)
                    {
                        float num = Mathf.Pow(2f, (float) (-Mathf.Min(c.StunnedCount, 3) + 1));
                        buff.DurationSeconds *= num;
                    }
                }
                if (buff.Charms)
                {
                    CmdSetCharmCondition.ExecuteStatic(c, true);
                }
                if (buff.Confuses)
                {
                    CmdSetConfusedCondition.ExecuteStatic(c, true);
                }
                if (buff.TimeRemaining <= 0f)
                {
                    buff.TimeRemaining = buff.DurationSeconds;
                }
                buff.Character = c;
                this.m_characterBuffLists[c].Add(buff);
                this.m_allBuffs.Add(buff);
                if ((buff.DamagePerSecond > 0.0) || (buff.HealingPerSecond > 0.0))
                {
                    if ((buff.DurationSeconds % ConfigGameplay.BUFFS_TICK_INTERVAL) != 0f)
                    {
                        Debug.LogWarning(string.Concat(new object[] { "Buff duration ", buff.DurationSeconds, " is not a multiple of the tick duration ", ConfigGameplay.BUFFS_TICK_INTERVAL }));
                    }
                    buff.TickTimer = new ManualTimer(ConfigGameplay.BUFFS_TICK_INTERVAL);
                }
                else if (buff.ModifierBuildupTick > 0f)
                {
                    buff.TickTimer = new ManualTimer(buff.ModifierBuildupTick);
                }
                GameLogic.Binder.EventBus.BuffStarted(c, buff);
            }
        }

        public void startBuffFromPerk(CharacterInstance c, PerkType perkType, float durationSeconds, double modifier, BuffSource source, [Optional, DefaultParameterValue(null)] CharacterInstance sourceCharacter)
        {
            if (!c.IsDead)
            {
                if (source.IconProvider != null)
                {
                }
                ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[perkType];
                Buff buff2 = new Buff();
                buff2.Source = source;
                buff2.SourceCharacter = sourceCharacter;
                buff2.FromPerk = perkType;
                buff2.BaseStat1 = data.BuffBaseStat1;
                buff2.BaseStat2 = data.BuffBaseStat2;
                buff2.Stuns = data.Stuns;
                buff2.Modifier = (float) modifier;
                buff2.ModifierBuildupTick = data.ModifierBuildupTick;
                buff2.DurationSeconds = durationSeconds;
                buff2.AllSkillsCooldownBonus = !data.AllSkillsCooldownBonus ? 0f : ((float) modifier);
                buff2.DamagePerSecond = !data.DamagePerSecond ? 0.0 : modifier;
                buff2.ViewScaleModifier = data.ViewScaleModifier;
                buff2.HudSprite = (source.IconProvider == null) ? null : source.IconProvider.getSpriteId();
                buff2.HudShowStacked = data.ShowStackedInHud;
                buff2.HudShowModifier = data.ShowModifierInHud;
                buff2.HudHideTimer = data.HideTimerInHud;
                Buff buff = buff2;
                this.startBuff(c, buff);
            }
        }

        public void startOrRefreshBuff(Buff buff, float duration)
        {
            if (buff.Ended)
            {
                buff.TimeRemaining = duration;
                this.startBuff(buff.Character, buff);
            }
            else
            {
                this.refreshBuff(buff, duration);
            }
        }

        public void startOrRefreshBuffFromPerk(CharacterInstance c, PerkType perkType, float durationSeconds, double modifier, BuffSource source, [Optional, DefaultParameterValue(null)] CharacterInstance sourceCharacter)
        {
            Buff buff = this.getBuffFromSource(c, source);
            if (buff != null)
            {
                this.refreshBuff(buff, modifier, durationSeconds);
            }
            else
            {
                this.startBuffFromPerk(c, perkType, durationSeconds, modifier, source, sourceCharacter);
            }
        }
    }
}

