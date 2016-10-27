namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourcePriceTable
    {
        public List<PricePoint> PricePoints;
        public ResourceType Resource;

        public double evaluate(double timeSeconds)
        {
            PricePoint point = null;
            int num = -1;
            for (int i = this.PricePoints.Count - 1; i >= 0; i--)
            {
                if (timeSeconds >= this.PricePoints[i].TimeSeconds)
                {
                    point = this.PricePoints[i];
                    num = i;
                    break;
                }
            }
            if (point == null)
            {
                Debug.LogError("PricePoint not found for value: " + timeSeconds);
                return 0.0;
            }
            if (num == (this.PricePoints.Count - 1))
            {
                return ((point.Cost / point.TimeSeconds) * timeSeconds);
            }
            PricePoint point2 = this.PricePoints[num + 1];
            return ((((point2.Cost - point.Cost) / (point2.TimeSeconds - point.TimeSeconds)) * (timeSeconds - point.TimeSeconds)) + point.Cost);
        }

        public class PricePoint
        {
            public double Cost;
            public double TimeSeconds;
        }
    }
}

