namespace GameLogic
{
    using App;
    using System;
    using UnityEngine;

    public class CharacterProvider : IInstanceProvider<CharacterInstance>
    {
        private GameObjectProvider m_gop;
        private static int sm_spawnCounter;

        public CharacterProvider(int layer)
        {
            this.m_gop = new GameObjectProvider(string.Empty, App.Binder.PersistentObjectRootTm, layer);
        }

        public CharacterInstance instantiate()
        {
            CharacterInstance attachedToCharacter = new CharacterInstance();
            GameObject obj2 = this.m_gop.instantiate();
            obj2.name = "Character_" + sm_spawnCounter++;
            attachedToCharacter.PhysicsBody = obj2.AddComponent<PhysicsBody>();
            attachedToCharacter.PhysicsBody.onInstantiate(attachedToCharacter);
            obj2.SetActive(false);
            return attachedToCharacter;
        }

        public void onDestroy(CharacterInstance obj)
        {
            this.m_gop.onDestroy(obj.PhysicsBody.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(CharacterInstance obj)
        {
            this.m_gop.onReturn(obj.PhysicsBody.gameObject);
        }
    }
}

