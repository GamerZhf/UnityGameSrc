namespace UnityTest
{
    using System;
    using UnityEngine;

    public class IsRenderedByCamera : ComparerBaseGeneric<Renderer, Camera>
    {
        public CompareType compareType;

        protected override bool Compare(Renderer renderer, Camera camera)
        {
            bool flag = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), renderer.bounds);
            CompareType compareType = this.compareType;
            if (compareType != CompareType.IsVisible)
            {
                if (compareType != CompareType.IsNotVisible)
                {
                    throw new Exception();
                }
                return !flag;
            }
            return flag;
        }

        public enum CompareType
        {
            IsVisible,
            IsNotVisible
        }
    }
}

