using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKLongPressRecognizer : TKAbstractGestureRecognizer
{
    private Vector2 _beginLocation;
    private bool _waiting;
    public float allowableMovementCm;
    public float minimumPressDuration;
    public int requiredTouchesCount;

    public event Action<TKLongPressRecognizer> gestureCompleteEvent;

    public event Action<TKLongPressRecognizer> gestureRecognizedEvent;

    public TKLongPressRecognizer()
    {
        this.minimumPressDuration = 0.5f;
        this.requiredTouchesCount = -1;
        this.allowableMovementCm = 1f;
    }

    public TKLongPressRecognizer(float minimumPressDuration, float allowableMovement, int requiredTouchesCount)
    {
        this.minimumPressDuration = 0.5f;
        this.requiredTouchesCount = -1;
        this.allowableMovementCm = 1f;
        this.minimumPressDuration = minimumPressDuration;
        this.allowableMovementCm = allowableMovement;
        this.requiredTouchesCount = requiredTouchesCount;
    }

    [DebuggerHidden]
    private IEnumerator beginGesture()
    {
        <beginGesture>c__Iterator1F iteratorf = new <beginGesture>c__Iterator1F();
        iteratorf.<>f__this = this;
        return iteratorf;
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
        if ((!this._waiting && (base.state == TKGestureRecognizerState.Possible)) && ((this.requiredTouchesCount == -1) || (touches.Count == this.requiredTouchesCount)))
        {
            this._beginLocation = touches[0].position;
            this._waiting = true;
            TouchKit.instance.StartCoroutine(this.beginGesture());
            base._trackingTouches.Add(touches[0]);
            base.state = TKGestureRecognizerState.Began;
        }
        else if (this.requiredTouchesCount != -1)
        {
            this._waiting = false;
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
        this._waiting = false;
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        if ((base.state == TKGestureRecognizerState.Began) || (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing))
        {
            float num = Vector2.Distance(touches[0].position, this._beginLocation) / TouchKit.instance.ScreenPixelsPerCm;
            if (num > this.allowableMovementCm)
            {
                if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) && (this.gestureCompleteEvent != null))
                {
                    this.gestureCompleteEvent(this);
                }
                base.state = TKGestureRecognizerState.FailedOrEnded;
                this._waiting = false;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <beginGesture>c__Iterator1F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal TKLongPressRecognizer <>f__this;
        internal float <endTime>__0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<endTime>__0 = Time.time + this.<>f__this.minimumPressDuration;
                    break;

                case 1:
                    break;

                default:
                    goto Label_00B0;
            }
            if (this.<>f__this._waiting && (Time.time < this.<endTime>__0))
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if ((Time.time >= this.<endTime>__0) && (this.<>f__this.state == TKGestureRecognizerState.Began))
            {
                this.<>f__this.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
            }
            this.<>f__this._waiting = false;
            this.$PC = -1;
        Label_00B0:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

