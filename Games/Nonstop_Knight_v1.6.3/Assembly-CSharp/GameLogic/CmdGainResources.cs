namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ConsoleCommand("gain")]
    public class CmdGainResources : ICommand
    {
        private double m_amount;
        private Player m_player;
        private ResourceType m_resourceType;
        private string m_trackingId;
        private bool m_visualizationManuallyControlled;
        private Vector3? m_worldPt;

        public CmdGainResources(string[] serialized)
        {
            this.m_player = GameLogic.Binder.GameState.Player;
            this.m_resourceType = (ResourceType) ((int) Enum.Parse(typeof(ResourceType), LangUtil.FirstLetterToUpper(serialized[0])));
            this.m_amount = double.Parse(serialized[1]);
        }

        public CmdGainResources(Player player, ResourceType resourceType, double amount, [Optional, DefaultParameterValue(false)] bool visualizationManuallyControlled, [Optional, DefaultParameterValue("")] string trackingId, [Optional, DefaultParameterValue(null)] Vector3? worldPt)
        {
            this.m_player = player;
            this.m_resourceType = resourceType;
            this.m_amount = amount;
            this.m_visualizationManuallyControlled = visualizationManuallyControlled;
            this.m_trackingId = trackingId;
            this.m_worldPt = worldPt;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator57 iterator = new <executeRoutine>c__Iterator57();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player, ResourceType resourceType, double amount, [Optional, DefaultParameterValue(false)] bool visualizationManuallyControlled, [Optional, DefaultParameterValue("")] string trackingId, [Optional, DefaultParameterValue(null)] Vector3? worldPt)
        {
            string key = ConfigMeta.RESOURCE_TYPE_TO_STRING_MAPPING[resourceType];
            Dictionary<string, double> dictionary = player.getResources(resourceType != ResourceType.Coin);
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, 0.0);
            }
            amount = Math.Floor(amount);
            dictionary[key] = Math.Floor(MathUtil.ClampMin(dictionary[key] + amount, 0.0));
            if ((resourceType == ResourceType.Xp) && player.canRankUp())
            {
                CmdRankUpPlayer.ExecuteStatic(player, false);
            }
            GameLogic.Binder.EventBus.ResourcesGained(player, resourceType, amount, visualizationManuallyControlled, trackingId, worldPt);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator57 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainResources <>f__this;

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
                    CmdGainResources.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_resourceType, this.<>f__this.m_amount, this.<>f__this.m_visualizationManuallyControlled, this.<>f__this.m_trackingId, this.<>f__this.m_worldPt);
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

