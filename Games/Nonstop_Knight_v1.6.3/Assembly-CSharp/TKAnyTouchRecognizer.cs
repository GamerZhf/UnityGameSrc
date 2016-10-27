using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKAnyTouchRecognizer : TKAbstractGestureRecognizer
{
    public event Action<TKAnyTouchRecognizer> onEnteredEvent;

    public event Action<TKAnyTouchRecognizer> onExitedEvent;

    public TKAnyTouchRecognizer(TKRect frame)
    {
        base.alwaysSendTouchesMoved = true;
        base.boundaryFrame = new TKRect?(frame);
    }

    internal override void fireRecognizedEvent()
    {
    }

    private void onTouchEntered()
    {
        if ((base._trackingTouches.Count == 1) && (this.onEnteredEvent != null))
        {
            this.onEnteredEvent(this);
        }
    }

    private void onTouchExited()
    {
        if ((base._trackingTouches.Count == 0) && (this.onExitedEvent != null))
        {
            this.onExitedEvent(this);
        }
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
                    base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
                    this.onTouchEntered();
                    return true;
                }
            }
        }
        return false;
    }

    internal override void touchesEnded(List<TKTouch> touches)
    {
        for (int i = 0; i < touches.Count; i++)
        {
            if ((touches[i].phase == TouchPhase.Ended) && base._trackingTouches.Contains(touches[i]))
            {
                base._trackingTouches.Remove(touches[i]);
                base.state = TKGestureRecognizerState.FailedOrEnded;
                this.onTouchExited();
            }
        }
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        for (int i = 0; i < touches.Count; i++)
        {
            bool flag = base.isTouchWithinBoundaryFrame(touches[i]);
            bool flag2 = base._trackingTouches.Contains(touches[i]);
            if (!flag2 || !flag)
            {
                if (!flag2 && flag)
                {
                    base._trackingTouches.Add(touches[i]);
                    base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
                    this.onTouchEntered();
                }
                else if (flag2 && !flag)
                {
                    base._trackingTouches.Remove(touches[i]);
                    base.state = TKGestureRecognizerState.FailedOrEnded;
                    this.onTouchExited();
                }
            }
        }
    }
}

