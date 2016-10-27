using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AdjustBalls : MonoBehaviour
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map2;

    public void DoSomethingWithTheData(JsonData[] ssObjects)
    {
        OptionalMiddleStruct container = new OptionalMiddleStruct();
        for (int i = 0; i < ssObjects.Length; i++)
        {
            if (ssObjects[i].Keys.Contains("name"))
            {
                container.name = ssObjects[i]["name"].ToString();
            }
            if (ssObjects[i].Keys.Contains("color"))
            {
                container.color = this.GetColor(ssObjects[i]["color"].ToString());
            }
            if (ssObjects[i].Keys.Contains("drag"))
            {
                container.drag = float.Parse(ssObjects[i]["drag"].ToString());
            }
            this.UpdateObjectValues(container);
        }
    }

    private Color GetColor(string color)
    {
        string key = color;
        if (key != null)
        {
            int num;
            if (<>f__switch$map2 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(11);
                dictionary.Add("black", 0);
                dictionary.Add("blue", 1);
                dictionary.Add("clear", 2);
                dictionary.Add("cyan", 3);
                dictionary.Add("gray", 4);
                dictionary.Add("green", 5);
                dictionary.Add("grey", 6);
                dictionary.Add("magenta", 7);
                dictionary.Add("red", 8);
                dictionary.Add("white", 9);
                dictionary.Add("yellow", 10);
                <>f__switch$map2 = dictionary;
            }
            if (<>f__switch$map2.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        return Color.black;

                    case 1:
                        return Color.blue;

                    case 2:
                        return Color.clear;

                    case 3:
                        return Color.cyan;

                    case 4:
                        return Color.gray;

                    case 5:
                        return Color.green;

                    case 6:
                        return Color.grey;

                    case 7:
                        return Color.magenta;

                    case 8:
                        return Color.red;

                    case 9:
                        return Color.white;

                    case 10:
                        return Color.yellow;
                }
            }
        }
        return Color.grey;
    }

    public void ResetBalls()
    {
        OptionalMiddleStruct container = new OptionalMiddleStruct();
        container.color = Color.white;
        container.drag = 0f;
        string str = "Ball";
        for (int i = 1; i < 4; i++)
        {
            container.name = str + i.ToString();
            this.UpdateObjectValues(container);
        }
    }

    private void UpdateObjectValues(OptionalMiddleStruct container)
    {
        GameObject obj2 = GameObject.Find(container.name);
        obj2.GetComponent<Renderer>().sharedMaterial.color = container.color;
        obj2.GetComponent<Rigidbody>().drag = container.drag;
    }
}

