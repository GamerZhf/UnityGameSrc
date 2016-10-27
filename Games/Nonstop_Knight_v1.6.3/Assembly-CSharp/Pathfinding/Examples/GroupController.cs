namespace Pathfinding.Examples
{
    using Pathfinding;
    using Pathfinding.RVO;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_group_controller.php")]
    public class GroupController : MonoBehaviour
    {
        public bool adjustCamera = true;
        private Camera cam;
        private Vector2 end;
        private const float rad2Deg = 57.29578f;
        private List<RVOExampleAgent> selection = new List<RVOExampleAgent>();
        public GUIStyle selectionBox;
        private Simulator sim;
        private Vector2 start;
        private bool wasDown;

        public Color GetColor(float angle)
        {
            return AstarMath.HSVToRGB(angle * 57.29578f, 0.8f, 0.6f);
        }

        private void OnGUI()
        {
            if (((Event.current.type == UnityEngine.EventType.MouseUp) && (Event.current.button == 0)) && !Input.GetKey(KeyCode.A))
            {
                this.Select(this.start, this.end);
                this.wasDown = false;
            }
            if ((Event.current.type == UnityEngine.EventType.MouseDrag) && (Event.current.button == 0))
            {
                this.end = Event.current.mousePosition;
                if (!this.wasDown)
                {
                    this.start = this.end;
                    this.wasDown = true;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                this.wasDown = false;
            }
            if (this.wasDown)
            {
                float left = Mathf.Min(this.start.x, this.end.x);
                float top = Mathf.Min(this.start.y, this.end.y);
                float right = Mathf.Max(this.start.x, this.end.x);
                Rect position = Rect.MinMaxRect(left, top, right, Mathf.Max(this.start.y, this.end.y));
                if ((position.width > 4f) && (position.height > 4f))
                {
                    GUI.Box(position, string.Empty, this.selectionBox);
                }
            }
        }

        public void Order()
        {
            RaycastHit hit;
            if (Physics.Raycast(this.cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                float num = 0f;
                for (int i = 0; i < this.selection.Count; i++)
                {
                    num += this.selection[i].GetComponent<RVOController>().radius;
                }
                float num3 = num / 3.141593f;
                num3 *= 2f;
                for (int j = 0; j < this.selection.Count; j++)
                {
                    float f = (6.283185f * j) / ((float) this.selection.Count);
                    Vector3 target = hit.point + ((Vector3) (new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f)) * num3));
                    this.selection[j].SetTarget(target);
                    this.selection[j].SetColor(this.GetColor(f));
                    this.selection[j].RecalculatePath();
                }
            }
        }

        public void Select(Vector2 _start, Vector2 _end)
        {
            _start.y = Screen.height - _start.y;
            _end.y = Screen.height - _end.y;
            Vector2 vector = Vector2.Min(_start, _end);
            Vector2 vector2 = Vector2.Max(_start, _end);
            Vector2 vector4 = vector2 - vector;
            if (vector4.sqrMagnitude >= 16f)
            {
                this.selection.Clear();
                RVOExampleAgent[] agentArray = UnityEngine.Object.FindObjectsOfType(typeof(RVOExampleAgent)) as RVOExampleAgent[];
                for (int i = 0; i < agentArray.Length; i++)
                {
                    Vector2 vector3 = this.cam.WorldToScreenPoint(agentArray[i].transform.position);
                    if (((vector3.x > vector.x) && (vector3.y > vector.y)) && ((vector3.x < vector2.x) && (vector3.y < vector2.y)))
                    {
                        this.selection.Add(agentArray[i]);
                    }
                }
            }
        }

        public void Start()
        {
            this.cam = Camera.main;
            RVOSimulator simulator = UnityEngine.Object.FindObjectOfType(typeof(RVOSimulator)) as RVOSimulator;
            if (simulator == null)
            {
                base.enabled = false;
                throw new Exception("No RVOSimulator in the scene. Please add one");
            }
            this.sim = simulator.GetSimulator();
        }

        public void Update()
        {
            if (Screen.fullScreen && (Screen.width != Screen.resolutions[Screen.resolutions.Length - 1].width))
            {
                Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width, Screen.resolutions[Screen.resolutions.Length - 1].height, true);
            }
            if (this.adjustCamera)
            {
                List<Agent> agents = this.sim.GetAgents();
                float num = 0f;
                for (int i = 0; i < agents.Count; i++)
                {
                    float num3 = Mathf.Max(Mathf.Abs(agents[i].InterpolatedPosition.x), Mathf.Abs(agents[i].InterpolatedPosition.z));
                    if (num3 > num)
                    {
                        num = num3;
                    }
                }
                float a = num / Mathf.Tan((this.cam.fieldOfView * 0.01745329f) / 2f);
                float b = num / Mathf.Tan(Mathf.Atan(Mathf.Tan((this.cam.fieldOfView * 0.01745329f) / 2f) * this.cam.aspect));
                this.cam.transform.position = Vector3.Lerp(this.cam.transform.position, new Vector3(0f, Mathf.Max(a, b) * 1.1f, 0f), Time.smoothDeltaTime * 2f);
            }
            if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                this.Order();
            }
        }
    }
}

