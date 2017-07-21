using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ResourceIndustryDistrict
{
    class DistrictResource
    {
        public static List<DistrictResourceData> districtResourceList = new List<DistrictResourceData>();
        private static List<int> namedDistricts = new List<int>();

        static public void Calculate()
        {
            districtResourceList.Clear();
            NaturalResourceManager.ResourceCell[] resourcesFromMap = new NaturalResourceManager.ResourceCell[NaturalResourceManager.instance.m_naturalResources.Length];
            Array.Copy(NaturalResourceManager.instance.m_naturalResources, resourcesFromMap, NaturalResourceManager.instance.m_naturalResources.Length);

            for (int i = 0; i < DistrictManager.instance.m_districtGrid.Length; i++)
            {
                DistrictManager.Cell cell = DistrictManager.instance.m_districtGrid[i];
                int districtId = GetMatchingDistrictId(cell);
                string districtName = GetDistrictName(districtId);
                if (districtName != null)
                {
                    DistrictResourceData result = districtResourceList.Find(x => x.Name.Contains(districtName));
                    if (result == null)
                    {
                        District district = (District)DistrictManager.instance.m_districts.m_buffer.GetValue(districtId);
                        int districtType = (int)district.m_specializationPolicies;
                        districtResourceList.Add(new DistrictResourceData() { Name = districtName ,  Type = districtType });
                        result = districtResourceList[districtResourceList.Count - 1];
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
        }

        static public void WriteFile()
        {
            NaturalResourceManager.ResourceCell[] resourcesFromMap = new NaturalResourceManager.ResourceCell[NaturalResourceManager.instance.m_naturalResources.Length];
            Array.Copy(NaturalResourceManager.instance.m_naturalResources, resourcesFromMap, NaturalResourceManager.instance.m_naturalResources.Length);

            using (StreamWriter writeText = new StreamWriter("D:\\Workspace\\Cities\\oil2.txt"))
            {
                int sqrt = (int)(Math.Sqrt(NaturalResourceManager.instance.m_naturalResources.Length));
                int sqrtDistrict = (int)(Math.Sqrt(DistrictManager.instance.m_districtGrid.Length));
                writeText.WriteLine($"Lenght SQRT: {sqrt} and {sqrtDistrict}");
                for (int j = 0; j < sqrt; j++)
                {
                    string line = "";
                    for (int k = 0; k < sqrt; k++)
                    {
                        if (resourcesFromMap[sqrt * j + k].m_oil > 0)
                        {
                            line += resourcesFromMap[sqrt * j + k].m_oil;
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

        static public string GetDistrictName(int districtID)
        {
            if (namedDistricts.Contains(districtID))
            {
                return DistrictManager.instance.GetDistrictName(districtID);
            }

            for (int i = 0; i < 128; i++)
            {
                if (DistrictManager.instance.GetDistrictName(i) != null)
                {
                    namedDistricts.Add(districtID);
                    return DistrictManager.instance.GetDistrictName(districtID);
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
