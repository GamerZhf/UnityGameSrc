namespace Pathfinding
{
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_object_placer.php")]
    public class ObjectPlacer : MonoBehaviour
    {
        public bool direct;
        public GameObject go;
        public bool issueGUOs = true;

        public void PlaceObject()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity))
            {
                Vector3 point = hit.point;
                GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(this.go, point, Quaternion.identity);
                if (this.issueGUOs)
                {
                    GraphUpdateObject ob = new GraphUpdateObject(obj2.GetComponent<Collider>().bounds);
                    AstarPath.active.UpdateGraphs(ob);
                    if (this.direct)
                    {
                        AstarPath.active.FlushGraphUpdates();
                    }
                }
            }
        }

        public void RemoveObject()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity) && (!hit.collider.isTrigger && (hit.transform.gameObject.name != "Ground")))
            {
                Bounds b = hit.collider.bounds;
                UnityEngine.Object.Destroy(hit.collider);
                UnityEngine.Object.Destroy(hit.collider.gameObject);
                if (this.issueGUOs)
                {
                    GraphUpdateObject ob = new GraphUpdateObject(b);
                    AstarPath.active.UpdateGraphs(ob, 0f);
                    if (this.direct)
                    {
                        AstarPath.active.FlushGraphUpdates();
                    }
                }
            }
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown("p"))
            {
                this.PlaceObject();
            }
            if (Input.GetKeyDown("r"))
            {
                this.RemoveObject();
            }
        }
    }
}

