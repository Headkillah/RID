using ResourceIndustryDistrict;
using System;
using Xunit;

namespace RID_Test
{
    public class Class1
    {
        [Fact]
        public void DeclineTest()
        {
            ResourceIndustryDistrict.DistrictResource.getResource = () =>
            {
                Random r = new Random();
                NaturalResourceManager.ResourceCell[] resourcesFromMap = new NaturalResourceManager.ResourceCell[512 * 512];
                for (int i = 0; i < resourcesFromMap.Length; i++)
                {
                    resourcesFromMap[i].m_fertility = (byte)r.Next(0, 255);
                    resourcesFromMap[i].m_forest = (byte)r.Next(0, 255);
                    resourcesFromMap[i].m_oil = (byte)r.Next(0, 255);
                    resourcesFromMap[i].m_ore = (byte)r.Next(0, 255);
                }
                return resourcesFromMap;
            };
            ResourceIndustryDistrict.DistrictResource.getDistricts = () =>
            {
                DistrictManager.Cell[] districts = generateDistricts();
                return districts;

            };
            ResourceIndustryDistrict.DistrictResource.getDistrictNames = () =>
            {
                Array8<District> districtNames = new Array8<District>(128);
                return districtNames;
            };
            ResourceIndustryDistrict.DistrictResource.getDistrictNameFromId = (districtId) =>
            {
                return districtId.ToString();
            };
            ResourceIndustryDistrict.DistrictResource.Calculate2();

            ResourceIndustryDistrict.DistrictResource.getResource = () =>
            {
                Random r = new Random();
                NaturalResourceManager.ResourceCell[] resourcesFromMap = new NaturalResourceManager.ResourceCell[512 * 512];
                for (int i = 0; i < resourcesFromMap.Length; i++)
                {
                    resourcesFromMap[i].m_fertility = (byte)r.Next(0, 2);
                    resourcesFromMap[i].m_forest = (byte)r.Next(0, 2);
                    resourcesFromMap[i].m_oil = (byte)r.Next(0, 2);
                    resourcesFromMap[i].m_ore = (byte)r.Next(0, 2);
                }
                return resourcesFromMap;
            };

            ResourceIndustryDistrict.DistrictResource.Calculate2();
        }
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
            ResourceIndustryDistrict.DistrictResource.getResource = () =>
            {
                Random r = new Random();
                NaturalResourceManager.ResourceCell[] resourcesFromMap = new NaturalResourceManager.ResourceCell[512 * 512];
                for(int i = 0; i < resourcesFromMap.Length ; i++)
                {
                    resourcesFromMap[i].m_fertility = (byte)r.Next(0, 255);
                    resourcesFromMap[i].m_forest = (byte)r.Next(0, 255);
                    resourcesFromMap[i].m_oil = (byte)r.Next(0, 255);
                    resourcesFromMap[i].m_ore = (byte)r.Next(0, 255);
                }
                return resourcesFromMap;
            };
            ResourceIndustryDistrict.DistrictResource.getDistricts = () =>
            {
                DistrictManager.Cell[] districts = generateDistricts();
                return districts;

            };
            ResourceIndustryDistrict.DistrictResource.getDistrictNames = () =>
            {
                Array8<District> districtNames = new Array8<District>(128);
                return districtNames;
            };
            ResourceIndustryDistrict.DistrictResource.getDistrictNameFromId = (districtId) =>
            {
                return districtId.ToString();
            };
            ResourceIndustryDistrict.DistrictResource.Calculate2();

            ResourceIndustryDistrict.DistrictResource.Calculate2();

            ResourceIndustryDistrict.DistrictResource.Calculate2();

            ResourceIndustryDistrict.DistrictResource.districtResourceList.Sort(new LineComparer("Size", true, false));
        }

        [Fact]
        public void GetDistrictIndexTest()
        {
            DistrictResource.GetDistrictIndex(58368);
            DistrictResource.GetDistrictIndex(0);

            DistrictResource.GetDistrictIndex(113 * 512 + 113);
            DistrictResource.GetDistrictIndex(113 * 512 + 114);
            DistrictResource.GetDistrictIndex(114 * 512 + 114);

            DistrictResource.GetDistrictIndex(170 * 512 + 123);
            DistrictResource.GetDistrictIndex(256 * 512 + 256);
            DistrictResource.GetDistrictIndex(256 * 512 + 257);
            DistrictResource.GetDistrictIndex(257 * 512 + 257);

            DistrictResource.GetDistrictIndex(398 * 512 + 398);
            DistrictResource.GetDistrictIndex(398 * 512 + 399);
            DistrictResource.GetDistrictIndex(399 * 512 + 399);
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        public DistrictManager.Cell[] generateDistricts ()
        {
            string[] lines = System.IO.File.ReadAllLines(@"D:\Workspace\Cities\districtsFile.txt");
            DistrictManager.Cell[] districts = new DistrictManager.Cell[512*512];
            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                for(int j = 0; j < line.Length; j++)
                {
                    char c = line[j];
                    districts[i * lines.Length + j].m_district1 = (byte)c;
                }
            }
            return districts;
        }
    }
}
