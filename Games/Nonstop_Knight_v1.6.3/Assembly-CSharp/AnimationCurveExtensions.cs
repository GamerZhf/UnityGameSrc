using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[Extension]
public static class AnimationCurveExtensions
{
    [Extension]
    public static void Append(AnimationCurve self, Keyframe[] keys)
    {
        List<Keyframe> list = new List<Keyframe>(self.keys);
        Keyframe keyframe = list[list.Count - 1];
        float time = keyframe.time;
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].time += time;
            list.Add(keys[i]);
        }
        self.keys = list.ToArray();
    }

    [Extension]
    public static Keyframe Back(AnimationCurve self)
    {
        return self.keys[LastIndex(self)];
    }

    [Extension]
    public static void ClampValues(AnimationCurve self, float low, float high, bool clampTangents)
    {
        AnimationCurve curve = new AnimationCurve();
        bool flag = false;
        for (int i = 0; i < self.keys.Length; i++)
        {
            Keyframe key = self.keys[i];
            if (key.value > high)
            {
                key.value = high;
            }
            else if (key.value < low)
            {
                key.value = low;
            }
            if (flag)
            {
                key.inTangent = 0f;
                flag = false;
            }
            if (clampTangents && (i < (self.keys.Length - 1)))
            {
                foreach (float num2 in GetValuesInRange(self, key.time, self.keys[i + 1].time, 3))
                {
                    if ((num2 > high) || (num2 < low))
                    {
                        key.outTangent = 0f;
                        flag = true;
                    }
                }
            }
            curve.AddKey(key);
        }
        self.keys = curve.keys;
    }

    [Extension]
    public static Keyframe Front(AnimationCurve self)
    {
        return self.keys[0];
    }

    [Extension]
    public static float GetHighestKeyframeValue(AnimationCurve self)
    {
        float positiveInfinity = float.PositiveInfinity;
        foreach (Keyframe keyframe in self.keys)
        {
            if ((positiveInfinity == float.PositiveInfinity) || (keyframe.value > positiveInfinity))
            {
                positiveInfinity = keyframe.value;
            }
        }
        return positiveInfinity;
    }

    [Extension]
    public static float GetHighestTime(AnimationCurve self)
    {
        float negativeInfinity = float.NegativeInfinity;
        foreach (Keyframe keyframe in self.keys)
        {
            if (keyframe.time > negativeInfinity)
            {
                negativeInfinity = keyframe.time;
            }
        }
        return negativeInfinity;
    }

    [Extension]
    public static float GetHighestValue(AnimationCurve self, [Optional, DefaultParameterValue(10)] int steps)
    {
        float num = GetTimeAmplitude(self) / ((float) steps);
        float num2 = self.Evaluate(0f);
        for (int i = 1; i <= steps; i++)
        {
            float num4 = self.Evaluate(i * num);
            if (num4 > num2)
            {
                num2 = num4;
            }
        }
        return num2;
    }

    [Extension]
    public static float GetKeyframeValueAmplitude(AnimationCurve self)
    {
        return (GetHighestKeyframeValue(self) - GetLowestKeyframeValue(self));
    }

    [Extension]
    public static float GetLowestKeyframeValue(AnimationCurve self)
    {
        float positiveInfinity = float.PositiveInfinity;
        foreach (Keyframe keyframe in self.keys)
        {
            if ((positiveInfinity == float.PositiveInfinity) || (keyframe.value < positiveInfinity))
            {
                positiveInfinity = keyframe.value;
            }
        }
        return positiveInfinity;
    }

    [Extension]
    public static float GetLowestTime(AnimationCurve self)
    {
        float positiveInfinity = float.PositiveInfinity;
        foreach (Keyframe keyframe in self.keys)
        {
            if ((positiveInfinity == float.PositiveInfinity) || (keyframe.time < positiveInfinity))
            {
                positiveInfinity = keyframe.time;
            }
        }
        return positiveInfinity;
    }

    [Extension]
    public static float GetLowestValue(AnimationCurve self, [Optional, DefaultParameterValue(10)] int steps)
    {
        float num = GetTimeAmplitude(self) / ((float) steps);
        float num2 = self.Evaluate(0f);
        for (int i = 1; i <= steps; i++)
        {
            float num4 = self.Evaluate(i * num);
            if (num4 < num2)
            {
                num2 = num4;
            }
        }
        return num2;
    }

    [Extension]
    public static int GetNearestKeyframe(AnimationCurve self, float time)
    {
        float positiveInfinity = float.PositiveInfinity;
        int num2 = 0;
        for (int i = 0; i < self.keys.Length; i++)
        {
            float num4 = Mathf.Abs((float) (self.keys[i].time - time));
            if ((positiveInfinity == float.PositiveInfinity) || (num4 < positiveInfinity))
            {
                num2 = i;
            }
        }
        return num2;
    }

    [Extension]
    public static float GetTimeAmplitude(AnimationCurve self)
    {
        return (GetHighestTime(self) - GetLowestTime(self));
    }

    [Extension]
    public static float GetValueAmplitude(AnimationCurve self, [Optional, DefaultParameterValue(10)] int steps)
    {
        return (GetHighestValue(self, steps) - GetLowestValue(self, steps));
    }

    [Extension]
    public static float GetValueAt(AnimationCurve self, float relative)
    {
        float time = Mathf.Lerp(GetLowestTime(self), GetHighestTime(self), relative);
        return self.Evaluate(time);
    }

    [Extension]
    public static float[] GetValuesInRange(AnimationCurve self, float startTime, float endTime, int count)
    {
        float num = (endTime - startTime) / ((float) count);
        float[] numArray = new float[count];
        for (int i = 0; i < count; i++)
        {
            numArray[i] = self.Evaluate(startTime + ((i + 1) * num));
        }
        return numArray;
    }

    [Extension]
    public static int LastIndex(AnimationCurve self)
    {
        return (self.keys.Length - 1);
    }

    [Extension]
    public static void RemoveKeys(AnimationCurve self)
    {
        int length = self.keys.Length;
        for (int i = 0; i < length; i++)
        {
            self.RemoveKey(0);
        }
    }

    [Extension]
    public static void Reverse(AnimationCurve self)
    {
        List<Keyframe> list = new List<Keyframe>();
        Keyframe[] keys = self.keys;
        float lowestTime = GetLowestTime(self);
        float highestTime = GetHighestTime(self);
        for (int i = self.keys.Length - 1; i >= 0; i--)
        {
            Keyframe item = keys[i];
            float num4 = highestTime - (item.time - lowestTime);
            float num5 = -item.outTangent;
            float num6 = -item.inTangent;
            item.time = num4;
            item.inTangent = num5;
            item.outTangent = num6;
            list.Add(item);
        }
        self.keys = list.ToArray();
    }

    [Extension]
    public static void ScaleBy(AnimationCurve self, float timeScale, float valueScale)
    {
        float lowestTime = GetLowestTime(self);
        float highestTime = GetHighestTime(self);
        float lowestKeyframeValue = GetLowestKeyframeValue(self);
        float highestKeyframeValue = GetHighestKeyframeValue(self);
        Keyframe[] keys = self.keys;
        for (int i = 0; i < keys.Length; i++)
        {
            Keyframe keyframe = keys[i];
            if (timeScale != 1f)
            {
                float t = (keyframe.time - lowestTime) / (highestTime - lowestTime);
                float num7 = Mathf.Lerp(lowestTime, (GetTimeAmplitude(self) * timeScale) + lowestTime, t);
                keyframe.time = num7;
            }
            if (valueScale != 1f)
            {
                float num8 = (keyframe.value - lowestKeyframeValue) / (highestKeyframeValue - lowestKeyframeValue);
                float num9 = Mathf.Lerp(lowestKeyframeValue, (GetKeyframeValueAmplitude(self) * valueScale) + lowestKeyframeValue, num8);
                keyframe.value = num9;
            }
            keys[i] = keyframe;
        }
        self.keys = keys;
    }

    [Extension]
    public static void ScaleTo(AnimationCurve self, float timeAmplitude, float valueAmplitude)
    {
        float num = GetTimeAmplitude(self);
        float num2 = GetValueAmplitude(self, 10);
        ScaleBy(self, timeAmplitude / num, valueAmplitude / num2);
    }

    [Extension]
    public static void SetTangents(AnimationCurve self, int index, float inTangent, float outTangent)
    {
        Keyframe[] keys = self.keys;
        keys[index].inTangent = inTangent;
        keys[index].outTangent = outTangent;
        self.keys = keys;
    }

    [Extension]
    public static void SetValue(AnimationCurve self, int index, float value)
    {
        AnimationCurve curve = new AnimationCurve(self.keys);
        Keyframe keyframe = curve.keys[index];
        keyframe.value = value;
        curve.keys[index] = keyframe;
        self.keys = curve.keys;
    }

    [Extension]
    public static void SnapKeyToTime(AnimationCurve self, int key, float time)
    {
        Keyframe[] keys = self.keys;
        Keyframe keyframe = keys[key];
        keyframe.time = time;
        self.keys = keys;
    }

    [Extension]
    public static void SnapKeyToValue(AnimationCurve self, int key, float value)
    {
        Keyframe[] keys = self.keys;
        Keyframe keyframe = keys[key];
        keyframe.value = value;
        self.keys = keys;
    }
}

