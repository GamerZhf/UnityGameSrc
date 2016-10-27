namespace App
{
    using System;
    using System.Collections.Generic;

    public class QueuedEventHandlers
    {
        public List<object> Delegates;

        public void register(object handler)
        {
            if (this.Delegates == null)
            {
                this.Delegates = new List<object>(8);
            }
            this.Delegates.Add(handler);
        }

        public void unregister(object handler)
        {
            for (int i = this.Delegates.Count - 1; i >= 0; i--)
            {
                if (this.Delegates[i].Equals(handler))
                {
                    this.Delegates.RemoveAt(i);
                }
            }
        }

        public void unregisterAll()
        {
            if (this.Delegates != null)
            {
                this.Delegates.Clear();
            }
        }
    }
}

