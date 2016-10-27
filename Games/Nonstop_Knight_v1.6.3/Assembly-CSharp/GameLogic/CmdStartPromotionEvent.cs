namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdStartPromotionEvent : ICommand
    {
        private ConfigPromotionEvents.Event m_data;
        private Player m_player;
        private string m_promotionId;

        public CmdStartPromotionEvent(Player player, string promotionId, ConfigPromotionEvents.Event data)
        {
            this.m_player = player;
            this.m_promotionId = promotionId;
            this.m_data = data;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorBE rbe = new <executeRoutine>c__IteratorBE();
            rbe.<>f__this = this;
            return rbe;
        }

        public static void ExecuteStatic(Player player, string promotionId, ConfigPromotionEvents.Event data)
        {
            if (player.PromotionEvents.Instances.ContainsKey(promotionId))
            {
                UnityEngine.Debug.LogWarning("Trying to start a promotion event that already exists (id: " + promotionId + ")");
            }
            else
            {
                PromotionEventInstance instance = new PromotionEventInstance();
                instance.PromotionEvents = player.PromotionEvents;
                instance.postDeserializeInitialization();
                instance.Id = promotionId;
                if (data.Missions != null)
                {
                    for (int i = 0; i < data.Missions.Instances.Count; i++)
                    {
                        ConfigPromotionEvents.EventMissionInstance instance2 = data.Missions.Instances[i];
                        string missionId = instance2.MissionId;
                        if (!ConfigMissions.MISSIONS.ContainsKey(missionId))
                        {
                            UnityEngine.Debug.LogError("Invalid promotion event mission id " + missionId);
                        }
                        else
                        {
                            MissionInstance item = new MissionInstance();
                            item.Missions = instance.Missions;
                            item.MissionId = missionId;
                            item.StartValue = 0.0;
                            item.Requirement = MathUtil.Clamp(instance2.Requirement, 1.0, double.MaxValue);
                            item.ForceCompleted = false;
                            item.RewardShopEntryId = null;
                            item.RewardDiamonds = instance2.RewardDiamonds;
                            item.RewardClaimed = false;
                            item.CooldownStartTimestamp = data.StartTimestamp;
                            item.CooldownDurationSeconds = instance2.getStartTimestampOffset(data);
                            item.OnCooldown = true;
                            item.Inspected = true;
                            instance.Missions.Instances.Add(item);
                        }
                    }
                }
                instance.StartTimestamp = data.StartTimestamp;
                instance.EndTimestamp = data.EndTimestamp;
                instance.Inspected = false;
                player.PromotionEvents.Instances.Add(promotionId, instance);
                GameLogic.Binder.EventBus.PromotionEventStarted(player, promotionId);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorBE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdStartPromotionEvent <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    CmdStartPromotionEvent.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_promotionId, this.<>f__this.m_data);
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

