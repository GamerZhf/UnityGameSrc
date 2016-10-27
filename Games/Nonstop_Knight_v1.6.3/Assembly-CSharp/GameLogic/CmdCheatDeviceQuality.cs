namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [ConsoleCommand("quality")]
    public class CmdCheatDeviceQuality : ICommand
    {
        private DeviceQualityType m_deviceQualityType;

        public CmdCheatDeviceQuality(DeviceQualityType deviceQualityType)
        {
            this.m_deviceQualityType = deviceQualityType;
        }

        public CmdCheatDeviceQuality(string[] serialized)
        {
            try
            {
                string str = LangUtil.FirstLetterToUpper(serialized[0].ToLower());
                this.m_deviceQualityType = (DeviceQualityType) ((int) Enum.Parse(typeof(DeviceQualityType), str));
            }
            catch
            {
                this.m_deviceQualityType = DeviceQualityType.Auto;
            }
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator3E iteratore = new <executeRoutine>c__Iterator3E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public static void ExecuteStatic(DeviceQualityType deviceQualityType)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            if ((((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && ((player != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))) && ((deviceQualityType != DeviceQualityType.Auto) && (ConfigDevice.DeviceQuality() != deviceQualityType)))
            {
                ConfigApp.CHEAT_SIMULATE_DEVICE_QUALITY = deviceQualityType;
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                App.Binder.AppContext.hardReset(null);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator3E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCheatDeviceQuality <>f__this;

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
                    CmdCheatDeviceQuality.ExecuteStatic(this.<>f__this.m_deviceQualityType);
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

