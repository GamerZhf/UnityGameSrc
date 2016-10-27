namespace Pathfinding
{
    using System;
    using UnityEngine;

    [Serializable, HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_alternative_path.php"), AddComponentMenu("Pathfinding/Modifiers/Alternative Path")]
    public class AlternativePath : MonoModifier
    {
        private bool destroyed;
        private readonly object lockObject = new object();
        public int penalty = 0x3e8;
        private GraphNode[] prevNodes;
        private int prevPenalty;
        private int prevSeed;
        public int randomStep = 10;
        private System.Random rnd = new System.Random();
        private readonly System.Random seedGenerator = new System.Random();
        private GraphNode[] toBeApplied;
        private bool waitingForApply;

        public override void Apply(Path p)
        {
            if (this != null)
            {
                object lockObject = this.lockObject;
                lock (lockObject)
                {
                    this.toBeApplied = p.path.ToArray();
                    if (!this.waitingForApply)
                    {
                        this.waitingForApply = true;
                        AstarPath.OnPathPreSearch = (OnPathDelegate) Delegate.Combine(AstarPath.OnPathPreSearch, new OnPathDelegate(this.ApplyNow));
                    }
                }
            }
        }

        private void ApplyNow(Path somePath)
        {
            object lockObject = this.lockObject;
            lock (lockObject)
            {
                this.waitingForApply = false;
                AstarPath.OnPathPreSearch = (OnPathDelegate) Delegate.Remove(AstarPath.OnPathPreSearch, new OnPathDelegate(this.ApplyNow));
                this.InversePrevious();
                if (!this.destroyed)
                {
                    int seed = this.seedGenerator.Next();
                    this.rnd = new System.Random(seed);
                    if (this.toBeApplied != null)
                    {
                        for (int i = this.rnd.Next(this.randomStep); i < this.toBeApplied.Length; i += this.rnd.Next(1, this.randomStep))
                        {
                            this.toBeApplied[i].Penalty += (uint) this.penalty;
                        }
                    }
                    this.prevPenalty = this.penalty;
                    this.prevSeed = seed;
                    this.prevNodes = this.toBeApplied;
                }
            }
        }

        private void ClearOnDestroy(Path p)
        {
            object lockObject = this.lockObject;
            lock (lockObject)
            {
                AstarPath.OnPathPreSearch = (OnPathDelegate) Delegate.Remove(AstarPath.OnPathPreSearch, new OnPathDelegate(this.ClearOnDestroy));
                this.waitingForApply = false;
                this.InversePrevious();
            }
        }

        private void InversePrevious()
        {
            int prevSeed = this.prevSeed;
            this.rnd = new System.Random(prevSeed);
            if (this.prevNodes != null)
            {
                bool flag = false;
                for (int i = this.rnd.Next(this.randomStep); i < this.prevNodes.Length; i += this.rnd.Next(1, this.randomStep))
                {
                    if (this.prevNodes[i].Penalty < this.prevPenalty)
                    {
                        flag = true;
                    }
                    this.prevNodes[i].Penalty -= (uint) this.prevPenalty;
                }
                if (flag)
                {
                    Debug.LogWarning("Penalty for some nodes has been reset while this modifier was active. Penalties might not be correctly set.");
                }
            }
        }

        public void OnDestroy()
        {
            this.destroyed = true;
            object lockObject = this.lockObject;
            lock (lockObject)
            {
                if (!this.waitingForApply)
                {
                    this.waitingForApply = true;
                    AstarPath.OnPathPreSearch = (OnPathDelegate) Delegate.Combine(AstarPath.OnPathPreSearch, new OnPathDelegate(this.ClearOnDestroy));
                }
            }
            this.OnDestroy();
        }

        public override int Order
        {
            get
            {
                return 10;
            }
        }
    }
}

