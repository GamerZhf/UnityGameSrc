namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AccessoryStorage : MonoBehaviour
    {
        public void Awake()
        {
        }

        public Accessory getAccessory(AccessoryType accessoryType, [Optional, DefaultParameterValue(null)] string material)
        {
            Accessory component = ResourceUtil.Instantiate<GameObject>("Prefabs/Accessories/" + accessoryType).GetComponent<Accessory>();
            component.Type = accessoryType;
            if (!string.IsNullOrEmpty(material))
            {
                component.Renderer.sharedMaterial = ResourceUtil.Instantiate<Material>("Materials/" + material);
            }
            component.initializeFlashMaterials();
            return component;
        }

        public void releaseAccessory(Accessory accessory)
        {
            if (accessory.Trail != null)
            {
                accessory.Trail.Init();
                accessory.Trail.Deactivate();
                UnityEngine.Object.Destroy(accessory.Trail.mMeshObj);
                accessory.Trail.mMeshObj = null;
            }
            UnityEngine.Object.Destroy(accessory.gameObject);
        }
    }
}

