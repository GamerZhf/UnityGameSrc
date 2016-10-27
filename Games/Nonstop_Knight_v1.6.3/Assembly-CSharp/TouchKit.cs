using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TouchKit : MonoBehaviour
{
    private Vector2 _designTimeResolution = new Vector2(320f, 180f);
    private List<TKAbstractGestureRecognizer> _gestureRecognizers = new List<TKAbstractGestureRecognizer>(5);
    private static TouchKit _instance;
    private List<TKTouch> _liveTouches = new List<TKTouch>(2);
    private bool _shouldCheckForLostTouches;
    private TKTouch[] _touchCache;
    [CompilerGenerated]
    private Vector2 <pixelsToUnityUnitsMultiplier>k__BackingField;
    [CompilerGenerated]
    private float <runtimeDistanceModifier>k__BackingField;
    [CompilerGenerated]
    private Vector2 <runtimeScaleModifier>k__BackingField;
    public bool autoScaleRectsAndDistances = true;
    [HideInInspector]
    public bool drawDebugBoundaryFrames;
    [HideInInspector]
    public bool drawTouches;
    private const float inchesToCentimeters = 2.54f;
    public int maxTouchesToProcess = 2;
    public bool shouldAutoUpdateTouches = true;
    [HideInInspector]
    public bool simulateMultitouch = true;
    [HideInInspector]
    public bool simulateTouches = true;

    public static void addGestureRecognizer(TKAbstractGestureRecognizer recognizer)
    {
        instance._gestureRecognizers.Add(recognizer);
        if (recognizer.zIndex > 0)
        {
            _instance._gestureRecognizers.Sort();
            _instance._gestureRecognizers.Reverse();
        }
    }

    private void addTouchesUnityForgotToEndToLiveTouchesList()
    {
        for (int i = 0; i < this._touchCache.Length; i++)
        {
            if (this._touchCache[i].phase != TouchPhase.Ended)
            {
                Debug.LogWarning("found touch Unity forgot to end with phase: " + this._touchCache[i].phase);
                this._touchCache[i].phase = TouchPhase.Ended;
                this._liveTouches.Add(this._touchCache[i]);
            }
        }
    }

    private void Awake()
    {
        this._touchCache = new TKTouch[this.maxTouchesToProcess];
        for (int i = 0; i < this.maxTouchesToProcess; i++)
        {
            this._touchCache[i] = new TKTouch(i);
        }
    }

    private void internalUpdateTouches()
    {
        if (Input.touchCount > 0)
        {
            this._shouldCheckForLostTouches = true;
            int num = Mathf.Min(Input.touches.Length, this.maxTouchesToProcess);
            for (int i = 0; i < num; i++)
            {
                Touch touch = Input.touches[i];
                if (touch.fingerId < this.maxTouchesToProcess)
                {
                    this._liveTouches.Add(this._touchCache[touch.fingerId].populateWithTouch(touch));
                }
            }
        }
        else if (this._shouldCheckForLostTouches)
        {
            this.addTouchesUnityForgotToEndToLiveTouchesList();
            this._shouldCheckForLostTouches = false;
        }
        if (this._liveTouches.Count > 0)
        {
            for (int j = 0; j < this._gestureRecognizers.Count; j++)
            {
                this._gestureRecognizers[j].recognizeTouches(this._liveTouches);
            }
            this._liveTouches.Clear();
        }
    }

    private void OnApplicationQuit()
    {
        _instance = null;
        UnityEngine.Object.Destroy(base.gameObject);
    }

    public static void removeAllGestureRecognizers()
    {
        if (_instance != null)
        {
            instance._gestureRecognizers.Clear();
        }
    }

    public static void removeGestureRecognizer(TKAbstractGestureRecognizer recognizer)
    {
        if (_instance != null)
        {
            if (!_instance._gestureRecognizers.Contains(recognizer))
            {
                Debug.LogError("Trying to remove gesture recognizer that has not been added: " + recognizer);
            }
            else
            {
                recognizer.reset();
                instance._gestureRecognizers.Remove(recognizer);
            }
        }
    }

    public static void setupPixelsToUnityUnitsMultiplierWithCamera(Camera cam)
    {
        if (!cam.orthographic)
        {
            Debug.LogError("Attempting to setup unity pixel-to-units modifier with a non-orthographic camera");
        }
        else
        {
            Vector2 vector = new Vector2((cam.aspect * cam.orthographicSize) * 2f, cam.orthographicSize * 2f);
            _instance.pixelsToUnityUnitsMultiplier = new Vector2(vector.x / ((float) Screen.width), vector.y / ((float) Screen.height));
        }
    }

    protected void setupRuntimeScale()
    {
        _instance.runtimeScaleModifier = new Vector2(((float) Screen.width) / _instance.designTimeResolution.x, ((float) Screen.height) / _instance.designTimeResolution.y);
        _instance.runtimeDistanceModifier = (_instance.runtimeScaleModifier.x + _instance.runtimeScaleModifier.y) / 2f;
        if (!_instance.autoScaleRectsAndDistances)
        {
            _instance.runtimeScaleModifier = Vector2.one;
            _instance.runtimeDistanceModifier = 1f;
        }
    }

    private void Update()
    {
        if (this.shouldAutoUpdateTouches)
        {
            this.internalUpdateTouches();
        }
    }

    public static void updateTouches()
    {
        if (_instance != null)
        {
            _instance.internalUpdateTouches();
        }
    }

    public Vector2 designTimeResolution
    {
        get
        {
            return this._designTimeResolution;
        }
        set
        {
            this._designTimeResolution = value;
            this.setupRuntimeScale();
        }
    }

    public static TouchKit instance
    {
        get
        {
            if (_instance == null)
            {
                Camera camera;
                _instance = UnityEngine.Object.FindObjectOfType(typeof(TouchKit)) as TouchKit;
                if (_instance == null)
                {
                    GameObject target = new GameObject("TouchKit");
                    _instance = target.AddComponent<TouchKit>();
                    UnityEngine.Object.DontDestroyOnLoad(target);
                }
                Camera main = Camera.main;
                if (main != null)
                {
                    camera = main;
                }
                else
                {
                    camera = Camera.allCameras[0];
                }
                if (camera.orthographic)
                {
                    setupPixelsToUnityUnitsMultiplierWithCamera(camera);
                }
                else
                {
                    _instance.pixelsToUnityUnitsMultiplier = Vector2.one;
                }
                _instance.setupRuntimeScale();
            }
            return _instance;
        }
    }

    public Vector2 pixelsToUnityUnitsMultiplier
    {
        [CompilerGenerated]
        get
        {
            return this.<pixelsToUnityUnitsMultiplier>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<pixelsToUnityUnitsMultiplier>k__BackingField = value;
        }
    }

    public float runtimeDistanceModifier
    {
        [CompilerGenerated]
        get
        {
            return this.<runtimeDistanceModifier>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<runtimeDistanceModifier>k__BackingField = value;
        }
    }

    public Vector2 runtimeScaleModifier
    {
        [CompilerGenerated]
        get
        {
            return this.<runtimeScaleModifier>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<runtimeScaleModifier>k__BackingField = value;
        }
    }

    public float ScreenPixelsPerCm
    {
        get
        {
            float num = 72f;
            num = 160f;
            return ((Screen.dpi != 0f) ? (Screen.dpi / 2.54f) : (num / 2.54f));
        }
    }
}

