using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransformAnimationTask
{
    private AnimationCurve m_animation;
    private Action<int> m_cancelCallback;
    private float m_delay;
    private TimeUtil.DeltaTimeType m_dtType;
    private float m_duration;
    private Action<TransformAnimation> m_endCallback;
    private RectTransform m_rectTransform;
    private Rotation m_rotation;
    private Scaling m_scaling;
    private Action<TransformAnimation> m_startCallback;
    private float m_timer;
    private Transform m_transform;
    private Translation m_translation;
    private TranslationAnchoredPosition m_translationAnchoredPosition;

    public TransformAnimationTask(Transform tm, TransformAnimationTask preset, [Optional, DefaultParameterValue(2)] TimeUtil.DeltaTimeType dtType)
    {
        this.m_transform = tm;
        this.m_rectTransform = !(tm is RectTransform) ? null : ((RectTransform) tm);
        if (preset.m_translation != null)
        {
            this.m_translation = new Translation();
            this.m_translation.axis = preset.m_translation.axis;
            this.m_translation.start = !preset.m_translation.local ? this.m_transform.position : this.m_transform.localPosition;
            this.m_translation.target = preset.m_translation.target;
            this.m_translation.local = preset.m_translation.local;
            this.m_translation.easing = preset.m_translation.easing;
            this.m_translation.algorithm = preset.m_translation.algorithm;
            this.m_translation.cp0 = preset.m_translation.cp0;
            this.m_translation.cp1 = preset.m_translation.cp1;
        }
        if (preset.m_translationAnchoredPosition != null)
        {
            this.m_translationAnchoredPosition = new TranslationAnchoredPosition();
            this.m_translationAnchoredPosition.start = this.m_rectTransform.anchoredPosition;
            this.m_translationAnchoredPosition.target = preset.m_translationAnchoredPosition.target;
            this.m_translationAnchoredPosition.easing = preset.m_translationAnchoredPosition.easing;
            this.m_translationAnchoredPosition.algorithm = preset.m_translationAnchoredPosition.algorithm;
        }
        if (preset.m_rotation != null)
        {
            this.m_rotation = new Rotation();
            this.m_rotation.axis = preset.m_rotation.axis;
            this.m_rotation.start = !preset.m_rotation.local ? this.m_transform.rotation : this.m_transform.localRotation;
            this.m_rotation.target = preset.m_rotation.target;
            this.m_rotation.rotateBy = preset.m_rotation.rotateBy;
            this.m_rotation.local = preset.m_rotation.local;
            this.m_rotation.easing = preset.m_rotation.easing;
            this.m_rotation.lazy = preset.m_rotation.lazy;
        }
        if (preset.m_scaling != null)
        {
            this.m_scaling = new Scaling();
            this.m_scaling.axis = preset.m_scaling.axis;
            this.m_scaling.start = !preset.m_scaling.local ? this.m_transform.lossyScale : this.m_transform.localScale;
            this.m_scaling.target = preset.m_scaling.target;
            this.m_scaling.local = preset.m_scaling.local;
            this.m_scaling.easing = preset.m_scaling.easing;
        }
        this.m_timer = 0f;
        this.m_duration = preset.m_duration;
        this.m_delay = preset.m_delay;
        this.m_dtType = dtType;
    }

    public TransformAnimationTask(Transform tm, float duration, [Optional, DefaultParameterValue(0f)] float delay, [Optional, DefaultParameterValue(2)] TimeUtil.DeltaTimeType dtType)
    {
        this.m_transform = tm;
        this.m_rectTransform = !(tm is RectTransform) ? null : ((RectTransform) tm);
        this.m_timer = 0f;
        this.m_duration = duration;
        this.m_delay = delay;
        this.m_dtType = dtType;
    }

    public TransformAnimationTask(Transform tm, AnimationCurve animation, float duration, [Optional, DefaultParameterValue(0f)] float delay, [Optional, DefaultParameterValue(2)] TimeUtil.DeltaTimeType dtType)
    {
        if (animation == null)
        {
            Debug.LogError("Animation was null!");
        }
        else if (animation.keys.Length < 2)
        {
            Debug.LogError("Animation had less than two keyframes!");
        }
        this.m_transform = tm;
        this.m_rectTransform = !(tm is RectTransform) ? null : ((RectTransform) tm);
        this.m_animation = animation;
        AnimationCurveExtensions.SnapKeyToValue(this.m_animation, 0, 0f);
        AnimationCurveExtensions.SnapKeyToValue(this.m_animation, AnimationCurveExtensions.LastIndex(this.m_animation), 1f);
        this.m_timer = 0f;
        this.m_duration = duration;
        this.m_delay = delay;
        this.m_dtType = dtType;
    }

    public void apply()
    {
        float deltaTime = TimeUtil.GetDeltaTime(this.m_dtType);
        float num2 = 0f;
        if (this.m_delay > 0f)
        {
            this.m_delay -= deltaTime;
            if (this.m_delay > 0f)
            {
                return;
            }
            num2 = this.m_delay * -1f;
            this.m_delay = 0f;
        }
        if (this.m_timer == 0f)
        {
            if (this.m_translation != null)
            {
                this.m_translation.start = !this.m_translation.local ? this.m_transform.position : this.m_transform.localPosition;
            }
            if (this.m_translationAnchoredPosition != null)
            {
                this.m_translationAnchoredPosition.start = this.m_rectTransform.anchoredPosition;
            }
            if (this.m_rotation != null)
            {
                this.m_rotation.start = !this.m_rotation.local ? this.m_transform.rotation : this.m_transform.localRotation;
            }
            if (this.m_scaling != null)
            {
                this.m_scaling.start = !this.m_scaling.local ? this.m_transform.lossyScale : this.m_transform.localScale;
            }
        }
        this.m_timer = Mathf.Min((this.m_timer + deltaTime) + num2, this.m_duration);
        float progress = (this.m_duration <= 0f) ? 1f : Mathf.Clamp01(this.m_timer / this.m_duration);
        if (this.m_translation != null)
        {
            this.applyTranslation(progress);
        }
        if (this.m_translationAnchoredPosition != null)
        {
            this.applyTranslationToAnchoredPos(progress);
        }
        if (this.m_rotation != null)
        {
            this.applyRotation(progress);
        }
        if (this.m_scaling != null)
        {
            this.applyScaling(progress);
        }
    }

    private void applyRotation(float progress)
    {
        Quaternion quaternion;
        float valueAt;
        if (this.m_animation != null)
        {
            valueAt = AnimationCurveExtensions.GetValueAt(this.m_animation, progress);
        }
        else
        {
            valueAt = Easing.Apply(progress, this.m_rotation.easing);
        }
        if (this.m_rotation.lazy)
        {
            quaternion = Quaternion.Lerp(this.m_rotation.start, this.m_rotation.target, valueAt);
        }
        else
        {
            quaternion = Quaternion.Euler(this.m_rotation.start.eulerAngles + ((Vector3) (this.m_rotation.rotateBy * valueAt)));
        }
        Vector3 eulerAngles = quaternion.eulerAngles;
        if (this.m_rotation.local)
        {
            Vector3 vector2 = this.m_transform.localRotation.eulerAngles;
            this.m_transform.localRotation = Quaternion.Euler(new Vector3(!this.m_rotation.axis[0] ? vector2.x : eulerAngles.x, !this.m_rotation.axis[1] ? vector2.y : eulerAngles.y, !this.m_rotation.axis[2] ? vector2.z : eulerAngles.z));
        }
        else
        {
            Vector3 vector3 = this.m_transform.rotation.eulerAngles;
            this.m_transform.rotation = Quaternion.Euler(new Vector3(!this.m_rotation.axis[0] ? vector3.x : eulerAngles.x, !this.m_rotation.axis[1] ? vector3.y : eulerAngles.y, !this.m_rotation.axis[2] ? vector3.z : eulerAngles.z));
        }
    }

    private void applyScaling(float progress)
    {
        float valueAt;
        if (this.m_animation != null)
        {
            valueAt = AnimationCurveExtensions.GetValueAt(this.m_animation, progress);
        }
        else
        {
            valueAt = Easing.Apply(progress, this.m_scaling.easing);
        }
        Vector3 scale = this.m_scaling.start + ((Vector3) ((this.m_scaling.target - this.m_scaling.start) * valueAt));
        if (this.m_scaling.local)
        {
            this.m_transform.localScale = new Vector3(!this.m_scaling.axis[0] ? this.m_transform.localScale.x : scale.x, !this.m_scaling.axis[1] ? this.m_transform.localScale.y : scale.y, !this.m_scaling.axis[2] ? this.m_transform.localScale.z : scale.z);
        }
        else
        {
            scale = new Vector3(!this.m_scaling.axis[0] ? this.m_transform.lossyScale.x : scale.x, !this.m_scaling.axis[1] ? this.m_transform.lossyScale.y : scale.y, !this.m_scaling.axis[2] ? this.m_transform.lossyScale.z : scale.z);
            TransformExtensions.SetWorldScale(this.m_transform, scale);
        }
    }

    private void applyTranslation(float progress)
    {
        float valueAt;
        Vector3 zero = Vector3.zero;
        if (this.m_animation != null)
        {
            valueAt = AnimationCurveExtensions.GetValueAt(this.m_animation, progress);
        }
        else
        {
            valueAt = Easing.Apply(progress, this.m_translation.easing);
        }
        if (this.m_translation.algorithm == TranslationAlgorithm.LINEAR)
        {
            zero = this.m_translation.start + ((Vector3) ((this.m_translation.target - this.m_translation.start) * valueAt));
        }
        else if (this.m_translation.algorithm == TranslationAlgorithm.CATMULL_ROM)
        {
            zero = MathUtil.CatmullRom(valueAt, this.m_translation.start + this.m_translation.cp0, this.m_translation.start, this.m_translation.target, this.m_translation.target + this.m_translation.cp1);
        }
        else if (this.m_translation.algorithm == TranslationAlgorithm.QUADRATIC_BEZIER)
        {
            zero = MathUtil.QuadraticBezier(valueAt, this.m_translation.start, this.m_translation.cp0, this.m_translation.target);
        }
        else if (this.m_translation.algorithm == TranslationAlgorithm.CUBIC_BEZIER)
        {
            zero = MathUtil.CubicBezier(valueAt, this.m_translation.start, this.m_translation.cp0, this.m_translation.cp1, this.m_translation.target);
        }
        else if (this.m_translation.algorithm == TranslationAlgorithm.QUARTIC_BEZIER)
        {
            zero = MathUtil.QuarticBezier(valueAt, this.m_translation.start, this.m_translation.cp0, this.m_translation.cp1, this.m_translation.cp2, this.m_translation.target);
        }
        if (this.m_translation.local)
        {
            this.m_transform.localPosition = new Vector3(!this.m_translation.axis[0] ? this.m_transform.localPosition.x : zero.x, !this.m_translation.axis[1] ? this.m_transform.localPosition.y : zero.y, !this.m_translation.axis[2] ? this.m_transform.localPosition.z : zero.z);
        }
        else
        {
            this.m_transform.position = new Vector3(!this.m_translation.axis[0] ? this.m_transform.position.x : zero.x, !this.m_translation.axis[1] ? this.m_transform.position.y : zero.y, !this.m_translation.axis[2] ? this.m_transform.position.z : zero.z);
        }
    }

    private void applyTranslationToAnchoredPos(float progress)
    {
        float valueAt;
        Vector2 zero = Vector2.zero;
        if (this.m_animation != null)
        {
            valueAt = AnimationCurveExtensions.GetValueAt(this.m_animation, progress);
        }
        else
        {
            valueAt = Easing.Apply(progress, this.m_translationAnchoredPosition.easing);
        }
        if (this.m_translationAnchoredPosition.algorithm == TranslationAlgorithm.LINEAR)
        {
            zero = this.m_translationAnchoredPosition.start + ((Vector2) ((this.m_translationAnchoredPosition.target - this.m_translationAnchoredPosition.start) * valueAt));
        }
        this.m_rectTransform.anchoredPosition = new Vector2(zero.x, zero.y);
    }

    public void reset()
    {
        this.m_timer = 0f;
    }

    public void reset(float duration, float delay, Easing.Function easingFunction)
    {
        this.reset();
        this.m_duration = duration;
        this.m_delay = delay;
        if (this.m_translation != null)
        {
            this.m_translation.easing = easingFunction;
        }
        if (this.m_translationAnchoredPosition != null)
        {
            this.m_translationAnchoredPosition.easing = easingFunction;
        }
        if (this.m_rotation != null)
        {
            this.m_rotation.easing = easingFunction;
        }
        if (this.m_scaling != null)
        {
            this.m_scaling.easing = easingFunction;
        }
    }

    public void rotate(Quaternion target, bool locally, [Optional, DefaultParameterValue(0)] Easing.Function easing)
    {
        if (this.m_timer > 0f)
        {
            Debug.LogError("Cannot modify task after it has been started!");
        }
        else
        {
            this.m_rotation = new Rotation();
            this.m_rotation.axis = new bool[] { true, true, true };
            this.m_rotation.start = !locally ? this.m_transform.rotation : this.m_transform.localRotation;
            this.m_rotation.target = target;
            this.m_rotation.local = locally;
            this.m_rotation.easing = easing;
            this.m_rotation.lazy = true;
        }
    }

    public void rotate(Vector3 rotateBy, bool locally, [Optional, DefaultParameterValue(0)] Easing.Function easing)
    {
        if (this.m_timer > 0f)
        {
            Debug.LogError("Cannot modify task after it has been started!");
        }
        else
        {
            this.m_rotation = new Rotation();
            this.m_rotation.axis = new bool[] { true, true, true };
            this.m_rotation.start = !locally ? this.m_transform.rotation : this.m_transform.localRotation;
            this.m_rotation.rotateBy = rotateBy;
            this.m_rotation.local = locally;
            this.m_rotation.easing = easing;
            this.m_rotation.lazy = false;
        }
    }

    public void scale(Vector3 target, bool locally, [Optional, DefaultParameterValue(0)] Easing.Function easing)
    {
        if (this.m_timer > 0f)
        {
            Debug.LogError("Cannot modify task after it has been started!");
        }
        else
        {
            this.m_scaling = new Scaling();
            this.m_scaling.axis = new bool[] { true, true, true };
            this.m_scaling.start = !locally ? this.m_transform.lossyScale : this.m_transform.localScale;
            this.m_scaling.target = target;
            this.m_scaling.local = locally;
            this.m_scaling.easing = easing;
        }
    }

    public void setRotationAxises(bool x, bool y, bool z)
    {
        if (this.m_rotation.axis == null)
        {
            this.m_rotation.axis = new bool[] { x, y, z };
        }
        else
        {
            this.m_rotation.axis[0] = x;
            this.m_rotation.axis[1] = y;
            this.m_rotation.axis[2] = z;
        }
    }

    public void setScalingAxises(bool x, bool y, bool z)
    {
        if (this.m_scaling.axis == null)
        {
            this.m_scaling.axis = new bool[] { x, y, z };
        }
        else
        {
            this.m_scaling.axis[0] = x;
            this.m_scaling.axis[1] = y;
            this.m_scaling.axis[2] = z;
        }
    }

    public void setTranslationAxises(bool x, bool y, bool z)
    {
        if (this.m_translation.axis == null)
        {
            this.m_translation.axis = new bool[] { x, y, z };
        }
        else
        {
            this.m_translation.axis[0] = x;
            this.m_translation.axis[1] = y;
            this.m_translation.axis[2] = z;
        }
    }

    public void translate(Vector3 target, bool locally, [Optional, DefaultParameterValue(0)] Easing.Function easing)
    {
        if (this.m_timer > 0f)
        {
            Debug.LogError("Cannot modify task after it has been started!");
        }
        else
        {
            this.m_translation = new Translation();
            this.m_translation.axis = new bool[] { true, true, true };
            this.m_translation.start = !locally ? this.m_transform.position : this.m_transform.localPosition;
            this.m_translation.target = target;
            this.m_translation.local = locally;
            this.m_translation.easing = easing;
            this.m_translation.algorithm = TranslationAlgorithm.LINEAR;
        }
    }

    public void translateToAnchoredPos(Vector2 targetAnchoredPos, [Optional, DefaultParameterValue(0)] Easing.Function easing)
    {
        if (this.m_timer > 0f)
        {
            Debug.LogError("Cannot modify task after it has been started!");
        }
        else if (this.m_rectTransform == null)
        {
            Debug.LogError("No RectTransform specified before attempting to translate to anchored position!");
        }
        else
        {
            this.m_translationAnchoredPosition = new TranslationAnchoredPosition();
            this.m_translationAnchoredPosition.start = this.m_rectTransform.anchoredPosition;
            this.m_translationAnchoredPosition.target = targetAnchoredPos;
            this.m_translationAnchoredPosition.easing = easing;
            this.m_translationAnchoredPosition.algorithm = TranslationAlgorithm.LINEAR;
        }
    }

    public void translateWithCatmullRom(Vector3 target, bool locally, Vector3 catmullRomP0, Vector3 catmullRomP1, [Optional, DefaultParameterValue(0)] Easing.Function easing)
    {
        if (this.m_timer > 0f)
        {
            Debug.LogError("Cannot modify task after it has been started!");
        }
        else
        {
            this.m_translation = new Translation();
            this.m_translation.axis = new bool[] { true, true, true };
            this.m_translation.start = !locally ? this.m_transform.position : this.m_transform.localPosition;
            this.m_translation.target = target;
            this.m_translation.local = locally;
            this.m_translation.easing = easing;
            this.m_translation.algorithm = TranslationAlgorithm.CATMULL_ROM;
            this.m_translation.cp0 = catmullRomP0;
            this.m_translation.cp1 = catmullRomP1;
        }
    }

    public void translateWithCubicBezier(Vector3 target, bool locally, Vector3 bezierP1, Vector3 bezierP2, [Optional, DefaultParameterValue(0)] Easing.Function easing)
    {
        if (this.m_timer > 0f)
        {
            Debug.LogError("Cannot modify task after it has been started!");
        }
        else
        {
            this.m_translation = new Translation();
            this.m_translation.axis = new bool[] { true, true, true };
            this.m_translation.start = !locally ? this.m_transform.position : this.m_transform.localPosition;
            this.m_translation.target = target;
            this.m_translation.local = locally;
            this.m_translation.easing = easing;
            this.m_translation.algorithm = TranslationAlgorithm.CUBIC_BEZIER;
            this.m_translation.cp0 = bezierP1;
            this.m_translation.cp1 = bezierP2;
        }
    }

    public void translateWithQuadraticBezier(Vector3 target, bool locally, Vector3 bezierP1, [Optional, DefaultParameterValue(0)] Easing.Function easing)
    {
        if (this.m_timer > 0f)
        {
            Debug.LogError("Cannot modify task after it has been started!");
        }
        else
        {
            this.m_translation = new Translation();
            this.m_translation.axis = new bool[] { true, true, true };
            this.m_translation.start = !locally ? this.m_transform.position : this.m_transform.localPosition;
            this.m_translation.target = target;
            this.m_translation.local = locally;
            this.m_translation.easing = easing;
            this.m_translation.algorithm = TranslationAlgorithm.QUADRATIC_BEZIER;
            this.m_translation.cp0 = bezierP1;
        }
    }

    public void translateWithQuarticBezier(Vector3 target, bool locally, Vector3 bezierP1, Vector3 bezierP2, Vector3 bezierP3, [Optional, DefaultParameterValue(0)] Easing.Function easing)
    {
        if (this.m_timer > 0f)
        {
            Debug.LogError("Cannot modify task after it has been started!");
        }
        else
        {
            this.m_translation = new Translation();
            this.m_translation.axis = new bool[] { true, true, true };
            this.m_translation.start = !locally ? this.m_transform.position : this.m_transform.localPosition;
            this.m_translation.target = target;
            this.m_translation.local = locally;
            this.m_translation.easing = easing;
            this.m_translation.algorithm = TranslationAlgorithm.QUARTIC_BEZIER;
            this.m_translation.cp0 = bezierP1;
            this.m_translation.cp1 = bezierP2;
            this.m_translation.cp2 = bezierP3;
        }
    }

    public Action<int> CancelCallback
    {
        get
        {
            return this.m_cancelCallback;
        }
        set
        {
            this.m_cancelCallback = value;
        }
    }

    public bool Completed
    {
        get
        {
            return (this.m_timer == this.m_duration);
        }
    }

    public float Duration
    {
        get
        {
            return this.m_duration;
        }
    }

    public Action<TransformAnimation> EndCallback
    {
        get
        {
            return this.m_endCallback;
        }
        set
        {
            this.m_endCallback = value;
        }
    }

    public float Progress
    {
        get
        {
            return (this.m_timer / this.m_duration);
        }
    }

    public Action<TransformAnimation> StartCallback
    {
        get
        {
            return this.m_startCallback;
        }
        set
        {
            this.m_startCallback = value;
        }
    }

    public Vector2 TargetAnchoredPosition
    {
        get
        {
            return this.m_translationAnchoredPosition.target;
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return this.m_translation.target;
        }
    }

    public Quaternion TargetRotation
    {
        get
        {
            return this.m_rotation.target;
        }
    }

    public Vector3 TargetScale
    {
        get
        {
            return this.m_scaling.target;
        }
    }

    private class Rotation
    {
        public bool[] axis;
        public Easing.Function easing;
        public bool lazy;
        public bool local;
        public Vector3 rotateBy;
        public Quaternion start;
        public Quaternion target;
    }

    private class Scaling
    {
        public bool[] axis;
        public Easing.Function easing;
        public bool local;
        public Vector3 start;
        public Vector3 target;
    }

    private class Translation
    {
        public TransformAnimationTask.TranslationAlgorithm algorithm;
        public bool[] axis;
        public Vector3 cp0;
        public Vector3 cp1;
        public Vector3 cp2;
        public Easing.Function easing;
        public bool local;
        public Vector3 start;
        public Vector3 target;
    }

    private enum TranslationAlgorithm
    {
        LINEAR,
        CATMULL_ROM,
        QUADRATIC_BEZIER,
        CUBIC_BEZIER,
        QUARTIC_BEZIER
    }

    private class TranslationAnchoredPosition
    {
        public TransformAnimationTask.TranslationAlgorithm algorithm;
        public Easing.Function easing;
        public Vector2 start;
        public Vector2 target;
    }
}

