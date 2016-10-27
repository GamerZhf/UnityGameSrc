using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKSwipeRecognizer : TKAbstractGestureRecognizer
{
    private float _minimumDistance;
    private List<Vector2> _points;
    private float _startTime;
    [CompilerGenerated]
    private TKSwipeDirection <completedSwipeDirection>k__BackingField;
    [CompilerGenerated]
    private float <swipeVelocity>k__BackingField;
    public int maximumNumberOfTouches;
    public int minimumNumberOfTouches;
    public float timeToSwipe;
    public bool triggerWhenCriteriaMet;

    public event Action<TKSwipeRecognizer> gestureRecognizedEvent;

    public TKSwipeRecognizer() : this(2f)
    {
    }

    public TKSwipeRecognizer(float minimumDistanceCm)
    {
        this.timeToSwipe = 0.5f;
        this.minimumNumberOfTouches = 1;
        this.maximumNumberOfTouches = 2;
        this.triggerWhenCriteriaMet = true;
        this._minimumDistance = 2f;
        this._points = new List<Vector2>();
        this._minimumDistance = minimumDistanceCm;
    }

    private bool checkForSwipeCompletion(TKTouch touch)
    {
        if ((this.timeToSwipe > 0f) && ((Time.time - this._startTime) > this.timeToSwipe))
        {
            return false;
        }
        if (this._points.Count < 2)
        {
            return false;
        }
        float num = Vector2.Distance(this.startPoint, this.endPoint);
        float num2 = num / TouchKit.instance.ScreenPixelsPerCm;
        if (num2 < this._minimumDistance)
        {
            return false;
        }
        float num3 = 0f;
        for (int i = 1; i < this._points.Count; i++)
        {
            num3 += Vector2.Distance(this._points[i], this._points[i - 1]);
        }
        if (num3 > (num * 1.1f))
        {
            return false;
        }
        this.swipeVelocity = num2 / (Time.time - this._startTime);
        Vector2 vector2 = this.endPoint - this.startPoint;
        Vector2 normalized = vector2.normalized;
        float num5 = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
        if (num5 < 0f)
        {
            num5 = 360f + num5;
        }
        num5 = 360f - num5;
        if ((num5 >= 292.5f) && (num5 <= 337.5f))
        {
            this.completedSwipeDirection = TKSwipeDirection.UpRight;
        }
        else if ((num5 >= 247.5f) && (num5 <= 292.5f))
        {
            this.completedSwipeDirection = TKSwipeDirection.Up;
        }
        else if ((num5 >= 202.5f) && (num5 <= 247.5f))
        {
            this.completedSwipeDirection = TKSwipeDirection.UpLeft;
        }
        else if ((num5 >= 157.5f) && (num5 <= 202.5f))
        {
            this.completedSwipeDirection = TKSwipeDirection.Left;
        }
        else if ((num5 >= 112.5f) && (num5 <= 157.5f))
        {
            this.completedSwipeDirection = TKSwipeDirection.DownLeft;
        }
        else if ((num5 >= 67.5f) && (num5 <= 112.5f))
        {
            this.completedSwipeDirection = TKSwipeDirection.Down;
        }
        else if ((num5 >= 22.5f) && (num5 <= 67.5f))
        {
            this.completedSwipeDirection = TKSwipeDirection.DownRight;
        }
        else
        {
            this.completedSwipeDirection = TKSwipeDirection.Right;
        }
        return true;
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
        object[] args = new object[] { base.ToString(), this.completedSwipeDirection, this.swipeVelocity, this.startPoint, this.endPoint };
        return string.Format("{0}, swipe direction: {1}, swipe velocity: {2}, start point: {3}, end point: {4}", args);
    }

    internal override bool touchesBegan(List<TKTouch> touches)
    {
        if (base.state == TKGestureRecognizerState.Possible)
        {
            for (int i = 0; i < touches.Count; i++)
            {
                base._trackingTouches.Add(touches[i]);
            }
            if ((base._trackingTouches.Count >= this.minimumNumberOfTouches) && (base._trackingTouches.Count <= this.maximumNumberOfTouches))
            {
                this._points.Clear();
                this._points.Add(touches[0].position);
                this._startTime = Time.time;
                base.state = TKGestureRecognizerState.Began;
            }
        }
        return false;
    }

    internal override void touchesEnded(List<TKTouch> touches)
    {
        if (base.state == TKGestureRecognizerState.Began)
        {
            this._points.Add(touches[0].position);
            if (this.checkForSwipeCompletion(touches[0]))
            {
                base.state = TKGestureRecognizerState.Recognized;
            }
            else
            {
                base.state = TKGestureRecognizerState.FailedOrEnded;
            }
        }
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        if (base.state == TKGestureRecognizerState.Began)
        {
            this._points.Add(touches[0].position);
            if (this.triggerWhenCriteriaMet && this.checkForSwipeCompletion(touches[0]))
            {
                base.state = TKGestureRecognizerState.Recognized;
            }
        }
    }

    public TKSwipeDirection completedSwipeDirection
    {
        [CompilerGenerated]
        get
        {
            return this.<completedSwipeDirection>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<completedSwipeDirection>k__BackingField = value;
        }
    }

    public Vector2 endPoint
    {
        get
        {
            return Enumerable.LastOrDefault<Vector2>(this._points);
        }
    }

    public Vector2 startPoint
    {
        get
        {
            return Enumerable.FirstOrDefault<Vector2>(this._points);
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
}

