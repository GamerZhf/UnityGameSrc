using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKPinchRecognizer : TKAbstractGestureRecognizer
{
    private float _firstDistance;
    private float _intialDistance;
    private float _previousDistance;
    public float deltaScale;
    public float minimumScaleDistanceToRecognize;

    public event Action<TKPinchRecognizer> gestureCompleteEvent;

    public event Action<TKPinchRecognizer> gestureRecognizedEvent;

    private float distanceBetweenTrackedTouches()
    {
        float b = Vector2.Distance(base._trackingTouches[0].position, base._trackingTouches[1].position);
        return (Mathf.Max(0.0001f, b) / TouchKit.instance.ScreenPixelsPerCm);
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
        return string.Format("[{0}] state: {1}, deltaScale: {2}", base.GetType(), base.state, this.deltaScale);
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
                this._firstDistance = this.distanceBetweenTrackedTouches();
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
            this.deltaScale = 0f;
        }
        else
        {
            base.state = TKGestureRecognizerState.FailedOrEnded;
        }
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        if (base._trackingTouches.Count == 2)
        {
            if (base.state == TKGestureRecognizerState.Possible)
            {
                if (Mathf.Abs((float) (this.distanceBetweenTrackedTouches() - this._firstDistance)) >= this.minimumScaleDistanceToRecognize)
                {
                    this.deltaScale = 0f;
                    this._intialDistance = this.distanceBetweenTrackedTouches();
                    this._previousDistance = this._intialDistance;
                    base.state = TKGestureRecognizerState.Began;
                }
            }
            else if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) || (base.state == TKGestureRecognizerState.Began))
            {
                float num = this.distanceBetweenTrackedTouches();
                this.deltaScale = (num - this._previousDistance) / this._intialDistance;
                this._previousDistance = num;
                base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
            }
        }
    }

    public float accumulatedScale
    {
        get
        {
            return (this.distanceBetweenTrackedTouches() / this._intialDistance);
        }
    }
}

