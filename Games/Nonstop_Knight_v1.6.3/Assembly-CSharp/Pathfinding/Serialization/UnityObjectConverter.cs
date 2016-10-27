namespace Pathfinding.Serialization
{
    using Pathfinding;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UnityObjectConverter : JsonConverter
    {
        public override bool CanConvert(System.Type type)
        {
            return typeof(UnityEngine.Object).IsAssignableFrom(type);
        }

        public override object ReadJson(System.Type objectType, Dictionary<string, object> values)
        {
            if (values != null)
            {
                string path = (string) values["Name"];
                if (path == null)
                {
                    return null;
                }
                string typeName = (string) values["Type"];
                System.Type objA = System.Type.GetType(typeName);
                if (object.Equals(objA, null))
                {
                    Debug.LogError("Could not find type '" + typeName + "'. Cannot deserialize Unity reference");
                    return null;
                }
                if (values.ContainsKey("GUID"))
                {
                    string str3 = (string) values["GUID"];
                    UnityReferenceHelper[] helperArray = UnityEngine.Object.FindObjectsOfType(typeof(UnityReferenceHelper)) as UnityReferenceHelper[];
                    for (int j = 0; j < helperArray.Length; j++)
                    {
                        if (helperArray[j].GetGUID() == str3)
                        {
                            if (object.Equals(objA, typeof(GameObject)))
                            {
                                return helperArray[j].gameObject;
                            }
                            return helperArray[j].GetComponent(objA);
                        }
                    }
                }
                UnityEngine.Object[] objArray = Resources.LoadAll(path, objA);
                for (int i = 0; i < objArray.Length; i++)
                {
                    if ((objArray[i].name == path) || (objArray.Length == 1))
                    {
                        return objArray[i];
                    }
                }
            }
            return null;
        }

        public override Dictionary<string, object> WriteJson(System.Type type, object value)
        {
            UnityEngine.Object obj2 = (UnityEngine.Object) value;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (value == null)
            {
                dictionary.Add("Name", null);
                return dictionary;
            }
            dictionary.Add("Name", obj2.name);
            dictionary.Add("Type", obj2.GetType().AssemblyQualifiedName);
            Component component = value as Component;
            GameObject gameObject = value as GameObject;
            if ((component != null) || (gameObject != null))
            {
                if ((component != null) && (gameObject == null))
                {
                    gameObject = component.gameObject;
                }
                UnityReferenceHelper helper = gameObject.GetComponent<UnityReferenceHelper>();
                if (helper == null)
                {
                    Debug.Log("Adding UnityReferenceHelper to Unity Reference '" + obj2.name + "'");
                    helper = gameObject.AddComponent<UnityReferenceHelper>();
                }
                helper.Reset();
                dictionary.Add("GUID", helper.GetGUID());
            }
            return dictionary;
        }
    }
}

