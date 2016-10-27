namespace Pathfinding
{
    using Pathfinding.Util;
    using System;

    internal class PathReturnQueue
    {
        private Path pathReturnPop;
        private LockFreeStack pathReturnStack = new LockFreeStack();
        private object pathsClaimedSilentlyBy;

        public PathReturnQueue(object pathsClaimedSilentlyBy)
        {
            this.pathsClaimedSilentlyBy = pathsClaimedSilentlyBy;
        }

        public void Enqueue(Path path)
        {
            this.pathReturnStack.Push(path);
        }

        public void ReturnPaths(bool timeSlice)
        {
            Path path = this.pathReturnStack.PopAll();
            if (this.pathReturnPop == null)
            {
                this.pathReturnPop = path;
            }
            else
            {
                Path pathReturnPop = this.pathReturnPop;
                while (pathReturnPop.next != null)
                {
                    pathReturnPop = pathReturnPop.next;
                }
                pathReturnPop.next = path;
            }
            long num = !timeSlice ? 0L : (DateTime.UtcNow.Ticks + 0x2710L);
            int num2 = 0;
            while (this.pathReturnPop != null)
            {
                Path path3 = this.pathReturnPop;
                this.pathReturnPop = this.pathReturnPop.next;
                path3.next = null;
                path3.ReturnPath();
                path3.AdvanceState(PathState.Returned);
                path3.Release(this.pathsClaimedSilentlyBy, true);
                num2++;
                if ((num2 > 5) && timeSlice)
                {
                    num2 = 0;
                    if (DateTime.UtcNow.Ticks >= num)
                    {
                        return;
                    }
                }
            }
        }
    }
}

