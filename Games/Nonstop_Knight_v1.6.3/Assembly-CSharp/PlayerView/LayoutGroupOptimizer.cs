namespace PlayerView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class LayoutGroupOptimizer : MonoBehaviour
    {
        protected void Start()
        {
            VerticalLayoutGroup component = base.GetComponent<VerticalLayoutGroup>();
            if (component != null)
            {
                component.spacing = Mathf.Max(component.spacing, 1f);
            }
            HorizontalLayoutGroup group2 = base.GetComponent<HorizontalLayoutGroup>();
            if (group2 != null)
            {
                group2.spacing = Mathf.Max(group2.spacing, 1f);
            }
            GridLayoutGroup group3 = base.GetComponent<GridLayoutGroup>();
            if (group3 != null)
            {
                group3.spacing = new Vector2(Mathf.Max(group3.spacing.x, 1f), Mathf.Max(group3.spacing.y, 1f));
            }
        }
    }
}

