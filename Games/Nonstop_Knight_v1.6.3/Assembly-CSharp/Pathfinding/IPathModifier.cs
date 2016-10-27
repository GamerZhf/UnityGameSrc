namespace Pathfinding
{
    using System;

    public interface IPathModifier
    {
        void Apply(Path p);
        void PreProcess(Path p);

        int Order { get; }
    }
}

