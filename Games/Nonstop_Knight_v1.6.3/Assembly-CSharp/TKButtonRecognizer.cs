using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TKButtonRecognizer : TKAbstractGestureRecognizer
{
    private TKRect _defaultFrame;
    private TKRect _highlightedFrame;

    public event Action<TKButtonRecognizer> onDeselectedEvent;

    public event Action<TKButtonRecognizer> onSelectedEvent;

    public event Action<TKButtonRecognizer> onTouchUpInsideEvent;

    public TKButtonRecognizer(TKRect defaultFrame) : this(defaultFrame, (float) 40f)
    {
    }

    public TKButtonRecognizer(TKRect defaultFrame, float highlightedExpansion) : this(defaultFrame, defaultFrame.copyWithExpansion(highlightedExpansion))
    {
    }

    public TKButtonRecognizer(TKRect defaultFrame, TKRect highlightedFrame)
    {
        this._defaultFrame = defaultFrame;
        this._highlightedFrame = highlightedFrame;
        base.boundaryFrame = new TKRect?(this._defaultFrame);
    }

    internal override void fireRecognizedEvent()
    {
    }

    protected virtual void onDeselected()
    {
        if (this.onDeselectedEvent != null)
        {
            this.onDeselectedEvent(this);
        }
    }

    protected virtual void onSelected()
    {
        base.boundaryFrame = new TKRect?(this._highlightedFrame);
        if (this.onSelectedEvent != null)
        {
            this.onSelectedEvent(this);
        }
    }

    protected virtual void onTouchUpInside()
    {
        if (this.onTouchUpInsideEvent != null)
        {
            this.onTouchUpInsideEvent(this);
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
                    this.onSelected();
                    return true;
                }
            }
        }
        return false;
    }

    internal override void touchesEnded(List<TKTouch> touches)
    {
        if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing)
        {
            this.onTouchUpInside();
        }
        base.boundaryFrame = new TKRect?(this._defaultFrame);
        base.state = TKGestureRecognizerState.FailedOrEnded;
    }

    internal override void touchesMoved(List<TKTouch> touches)
    {
        for (int i = 0; i < touches.Count; i++)
        {
            if (touches[i].phase == TouchPhase.Stationary)
            {
                bool flag = base.isTouchWithinBoundaryFrame(touches[i]);
                if ((base.state == TKGestureRecognizerState.Began) && flag)
                {
                    base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
                    this.onSelected();
                }
                else if ((base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) && !flag)
                {
                    base.state = TKGestureRecognizerState.FailedOrEnded;
                    this.onDeselected();
                }
            }
        }
    }
}

