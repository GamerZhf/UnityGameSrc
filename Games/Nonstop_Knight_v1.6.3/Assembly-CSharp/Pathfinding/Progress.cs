namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Progress
    {
        public readonly float progress;
        public readonly string description;
        public Progress(float p, string d)
        {
            this.progress = p;
            this.description = d;
        }

        public override string ToString()
        {
            return (this.progress.ToString("0.0") + " " + this.description);
        }
    }
}

