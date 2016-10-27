namespace GameLogic
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct BuffSource
    {
        public object Object;
        public IBuffIconProvider IconProvider;
    }
}

