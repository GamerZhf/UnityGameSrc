namespace Pathfinding.Examples
{
    using Pathfinding;
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_door_controller.php")]
    public class DoorController : MonoBehaviour
    {
        private Bounds bounds;
        public int closedtag = 1;
        private bool open;
        public int opentag = 1;
        public bool updateGraphsWithGUO = true;
        public float yOffset = 5f;

        private void OnGUI()
        {
            if (GUI.Button(new Rect(5f, this.yOffset, 100f, 22f), "Toggle Door"))
            {
                this.SetState(!this.open);
            }
        }

        public void SetState(bool open)
        {
            this.open = open;
            if (this.updateGraphsWithGUO)
            {
                GraphUpdateObject ob = new GraphUpdateObject(this.bounds);
                int num = !open ? this.closedtag : this.opentag;
                if (num > 0x1f)
                {
                    Debug.LogError("tag > 31");
                    return;
                }
                ob.modifyTag = true;
                ob.setTag = num;
                ob.updatePhysics = false;
                AstarPath.active.UpdateGraphs(ob);
            }
            if (open)
            {
                base.GetComponent<Animation>().Play("Open");
            }
            else
            {
                base.GetComponent<Animation>().Play("Close");
            }
        }

        public void Start()
        {
            this.bounds = base.GetComponent<Collider>().bounds;
            this.SetState(this.open);
        }
    }
}

