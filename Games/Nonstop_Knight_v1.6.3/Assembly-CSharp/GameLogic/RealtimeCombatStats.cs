namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class RealtimeCombatStats
    {
        private List<Datapoint> m_datapoints;
        private Datapoint m_nextFreeDatapoint;
        private float m_totalTimeElapsed;
        private float m_windowWidthSeconds;

        public RealtimeCombatStats(int maxCapacity, float windowWidthSeconds)
        {
            this.m_datapoints = new List<Datapoint>(maxCapacity);
            for (int i = 0; i < this.m_datapoints.Capacity; i++)
            {
                this.m_datapoints.Add(new Datapoint());
            }
            this.m_nextFreeDatapoint = this.m_datapoints[0];
            this.m_windowWidthSeconds = windowWidthSeconds;
        }

        public void addDatapoint(double value, DatapointType type)
        {
            if (this.m_nextFreeDatapoint == null)
            {
                if (Application.isEditor)
                {
                    Debug.LogWarning("Trying to add more datapoints than max capacity, skipping..");
                }
            }
            else
            {
                this.m_nextFreeDatapoint.Timer = this.m_windowWidthSeconds;
                this.m_nextFreeDatapoint.Value = value;
                this.m_nextFreeDatapoint.Type = type;
                this.m_nextFreeDatapoint = this.getNextFreeOrOldestDatapoint();
            }
        }

        private Datapoint getNextFreeDatapoint()
        {
            for (int i = 0; i < this.m_datapoints.Count; i++)
            {
                if (!this.m_datapoints[i].IsActive)
                {
                    return this.m_datapoints[i];
                }
            }
            return null;
        }

        private Datapoint getNextFreeOrOldestDatapoint()
        {
            Datapoint datapoint = this.m_datapoints[0];
            for (int i = 0; i < this.m_datapoints.Count; i++)
            {
                if (!this.m_datapoints[i].IsActive)
                {
                    return this.m_datapoints[i];
                }
                if (this.m_datapoints[i].Timer < datapoint.Timer)
                {
                    datapoint = this.m_datapoints[i];
                }
            }
            return datapoint;
        }

        public void getStats(out double weaponDps, out double skillsDps, out double supportDps, out double cps)
        {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            for (int i = 0; i < this.m_datapoints.Count; i++)
            {
                Datapoint datapoint = this.m_datapoints[i];
                if (datapoint.IsActive)
                {
                    switch (datapoint.Type)
                    {
                        case DatapointType.DamageWeapon:
                            num += datapoint.Value;
                            break;

                        case DatapointType.DamageSkill:
                            num2 += datapoint.Value;
                            break;

                        case DatapointType.DamageSupport:
                            num3 += datapoint.Value;
                            break;

                        case DatapointType.Coins:
                            num4 += datapoint.Value;
                            break;
                    }
                }
            }
            float num6 = this.getWindowWidthDivider();
            weaponDps = num / ((double) num6);
            skillsDps = num2 / ((double) num6);
            supportDps = num3 / ((double) num6);
            cps = num4 / ((double) num6);
        }

        private float getWindowWidthDivider()
        {
            if (this.m_totalTimeElapsed <= 0f)
            {
                return this.m_windowWidthSeconds;
            }
            return Mathf.Min(this.m_totalTimeElapsed, this.m_windowWidthSeconds);
        }

        public void reset()
        {
            for (int i = 0; i < this.m_datapoints.Count; i++)
            {
                this.m_datapoints[i].Timer = 0f;
            }
            this.m_totalTimeElapsed = 0f;
        }

        public void tick(float dt)
        {
            for (int i = 0; i < this.m_datapoints.Count; i++)
            {
                Datapoint datapoint = this.m_datapoints[i];
                datapoint.Timer = Mathf.Max((float) (datapoint.Timer - dt), (float) 0f);
            }
            this.m_totalTimeElapsed += dt;
        }

        private class Datapoint
        {
            public float Timer;
            public RealtimeCombatStats.DatapointType Type;
            public double Value;

            public bool IsActive
            {
                get
                {
                    return (this.Timer > 0f);
                }
            }
        }

        public enum DatapointType
        {
            UNDEFINED,
            DamageWeapon,
            DamageSkill,
            DamageSupport,
            Coins
        }
    }
}

