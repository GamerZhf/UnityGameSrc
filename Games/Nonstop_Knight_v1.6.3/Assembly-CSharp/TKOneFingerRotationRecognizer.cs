using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKOneFingerRotationRecognizer : TKRotationRecognizer
{
    public Vector2 targetPosition;

    public event Action<TKOneFingerRotationRecognizer> gestureCompleteEvent;

    public event Action<TKOneFingerRotationRecognizer> gestureRecognizedEvent;

    internal override void fireRecognizedEvent()
    {
        if (this.gestureRecognizedEvent != null)
        {
            this.gestureRecognizedEvent(this);
        }
    }

    internal override bool touchesBegan(List<TKTouch> touches)
    {
        if (base.state == TKGestureRecognizerState.Possible)
        {
            base._trackingTouches.Add(touches[0]);
            base.deltaRotation = 0f;
            base._previousRotation = TKRotationRecognizer.angleBetweenPoints(this.targetPosition, base._trackingTouches[0].position);
            base.state = TKGestureRecognizerState.Began;
        }
        return false;
    }

    internal override void touchesEnded(List<TKTouch> touches)
    {
        if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) && (this.gestureCompleteEvent != null))
        {
            this.gestureCompleteEvent(this);
        }
        base.state = TKGestureRecognizerState.FailedOrEnded;
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) || (base.state == TKGestureRecognizerState.Began))
        {
            float current = TKRotationRecognizer.angleBetweenPoints(this.targetPosition, base._trackingTouches[0].position);
            base.deltaRotation = Mathf.DeltaAngle(current, base._previousRotation);
            base._previousRotation = current;
            base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
        }
    }
}

