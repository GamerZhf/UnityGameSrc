using System;
using UnityEngine;

public class TKTouch
{
    public Vector2 deltaPosition;
    public float deltaTime;
    public readonly int fingerId;
    public TouchPhase phase = TouchPhase.Ended;
    public Vector2 position;
    public Vector2 startPosition;
    public int tapCount;

    public TKTouch(int fingerId)
    {
        this.fingerId = fingerId;
    }

    public TKTouch populateWithTouch(Touch touch)
    {
        this.position = touch.position;
        this.deltaPosition = touch.deltaPosition;
        this.deltaTime = touch.deltaTime;
        this.tapCount = touch.tapCount;
        if (touch.phase == TouchPhase.Began)
        {
            this.startPosition = this.position;
        }
        if (touch.phase == TouchPhase.Canceled)
        {
            this.phase = TouchPhase.Ended;
        }
        else
        {
            this.phase = touch.phase;
        }
        return this;
    }

    public override string ToString()
    {
        return string.Format("[TKTouch] fingerId: {0}, phase: {1}, position: {2}", this.fingerId, this.phase, this.position);
    }

    public Vector2 previousPosition
    {
        get
        {
            return (this.position - this.deltaPosition);
        }
    }
}

