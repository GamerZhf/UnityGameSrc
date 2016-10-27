using System;
using UnityEngine;

[Serializable]
public class MechAnimationTest : MonoBehaviour
{
    public SignalSender footstepSignals;
    public AnimationClip idle;
    public Rigidbody rigid;
    public float turning;
    public AnimationClip turnLeft;
    public float turnOffset;
    public AnimationClip turnRight;
    public AnimationClip walk;
    public float walking;

    public override void FixedUpdate()
    {
        this.GetComponent<Animation>()[this.walk.name].speed = Mathf.Lerp((float) 1, this.GetComponent<Animation>()[this.walk.name].length / this.GetComponent<Animation>()[this.turnLeft.name].length, Mathf.Abs(this.turning));
        this.GetComponent<Animation>()[this.turnLeft.name].time = this.GetComponent<Animation>()[this.walk.name].time + this.turnOffset;
        this.GetComponent<Animation>()[this.turnRight.name].time = this.GetComponent<Animation>()[this.walk.name].time + this.turnOffset;
        this.rigid.velocity = (Vector3) ((this.rigid.transform.forward * 2.5f) * this.walking);
        this.rigid.angularVelocity = (Vector3) (((Vector3.up * this.turning) * 100) * 0.01745329f);
        float num = (this.rigid.angularVelocity.y * 57.29578f) / 100f;
        float num2 = this.rigid.velocity.magnitude / 2.5f;
        this.GetComponent<Animation>()[this.turnLeft.name].weight = Mathf.Clamp01(-num);
        this.GetComponent<Animation>()[this.turnRight.name].weight = Mathf.Clamp01(num);
        this.GetComponent<Animation>()[this.walk.name].weight = Mathf.Clamp01(num2);
    }

    public override void Main()
    {
    }

    public override void OnEnable()
    {
        this.GetComponent<Animation>()[this.idle.name].layer = 0;
        this.GetComponent<Animation>()[this.idle.name].weight = 1;
        this.GetComponent<Animation>()[this.idle.name].enabled = true;
        this.GetComponent<Animation>()[this.walk.name].layer = 1;
        this.GetComponent<Animation>()[this.turnLeft.name].layer = 1;
        this.GetComponent<Animation>()[this.turnRight.name].layer = 1;
        this.GetComponent<Animation>()[this.walk.name].weight = 1;
        this.GetComponent<Animation>()[this.turnLeft.name].weight = 0;
        this.GetComponent<Animation>()[this.turnRight.name].weight = 0;
        this.GetComponent<Animation>()[this.walk.name].enabled = true;
        this.GetComponent<Animation>()[this.turnLeft.name].enabled = true;
        this.GetComponent<Animation>()[this.turnRight.name].enabled = true;
    }

    public override void OnGUI()
    {
        GUILayout.Label("Walking (0 to 1): " + this.walking.ToString("0.00"), new GUILayoutOption[0]);
        GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width((float) 100) };
        this.walking = GUILayout.HorizontalSlider(this.walking, (float) 0, (float) 1, options);
        if (GUI.changed)
        {
            this.turning = Mathf.Clamp(Mathf.Abs(this.turning), (float) 0, 1 - this.walking) * Mathf.Sign(this.turning);
            GUI.changed = false;
        }
        GUILayout.Label("Turning (-1 to 1): " + this.turning.ToString("0.00"), new GUILayoutOption[0]);
        GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.Width((float) 100) };
        this.turning = GUILayout.HorizontalSlider(this.turning, (float) (-1), (float) 1, optionArray2);
        if (Mathf.Abs(this.turning) < 0.1f)
        {
            this.turning = 0;
        }
        if (GUI.changed)
        {
            this.walking = Mathf.Clamp(this.walking, (float) 0, 1 - Mathf.Abs(this.turning));
            GUI.changed = false;
        }
        GUILayout.Label("Offset to turning anims (-0.5 to 0.5): " + this.turnOffset.ToString("0.00"), new GUILayoutOption[0]);
        GUILayoutOption[] optionArray3 = new GUILayoutOption[] { GUILayout.Width((float) 100) };
        this.turnOffset = GUILayout.HorizontalSlider(this.turnOffset, -0.5f, 0.5f, optionArray3);
        if (Mathf.Abs(this.turnOffset) < 0.05f)
        {
            this.turnOffset = 0;
        }
    }
}

