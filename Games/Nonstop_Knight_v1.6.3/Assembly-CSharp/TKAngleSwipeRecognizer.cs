using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class TKAngleSwipeRecognizer : TKAbstractGestureRecognizer
{
    private List<AngleListener> _angleRecognizedEvents;
    private Vector2 _endPoint;
    private Vector2 _startPoint;
    private float _startTime;
    [CompilerGenerated]
    private float <swipeAngle>k__BackingField;
    [CompilerGenerated]
    private float <swipeVelocity>k__BackingField;
    [CompilerGenerated]
    private Vector2 <swipeVelVector>k__BackingField;
    public float minimumDistance;
    public float timeToSwipe;

    public event Action<TKAngleSwipeRecognizer> gestureRecognizedEvent;

    public TKAngleSwipeRecognizer() : this(2f)
    {
    }

    public TKAngleSwipeRecognizer(float minimumDistanceCm)
    {
        this._angleRecognizedEvents = new List<AngleListener>();
        this.timeToSwipe = 0.5f;
        this.minimumDistance = 2f;
        this.minimumDistance = minimumDistanceCm;
    }

    public void addAngleRecogizedEvents(Action<TKAngleSwipeRecognizer> action, Vector2 direction, float angleVarience)
    {
        this._angleRecognizedEvents.Add(new AngleListener(direction, angleVarience, action));
    }

    private bool checkForSwipeCompletion(TKTouch touch)
    {
        if ((this.timeToSwipe > 0f) && ((Time.time - this._startTime) > this.timeToSwipe))
        {
            base.state = TKGestureRecognizerState.FailedOrEnded;
            return false;
        }
        float num = Mathf.Abs((float) (this._startPoint.x - touch.position.x)) / TouchKit.instance.ScreenPixelsPerCm;
        float num2 = Mathf.Abs((float) (this._startPoint.y - touch.position.y)) / TouchKit.instance.ScreenPixelsPerCm;
        this._endPoint = touch.position;
        this.swipeVelocity = Mathf.Sqrt((num * num) + (num2 * num2));
        Vector2 vector = this.endPoint - this.startPoint;
        this.swipeAngle = 57.29578f * Mathf.Atan2(vector.y, vector.x);
        if (this.swipeAngle < 0f)
        {
            this.swipeAngle += 360f;
        }
        this.swipeVelVector = this._endPoint - this._startPoint;
        return (this.swipeVelocity > this.minimumDistance);
    }

    public void fireAngleRecognizedEvents()
    {
        int num = 0;
        int count = this._angleRecognizedEvents.Count;
        while (num < count)
        {
            AngleListener listener = this._angleRecognizedEvents[num];
            if (listener.Varience > Vector2.Angle(listener.Direction, this.swipeVelVector))
            {
                listener.Action(this);
            }
            num++;
        }
    }

    internal override void fireRecognizedEvent()
    {
        if (this.gestureRecognizedEvent != null)
        {
            this.gestureRecognizedEvent(this);
        }
        this.fireAngleRecognizedEvents();
    }

    public void removeAllAngleRecongnizedEvents()
    {
        this._angleRecognizedEvents.Clear();
    }

    public void removeAngleRecognizedEvents(Action<TKAngleSwipeRecognizer> action)
    {
        <removeAngleRecognizedEvents>c__AnonStorey263 storey = new <removeAngleRecognizedEvents>c__AnonStorey263();
        storey.action = action;
        this._angleRecognizedEvents.RemoveAll(new Predicate<AngleListener>(storey.<>m__57));
    }

    public override string ToString()
    {
        object[] args = new object[] { base.ToString(), this.swipeVelocity, this.swipeAngle, this.startPoint, this.endPoint };
        return string.Format("{0}, velocity: {1}, angle: {2}, start point: {3}, end point: {4}", args);
    }

    internal override bool touchesBegan(List<TKTouch> touches)
    {
        if (base.state == TKGestureRecognizerState.Possible)
        {
            this._startPoint = touches[0].position;
            this._startTime = Time.time;
            base._trackingTouches.Add(touches[0]);
            base.state = TKGestureRecognizerState.Began;
        }
        return false;
    }

    internal override void touchesEnded(List<TKTouch> touches)
    {
        base.state = TKGestureRecognizerState.FailedOrEnded;
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        if ((base.state == TKGestureRecognizerState.Began) && this.checkForSwipeCompletion(touches[0]))
        {
            base.state = TKGestureRecognizerState.Recognized;
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

    public float swipeAngle
    {
        [CompilerGenerated]
        get
        {
            return this.<swipeAngle>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<swipeAngle>k__BackingField = value;
        }
    }

    public float swipeVelocity
    {
        [CompilerGenerated]
        get
        {
            return this.<swipeVelocity>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<swipeVelocity>k__BackingField = value;
        }
    }

    public Vector2 swipeVelVector
    {
        [CompilerGenerated]
        get
        {
            return this.<swipeVelVector>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<swipeVelVector>k__BackingField = value;
        }
    }

    [CompilerGenerated]
    private sealed class <removeAngleRecognizedEvents>c__AnonStorey263
    {
        internal Action<TKAngleSwipeRecognizer> action;

        internal bool <>m__57(TKAngleSwipeRecognizer.AngleListener listener)
        {
            return (listener.Action == this.action);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct AngleListener
    {
        public float Varience;
        public Vector2 Direction;
        public Action<TKAngleSwipeRecognizer> Action;
        public AngleListener(Vector2 direction, float varience, Action<TKAngleSwipeRecognizer> action)
        {
            this.Varience = varience;
            this.Direction = direction;
            this.Action = action;
        }
    }
}

