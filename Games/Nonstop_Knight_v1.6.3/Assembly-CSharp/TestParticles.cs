using System;
using UnityEngine;

public class TestParticles : MonoBehaviour
{
    private int m_CurrentElementIndex = -1;
    private GameObject[] m_CurrentElementList;
    private GameObject m_CurrentParticle;
    private int m_CurrentParticleIndex = -1;
    private string m_ElementName = string.Empty;
    private string m_ParticleName = string.Empty;
    public GameObject[] m_PrefabListDarkness;
    public GameObject[] m_PrefabListEarth;
    public GameObject[] m_PrefabListFire;
    public GameObject[] m_PrefabListIce;
    public GameObject[] m_PrefabListLight;
    public GameObject[] m_PrefabListThunder;
    public GameObject[] m_PrefabListWater;
    public GameObject[] m_PrefabListWind;

    private void InfoWindow(int id)
    {
        GUI.Label(new Rect(15f, 25f, 240f, 20f), "Elementals 1.1.1");
        GUI.Label(new Rect(15f, 50f, 240f, 20f), "www.ge-team.com/pages");
    }

    private void OnGUI()
    {
        GUI.Window(1, new Rect((float) (Screen.width - 260), 5f, 250f, 80f), new GUI.WindowFunction(this.InfoWindow), "Info");
        GUI.Window(2, new Rect((float) (Screen.width - 260), (float) (Screen.height - 0x55), 250f, 80f), new GUI.WindowFunction(this.ParticleInformationWindow), "Help");
    }

    private void ParticleInformationWindow(int id)
    {
        GUI.Label(new Rect(12f, 25f, 280f, 20f), string.Concat(new object[] { "Up/Down: ", this.m_ElementName, " (", this.m_CurrentParticleIndex + 1, "/", this.m_CurrentElementList.Length, ")" }));
        GUI.Label(new Rect(12f, 50f, 280f, 20f), "Left/Right: " + this.m_ParticleName.ToUpper());
    }

    private void ShowParticle()
    {
        if (this.m_CurrentElementIndex > 7)
        {
            this.m_CurrentElementIndex = 0;
        }
        else if (this.m_CurrentElementIndex < 0)
        {
            this.m_CurrentElementIndex = 7;
        }
        if (this.m_CurrentElementIndex == 0)
        {
            this.m_CurrentElementList = this.m_PrefabListFire;
            this.m_ElementName = "FIRE";
        }
        else if (this.m_CurrentElementIndex == 1)
        {
            this.m_CurrentElementList = this.m_PrefabListWater;
            this.m_ElementName = "WATER";
        }
        else if (this.m_CurrentElementIndex == 2)
        {
            this.m_CurrentElementList = this.m_PrefabListWind;
            this.m_ElementName = "WIND";
        }
        else if (this.m_CurrentElementIndex == 3)
        {
            this.m_CurrentElementList = this.m_PrefabListEarth;
            this.m_ElementName = "EARTH";
        }
        else if (this.m_CurrentElementIndex == 4)
        {
            this.m_CurrentElementList = this.m_PrefabListThunder;
            this.m_ElementName = "THUNDER";
        }
        else if (this.m_CurrentElementIndex == 5)
        {
            this.m_CurrentElementList = this.m_PrefabListIce;
            this.m_ElementName = "ICE";
        }
        else if (this.m_CurrentElementIndex == 6)
        {
            this.m_CurrentElementList = this.m_PrefabListLight;
            this.m_ElementName = "LIGHT";
        }
        else if (this.m_CurrentElementIndex == 7)
        {
            this.m_CurrentElementList = this.m_PrefabListDarkness;
            this.m_ElementName = "DARKNESS";
        }
        if (this.m_CurrentParticleIndex >= this.m_CurrentElementList.Length)
        {
            this.m_CurrentParticleIndex = 0;
        }
        else if (this.m_CurrentParticleIndex < 0)
        {
            this.m_CurrentParticleIndex = this.m_CurrentElementList.Length - 1;
        }
        this.m_ParticleName = this.m_CurrentElementList[this.m_CurrentParticleIndex].name;
        if (this.m_CurrentParticle != null)
        {
            UnityEngine.Object.DestroyObject(this.m_CurrentParticle);
        }
        this.m_CurrentParticle = UnityEngine.Object.Instantiate<GameObject>(this.m_CurrentElementList[this.m_CurrentParticleIndex]);
    }

    private void Start()
    {
        if ((((this.m_PrefabListFire.Length > 0) || (this.m_PrefabListWind.Length > 0)) || ((this.m_PrefabListWater.Length > 0) || (this.m_PrefabListEarth.Length > 0))) || (((this.m_PrefabListIce.Length > 0) || (this.m_PrefabListThunder.Length > 0)) || ((this.m_PrefabListLight.Length > 0) || (this.m_PrefabListDarkness.Length > 0))))
        {
            this.m_CurrentElementIndex = 0;
            this.m_CurrentParticleIndex = 0;
            this.ShowParticle();
        }
    }

    private void Update()
    {
        if ((this.m_CurrentElementIndex != -1) && (this.m_CurrentParticleIndex != -1))
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                this.m_CurrentElementIndex++;
                this.m_CurrentParticleIndex = 0;
                this.ShowParticle();
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                this.m_CurrentElementIndex--;
                this.m_CurrentParticleIndex = 0;
                this.ShowParticle();
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                this.m_CurrentParticleIndex--;
                this.ShowParticle();
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                this.m_CurrentParticleIndex++;
                this.ShowParticle();
            }
        }
    }
}

