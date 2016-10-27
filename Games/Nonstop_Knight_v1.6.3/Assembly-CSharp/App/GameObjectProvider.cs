namespace App
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class GameObjectProvider : IInstanceProvider<GameObject>
    {
        protected int m_layer;
        protected Transform m_objectPoolParentTm;
        protected GameObject m_prototype;
        protected string m_resourceName;
        protected int m_runningNumber;

        public GameObjectProvider()
        {
        }

        public GameObjectProvider(string resourceName, Transform objectPoolParentTm, [Optional, DefaultParameterValue(-1)] int layer)
        {
            this.m_resourceName = resourceName;
            this.m_layer = layer;
            this.m_objectPoolParentTm = objectPoolParentTm;
        }

        public GameObject instantiate()
        {
            GameObject obj2 = null;
            if ((this.m_prototype == null) && !string.IsNullOrEmpty(this.m_resourceName))
            {
                this.m_prototype = Resources.Load<GameObject>(this.m_resourceName);
            }
            if (this.m_prototype == null)
            {
                obj2 = new GameObject();
            }
            else
            {
                obj2 = UnityEngine.Object.Instantiate<GameObject>(this.m_prototype);
            }
            obj2.transform.SetParent(this.m_objectPoolParentTm);
            if (this.m_layer != -1)
            {
                obj2.layer = this.m_layer;
            }
            obj2.name = this.m_resourceName + "_" + this.m_runningNumber++;
            return obj2;
        }

        public void onDestroy(GameObject obj)
        {
            UnityEngine.Object.Destroy(obj);
        }

        public void onReset()
        {
            this.m_prototype = null;
        }

        public void onReturn(GameObject go)
        {
            go.transform.SetParent(this.m_objectPoolParentTm);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = (this.m_prototype == null) ? Vector3.one : this.m_prototype.transform.localScale;
            go.transform.localRotation = (this.m_prototype == null) ? Quaternion.identity : this.m_prototype.transform.localRotation;
            go.tag = Tags.UNTAGGED;
            go.SetActive(false);
        }
    }
}

