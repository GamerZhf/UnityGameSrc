namespace Service
{
    using PlayerView;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class NSKRTLManager : MonoBehaviour
    {
        private List<IRTLProcessor> customProcessors;
        private List<GameObject> m_canvas;
        private List<Text> m_tmpTextComponents;

        private void FixRTL(List<GameObject> parents, bool includeInactive)
        {
            for (int i = 0; i < parents.Count; i++)
            {
                this.FixRTL(parents[i], includeInactive);
            }
        }

        private void FixRTL(GameObject parent, bool includeInactive)
        {
            this.m_tmpTextComponents.Clear();
            parent.GetComponentsInChildren<Text>(includeInactive, this.m_tmpTextComponents);
            for (int i = 0; i < this.m_tmpTextComponents.Count; i++)
            {
                Text textComp = this.m_tmpTextComponents[i];
                GameObject gameObject = textComp.gameObject;
                RTLConverter component = gameObject.GetComponent<RTLConverter>();
                if (component == null)
                {
                    component = gameObject.AddComponent<RTLConverter>();
                    component.Init(textComp, this.customProcessors);
                }
                component.ProcessTextComp();
            }
        }

        public void Initialize(string uiRootName)
        {
            Debug.Log("RTL text initializing");
            this.m_tmpTextComponents = new List<Text>();
            List<IRTLProcessor> list = new List<IRTLProcessor>();
            list.Add(new SkipOnLineBreaksProcessor());
            list.Add(new ColorTagProcessor());
            this.customProcessors = list;
            GameObject obj2 = GameObject.Find(uiRootName);
            if (obj2 != null)
            {
                this.m_canvas = new List<GameObject>();
                foreach (Canvas canvas in obj2.GetComponentsInChildren<Canvas>(true))
                {
                    this.m_canvas.Add(canvas.gameObject);
                }
                this.FixRTL(this.m_canvas, false);
                PlayerView.Binder.EventBus.OnMenuChangeStarted += new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
                PlayerView.Binder.EventBus.OnMenuChanged += new PlayerView.Events.MenuChanged(this.onMenuChanged);
                PlayerView.Binder.EventBus.OnMenuContentChanged += new PlayerView.Events.MenuContentChanged(this.onMenuContent);
            }
        }

        private void onMenuChanged(Menu sourceMenu, Menu targetMenu)
        {
            this.FixRTL(this.m_canvas, false);
        }

        private void onMenuChangeStarted(MenuType sourcemenutype, MenuType targetmenutype)
        {
            this.FixRTL(this.m_canvas, false);
        }

        private void onMenuContent(GameObject content)
        {
            this.FixRTL(content, true);
        }

        public void UnInitialize()
        {
            Debug.Log("RTL text uninitializing");
            PlayerView.Binder.EventBus.OnMenuChangeStarted -= new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChanged -= new PlayerView.Events.MenuChanged(this.onMenuChanged);
            PlayerView.Binder.EventBus.OnMenuContentChanged -= new PlayerView.Events.MenuContentChanged(this.onMenuContent);
        }
    }
}

