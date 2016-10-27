namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ConsoleCommand("language")]
    public class CmdCheatLanguage : ICommand
    {
        private SystemLanguage m_language;

        public CmdCheatLanguage(SystemLanguage language)
        {
            this.m_language = language;
        }

        public CmdCheatLanguage(string[] serialized)
        {
            try
            {
                string str = LangUtil.FirstLetterToUpper(serialized[0]);
                this.m_language = (SystemLanguage) ((int) Enum.Parse(typeof(SystemLanguage), str));
            }
            catch
            {
                this.m_language = SystemLanguage.Unknown;
            }
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator3F iteratorf = new <executeRoutine>c__Iterator3F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public static void ExecuteStatic(SystemLanguage language)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            if ((((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && ((player != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))) && ((language != SystemLanguage.Unknown) && (App.Binder.LocaSystem.selectedLanguage != language)))
            {
                player.Preferences.DevLanguageId = language.ToString();
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                App.Binder.LocaSystem.Initialize(language);
                App.Binder.AppContext.hardReset(null);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator3F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCheatLanguage <>f__this;

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
                    CmdCheatLanguage.ExecuteStatic(this.<>f__this.m_language);
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

