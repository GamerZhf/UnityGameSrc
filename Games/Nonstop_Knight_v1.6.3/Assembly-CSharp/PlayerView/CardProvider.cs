namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class CardProvider : IInstanceProvider<Card>
    {
        private GameObjectProvider m_gop;

        public CardProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public Card instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            Card component = obj2.GetComponent<Card>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(Card obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(Card obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

