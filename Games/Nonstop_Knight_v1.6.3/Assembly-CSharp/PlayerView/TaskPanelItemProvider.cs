namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class TaskPanelItemProvider : IInstanceProvider<TaskPanelItem>
    {
        private GameObjectProvider m_gop;

        public TaskPanelItemProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public TaskPanelItem instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            TaskPanelItem component = obj2.GetComponent<TaskPanelItem>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(TaskPanelItem obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(TaskPanelItem obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

