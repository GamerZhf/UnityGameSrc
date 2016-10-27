using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKTapRecognizer : TKAbstractGestureRecognizer
{
    private float _maxDeltaMovementForTapConsideration;
    private float _maxDurationForTapConsideration;
    private int _preformedTapsCount;
    private float _touchBeganTime;
    public int numberOfTapsRequired;
    public int numberOfTouchesRequired;

    public event Action<TKTapRecognizer> gestureRecognizedEvent;

    public TKTapRecognizer() : this(0.5f, 1f)
    {
    }

    public TKTapRecognizer(float maxDurationForTapConsideration, float maxDeltaMovementForTapConsiderationCm)
    {
        this.numberOfTapsRequired = 1;
        this.numberOfTouchesRequired = 1;
        this._maxDurationForTapConsideration = 0.5f;
        this._maxDeltaMovementForTapConsideration = 1f;
        this._maxDurationForTapConsideration = maxDurationForTapConsideration;
        this._maxDeltaMovementForTapConsideration = maxDeltaMovementForTapConsiderationCm;
    }

    internal override void fireRecognizedEvent()
    {
        if (this.gestureRecognizedEvent != null)
        {
            this.gestureRecognizedEvent(this);
        }
    }

    internal override bool touchesBegan(List<TKTouch> touches)
    {
        if (((Time.time > (this._touchBeganTime + this._maxDurationForTapConsideration)) && (this._preformedTapsCount != 0)) && (this._preformedTapsCount < this.numberOfTapsRequired))
        {
            base.state = TKGestureRecognizerState.FailedOrEnded;
        }
        if (base.state == TKGestureRecognizerState.Possible)
        {
            for (int i = 0; i < touches.Count; i++)
            {
                if (touches[i].phase == TouchPhase.Began)
                {
                    base._trackingTouches.Add(touches[i]);
                    if (base._trackingTouches.Count == this.numberOfTouchesRequired)
                    {
                        break;
                    }
                }
            }
            if (base._trackingTouches.Count == this.numberOfTouchesRequired)
            {
                this._touchBeganTime = Time.time;
                this._preformedTapsCount = 0;
                base.state = TKGestureRecognizerState.Began;
                return true;
            }
        }
        return false;
    }

    internal override void touchesEnded(List<TKTouch> touches)
    {
        if ((base.state == TKGestureRecognizerState.Began) && (Time.time <= (this._touchBeganTime + this._maxDurationForTapConsideration)))
        {
            this._preformedTapsCount++;
            if (this._preformedTapsCount == this.numberOfTapsRequired)
            {
                base.state = TKGestureRecognizerState.Recognized;
            }
        }
        else
        {
            base.state = TKGestureRecognizerState.FailedOrEnded;
        }
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        if (base.state == TKGestureRecognizerState.Began)
        {
            for (int i = 0; i < touches.Count; i++)
            {
                if (((Math.Abs((float) (touches[i].position.x - touches[i].startPosition.x)) / TouchKit.instance.ScreenPixelsPerCm) > this._maxDeltaMovementForTapConsideration) || ((Math.Abs((float) (touches[i].position.y - touches[i].startPosition.y)) / TouchKit.instance.ScreenPixelsPerCm) > this._maxDeltaMovementForTapConsideration))
                {
                    base.state = TKGestureRecognizerState.FailedOrEnded;
                    break;
                }
            }
        }
    }
}

