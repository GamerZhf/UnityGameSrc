using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKRotationRecognizer : TKAbstractGestureRecognizer
{
    protected float _firstRotation;
    protected float _initialRotation;
    protected float _previousRotation;
    public float deltaRotation;
    public float minimumRotationToRecognize;

    public event Action<TKRotationRecognizer> gestureCompleteEvent;

    public event Action<TKRotationRecognizer> gestureRecognizedEvent;

    public static float angleBetweenPoints(Vector2 position1, Vector2 position2)
    {
        Vector2 from = position2 - position1;
        Vector2 to = new Vector2(1f, 0f);
        float num = Vector2.Angle(from, to);
        if (Vector3.Cross((Vector3) from, (Vector3) to).z > 0f)
        {
            num = 360f - num;
        }
        return num;
    }

    internal override void fireRecognizedEvent()
    {
        if (this.gestureRecognizedEvent != null)
        {
            this.gestureRecognizedEvent(this);
        }
    }

    public override string ToString()
    {
        object[] args = new object[] { base.GetType(), base.state, base.touchLocation(), this.deltaRotation };
        return string.Format("[{0}] state: {1}, location: {2}, rotation: {3}", args);
    }

    internal override bool touchesBegan(List<TKTouch> touches)
    {
        if (base.state == TKGestureRecognizerState.Possible)
        {
            for (int i = 0; i < touches.Count; i++)
            {
                if (touches[i].phase == TouchPhase.Began)
                {
                    base._trackingTouches.Add(touches[i]);
                    if (base._trackingTouches.Count == 2)
                    {
                        break;
                    }
                }
            }
            if (base._trackingTouches.Count == 2)
            {
                if (this.minimumRotationToRecognize == 0f)
                {
                    this.deltaRotation = 0f;
                    this._previousRotation = angleBetweenPoints(base._trackingTouches[0].position, base._trackingTouches[1].position);
                    base.state = TKGestureRecognizerState.Began;
                }
                else
                {
                    this._firstRotation = angleBetweenPoints(base._trackingTouches[0].position, base._trackingTouches[1].position);
                }
            }
        }
        return false;
    }

    internal override void touchesEnded(List<TKTouch> touches)
    {
        for (int i = 0; i < touches.Count; i++)
        {
            if (touches[i].phase == TouchPhase.Ended)
            {
                base._trackingTouches.Remove(touches[i]);
            }
        }
        if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) && (this.gestureCompleteEvent != null))
        {
            this.gestureCompleteEvent(this);
        }
        if (base._trackingTouches.Count == 1)
        {
            base.state = TKGestureRecognizerState.Possible;
            this.deltaRotation = 0f;
        }
        else
        {
            base.state = TKGestureRecognizerState.FailedOrEnded;
            this._initialRotation = 0f;
        }
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        if ((base.state == TKGestureRecognizerState.Possible) && (base._trackingTouches.Count == 2))
        {
            float current = angleBetweenPoints(base._trackingTouches[0].position, base._trackingTouches[1].position);
            if (Mathf.Abs(Mathf.DeltaAngle(current, this._firstRotation)) > this.minimumRotationToRecognize)
            {
                this._initialRotation = current;
                this.deltaRotation = 0f;
                this._previousRotation = angleBetweenPoints(base._trackingTouches[0].position, base._trackingTouches[1].position);
                base.state = TKGestureRecognizerState.Began;
            }
        }
        if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) || (base.state == TKGestureRecognizerState.Began))
        {
            float num3 = angleBetweenPoints(base._trackingTouches[0].position, base._trackingTouches[1].position);
            this.deltaRotation = Mathf.DeltaAngle(num3, this._previousRotation);
            this._previousRotation = num3;
            base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
        }
    }

    public float accumulatedRotation
    {
        get
        {
            if (base._trackingTouches.Count == 2)
            {
                return Mathf.DeltaAngle(angleBetweenPoints(base._trackingTouches[0].position, base._trackingTouches[1].position), this._initialRotation);
            }
            return 0f;
        }
    }
}

