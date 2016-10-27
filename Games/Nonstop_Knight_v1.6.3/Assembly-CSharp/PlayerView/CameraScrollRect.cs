namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class CameraScrollRect : MonoBehaviour
    {
        [CompilerGenerated]
        private bool <Initialized>k__BackingField;
        public Vector3 BoundsMax;
        public Vector3 BoundsMin;
        private Camera m_canvasCamera;
        private Transform m_canvasCameraTm;
        private List<RectTransform> m_contentChildTms = new List<RectTransform>();
        private Vector2[] m_cullingScreenTestPts = new Vector2[] { new Vector2(Screen.width * 0.5f, -Screen.height * 0.25f), new Vector2(Screen.width * 0.5f, 0f), new Vector2(Screen.width * 0.5f, Screen.height * 0.25f), new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), new Vector2(Screen.width * 0.5f, Screen.height * 0.75f), new Vector2(Screen.width * 0.5f, (float) Screen.height), new Vector2(Screen.width * 0.5f, Screen.height * 1.25f) };
        private Vector2 m_deltaScreenPosMovingAverage;
        private bool m_dragging;
        private Vector2 m_dragStartPos;
        private Dictionary<RectTransform, List<Graphic>> m_enabledChildGraphics = new Dictionary<RectTransform, List<Graphic>>();
        private Vector2 m_lastMouseScreenPos;
        private RectTransform m_rectTm;
        private Vector2 m_velocity;

        protected void Awake()
        {
            this.m_rectTm = base.GetComponent<RectTransform>();
        }

        private void cullGraphics()
        {
            for (int i = 0; i < this.m_contentChildTms.Count; i++)
            {
                RectTransform rect = this.m_contentChildTms[i];
                bool flag = false;
                for (int j = 0; j < this.m_cullingScreenTestPts.Length; j++)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(rect, this.m_cullingScreenTestPts[j], this.m_canvasCamera))
                    {
                        flag = true;
                        break;
                    }
                }
                List<Graphic> list = this.m_enabledChildGraphics[rect];
                for (int k = 0; k < list.Count; k++)
                {
                    list[k].enabled = flag;
                }
            }
        }

        public Vector3 getCameraPos()
        {
            return this.m_canvasCameraTm.position;
        }

        public void initialize(RectTransform viewRect, Camera canvasCamera)
        {
            this.m_canvasCamera = canvasCamera;
            this.m_canvasCameraTm = this.m_canvasCamera.transform;
            foreach (Transform transform in TransformExtensions.GetChildren(this.m_rectTm, false))
            {
                RectTransform component = transform.GetComponent<RectTransform>();
                this.m_contentChildTms.Add(component);
                Graphic[] componentsInChildren = component.GetComponentsInChildren<Graphic>(false);
                this.m_enabledChildGraphics[component] = new List<Graphic>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    if (componentsInChildren[i].isActiveAndEnabled)
                    {
                        this.m_enabledChildGraphics[component].Add(componentsInChildren[i]);
                    }
                }
            }
            this.Initialized = true;
        }

        public void reset()
        {
            this.m_dragging = false;
        }

        public void setCameraPos(Vector3 pos, bool limitToBounds, bool limitToBoundsWithLerping)
        {
            if (limitToBounds)
            {
                if (limitToBoundsWithLerping)
                {
                    pos.y = Mathf.Lerp(this.m_canvasCameraTm.position.y, Mathf.Clamp(pos.y, this.BoundsMin.y, this.BoundsMax.y), Time.unscaledDeltaTime * 10f);
                }
                else
                {
                    pos.y = Mathf.Clamp(pos.y, this.BoundsMin.y, this.BoundsMax.y);
                }
            }
            this.m_canvasCameraTm.position = pos;
        }

        public void setCameraPosY(float y)
        {
            Vector3 position = this.m_canvasCameraTm.position;
            position.y = y;
            this.setCameraPos(position, true, false);
        }

        public void update(float dt)
        {
            bool flag = Binder.InputSystem.UnityEventSystem.currentInputModule is TouchInputModule;
            if ((Binder.InputSystem.InputEnabled && Input.GetMouseButtonDown(0)) || ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)))
            {
                this.m_dragging = true;
                this.m_velocity = Vector2.zero;
                this.m_deltaScreenPosMovingAverage = Vector2.zero;
                if (flag)
                {
                    this.m_dragStartPos = Input.GetTouch(0).position;
                }
                else
                {
                    this.m_lastMouseScreenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    this.m_dragStartPos = this.m_lastMouseScreenPos;
                }
            }
            if (this.m_dragging)
            {
                bool flag2 = false;
                if ((flag && (Input.touchCount > 0)) && (Input.GetTouch(0).phase == TouchPhase.Moved))
                {
                    flag2 = true;
                }
                else if (!flag)
                {
                    flag2 = true;
                }
                if (flag2)
                {
                    Vector2 vector;
                    float num = 45f;
                    if (flag)
                    {
                        vector = -Input.GetTouch(0).deltaPosition;
                    }
                    else
                    {
                        Vector2 vector2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                        vector = this.m_lastMouseScreenPos - vector2;
                        this.m_lastMouseScreenPos = vector2;
                    }
                    this.m_deltaScreenPosMovingAverage = (Vector2) ((vector * 0.5f) + (this.m_deltaScreenPosMovingAverage * 0.5f));
                    float num3 = ((this.m_deltaScreenPosMovingAverage.y / ((float) Screen.height)) * num) / dt;
                    float num4 = 1f;
                    float num6 = (this.m_canvasCameraTm.position.y + num3) - this.BoundsMax.y;
                    if (num6 > 0f)
                    {
                        num4 = 1f - Mathf.Clamp01(num6 / 400f);
                    }
                    else
                    {
                        float num7 = this.BoundsMin.y - (this.m_canvasCameraTm.position.y + num3);
                        if (num7 > 0f)
                        {
                            num4 = 1f - Mathf.Clamp01(num7 / 400f);
                        }
                    }
                    num3 *= num4;
                    Vector3 pos = new Vector3(this.m_canvasCameraTm.position.x, this.m_canvasCameraTm.position.y + num3, this.m_canvasCameraTm.position.z);
                    this.m_velocity = Vector3.Lerp((Vector3) this.m_velocity, pos - this.m_canvasCameraTm.position, dt * 10f);
                    this.setCameraPos(pos, false, false);
                }
            }
            if (Input.GetMouseButtonUp(0) || ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended)))
            {
                Vector2 position;
                this.m_dragging = false;
                if (flag)
                {
                    position = Input.GetTouch(0).position;
                }
                else
                {
                    position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
                if (Mathf.Abs((float) (this.m_dragStartPos.y - position.y)) < (Screen.height * 0.05f))
                {
                    this.m_velocity = Vector2.zero;
                }
                else
                {
                    this.m_velocity = (Vector2) (this.m_velocity * 5f);
                }
            }
            if (!this.m_dragging)
            {
                this.m_velocity.y *= Mathf.Pow(0.135f, dt);
                if (Mathf.Abs(this.m_velocity.y) < 1f)
                {
                    this.m_velocity.y = 0f;
                }
                this.m_velocity.x = 0f;
                this.setCameraPos(this.m_canvasCameraTm.position + this.m_velocity, true, true);
            }
        }

        public bool Initialized
        {
            [CompilerGenerated]
            get
            {
                return this.<Initialized>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Initialized>k__BackingField = value;
            }
        }
    }
}

