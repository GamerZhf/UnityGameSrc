using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKTouchPadRecognizer : TKAbstractGestureRecognizer
{
    public AnimationCurve inputCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public Vector2 value;

    public event Action<TKTouchPadRecognizer> gestureCompleteEvent;

    public event Action<TKTouchPadRecognizer> gestureRecognizedEvent;

    public TKTouchPadRecognizer(TKRect frame)
    {
        base.boundaryFrame = new TKRect?(frame);
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
        return string.Format("[{0}] state: {1}, value: {2}", base.GetType(), base.state, this.value);
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
                }
            }
            if (base._trackingTouches.Count > 0)
            {
                base.state = TKGestureRecognizerState.Began;
                this.touchesMoved(touches);
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
        this.value = Vector2.zero;
        base.state = TKGestureRecognizerState.FailedOrEnded;
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) || (base.state == TKGestureRecognizerState.Began))
        {
            Vector2 vector = base.touchLocation();
            this.value = vector - this.boundaryFrame.Value.center;
            this.value.x = Mathf.Clamp((float) (this.value.x / (this.boundaryFrame.Value.width * 0.5f)), (float) -1f, (float) 1f);
            this.value.y = Mathf.Clamp((float) (this.value.y / (this.boundaryFrame.Value.height * 0.5f)), (float) -1f, (float) 1f);
            float introduced4 = this.inputCurve.Evaluate(Mathf.Abs(this.value.x));
            this.value.x = introduced4 * Mathf.Sign(this.value.x);
            float introduced5 = this.inputCurve.Evaluate(Mathf.Abs(this.value.y));
            this.value.y = introduced5 * Mathf.Sign(this.value.y);
            base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
        }
    }
}

