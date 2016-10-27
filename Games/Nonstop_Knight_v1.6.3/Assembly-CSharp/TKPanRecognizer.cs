using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class TKPanRecognizer : TKAbstractGestureRecognizer
{
    private Vector2 _endPoint;
    private float _minDistanceToPanCm;
    private Vector2 _previousLocation;
    private Vector2 _startPoint;
    public Vector2 deltaTranslation;
    public float deltaTranslationCm;
    public int maximumNumberOfTouches = 2;
    public int minimumNumberOfTouches = 1;
    private float totalDeltaMovementInCm;

    public event Action<TKPanRecognizer> gestureCompleteEvent;

    public event Action<TKPanRecognizer> gestureRecognizedEvent;

    public TKPanRecognizer([Optional, DefaultParameterValue(0.5f)] float minPanDistanceCm)
    {
        this._minDistanceToPanCm = minPanDistanceCm;
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
        object[] args = new object[] { base.GetType(), base.state, base.touchLocation(), this.deltaTranslation };
        return string.Format("[{0}] state: {1}, location: {2}, deltaTranslation: {3}", args);
    }

    internal override bool touchesBegan(List<TKTouch> touches)
    {
        if ((base._trackingTouches.Count + touches.Count) > this.maximumNumberOfTouches)
        {
            base.state = TKGestureRecognizerState.FailedOrEnded;
            return false;
        }
        if ((base.state == TKGestureRecognizerState.Possible) || (((base.state == TKGestureRecognizerState.Began) || (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing)) && (base._trackingTouches.Count < this.maximumNumberOfTouches)))
        {
            for (int i = 0; i < touches.Count; i++)
            {
                if (touches[i].phase == TouchPhase.Began)
                {
                    base._trackingTouches.Add(touches[i]);
                    this._startPoint = touches[0].position;
                    if (base._trackingTouches.Count == this.maximumNumberOfTouches)
                    {
                        break;
                    }
                }
            }
            if ((base._trackingTouches.Count >= this.minimumNumberOfTouches) && (base._trackingTouches.Count <= this.maximumNumberOfTouches))
            {
                this._previousLocation = base.touchLocation();
                if (base.state != TKGestureRecognizerState.RecognizedAndStillRecognizing)
                {
                    this.totalDeltaMovementInCm = 0f;
                    base.state = TKGestureRecognizerState.Began;
                }
            }
        }
        return false;
    }

    internal override void touchesEnded(List<TKTouch> touches)
    {
        this._endPoint = base.touchLocation();
        for (int i = 0; i < touches.Count; i++)
        {
            if (touches[i].phase == TouchPhase.Ended)
            {
                base._trackingTouches.Remove(touches[i]);
            }
        }
        if (base._trackingTouches.Count >= this.minimumNumberOfTouches)
        {
            this._previousLocation = base.touchLocation();
            base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
        }
        else
        {
            if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) && (this.gestureCompleteEvent != null))
            {
                this.gestureCompleteEvent(this);
            }
            base.state = TKGestureRecognizerState.FailedOrEnded;
        }
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        if ((base._trackingTouches.Count >= this.minimumNumberOfTouches) && (base._trackingTouches.Count <= this.maximumNumberOfTouches))
        {
            Vector2 vector = base.touchLocation();
            this.deltaTranslation = vector - this._previousLocation;
            this.deltaTranslationCm = this.deltaTranslation.magnitude / TouchKit.instance.ScreenPixelsPerCm;
            this._previousLocation = vector;
            if (base.state == TKGestureRecognizerState.Began)
            {
                this.totalDeltaMovementInCm += this.deltaTranslationCm;
                if (Math.Abs(this.totalDeltaMovementInCm) >= this._minDistanceToPanCm)
                {
                    base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
                }
            }
            else
            {
                base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
            }
        }
    }

    public Vector2 endPoint
    {
        get
        {
            return this._endPoint;
        }
    }

    public Vector2 startPoint
    {
        get
        {
            return this._startPoint;
        }
    }
}

