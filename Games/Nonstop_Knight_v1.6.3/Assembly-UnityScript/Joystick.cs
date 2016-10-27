using System;
using UnityEngine;

[Serializable, RequireComponent(typeof(GUITexture))]
public class Joystick : MonoBehaviour
{
    public float deadZone;
    private Rect defaultRect;
    [NonSerialized]
    private static bool enumeratedJoysticks;
    private Vector2 fingerDownPos;
    private float fingerDownTime;
    private float firstDeltaTime = 0.5f;
    private GUITexture gui;
    private Boundary guiBoundary = new Boundary();
    private Vector2 guiCenter;
    private Vector2 guiTouchOffset;
    [NonSerialized]
    private static Joystick[] joysticks;
    private int lastFingerId = -1;
    public bool normalize;
    public Vector2 position;
    public int tapCount;
    [NonSerialized]
    private static float tapTimeDelta = 0.3f;
    private float tapTimeWindow;
    public bool touchPad;
    public Rect touchZone;

    public override void Disable()
    {
        this.gameObject.SetActive(false);
        enumeratedJoysticks = false;
    }

    public override bool IsFingerDown()
    {
        return (this.lastFingerId != -1);
    }

    public override void LatchedFinger(int fingerId)
    {
        if (this.lastFingerId == fingerId)
        {
            this.ResetJoystick();
        }
    }

    public override void Main()
    {
    }

    public override void ResetJoystick()
    {
        this.gui.pixelInset = this.defaultRect;
        this.lastFingerId = -1;
        this.position = Vector2.zero;
        this.fingerDownPos = Vector2.zero;
        if (this.touchPad)
        {
            float num;
            Color color;
            float single1 = num = 0.025f;
            Color color1 = color = this.gui.color;
            float single2 = color.a = num;
            Color color3 = this.gui.color = color;
        }
    }

    public override void Start()
    {
        float num;
        Vector3 vector;
        float num2;
        Vector3 vector2;
        this.gui = this.GetComponent<GUITexture>();
        this.defaultRect = this.gui.pixelInset;
        this.defaultRect.x += this.transform.position.x * Screen.width;
        this.defaultRect.y += this.transform.position.y * Screen.height;
        float single1 = num = 0;
        Vector3 vector1 = vector = this.transform.position;
        float single2 = vector.x = num;
        Vector3 vector7 = this.transform.position = vector;
        float single3 = num2 = 0;
        Vector3 vector8 = vector2 = this.transform.position;
        float single4 = vector2.y = num2;
        Vector3 vector9 = this.transform.position = vector2;
        if (this.touchPad)
        {
            if (this.gui.texture != null)
            {
                this.touchZone = this.defaultRect;
            }
        }
        else
        {
            this.guiTouchOffset.x = this.defaultRect.width * 0.5f;
            this.guiTouchOffset.y = this.defaultRect.height * 0.5f;
            this.guiCenter.x = this.defaultRect.x + this.guiTouchOffset.x;
            this.guiCenter.y = this.defaultRect.y + this.guiTouchOffset.y;
            this.guiBoundary.min.x = this.defaultRect.x - this.guiTouchOffset.x;
            this.guiBoundary.max.x = this.defaultRect.x + this.guiTouchOffset.x;
            this.guiBoundary.min.y = this.defaultRect.y - this.guiTouchOffset.y;
            this.guiBoundary.max.y = this.defaultRect.y + this.guiTouchOffset.y;
        }
    }

    public override void Update()
    {
        if (!enumeratedJoysticks)
        {
            Joystick.joysticks = ((Joystick[]) UnityEngine.Object.FindObjectsOfType(typeof(Joystick))) as Joystick[];
            enumeratedJoysticks = true;
        }
        int touchCount = Input.touchCount;
        if (this.tapTimeWindow > 0)
        {
            this.tapTimeWindow -= Time.deltaTime;
        }
        else
        {
            this.tapCount = 0;
        }
        if (touchCount == 0)
        {
            this.ResetJoystick();
        }
        else
        {
            for (int i = 0; i < touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 vector = touch.position - this.guiTouchOffset;
                bool flag = false;
                if (this.touchPad)
                {
                    if (this.touchZone.Contains(touch.position))
                    {
                        flag = true;
                    }
                }
                else if (this.gui.HitTest((Vector3) touch.position))
                {
                    flag = true;
                }
                if (flag && ((this.lastFingerId == -1) || (this.lastFingerId != touch.fingerId)))
                {
                    if (this.touchPad)
                    {
                        float num6;
                        Color color;
                        float single1 = num6 = 0.15f;
                        Color color1 = color = this.gui.color;
                        float single2 = color.a = num6;
                        Color color3 = this.gui.color = color;
                        this.lastFingerId = touch.fingerId;
                        this.fingerDownPos = touch.position;
                        this.fingerDownTime = Time.time;
                    }
                    this.lastFingerId = touch.fingerId;
                    if (this.tapTimeWindow > 0)
                    {
                        this.tapCount++;
                    }
                    else
                    {
                        this.tapCount = 1;
                        this.tapTimeWindow = tapTimeDelta;
                    }
                    int index = 0;
                    Joystick[] joysticks = Joystick.joysticks;
                    int length = joysticks.Length;
                    while (index < length)
                    {
                        if ((joysticks[index] != null) && (joysticks[index] != this))
                        {
                            joysticks[index].LatchedFinger(touch.fingerId);
                        }
                        index++;
                    }
                }
                if (this.lastFingerId == touch.fingerId)
                {
                    if (touch.tapCount > this.tapCount)
                    {
                        this.tapCount = touch.tapCount;
                    }
                    if (this.touchPad)
                    {
                        this.position.x = Mathf.Clamp((touch.position.x - this.fingerDownPos.x) / (this.touchZone.width / ((float) 2)), (float) (-1), (float) 1);
                        this.position.y = Mathf.Clamp((touch.position.y - this.fingerDownPos.y) / (this.touchZone.height / ((float) 2)), (float) (-1), (float) 1);
                    }
                    else
                    {
                        this.position.x = (touch.position.x - this.guiCenter.x) / this.guiTouchOffset.x;
                        this.position.y = (touch.position.y - this.guiCenter.y) / this.guiTouchOffset.y;
                    }
                    if ((touch.phase == TouchPhase.Ended) || (touch.phase == TouchPhase.Canceled))
                    {
                        this.ResetJoystick();
                    }
                }
            }
        }
        float magnitude = this.position.magnitude;
        if (magnitude < this.deadZone)
        {
            this.position = Vector2.zero;
        }
        else if (magnitude > 1)
        {
            this.position = (Vector2) (this.position / magnitude);
        }
        else if (this.normalize)
        {
            this.position = (Vector2) ((this.position / magnitude) * Mathf.InverseLerp(magnitude, this.deadZone, (float) 1));
        }
        if (!this.touchPad)
        {
            float num7;
            Rect rect;
            float num8;
            Rect rect2;
            float single3 = num7 = ((this.position.x - 1) * this.guiTouchOffset.x) + this.guiCenter.x;
            Rect rect1 = rect = this.gui.pixelInset;
            rect.x = num7;
            Rect rect5 = this.gui.pixelInset = rect;
            float single4 = num8 = ((this.position.y - 1) * this.guiTouchOffset.y) + this.guiCenter.y;
            Rect rect6 = rect2 = this.gui.pixelInset;
            rect2.y = num8;
            Rect rect7 = this.gui.pixelInset = rect2;
        }
    }
}

