using SupersonicJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SupersonicEvents : MonoBehaviour
{
    private const string ERROR_CODE = "error_code";
    private const string ERROR_DESCRIPTION = "error_description";

    private static  event Action<SupersonicError> _onGetOfferwallCreditsFailEvent;

    private static  event Action _onInterstitialClickEvent;

    private static  event Action _onInterstitialCloseEvent;

    private static  event Action<SupersonicError> _onInterstitialInitFailedEvent;

    private static  event Action _onInterstitialInitSuccessEvent;

    private static  event Action<SupersonicError> _onInterstitialLoadFailedEvent;

    private static  event Action _onInterstitialOpenEvent;

    private static  event Action _onInterstitialReadyEvent;

    private static  event Action<SupersonicError> _onInterstitialShowFailedEvent;

    private static  event Action _onInterstitialShowSuccessEvent;

    private static  event Action<Dictionary<string, object>> _onOfferwallAdCreditedEvent;

    private static  event Action _onOfferwallClosedEvent;

    private static  event Action<SupersonicError> _onOfferwallInitFailEvent;

    private static  event Action _onOfferwallInitSuccessEvent;

    private static  event Action _onOfferwallOpenedEvent;

    private static  event Action<SupersonicError> _onOfferwallShowFailEvent;

    private static  event Action _onRewardedVideoAdClosedEvent;

    private static  event Action _onRewardedVideoAdOpenedEvent;

    private static  event Action<SupersonicPlacement> _onRewardedVideoAdRewardedEvent;

    private static  event Action<SupersonicError> _onRewardedVideoInitFailEvent;

    private static  event Action _onRewardedVideoInitSuccessEvent;

    private static  event Action<SupersonicError> _onRewardedVideoShowFailEvent;

    private static  event Action<bool> _onVideoAvailabilityChangedEvent;

    private static  event Action _onVideoEndEvent;

    private static  event Action _onVideoStartEvent;

    public static  event Action<SupersonicError> onGetOfferwallCreditsFailEvent
    {
        add
        {
            if ((_onGetOfferwallCreditsFailEvent == null) || !Enumerable.Contains<Delegate>(_onGetOfferwallCreditsFailEvent.GetInvocationList(), value))
            {
                _onGetOfferwallCreditsFailEvent = (Action<SupersonicError>) Delegate.Combine(_onGetOfferwallCreditsFailEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onGetOfferwallCreditsFailEvent.GetInvocationList(), value))
            {
                _onGetOfferwallCreditsFailEvent = (Action<SupersonicError>) Delegate.Remove(_onGetOfferwallCreditsFailEvent, value);
            }
        }
    }

    public static  event Action onInterstitialClickEvent
    {
        add
        {
            if ((_onInterstitialClickEvent == null) || !Enumerable.Contains<Delegate>(_onInterstitialClickEvent.GetInvocationList(), value))
            {
                _onInterstitialClickEvent = (Action) Delegate.Combine(_onInterstitialClickEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onInterstitialClickEvent.GetInvocationList(), value))
            {
                _onInterstitialClickEvent = (Action) Delegate.Remove(_onInterstitialClickEvent, value);
            }
        }
    }

    public static  event Action onInterstitialCloseEvent
    {
        add
        {
            if ((_onInterstitialCloseEvent == null) || !Enumerable.Contains<Delegate>(_onInterstitialCloseEvent.GetInvocationList(), value))
            {
                _onInterstitialCloseEvent = (Action) Delegate.Combine(_onInterstitialCloseEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onInterstitialCloseEvent.GetInvocationList(), value))
            {
                _onInterstitialCloseEvent = (Action) Delegate.Remove(_onInterstitialCloseEvent, value);
            }
        }
    }

    public static  event Action<SupersonicError> onInterstitialInitFailedEvent
    {
        add
        {
            if ((_onInterstitialInitFailedEvent == null) || !Enumerable.Contains<Delegate>(_onInterstitialInitFailedEvent.GetInvocationList(), value))
            {
                _onInterstitialInitFailedEvent = (Action<SupersonicError>) Delegate.Combine(_onInterstitialInitFailedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onInterstitialInitFailedEvent.GetInvocationList(), value))
            {
                _onInterstitialInitFailedEvent = (Action<SupersonicError>) Delegate.Remove(_onInterstitialInitFailedEvent, value);
            }
        }
    }

    public static  event Action onInterstitialInitSuccessEvent
    {
        add
        {
            if ((_onInterstitialInitSuccessEvent == null) || !Enumerable.Contains<Delegate>(_onInterstitialInitSuccessEvent.GetInvocationList(), value))
            {
                _onInterstitialInitSuccessEvent = (Action) Delegate.Combine(_onInterstitialInitSuccessEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onInterstitialInitSuccessEvent.GetInvocationList(), value))
            {
                _onInterstitialInitSuccessEvent = (Action) Delegate.Remove(_onInterstitialInitSuccessEvent, value);
            }
        }
    }

    public static  event Action<SupersonicError> onInterstitialLoadFailedEvent
    {
        add
        {
            if ((_onInterstitialLoadFailedEvent == null) || !Enumerable.Contains<Delegate>(_onInterstitialLoadFailedEvent.GetInvocationList(), value))
            {
                _onInterstitialLoadFailedEvent = (Action<SupersonicError>) Delegate.Combine(_onInterstitialLoadFailedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onInterstitialLoadFailedEvent.GetInvocationList(), value))
            {
                _onInterstitialLoadFailedEvent = (Action<SupersonicError>) Delegate.Remove(_onInterstitialLoadFailedEvent, value);
            }
        }
    }

    public static  event Action onInterstitialOpenEvent
    {
        add
        {
            if ((_onInterstitialOpenEvent == null) || !Enumerable.Contains<Delegate>(_onInterstitialOpenEvent.GetInvocationList(), value))
            {
                _onInterstitialOpenEvent = (Action) Delegate.Combine(_onInterstitialOpenEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onInterstitialOpenEvent.GetInvocationList(), value))
            {
                _onInterstitialOpenEvent = (Action) Delegate.Remove(_onInterstitialOpenEvent, value);
            }
        }
    }

    public static  event Action onInterstitialReadyEvent
    {
        add
        {
            if ((_onInterstitialReadyEvent == null) || !Enumerable.Contains<Delegate>(_onInterstitialReadyEvent.GetInvocationList(), value))
            {
                _onInterstitialReadyEvent = (Action) Delegate.Combine(_onInterstitialReadyEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onInterstitialReadyEvent.GetInvocationList(), value))
            {
                _onInterstitialReadyEvent = (Action) Delegate.Remove(_onInterstitialReadyEvent, value);
            }
        }
    }

    public static  event Action<SupersonicError> onInterstitialShowFailedEvent
    {
        add
        {
            if ((_onInterstitialShowFailedEvent == null) || !Enumerable.Contains<Delegate>(_onInterstitialShowFailedEvent.GetInvocationList(), value))
            {
                _onInterstitialShowFailedEvent = (Action<SupersonicError>) Delegate.Combine(_onInterstitialShowFailedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onInterstitialShowFailedEvent.GetInvocationList(), value))
            {
                _onInterstitialShowFailedEvent = (Action<SupersonicError>) Delegate.Remove(_onInterstitialShowFailedEvent, value);
            }
        }
    }

    public static  event Action onInterstitialShowSuccessEvent
    {
        add
        {
            if ((_onInterstitialShowSuccessEvent == null) || !Enumerable.Contains<Delegate>(_onInterstitialShowSuccessEvent.GetInvocationList(), value))
            {
                _onInterstitialShowSuccessEvent = (Action) Delegate.Combine(_onInterstitialShowSuccessEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onInterstitialShowSuccessEvent.GetInvocationList(), value))
            {
                _onInterstitialShowSuccessEvent = (Action) Delegate.Remove(_onInterstitialShowSuccessEvent, value);
            }
        }
    }

    public static  event Action<Dictionary<string, object>> onOfferwallAdCreditedEvent
    {
        add
        {
            if ((_onOfferwallAdCreditedEvent == null) || !Enumerable.Contains<Delegate>(_onOfferwallAdCreditedEvent.GetInvocationList(), value))
            {
                _onOfferwallAdCreditedEvent = (Action<Dictionary<string, object>>) Delegate.Combine(_onOfferwallAdCreditedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onOfferwallAdCreditedEvent.GetInvocationList(), value))
            {
                _onOfferwallAdCreditedEvent = (Action<Dictionary<string, object>>) Delegate.Remove(_onOfferwallAdCreditedEvent, value);
            }
        }
    }

    public static  event Action onOfferwallClosedEvent
    {
        add
        {
            if ((_onOfferwallClosedEvent == null) || !Enumerable.Contains<Delegate>(_onOfferwallClosedEvent.GetInvocationList(), value))
            {
                _onOfferwallClosedEvent = (Action) Delegate.Combine(_onOfferwallClosedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onOfferwallClosedEvent.GetInvocationList(), value))
            {
                _onOfferwallClosedEvent = (Action) Delegate.Remove(_onOfferwallClosedEvent, value);
            }
        }
    }

    public static  event Action<SupersonicError> onOfferwallInitFailEvent
    {
        add
        {
            if ((_onOfferwallInitFailEvent == null) || !Enumerable.Contains<Delegate>(_onOfferwallInitFailEvent.GetInvocationList(), value))
            {
                _onOfferwallInitFailEvent = (Action<SupersonicError>) Delegate.Combine(_onOfferwallInitFailEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onOfferwallInitFailEvent.GetInvocationList(), value))
            {
                _onOfferwallInitFailEvent = (Action<SupersonicError>) Delegate.Remove(_onOfferwallInitFailEvent, value);
            }
        }
    }

    public static  event Action onOfferwallInitSuccessEvent
    {
        add
        {
            if ((_onOfferwallInitSuccessEvent == null) || !Enumerable.Contains<Delegate>(_onOfferwallInitSuccessEvent.GetInvocationList(), value))
            {
                _onOfferwallInitSuccessEvent = (Action) Delegate.Combine(_onOfferwallInitSuccessEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onOfferwallInitSuccessEvent.GetInvocationList(), value))
            {
                _onOfferwallInitSuccessEvent = (Action) Delegate.Remove(_onOfferwallInitSuccessEvent, value);
            }
        }
    }

    public static  event Action onOfferwallOpenedEvent
    {
        add
        {
            if ((_onOfferwallOpenedEvent == null) || !Enumerable.Contains<Delegate>(_onOfferwallOpenedEvent.GetInvocationList(), value))
            {
                _onOfferwallOpenedEvent = (Action) Delegate.Combine(_onOfferwallOpenedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onOfferwallOpenedEvent.GetInvocationList(), value))
            {
                _onOfferwallOpenedEvent = (Action) Delegate.Remove(_onOfferwallOpenedEvent, value);
            }
        }
    }

    public static  event Action<SupersonicError> onOfferwallShowFailEvent
    {
        add
        {
            if ((_onOfferwallShowFailEvent == null) || !Enumerable.Contains<Delegate>(_onOfferwallShowFailEvent.GetInvocationList(), value))
            {
                _onOfferwallShowFailEvent = (Action<SupersonicError>) Delegate.Combine(_onOfferwallShowFailEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onOfferwallShowFailEvent.GetInvocationList(), value))
            {
                _onOfferwallShowFailEvent = (Action<SupersonicError>) Delegate.Remove(_onOfferwallShowFailEvent, value);
            }
        }
    }

    public static  event Action onRewardedVideoAdClosedEvent
    {
        add
        {
            if ((_onRewardedVideoAdClosedEvent == null) || !Enumerable.Contains<Delegate>(_onRewardedVideoAdClosedEvent.GetInvocationList(), value))
            {
                _onRewardedVideoAdClosedEvent = (Action) Delegate.Combine(_onRewardedVideoAdClosedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onRewardedVideoAdClosedEvent.GetInvocationList(), value))
            {
                _onRewardedVideoAdClosedEvent = (Action) Delegate.Remove(_onRewardedVideoAdClosedEvent, value);
            }
        }
    }

    public static  event Action onRewardedVideoAdOpenedEvent
    {
        add
        {
            if ((_onRewardedVideoAdOpenedEvent == null) || !Enumerable.Contains<Delegate>(_onRewardedVideoAdOpenedEvent.GetInvocationList(), value))
            {
                _onRewardedVideoAdOpenedEvent = (Action) Delegate.Combine(_onRewardedVideoAdOpenedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onRewardedVideoAdOpenedEvent.GetInvocationList(), value))
            {
                _onRewardedVideoAdOpenedEvent = (Action) Delegate.Remove(_onRewardedVideoAdOpenedEvent, value);
            }
        }
    }

    public static  event Action<SupersonicPlacement> onRewardedVideoAdRewardedEvent
    {
        add
        {
            if ((_onRewardedVideoAdRewardedEvent == null) || !Enumerable.Contains<Delegate>(_onRewardedVideoAdRewardedEvent.GetInvocationList(), value))
            {
                _onRewardedVideoAdRewardedEvent = (Action<SupersonicPlacement>) Delegate.Combine(_onRewardedVideoAdRewardedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onRewardedVideoAdRewardedEvent.GetInvocationList(), value))
            {
                _onRewardedVideoAdRewardedEvent = (Action<SupersonicPlacement>) Delegate.Remove(_onRewardedVideoAdRewardedEvent, value);
            }
        }
    }

    public static  event Action<SupersonicError> onRewardedVideoInitFailEvent
    {
        add
        {
            if ((_onRewardedVideoInitFailEvent == null) || !Enumerable.Contains<Delegate>(_onRewardedVideoInitFailEvent.GetInvocationList(), value))
            {
                _onRewardedVideoInitFailEvent = (Action<SupersonicError>) Delegate.Combine(_onRewardedVideoInitFailEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onRewardedVideoInitFailEvent.GetInvocationList(), value))
            {
                _onRewardedVideoInitFailEvent = (Action<SupersonicError>) Delegate.Remove(_onRewardedVideoInitFailEvent, value);
            }
        }
    }

    public static  event Action onRewardedVideoInitSuccessEvent
    {
        add
        {
            if ((_onRewardedVideoInitSuccessEvent == null) || !Enumerable.Contains<Delegate>(_onRewardedVideoInitSuccessEvent.GetInvocationList(), value))
            {
                _onRewardedVideoInitSuccessEvent = (Action) Delegate.Combine(_onRewardedVideoInitSuccessEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onRewardedVideoInitSuccessEvent.GetInvocationList(), value))
            {
                _onRewardedVideoInitSuccessEvent = (Action) Delegate.Remove(_onRewardedVideoInitSuccessEvent, value);
            }
        }
    }

    public static  event Action<SupersonicError> onRewardedVideoShowFailEvent
    {
        add
        {
            if ((_onRewardedVideoShowFailEvent == null) || !Enumerable.Contains<Delegate>(_onRewardedVideoShowFailEvent.GetInvocationList(), value))
            {
                _onRewardedVideoShowFailEvent = (Action<SupersonicError>) Delegate.Combine(_onRewardedVideoShowFailEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onRewardedVideoShowFailEvent.GetInvocationList(), value))
            {
                _onRewardedVideoShowFailEvent = (Action<SupersonicError>) Delegate.Remove(_onRewardedVideoShowFailEvent, value);
            }
        }
    }

    public static  event Action<bool> onVideoAvailabilityChangedEvent
    {
        add
        {
            if ((_onVideoAvailabilityChangedEvent == null) || !Enumerable.Contains<Delegate>(_onVideoAvailabilityChangedEvent.GetInvocationList(), value))
            {
                _onVideoAvailabilityChangedEvent = (Action<bool>) Delegate.Combine(_onVideoAvailabilityChangedEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onVideoAvailabilityChangedEvent.GetInvocationList(), value))
            {
                _onVideoAvailabilityChangedEvent = (Action<bool>) Delegate.Remove(_onVideoAvailabilityChangedEvent, value);
            }
        }
    }

    public static  event Action onVideoEndEvent
    {
        add
        {
            if ((_onVideoEndEvent == null) || !Enumerable.Contains<Delegate>(_onVideoEndEvent.GetInvocationList(), value))
            {
                _onVideoEndEvent = (Action) Delegate.Combine(_onVideoEndEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onVideoEndEvent.GetInvocationList(), value))
            {
                _onVideoEndEvent = (Action) Delegate.Remove(_onVideoEndEvent, value);
            }
        }
    }

    public static  event Action onVideoStartEvent
    {
        add
        {
            if ((_onVideoStartEvent == null) || !Enumerable.Contains<Delegate>(_onVideoStartEvent.GetInvocationList(), value))
            {
                _onVideoStartEvent = (Action) Delegate.Combine(_onVideoStartEvent, value);
            }
        }
        remove
        {
            if (Enumerable.Contains<Delegate>(_onVideoStartEvent.GetInvocationList(), value))
            {
                _onVideoStartEvent = (Action) Delegate.Remove(_onVideoStartEvent, value);
            }
        }
    }

    private void Awake()
    {
        base.gameObject.name = "SupersonicEvents";
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    public SupersonicError getErrorFromErrorString(string description)
    {
        if (!string.IsNullOrEmpty(description))
        {
            Dictionary<string, object> dictionary = Json.Deserialize(description) as Dictionary<string, object>;
            if (dictionary != null)
            {
                int errCode = Convert.ToInt32(dictionary["error_code"].ToString());
                return new SupersonicError(errCode, dictionary["error_description"].ToString());
            }
            return new SupersonicError(-1, string.Empty);
        }
        return new SupersonicError(-1, string.Empty);
    }

    public SupersonicPlacement getPlacementFromString(string jsonPlacement)
    {
        Dictionary<string, object> dictionary = Json.Deserialize(jsonPlacement) as Dictionary<string, object>;
        int rAmount = Convert.ToInt32(dictionary["placement_reward_amount"].ToString());
        return new SupersonicPlacement(dictionary["placement_name"].ToString(), dictionary["placement_reward_name"].ToString(), rAmount);
    }

    public void onGetOfferwallCreditsFail(string description)
    {
        if (_onGetOfferwallCreditsFailEvent != null)
        {
            SupersonicError error = this.getErrorFromErrorString(description);
            _onGetOfferwallCreditsFailEvent(error);
        }
    }

    public void onInterstitialClick(string empty)
    {
        if (_onInterstitialClickEvent != null)
        {
            _onInterstitialClickEvent();
        }
    }

    public void onInterstitialClose(string empty)
    {
        if (_onInterstitialCloseEvent != null)
        {
            _onInterstitialCloseEvent();
        }
    }

    public void onInterstitialInitFailed(string description)
    {
        if (_onInterstitialInitFailedEvent != null)
        {
            SupersonicError error = this.getErrorFromErrorString(description);
            _onInterstitialInitFailedEvent(error);
        }
    }

    public void onInterstitialInitSuccess(string empty)
    {
        if (_onInterstitialInitSuccessEvent != null)
        {
            _onInterstitialInitSuccessEvent();
        }
    }

    public void onInterstitialLoadFailed(string description)
    {
        if (_onInterstitialLoadFailedEvent != null)
        {
            SupersonicError error = this.getErrorFromErrorString(description);
            _onInterstitialLoadFailedEvent(error);
        }
    }

    public void onInterstitialOpen(string empty)
    {
        if (_onInterstitialOpenEvent != null)
        {
            _onInterstitialOpenEvent();
        }
    }

    public void onInterstitialReady()
    {
        if (_onInterstitialReadyEvent != null)
        {
            _onInterstitialReadyEvent();
        }
    }

    public void onInterstitialShowFailed(string description)
    {
        if (_onInterstitialShowFailedEvent != null)
        {
            SupersonicError error = this.getErrorFromErrorString(description);
            _onInterstitialShowFailedEvent(error);
        }
    }

    public void onInterstitialShowSuccess(string empty)
    {
        if (_onInterstitialShowSuccessEvent != null)
        {
            _onInterstitialShowSuccessEvent();
        }
    }

    public void onOfferwallAdCredited(string json)
    {
        if (_onOfferwallAdCreditedEvent != null)
        {
            _onOfferwallAdCreditedEvent(Json.Deserialize(json) as Dictionary<string, object>);
        }
    }

    public void onOfferwallClosed(string empty)
    {
        if (_onOfferwallClosedEvent != null)
        {
            _onOfferwallClosedEvent();
        }
    }

    public void onOfferwallInitFail(string description)
    {
        if (_onOfferwallInitFailEvent != null)
        {
            SupersonicError error = this.getErrorFromErrorString(description);
            _onOfferwallInitFailEvent(error);
        }
    }

    public void onOfferwallInitSuccess(string empty)
    {
        if (_onOfferwallInitSuccessEvent != null)
        {
            _onOfferwallInitSuccessEvent();
        }
    }

    public void onOfferwallOpened(string empty)
    {
        if (_onOfferwallOpenedEvent != null)
        {
            _onOfferwallOpenedEvent();
        }
    }

    public void onOfferwallShowFail(string description)
    {
        if (_onOfferwallShowFailEvent != null)
        {
            SupersonicError error = this.getErrorFromErrorString(description);
            _onOfferwallShowFailEvent(error);
        }
    }

    public void onRewardedVideoAdClosed(string empty)
    {
        if (_onRewardedVideoAdClosedEvent != null)
        {
            _onRewardedVideoAdClosedEvent();
        }
    }

    public void onRewardedVideoAdOpened(string empty)
    {
        if (_onRewardedVideoAdOpenedEvent != null)
        {
            _onRewardedVideoAdOpenedEvent();
        }
    }

    public void onRewardedVideoAdRewarded(string description)
    {
        if (_onRewardedVideoAdRewardedEvent != null)
        {
            SupersonicPlacement placement = this.getPlacementFromString(description);
            _onRewardedVideoAdRewardedEvent(placement);
        }
    }

    public void onRewardedVideoInitFail(string description)
    {
        if (_onRewardedVideoInitFailEvent != null)
        {
            SupersonicError error = this.getErrorFromErrorString(description);
            _onRewardedVideoInitFailEvent(error);
        }
    }

    public void onRewardedVideoInitSuccess(string empty)
    {
        if (_onRewardedVideoInitSuccessEvent != null)
        {
            _onRewardedVideoInitSuccessEvent();
        }
    }

    public void onRewardedVideoShowFail(string description)
    {
        if (_onRewardedVideoShowFailEvent != null)
        {
            SupersonicError error = this.getErrorFromErrorString(description);
            _onRewardedVideoShowFailEvent(error);
        }
    }

    public void onVideoAvailabilityChanged(string stringAvailable)
    {
        bool flag = stringAvailable == "true";
        if (_onVideoAvailabilityChangedEvent != null)
        {
            _onVideoAvailabilityChangedEvent(flag);
        }
    }

    public void onVideoEnd(string empty)
    {
        if (_onVideoEndEvent != null)
        {
            _onVideoEndEvent();
        }
    }

    public void onVideoStart(string empty)
    {
        if (_onVideoStartEvent != null)
        {
            _onVideoStartEvent();
        }
    }
}

