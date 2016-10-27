using System;
using System.Collections.Generic;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    public string enemyLayerName = "Enemies";
    private int m_Deaths;
    private readonly Dictionary<string, int> m_Kills = new Dictionary<string, int>();
    private float m_StartTime;
    public string playerLayerName = "Player";
    private static GameScore s_Instance;

    public static int GetKills(string type)
    {
        if ((Instance != null) && Instance.m_Kills.ContainsKey(type))
        {
            return Instance.m_Kills[type];
        }
        return 0;
    }

    public void OnApplicationQuit()
    {
        s_Instance = null;
    }

    public void OnLevelWasLoaded(int level)
    {
        if (this.m_StartTime == 0f)
        {
            this.m_StartTime = Time.time;
        }
    }

    public static void RegisterDeath(GameObject deadObject)
    {
        if (Instance == null)
        {
            Debug.Log("Game score not loaded");
        }
        else
        {
            int num = LayerMask.NameToLayer(Instance.playerLayerName);
            int num2 = LayerMask.NameToLayer(Instance.enemyLayerName);
            if (deadObject.layer == num)
            {
                GameScore instance = Instance;
                instance.m_Deaths++;
            }
            else if (deadObject.layer == num2)
            {
                Instance.m_Kills[deadObject.name] = !Instance.m_Kills.ContainsKey(deadObject.name) ? 1 : (Instance.m_Kills[deadObject.name] + 1);
            }
        }
    }

    public static int Deaths
    {
        get
        {
            if (Instance == null)
            {
                return 0;
            }
            return Instance.m_Deaths;
        }
    }

    public static float GameTime
    {
        get
        {
            if (Instance == null)
            {
                return 0f;
            }
            return (Time.time - Instance.m_StartTime);
        }
    }

    private static GameScore Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = (GameScore) UnityEngine.Object.FindObjectOfType(typeof(GameScore));
            }
            return s_Instance;
        }
    }

    public static ICollection<string> KillTypes
    {
        get
        {
            if (Instance == null)
            {
                return new string[0];
            }
            return Instance.m_Kills.Keys;
        }
    }
}

