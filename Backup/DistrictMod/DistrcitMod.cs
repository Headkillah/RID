using ICities;
using UnityEngine;
using ColossalFramework.UI;
using ColossalFramework.Plugins;

namespace UIMod
{
    public class UIMod : IUserMod
    {
        public string Name { get { return "UIMod"; } }
        public string Description { get { return "UIMod"; } }
    }

    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            // this seems to get the default UIView
            UIView v = UIView.GetAView();

            //this adds an UIComponent to the view
            UIComponent uic = v.AddUIComponent(typeof(DistrictPanel));

            // ALTERNATIVELY, this seems to work like the lines above, but is a bit longer:
            // UIView v = UIView.GetAView ();
            // GameObject go = new GameObject ("panelthing", typeof(ExamplePanel));
            // UIComponent uic = v.AttachUIComponent (go);
        }

    }

    //whatever child class of UIComponent, here UIPanel
    public class DistrictPanel : UIScrollablePanel
    {
        //these overrides are not necessary, but obviously helpful
        //this seems to be called initially
        public override void Start()
        {
            //this makes the panel "visible", I don't know what sprites are available, but found this value to work
            this.backgroundSprite = "GenericPanel";
            this.color = new Color32(0, 0, 255, 100);
            this.width =  100;
            this.height = 200;


            

            int districtCount = DistrictManager.instance.m_districtCount;

            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "" + districtCount);

            UILabel[] labelList = new UILabel[districtCount];
            
            string name;
            int areaMinX;
            int areaMaxX;
            int areaMinZ;
            int areaMaxZ;
            int population;
            
            for (int i = 0; i <  128; i++)
            {
                name = DistrictManager.instance.GetDistrictName(i);
                DistrictManager.instance.GetDistrictArea((byte)i, out areaMinX, out areaMinZ, out areaMaxX, out areaMaxZ);
                
                if (name != null) 
                {
                    Array8<District> myDistrcitList = DistrictManager.instance.m_districts;
                    District myCurrentDistrict = myDistrcitList.m_buffer[i];
                    population = (int)myCurrentDistrict.m_populationData.m_finalCount;
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, i + ":" + name + ":" + population +"<" + areaMinX + "<" + areaMinZ + "<" + areaMaxX + "<" + areaMaxZ);
                }
                //labelList[i].text = DistrictManager.instance.GetDistrictName(i);
                //labelList[i].position = new Vector3(10, 20*(districtCount-i));
            }
            uint totalOre, totalOil, totalForest, totalFertility, totalWater;
            uint unlockableOre, unlockableOil, unlockableForest, unlockableFertility, unlockableWater;
            uint unlockedOre, unlockedOil, unlockedForest, unlockedFertility, unlockedWater;
            uint usedOre, usedOil, usedForest, usedFertility;

            NaturalResourceManager.instance.CalculateTotalResources(out totalOre, out totalOil, out totalForest, out totalFertility, out totalWater);
            NaturalResourceManager.instance.CalculateUnlockableResources(out unlockableOre, out unlockableOil, out unlockableForest, out unlockableFertility, out unlockableWater);
            NaturalResourceManager.instance.CalculateUnlockedResources(out unlockedOre, out unlockedOil, out unlockedForest, out unlockedFertility, out unlockedWater);
            NaturalResourceManager.instance.CalculateUsedResources(out usedOre, out usedOil, out usedForest, out usedFertility);
            
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "total:" + " " + totalOre + " " + totalOil + " " + totalForest + " " + totalFertility + " " + totalWater);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "unlockable:" + " " + unlockableOre + " " + unlockableOil + " " + unlockableForest + " " + unlockableFertility + " " + unlockableWater);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "unlocked:" + " " + unlockedOre + " " + unlockedOil + " " + unlockedForest + " " + unlockedFertility + " " + unlockedWater);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "used:" + " " + usedOre + " " + usedOil + " " + usedForest + " " + usedFertility);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "areaResource: " + NaturalResourceManager.instance.m_areaResources.Length + " naturalResource" + NaturalResourceManager.instance.m_naturalResources.Length);

            for (int i = 0; i < NaturalResourceManager.instance.m_areaResources.Length; i++)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "" + i + ": " +
                    NaturalResourceManager.instance.m_areaResources[i].m_finalOre + ": " +
                    NaturalResourceManager.instance.m_areaResources[i].m_finalOil + ": " +
                    NaturalResourceManager.instance.m_areaResources[i].m_finalForest + ": " +
                    NaturalResourceManager.instance.m_areaResources[i].m_finalFertility + ": " +
                    NaturalResourceManager.instance.m_areaResources[i].m_tempOre + ": " +
                    NaturalResourceManager.instance.m_areaResources[i].m_tempOre + ": " +
                    NaturalResourceManager.instance.m_areaResources[i].m_tempForest + ": " +
                    NaturalResourceManager.instance.m_areaResources[i].m_tempFertility); 
                    
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\Workspace\\test.txt");


            for (int i = 0; i < 1; i++)
            {
                string row = "";

                for (int j = 0; j < 10; i++)
                {
                    row += NaturalResourceManager.instance.m_naturalResources[i*512 + j].m_ore;
                    row += ",";
                }
                file.Write("i" + i + System.Environment.NewLine);
                file.Write(row);
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, row);
            } 
                
            /*
            for (int i = 0; i < 512; i++)
            {
                string row = "";

                for (int j = 0; j < 512; i++)
                {
                    uint ore, oil, forest, fertility, water;
                    NaturalResourceManager.instance.GetTileResources(i, j, out ore, out oil, out forest, out fertility,out water);
                    row += ore;
                    row += ",";
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, row);
                }
            }*/

            UILabel x = this.AddUIComponent<UILabel>();
            UILabel y = this.AddUIComponent<UILabel>();
            UILabel z = this.AddUIComponent<UILabel>();
            
/*
            x.text = "X";
            y.text = "Y";
            z.text = "Z";
            x.position = new Vector3(10, 100);
            y.position = new Vector3(10, 80);
            z.position = new Vector3(10, 60);
            */
            
            
            

            /*for (int i = 0; i < districtCount; i++)
            {
                labelList[i] = this.AddUIComponent<UILabel>();
                labelList[i].text = DistrictManager.instance.GetDistrictName(i);
            }
            
            UIListBox districtList = this.AddUIComponent<UIListBox>();
            districtList.Enable();
            districtList.size = new Vector2(80, 80);
            districtList.color = new Color32(255, 0, 0, 100);
            districtList.animateHover = true;
            districtList.itemHeight = 10;
            districtList.itemHighlight = "penis";
            districtList.itemHover = "poonani";
            districtList.itemPadding = new RectOffset(1, 2, 3, 4);
            districtList.itemTextColor = new Color32(255, 255, 255, 100);
            districtList.listPadding = new RectOffset(1, 2, 3, 4);
            districtList.scrollbar = new UIScrollbar();
            districtList.scrollPosition = 0;
            districtList.selectedIndex = 1;

          
            

            districtList.items[0] = "Hello";
            districtList.items[1] = "Caca";
              */

            //x.text = DistrictManager.instance.GetDistrictName(1);
            


        }
        //this gets called every frame
        public override void Update()
        {
        }
    }
}