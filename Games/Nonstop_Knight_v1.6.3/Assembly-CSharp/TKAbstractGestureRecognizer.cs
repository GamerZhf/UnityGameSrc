using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TKAbstractGestureRecognizer : IComparable<TKAbstractGestureRecognizer>
{
    private bool _sentTouchesBegan;
    private bool _sentTouchesEnded;
    private bool _sentTouchesMoved;
    private TKGestureRecognizerState _state;
    private List<TKTouch> _subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer = new List<TKTouch>();
    protected List<TKTouch> _trackingTouches = new List<TKTouch>();
    protected bool alwaysSendTouchesMoved;
    public TKRect? boundaryFrame;
    public bool enabled = true;
    public uint zIndex;

    protected TKAbstractGestureRecognizer()
    {
    }

    public int CompareTo(TKAbstractGestureRecognizer other)
    {
        return this.zIndex.CompareTo(other.zIndex);
    }

    internal abstract void fireRecognizedEvent();
    internal bool isTouchWithinBoundaryFrame(TKTouch touch)
    {
        return (!this.boundaryFrame.HasValue || this.boundaryFrame.Value.contains(touch.position));
    }

    protected bool isTrackingAnyTouch(List<TKTouch> touches)
    {
        for (int i = 0; i < touches.Count; i++)
        {
            if (this._trackingTouches.Contains(touches[i]))
            {
                return true;
            }
        }
        return false;
    }

    protected bool isTrackingTouch(TKTouch t)
    {
        return this._trackingTouches.Contains(t);
    }

    private bool populateSubsetOfTouchesBeingTracked(List<TKTouch> touches)
    {
        this._subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer.Clear();
        for (int i = 0; i < touches.Count; i++)
        {
            if (this.alwaysSendTouchesMoved || this.isTrackingTouch(touches[i]))
            {
                this._subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer.Add(touches[i]);
            }
        }
        return (this._subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer.Count > 0);
    }

    internal void recognizeTouches(List<TKTouch> touches)
    {
        if (this.shouldAttemptToRecognize)
        {
            this._sentTouchesBegan = this._sentTouchesMoved = this._sentTouchesEnded = false;
            for (int i = touches.Count - 1; i >= 0; i--)
            {
                int num2;
                int num3;
                TKTouch touch = touches[i];
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (this._sentTouchesBegan || !this.isTouchWithinBoundaryFrame(touches[i]))
                        {
                            continue;
                        }
                        if (!this.touchesBegan(touches) || (this.zIndex <= 0))
                        {
                            goto Label_00E0;
                        }
                        num2 = 0;
                        num3 = touches.Count - 1;
                        goto Label_00CC;

                    case TouchPhase.Moved:
                    {
                        if (!this._sentTouchesMoved && this.populateSubsetOfTouchesBeingTracked(touches))
                        {
                            this.touchesMoved(this._subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer);
                            this._sentTouchesMoved = true;
                        }
                        continue;
                    }
                    case TouchPhase.Stationary:
                    {
                        continue;
                    }
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                    {
                        if (!this._sentTouchesEnded && this.populateSubsetOfTouchesBeingTracked(touches))
                        {
                            this.touchesEnded(this._subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer);
                            this._sentTouchesEnded = true;
                        }
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
            Label_00AC:
                if (touches[num3].phase == TouchPhase.Began)
                {
                    touches.RemoveAt(num3);
                    num2++;
                }
                num3--;
            Label_00CC:
                if (num3 >= 0)
                {
                    goto Label_00AC;
                }
                if (num2 > 0)
                {
                    i -= num2 - 1;
                }
            Label_00E0:
                this._sentTouchesBegan = true;
            }
        }
    }

    internal void reset()
    {
        this._state = TKGestureRecognizerState.Possible;
        this._trackingTouches.Clear();
    }

    public Vector2 startTouchLocation()
    {
        float num = 0f;
        float num2 = 0f;
        float num3 = 0f;
        for (int i = 0; i < this._trackingTouches.Count; i++)
        {
            num += this._trackingTouches[i].startPosition.x;
            num2 += this._trackingTouches[i].startPosition.y;
            num3++;
        }
        if (num3 > 0f)
        {
            return new Vector2(num / num3, num2 / num3);
        }
        return Vector2.zero;
    }

    public override string ToString()
    {
        object[] args = new object[] { base.GetType(), this.state, this.touchLocation(), this.zIndex };
        return string.Format("[{0}] state: {1}, location: {2}, zIndex: {3}", args);
    }

    internal virtual bool touchesBegan(List<TKTouch> touches)
    {
        return false;
    }

    internal virtual void touchesEnded(List<TKTouch> touches)
    {
    }

    internal virtual void touchesMoved(List<TKTouch> touches)
    {
    }

    public Vector2 touchLocation()
    {
        float num = 0f;
        float num2 = 0f;
        float num3 = 0f;
        for (int i = 0; i < this._trackingTouches.Count; i++)
        {
            num += this._trackingTouches[i].position.x;
            num2 += this._trackingTouches[i].position.y;
            num3++;
        }
        if (num3 > 0f)
        {
            return new Vector2(num / num3, num2 / num3);
        }
        return Vector2.zero;
    }

    private bool shouldAttemptToRecognize
    {
        get
        {
            return ((this.enabled && (this.state != TKGestureRecognizerState.FailedOrEnded)) && (this.state != TKGestureRecognizerState.Recognized));
        }
    }

    public TKGestureRecognizerState state
    {
        get
        {
            return this._state;
        }
        set
        {
            this._state = value;
            if ((this._state == TKGestureRecognizerState.Recognized) || (this._state == TKGestureRecognizerState.RecognizedAndStillRecognizing))
            {
                this.fireRecognizedEvent();
            }
            if ((this._state == TKGestureRecognizerState.Recognized) || (this._state == TKGestureRecognizerState.FailedOrEnded))
            {
                this.reset();
            }
        }
    }
}

