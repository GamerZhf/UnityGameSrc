using OrbCreationExtensions;
using System;
using UnityEngine;

public class LODSwitcher : MonoBehaviour
{
    private Vector3 centerOffset;
    public Camera customCamera;
    public float deactivateAtDistance;
    private int fixedLODLevel = -1;
    private int frameInterval = 10;
    private int frameOffset;
    public GameObject[] lodGameObjects;
    private int lodLevel;
    public Mesh[] lodMeshes;
    public float[] lodScreenSizes;
    private MeshFilter meshFilter;
    private float objectSize;
    private float pixelsPerMeter;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    public void ComputeDimensions()
    {
        Bounds worldBounds = OrbCreationExtensions.GameObjectExtensions.GetWorldBounds(base.gameObject);
        this.centerOffset = worldBounds.center - base.transform.position;
        this.objectSize = worldBounds.size.magnitude;
        if ((this.skinnedMeshRenderer == null) && (this.meshFilter == null))
        {
            this.GetMeshFilter();
        }
        if (this.skinnedMeshRenderer != null)
        {
            worldBounds = this.skinnedMeshRenderer.localBounds;
            this.objectSize = worldBounds.size.magnitude;
            this.centerOffset = worldBounds.center;
            this.frameInterval = 1;
        }
        Camera customCamera = this.customCamera;
        if (customCamera == null)
        {
            customCamera = Camera.main;
        }
        if (customCamera == null)
        {
            Debug.LogWarning("No scene camera found yet, you need to call LODSwitcher.ComputeDimensions() again when you have your Camera set up");
        }
        else
        {
            Vector3 a = customCamera.ScreenToWorldPoint(new Vector3((Screen.width - 100f) / 2f, 0f, 1f));
            Vector3 b = customCamera.ScreenToWorldPoint(new Vector3((Screen.width + 100f) / 2f, 0f, 1f));
            this.pixelsPerMeter = 1f / (Vector3.Distance(a, b) / 100f);
        }
    }

    private void ComputeLODLevel()
    {
        int newLodLevel = 0;
        if (this.fixedLODLevel >= 0)
        {
            newLodLevel = this.fixedLODLevel;
        }
        else
        {
            float num2 = this.ScreenPortion();
            if (num2 >= 0f)
            {
                for (int i = 0; i < this.lodScreenSizes.Length; i++)
                {
                    if (num2 < this.lodScreenSizes[i])
                    {
                        newLodLevel++;
                    }
                }
            }
            else
            {
                newLodLevel = -1;
            }
        }
        if (newLodLevel != this.lodLevel)
        {
            this.SetLODLevel(newLodLevel);
        }
    }

    public int GetLODLevel()
    {
        return this.lodLevel;
    }

    public Mesh GetMesh(int aLevel)
    {
        if ((this.lodMeshes != null) && (this.lodMeshes.Length >= aLevel))
        {
            return this.lodMeshes[aLevel];
        }
        return null;
    }

    private void GetMeshFilter()
    {
        this.skinnedMeshRenderer = base.gameObject.GetComponent<SkinnedMeshRenderer>();
        if (this.skinnedMeshRenderer == null)
        {
            this.meshFilter = base.gameObject.GetComponent<MeshFilter>();
        }
    }

    public int MaxLODLevel()
    {
        if (this.lodScreenSizes == null)
        {
            return 0;
        }
        return this.lodScreenSizes.Length;
    }

    public Vector3 NearestCameraPositionForLOD(int aLevel)
    {
        this.ComputeDimensions();
        Camera customCamera = this.customCamera;
        if (customCamera == null)
        {
            customCamera = Camera.main;
        }
        if ((aLevel > 0) && (aLevel <= this.lodScreenSizes.Length))
        {
            float num = this.objectSize * this.pixelsPerMeter;
            float num2 = (num / ((float) Screen.width)) / this.lodScreenSizes[aLevel - 1];
            return ((base.transform.position + this.centerOffset) + ((Vector3) (customCamera.transform.rotation * (Vector3.back * num2))));
        }
        return customCamera.transform.position;
    }

    public void ReleaseFixedLODLevel()
    {
        this.fixedLODLevel = -1;
    }

    public float ScreenPortion()
    {
        Camera customCamera = this.customCamera;
        if (customCamera == null)
        {
            customCamera = Camera.main;
        }
        float num = Vector3.Distance(customCamera.transform.position, base.transform.position + this.centerOffset);
        if ((this.deactivateAtDistance > 0f) && (num > this.deactivateAtDistance))
        {
            return -1f;
        }
        float num2 = this.objectSize * this.pixelsPerMeter;
        float num3 = (num2 / num) / ((float) Screen.width);
        return (Mathf.RoundToInt(num3 * 40f) * 0.025f);
    }

    public void SetCustomCamera(Camera aCamera)
    {
        this.customCamera = aCamera;
        this.ComputeDimensions();
    }

    public void SetFixedLODLevel(int aLevel)
    {
        this.fixedLODLevel = Mathf.Max(0, Mathf.Min(this.MaxLODLevel(), aLevel));
    }

    public void SetLODLevel(int newLodLevel)
    {
        if (newLodLevel != this.lodLevel)
        {
            newLodLevel = Mathf.Min(this.MaxLODLevel(), newLodLevel);
            if (newLodLevel < 0)
            {
                base.gameObject.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                if ((this.lodMeshes != null) && (this.lodMeshes.Length > 0))
                {
                    base.gameObject.GetComponent<Renderer>().enabled = true;
                }
                if ((this.lodMeshes != null) && (this.lodMeshes.Length > newLodLevel))
                {
                    if ((this.skinnedMeshRenderer == null) && (this.meshFilter == null))
                    {
                        this.GetMeshFilter();
                    }
                    if (this.skinnedMeshRenderer != null)
                    {
                        this.skinnedMeshRenderer.sharedMesh = this.lodMeshes[newLodLevel];
                    }
                    else if (this.meshFilter != null)
                    {
                        this.meshFilter.sharedMesh = this.lodMeshes[newLodLevel];
                    }
                }
                for (int i = 0; (this.lodGameObjects != null) && (i < this.lodGameObjects.Length); i++)
                {
                    this.lodGameObjects[i].SetActive(i == newLodLevel);
                }
            }
            this.lodLevel = newLodLevel;
        }
    }

    public void SetMesh(Mesh aMesh, int aLevel)
    {
        if (this.lodMeshes == null)
        {
            this.lodMeshes = new Mesh[aLevel + 1];
        }
        if (this.lodMeshes.Length <= aLevel)
        {
            Array.Resize<Mesh>(ref this.lodMeshes, aLevel + 1);
        }
        if (aLevel > 0)
        {
            if (this.lodScreenSizes == null)
            {
                this.lodScreenSizes = new float[aLevel];
            }
            if (this.lodScreenSizes.Length < aLevel)
            {
                Array.Resize<float>(ref this.lodScreenSizes, aLevel);
            }
        }
        this.lodMeshes[aLevel] = aMesh;
        if (aLevel == this.lodLevel)
        {
            this.lodLevel = -1;
            this.SetLODLevel(aLevel);
        }
        this.ComputeDimensions();
    }

    public void SetRelativeScreenSize(float aValue, int aLevel)
    {
        if (this.lodScreenSizes == null)
        {
            this.lodScreenSizes = new float[aLevel];
        }
        if (this.lodScreenSizes.Length < aLevel)
        {
            Array.Resize<float>(ref this.lodScreenSizes, aLevel);
        }
        for (int i = 0; i < this.lodScreenSizes.Length; i++)
        {
            if ((i + 1) == aLevel)
            {
                this.lodScreenSizes[i] = aValue;
            }
            else if (this.lodScreenSizes[i] == 0f)
            {
                if (i == 0)
                {
                    this.lodScreenSizes[i] = 0.6f;
                }
                else
                {
                    this.lodScreenSizes[i] = this.lodScreenSizes[i - 1] * 0.5f;
                }
            }
        }
    }

    private void Start()
    {
        this.frameOffset = Mathf.RoundToInt(UnityEngine.Random.value * 10f);
        if (((this.lodMeshes == null) || (this.lodMeshes.Length <= 0)) && ((this.lodGameObjects == null) || (this.lodGameObjects.Length <= 0)))
        {
            Debug.LogWarning(base.gameObject.name + ".LODSwitcher: No lodMeshes/lodGameObjects set. LODSwitcher is now disabled.");
            base.enabled = false;
        }
        int a = 0;
        if (this.lodMeshes != null)
        {
            a = this.lodMeshes.Length - 1;
        }
        if (this.lodGameObjects != null)
        {
            a = Mathf.Max(a, this.lodGameObjects.Length - 1);
        }
        if (base.enabled && ((this.lodScreenSizes == null) || (this.lodScreenSizes.Length != a)))
        {
            Debug.LogWarning(string.Concat(new object[] { base.gameObject.name, ".LODSwitcher: lodScreenSizes should have a length of ", a, ". LODSwitcher is now disabled." }));
            base.enabled = false;
        }
        this.SetLODLevel(0);
        this.ComputeDimensions();
        this.lodLevel = -1;
        this.ComputeLODLevel();
    }

    private void Update()
    {
        if (((Time.frameCount + this.frameOffset) % this.frameInterval) == 0)
        {
            this.ComputeLODLevel();
        }
    }
}

