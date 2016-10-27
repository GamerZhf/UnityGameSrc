using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKCurveRecognizer : TKAbstractGestureRecognizer
{
    private Vector2 _deltaTranslation;
    private Vector2 _previousDeltaTranslation;
    private Vector2 _previousLocation;
    public float deltaRotation;
    public int maximumNumberOfTouches = 2;
    public float maxSharpnes = 50f;
    public int minimumNumberOfTouches = 1;
    public float reportRotationStep = 20f;
    public float squareDistance = 10f;

    public event Action<TKCurveRecognizer> gestureCompleteEvent;

    public event Action<TKCurveRecognizer> gestureRecognizedEvent;

    internal override void fireRecognizedEvent()
    {
        if (this.gestureRecognizedEvent != null)
        {
            this.gestureRecognizedEvent(this);
        }
    }

    public override string ToString()
    {
        object[] args = new object[] { base.GetType(), base.state, this._deltaTranslation, this._previousDeltaTranslation, this.deltaRotation };
        return string.Format("[{0}] state: {1}, trans: {2}, lastTrans: {3}, totalRot: {4}", args);
    }

    internal override bool touchesBegan(List<TKTouch> touches)
    {
        if ((base.state == TKGestureRecognizerState.Possible) || (((base.state == TKGestureRecognizerState.Began) || (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing)) && (base._trackingTouches.Count < this.maximumNumberOfTouches)))
        {
            for (int i = 0; i < touches.Count; i++)
            {
                if (touches[i].phase == TouchPhase.Began)
                {
                    base._trackingTouches.Add(touches[i]);
                    if (base._trackingTouches.Count == this.maximumNumberOfTouches)
                    {
                        break;
                    }
                }
            }
            if (base._trackingTouches.Count >= this.minimumNumberOfTouches)
            {
                this._previousLocation = base.touchLocation();
                if (base.state != TKGestureRecognizerState.RecognizedAndStillRecognizing)
                {
                    base.state = TKGestureRecognizerState.Possible;
                    this.deltaRotation = 0f;
                    this._deltaTranslation = Vector2.zero;
                    this._previousDeltaTranslation = Vector2.zero;
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
        if (base.state == TKGestureRecognizerState.Possible)
        {
            Vector2 vector = base.touchLocation();
            Vector2 vector2 = vector - this._previousLocation;
            this._deltaTranslation = vector2;
            this._previousLocation = vector;
            this._previousDeltaTranslation = this._deltaTranslation;
            base.state = TKGestureRecognizerState.Began;
        }
        else if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) || (base.state == TKGestureRecognizerState.Began))
        {
            Vector2 vector3 = base.touchLocation();
            Vector2 to = vector3 - this._previousLocation;
            if (to.sqrMagnitude >= 10f)
            {
                float num = Vector2.Angle(this._previousDeltaTranslation, to);
                if (num > this.maxSharpnes)
                {
                    Debug.Log(string.Concat(new object[] { "Curve is to sharp: ", num, "  max sharpnes set to:", this.maxSharpnes }));
                    base.state = TKGestureRecognizerState.FailedOrEnded;
                }
                else
                {
                    this._deltaTranslation = to;
                    if (Vector3.Cross((Vector3) this._previousDeltaTranslation, (Vector3) to).z > 0f)
                    {
                        this.deltaRotation -= num;
                    }
                    else
                    {
                        this.deltaRotation += num;
                    }
                    if (Mathf.Abs(this.deltaRotation) >= this.reportRotationStep)
                    {
                        base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
                        this.deltaRotation = 0f;
                    }
                    this._previousLocation = vector3;
                    this._previousDeltaTranslation = this._deltaTranslation;
                }
            }
        }
    }
}

