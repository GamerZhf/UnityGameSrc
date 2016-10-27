using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class Decal : MonoBehaviour
{
    public LayerMask affectedLayers = -1;
    public Material material;
    public float maxAngle = 90f;
    public float pushDistance = 0.009f;
    public Sprite sprite;

    public Bounds GetBounds()
    {
        Vector3 lossyScale = base.transform.lossyScale;
        Vector3 lhs = (Vector3) (-lossyScale / 2f);
        Vector3 vector3 = (Vector3) (lossyScale / 2f);
        Vector3[] vectorArray = new Vector3[] { new Vector3(lhs.x, lhs.y, lhs.z), new Vector3(vector3.x, lhs.y, lhs.z), new Vector3(lhs.x, vector3.y, lhs.z), new Vector3(vector3.x, vector3.y, lhs.z), new Vector3(lhs.x, lhs.y, vector3.z), new Vector3(vector3.x, lhs.y, vector3.z), new Vector3(lhs.x, vector3.y, vector3.z), new Vector3(vector3.x, vector3.y, vector3.z) };
        for (int i = 0; i < 8; i++)
        {
            vectorArray[i] = base.transform.TransformDirection(vectorArray[i]);
        }
        lhs = vector3 = vectorArray[0];
        foreach (Vector3 vector4 in vectorArray)
        {
            lhs = Vector3.Min(lhs, vector4);
            vector3 = Vector3.Max(vector3, vector4);
        }
        return new Bounds(base.transform.position, vector3 - lhs);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = base.transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}

