using ResourceIndustryDistrict;
using System;
using Xunit;

namespace RID_Test
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
            ResourceIndustryDistrict.DistrictResource.getResource = () =>
            {
                NaturalResourceManager.ResourceCell[] resourcesFromMap = new NaturalResourceManager.ResourceCell[512 * 512];
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
            ResourceIndustryDistrict.DistrictResource.Calculate();

            ResourceIndustryDistrict.DistrictResource.districtResourceList.Sort(new LineComparer("Size", true, false));
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
