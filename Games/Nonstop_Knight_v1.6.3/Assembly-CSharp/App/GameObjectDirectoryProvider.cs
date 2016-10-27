namespace App
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class GameObjectDirectoryProvider : IInstanceProvider<GameObject>
    {
        protected Dictionary<GameObject, GameObject> m_instanceToPrototypeMap;
        protected int m_layer;
        protected Transform m_objectPoolParentTm;
        protected GameObject[] m_prototypes;
        protected string m_resourceRoot;
        protected int m_runningNumber;

        public GameObjectDirectoryProvider()
        {
            this.m_instanceToPrototypeMap = new Dictionary<GameObject, GameObject>();
        }

        public GameObjectDirectoryProvider(string resourceRoot, Transform objectPoolParentTm, [Optional, DefaultParameterValue(-1)] int layer)
        {
            this.m_instanceToPrototypeMap = new Dictionary<GameObject, GameObject>();
            this.m_resourceRoot = resourceRoot;
            this.m_layer = layer;
            this.m_objectPoolParentTm = objectPoolParentTm;
        }

        public GameObject instantiate()
        {
            GameObject key = null;
            if ((this.m_prototypes == null) && !string.IsNullOrEmpty(this.m_resourceRoot))
            {
                this.m_prototypes = Resources.LoadAll<GameObject>(this.m_resourceRoot);
            }
            if ((this.m_prototypes == null) || (this.m_prototypes.Length == 0))
            {
                key = new GameObject();
            }
            else
            {
                GameObject original = this.m_prototypes[this.m_runningNumber % this.m_prototypes.Length];
                key = UnityEngine.Object.Instantiate<GameObject>(original);
                this.m_instanceToPrototypeMap.Add(key, original);
            }
            key.transform.SetParent(this.m_objectPoolParentTm);
            if (this.m_layer != -1)
            {
                key.layer = this.m_layer;
            }
            key.name = this.m_resourceRoot + "_" + this.m_runningNumber++;
            return key;
        }

        public void onDestroy(GameObject obj)
        {
            this.m_instanceToPrototypeMap.Remove(obj);
            UnityEngine.Object.Destroy(obj);
        }

        public void onReset()
        {
            this.m_instanceToPrototypeMap.Clear();
            this.m_prototypes = null;
        }

        public void onReturn(GameObject go)
        {
            GameObject obj2 = !this.m_instanceToPrototypeMap.ContainsKey(go) ? null : this.m_instanceToPrototypeMap[go];
            go.transform.SetParent(this.m_objectPoolParentTm);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = (obj2 == null) ? Vector3.one : obj2.transform.localScale;
            go.transform.localRotation = (obj2 == null) ? Quaternion.identity : obj2.transform.localRotation;
            go.tag = Tags.UNTAGGED;
            go.SetActive(false);
        }
    }
}

