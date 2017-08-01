using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ResourceIndustryDistrict
{
    public class DistrictResource
    {

        public static List<DistrictResourceData> districtResourceList = new List<DistrictResourceData>();
        private static List<int> namedDistricts = new List<int>();

        public static Func<NaturalResourceManager.ResourceCell[]> getResource;
        public static Func<DistrictManager.Cell[]> getDistricts;
        public static Func<Array8<District>> getDistrictNames;
        public static Func<int, string> getDistrictNameFromId;

        static public void Calculate()
        {
            List<DistrictResourceData> latestDistrictResourceList = new List<DistrictResourceData>();
            NaturalResourceManager.ResourceCell[] resourcesFromMap = getResource();
            DistrictManager.Cell[] districts = getDistricts();

            for (int i = 0; i < districts.Length; i++)
            {
                DistrictManager.Cell cell = districts[i];
                int districtId = GetMatchingDistrictId(cell);
                string districtName = GetDistrictName(districtId);
                if (districtName != null)
                {
                    DistrictResourceData result = latestDistrictResourceList.Find(x => x.Name.Contains(districtName));
                    if (result == null)
                    {
                        District district = (District)getDistrictNames().m_buffer.GetValue(districtId);
                        int districtType = (int)district.m_specializationPolicies;
                        latestDistrictResourceList.Add(new DistrictResourceData() { Name = districtName, Type = districtType });
                        result = latestDistrictResourceList[latestDistrictResourceList.Count - 1];
                    }
                    int resouceIndex = GetResourceIndex(i);
                    int oreFromMap = resourcesFromMap[resouceIndex].m_ore;
                    int oilFromMap = resourcesFromMap[resouceIndex].m_oil;
                    int forestFromMap = resourcesFromMap[resouceIndex].m_forest;
                    int farmingFromMap = resourcesFromMap[resouceIndex].m_fertility;
                    if (oreFromMap + oilFromMap + forestFromMap + farmingFromMap != 0)
                    {
                        result.Ore += resourcesFromMap[resouceIndex].m_ore;
                        result.Oil += resourcesFromMap[resouceIndex].m_oil;
                        result.Forest += resourcesFromMap[resouceIndex].m_forest;
                        result.Farming += resourcesFromMap[resouceIndex].m_fertility;
                        if (districtId != 0)
                        {
                            resourcesFromMap[resouceIndex].m_ore = 0;
                            resourcesFromMap[resouceIndex].m_oil = 0;
                            resourcesFromMap[resouceIndex].m_forest = 0;
                            resourcesFromMap[resouceIndex].m_fertility = 0;
                        }
                    }
                    result.Size++;
                }
            }

            // now check for decline
            foreach (DistrictResourceData d in latestDistrictResourceList)
            {
                var oldData = districtResourceList.Find(f => f.Name == d.Name);
                if (oldData != null)
                {
                    if (oldData.Oil > d.Oil && d.GetPrecentage(d.Oil) < 0.05)
                    {
                        d.OilDecline = true;
                    }
                    if (oldData.Ore > d.Ore && d.GetPrecentage(d.Ore) < 0.05)
                    {
                        d.OreDecline = true;
                    }
                }
            }
            districtResourceList = latestDistrictResourceList;
        }

        static public void Calculate2()
        {
            List<DistrictResourceData> latestDistrictResourceList = new List<DistrictResourceData>();
            NaturalResourceManager.ResourceCell[] resourcesFromMap = getResource();
            DistrictManager.Cell[] districts = getDistricts();
            
            for (int i = 0; i < resourcesFromMap.Length; i++)
            {
                int oreFromMap = resourcesFromMap[i].m_ore;
                int oilFromMap = resourcesFromMap[i].m_oil;
                int forestFromMap = resourcesFromMap[i].m_forest;
                int farmingFromMap = resourcesFromMap[i].m_fertility;
                if (oreFromMap + oilFromMap + forestFromMap + farmingFromMap != 0)
                {
                    int districtIndex = GetDistrictIndex(i);
                    if (districtIndex == -1)
                    {
                        // this resource cannot be in a district
                        continue;
                    }
                    DistrictManager.Cell cell = districts[districtIndex];
                    int districtId = GetMatchingDistrictId(cell);
                    string districtName = GetDistrictName(districtId);
                    if (districtName != null)
                    {
                        DistrictResourceData result = latestDistrictResourceList.Find(x => x.Name.Contains(districtName));
                        if (result == null)
                        {
                            // need to add this distric
                            District district = (District)getDistrictNames().m_buffer.GetValue(districtId);
                            int districtType = (int)district.m_specializationPolicies;
                            latestDistrictResourceList.Add(new DistrictResourceData() { Name = districtName, Type = districtType });
                            result = latestDistrictResourceList[latestDistrictResourceList.Count - 1];
                        }
                        // add the resources
                        result.Ore += oreFromMap;
                        result.Oil += oilFromMap;
                        result.Forest += forestFromMap;
                        result.Farming += farmingFromMap;

                        result.Size++;
                    }
                    
                }
            }

            for (int i = 0; i < districts.Length; i++)
            {
                DistrictManager.Cell cell = districts[i];
                int districtId = GetMatchingDistrictId(cell);
                string districtName = GetDistrictName(districtId);
                if (districtName != null)
                {
                    DistrictResourceData result = latestDistrictResourceList.Find(x => x.Name.Contains(districtName));
                    if (result == null)
                    {
                        District district = (District)getDistrictNames().m_buffer.GetValue(districtId);
                        int districtType = (int)district.m_specializationPolicies;
                        latestDistrictResourceList.Add(new DistrictResourceData() { Name = districtName, Type = districtType });
                        result = latestDistrictResourceList[latestDistrictResourceList.Count - 1];
                    }
                }
            }

            // now check for decline
            foreach (DistrictResourceData d in latestDistrictResourceList)
            {
                var oldData = districtResourceList.Find(f => f.Name == d.Name);
                if (oldData != null)
                {
                    if (oldData.Oil > d.Oil && d.GetPrecentage(d.Oil) < 0.05)
                    {
                        d.OilDecline = true;
                    }
                    if (oldData.Ore > d.Ore && d.GetPrecentage(d.Ore) < 0.05)
                    {
                        d.OreDecline = true;
                    }
                }
            }
            districtResourceList = latestDistrictResourceList;
        }

        static public void WriteResourceFile()
        {
            var resources = getResource();

            using (StreamWriter writeText = new StreamWriter("D:\\Workspace\\Cities\\resources.txt"))
            {
                int sqrt = (int)(Math.Sqrt(resources.Length));
                int sqrtDistrict = (int)(Math.Sqrt(resources.Length));
                writeText.WriteLine($"Lenght SQRT: {sqrt} and {sqrtDistrict}");
                for (int j = 0; j < sqrt; j++)
                {
                    string line = "";
                    for (int k = 0; k < sqrt; k++)
                    {
                        if (resources[sqrt * j + k].m_oil > 0)
                        {
                            line += "X";
                        }
                        else
                        {
                            line += "_";
                        }
                    }
                    writeText.WriteLine(line);
                }
            }
        }

        static public void WriteDistrictsFile()
        {
            var districts = getDistricts();

            using (StreamWriter writeText = new StreamWriter("D:\\Workspace\\Cities\\districts.txt"))
            {
                int sqrt = (int)(Math.Sqrt(districts.Length));
                int sqrtDistrict = (int)(Math.Sqrt(districts.Length));
                writeText.WriteLine($"Lenght SQRT: {sqrt} and {sqrtDistrict}");
                for (int j = 0; j < sqrt; j++)
                {
                    string line = "";
                    for (int k = 0; k < sqrt; k++)
                    {
                        DistrictManager.Cell cell = districts[j* sqrt + k];
                        int districtId = GetMatchingDistrictId(cell);

                        if (districtId != 0)
                        {
                            line += Convert.ToChar(districtId);
                        }
                        else
                        {
                            line += "_";
                        }
                    }
                    writeText.WriteLine(line);
                }
            }
        }

        static public int GetResourceIndex(int districtIndex)
        {
            return districtIndex;
        }

        static public int GetDistrictIndex(int resourceIndex)
        {
            int row = resourceIndex / 512;
            int col = resourceIndex % 512;
            if (row >= 114 && col >= 114 && row <= 398 && col <= 398)
            {
                double newRow = (row - 114) * 9 / 5;
                double newCol = (col - 114) * 9 / 5;
                double val = (newRow) * 512 + newCol;
                return (int)val;
            }
            else
            {
                return -1;
            }
        }

        static public string GetDistrictName(int districtID)
        {
            if (namedDistricts.Contains(districtID))
            {
                return getDistrictNameFromId(districtID);
            }

            for (int i = 0; i < 128; i++)
            {
                if (getDistrictNameFromId(i) != null)
                {
                    namedDistricts.Add(districtID);
                    return getDistrictNameFromId(districtID);
                }
            }
            return null;
        }

        static public int GetMatchingDistrictId(DistrictManager.Cell cell)
        {
            if (GetDistrictName(cell.m_district1) != null)
            {
                return (cell.m_district1);
            }
            else if (GetDistrictName(cell.m_district2) != null)
            {
                return (cell.m_district2);
            }
            else if (GetDistrictName(cell.m_district3) != null)
            {
                return (cell.m_district3);
            }
            else if (GetDistrictName(cell.m_district4) != null)
            {
                return (cell.m_district4);
            }
            else
            {
                return 0;
            }
        }
    }
}
