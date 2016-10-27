using System;
using UnityEngine;

[Serializable]
public class TriggerOnMouseOrJoystick : MonoBehaviour
{
    private Joystick[] joysticks;
    public SignalSender mouseDownSignals;
    public SignalSender mouseUpSignals;
    private bool state;

    public override void Main()
    {
    }

    public override void Start()
    {
        this.joysticks = ((Joystick[]) UnityEngine.Object.FindObjectsOfType(typeof(Joystick))) as Joystick[];
    }

    public override void Update()
    {
        if (!this.state && (this.joysticks[0].tapCount > 0))
        {
            this.mouseDownSignals.SendSignals(this);
            this.state = true;
        }
        else if (this.joysticks[0].tapCount <= 0)
        {
            this.mouseUpSignals.SendSignals(this);
            this.state = false;
        }
    }
}

