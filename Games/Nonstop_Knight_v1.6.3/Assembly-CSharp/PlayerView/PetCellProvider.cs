namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class PetCellProvider : IInstanceProvider<PetCell>
    {
        private GameObjectProvider m_gop;

        public PetCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public PetCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            PetCell component = obj2.GetComponent<PetCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(PetCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(PetCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

