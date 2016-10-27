using System;
using UnityEngine;

[Serializable]
public class SpiderAnimationTest : MonoBehaviour
{
    public float angle;
    public AnimationClip backAnim;
    public AnimationClip forwardAnim;
    public AnimationClip leftAnim;
    public AnimationClip rightAnim;
    public Rigidbody rigid;
    private Transform tr;
    public float walking;

    public static float HorizontalAngle(Vector3 direction)
    {
        return (Mathf.Atan2(direction.x, direction.z) * 57.29578f);
    }

    public override void Main()
    {
    }

    public override void OnEnable()
    {
        this.tr = this.rigid.transform;
        this.GetComponent<Animation>()[this.forwardAnim.name].layer = 1;
        this.GetComponent<Animation>()[this.forwardAnim.name].enabled = true;
        this.GetComponent<Animation>()[this.backAnim.name].layer = 1;
        this.GetComponent<Animation>()[this.backAnim.name].enabled = true;
        this.GetComponent<Animation>()[this.leftAnim.name].layer = 1;
        this.GetComponent<Animation>()[this.leftAnim.name].enabled = true;
        this.GetComponent<Animation>()[this.rightAnim.name].layer = 1;
        this.GetComponent<Animation>()[this.rightAnim.name].enabled = true;
        this.GetComponent<Animation>().SyncLayer(1);
    }

    public override void OnGUI()
    {
        GUILayout.Label("Angle (0 to 360): " + this.angle.ToString("0.00"), new GUILayoutOption[0]);
        GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width((float) 200) };
        this.angle = GUILayout.HorizontalSlider(this.angle, (float) 0, (float) 360, options);
        for (int i = 0; i <= 360; i += 0x2d)
        {
            if (Mathf.Abs((float) (this.angle - i)) < 10)
            {
                this.angle = i;
            }
        }
        GUILayout.Label("Walking (0 to 1): " + this.walking.ToString("0.00"), new GUILayoutOption[0]);
        GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.Width((float) 100) };
        this.walking = GUILayout.HorizontalSlider(this.walking, (float) 0, (float) 1, optionArray2);
    }

    public override void Update()
    {
        this.rigid.velocity = (Vector3) (((Quaternion.Euler((float) 0, this.angle, (float) 0) * this.rigid.transform.forward) * 2.4f) * this.walking);
        Vector3 velocity = this.rigid.velocity;
        velocity.y = 0;
        float num = velocity.magnitude / 2.4f;
        this.GetComponent<Animation>()[this.forwardAnim.name].speed = num;
        this.GetComponent<Animation>()[this.rightAnim.name].speed = num;
        this.GetComponent<Animation>()[this.backAnim.name].speed = num;
        this.GetComponent<Animation>()[this.leftAnim.name].speed = num;
        if (velocity != Vector3.zero)
        {
            float num2 = Mathf.DeltaAngle(HorizontalAngle(this.tr.forward), HorizontalAngle(this.rigid.velocity));
            float num3 = new float();
            if (num2 < -90)
            {
                num3 = Mathf.InverseLerp((float) (-180), (float) (-90), num2);
                this.GetComponent<Animation>()[this.forwardAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.rightAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.backAnim.name].weight = 1 - num3;
                this.GetComponent<Animation>()[this.leftAnim.name].weight = 1;
            }
            else if (num2 < 0)
            {
                num3 = Mathf.InverseLerp((float) (-90), (float) 0, num2);
                this.GetComponent<Animation>()[this.forwardAnim.name].weight = num3;
                this.GetComponent<Animation>()[this.rightAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.backAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.leftAnim.name].weight = 1 - num3;
            }
            else if (num2 < 90)
            {
                num3 = Mathf.InverseLerp((float) 0, (float) 90, num2);
                this.GetComponent<Animation>()[this.forwardAnim.name].weight = 1 - num3;
                this.GetComponent<Animation>()[this.rightAnim.name].weight = num3;
                this.GetComponent<Animation>()[this.backAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.leftAnim.name].weight = 0;
            }
            else
            {
                num3 = Mathf.InverseLerp((float) 90, (float) 180, num2);
                this.GetComponent<Animation>()[this.forwardAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.rightAnim.name].weight = 1 - num3;
                this.GetComponent<Animation>()[this.backAnim.name].weight = num3;
                this.GetComponent<Animation>()[this.leftAnim.name].weight = 0;
            }
        }
    }
}

