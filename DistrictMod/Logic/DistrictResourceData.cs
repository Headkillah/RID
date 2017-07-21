using System;
using System.Collections.Generic;

namespace ResourceIndustryDistrict
{
    public class DistrictResourceData : IEquatable<DistrictResourceData>
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public int Size { get; set; }
        public int Farming { get; set; }
        public int Forest { get; set; }
        public int Oil { get; set; }
        public int Ore { get; set; }
        public bool OreDecline { get; set; }
        public bool OilDecline { get; set; }

        public bool Equals(DistrictResourceData other)
        {
            return this.Name == other.Name;
        }

        public double GetPrecentage(int resource)
        {
            double value = ((double)resource) / ((double)Size * 255);
            return value;
        }
    }
}