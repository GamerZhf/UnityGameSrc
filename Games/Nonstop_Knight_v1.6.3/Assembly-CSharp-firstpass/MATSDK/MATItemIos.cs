namespace MATSDK
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct MATItemIos
    {
        public string name;
        public double unitPrice;
        public int quantity;
        public double revenue;
        public string attribute1;
        public string attribute2;
        public string attribute3;
        public string attribute4;
        public string attribute5;
        public MATItemIos(string name)
        {
            this.name = name;
            this.unitPrice = 0.0;
            this.quantity = 0;
            this.revenue = 0.0;
            this.attribute1 = null;
            this.attribute2 = null;
            this.attribute3 = null;
            this.attribute4 = null;
            this.attribute5 = null;
        }

        public MATItemIos(MATItem matItem)
        {
            this.name = matItem.name;
            double? unitPrice = matItem.unitPrice;
            this.unitPrice = !unitPrice.HasValue ? 0.0 : unitPrice.Value;
            int? quantity = matItem.quantity;
            this.quantity = !quantity.HasValue ? 0 : quantity.Value;
            double? revenue = matItem.revenue;
            this.revenue = !revenue.HasValue ? 0.0 : revenue.Value;
            this.attribute1 = matItem.attribute1;
            this.attribute2 = matItem.attribute2;
            this.attribute3 = matItem.attribute3;
            this.attribute4 = matItem.attribute4;
            this.attribute5 = matItem.attribute5;
        }
    }
}

