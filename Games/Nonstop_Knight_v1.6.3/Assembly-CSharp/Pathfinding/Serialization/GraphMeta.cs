namespace Pathfinding.Serialization
{
    using Pathfinding;
    using System;

    public class GraphMeta
    {
        public int graphs;
        public string[] guids;
        public int[] nodeCounts;
        public string[] typeNames;
        public Version version;

        public Type GetGraphType(int i)
        {
            if (string.IsNullOrEmpty(this.typeNames[i]))
            {
                return null;
            }
            Type[] defaultGraphTypes = AstarData.DefaultGraphTypes;
            Type objA = null;
            for (int j = 0; j < defaultGraphTypes.Length; j++)
            {
                if (defaultGraphTypes[j].FullName == this.typeNames[i])
                {
                    objA = defaultGraphTypes[j];
                }
            }
            if (object.Equals(objA, null))
            {
                throw new Exception("No graph of type '" + this.typeNames[i] + "' could be created, type does not exist");
            }
            return objA;
        }
    }
}

