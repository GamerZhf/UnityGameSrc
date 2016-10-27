namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdSavePlayerDataToPersistentStorage : ICommand
    {
        private Player m_player;

        public CmdSavePlayerDataToPersistentStorage(Player player)
        {
            this.m_player = player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB2 rb = new <executeRoutine>c__IteratorB2();
            rb.<>f__this = this;
            return rb;
        }

        public static void ExecuteStatic(Player player)
        {
            if (Service.Binder.PromotionService != null)
            {
                foreach (RemotePromotion promotion in Service.Binder.PromotionService.Promotions)
                {
                    if (promotion.State != null)
                    {
                        if (player.PromotionStates.ContainsKey(promotion.promotionid))
                        {
                            player.PromotionStates[promotion.promotionid] = promotion.State.GetAsLong();
                        }
                        else
                        {
                            player.PromotionStates.Add(promotion.promotionid, promotion.State.GetAsLong());
                        }
                    }
                }
            }
            player.LastSerializationTimestamp = Service.Binder.ServerTime.GameTime;
            string content = JsonUtils.Serialize(player);
            string filename = Guid.NewGuid().ToString();
            if (!IOUtil.SaveToPersistentStorage(content, filename, ConfigApp.PersistentStorageEncryptionEnabled, true))
            {
                UnityEngine.Debug.LogError("Failed to save GUID file into persistent storage.");
            }
            else if (!IOUtil.FileExistsInPersistentStorage(filename))
            {
                UnityEngine.Debug.LogError("GUID file doesn't exist in persistent storage.");
            }
            else if (IOUtil.FileExistsInPersistentStorage(ConfigApp.LocalPlayerProfileFileBackup) && !IOUtil.DeleteFileFromPersistentStorage(ConfigApp.LocalPlayerProfileFileBackup))
            {
                UnityEngine.Debug.LogError("Failed to delete old backup file from persistent storage.");
            }
            else if (IOUtil.FileExistsInPersistentStorage(ConfigApp.LocalPlayerProfileFilePrimary) && !IOUtil.RenameFileInPersistentStorage(ConfigApp.LocalPlayerProfileFilePrimary, ConfigApp.LocalPlayerProfileFileBackup))
            {
                UnityEngine.Debug.LogError("Failed to rename primary file into backup in persistent storage.");
            }
            else if (!IOUtil.RenameFileInPersistentStorage(filename, ConfigApp.LocalPlayerProfileFilePrimary))
            {
                UnityEngine.Debug.LogError("Failed to rename GUID file into primary file in persistent storage.");
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSavePlayerDataToPersistentStorage <>f__this;

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
                    CmdSavePlayerDataToPersistentStorage.ExecuteStatic(this.<>f__this.m_player);
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

