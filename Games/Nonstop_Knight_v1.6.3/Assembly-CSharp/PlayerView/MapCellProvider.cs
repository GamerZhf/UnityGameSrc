namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class MapCellProvider : IInstanceProvider<MapCell>
    {
        private GameObjectProvider m_gop;

        public MapCellProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public MapCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            MapCell component = obj2.GetComponent<MapCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(MapCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(MapCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

