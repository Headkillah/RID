using System;
using ColossalFramework.UI;
using UnityEngine;

namespace ResourceIndustryDistrict
{
    class ResourceIndustryDistrictLineRow : UIPanel
    {

        public delegate void LineNameChangedHandler(ushort lineID);
        public event LineNameChangedHandler LineNameChanged;

        private UILabel districtNameLabel;
        private UISprite districtTypeLabel;
        private UILabel oilLabel;
        private UILabel farmingLabel;
        private UILabel oreLabel;
        private UILabel forestLabel;
        private UILabel sizeLabel;

        public string Name { get; set; }
        public int Type { get; set; }
        public double Oil { get; set; }
        public double Farming { get; set; }
        public double Ore { get; set; }
        public double Forest { get; set; }
        public int Size { get; set; }

        public bool IsOdd { get; set; }

        public override void Awake()
        {
            base.Awake();

            height = 32;
            width = 550;
        }

        public string GetSpriteFromType(int type)
        {
            switch(type)
            {
                case 0:
                    return "IconPolicyNone";
                case 1:
                    return "IconPolicyForest";
                case 2:
                    return "IconPolicyFarming";
                case 4:
                    return "IconPolicyOil";
                case 8:
                    return "IconPolicyOre";
                case 16:
                    return "IconPolicyLeisure";
                case 32:
                    return "IconPolicyTourist";
                default:
                    return "";
            }
        }

        public override void Start()
        {
            base.Start();

            districtNameLabel = AddUIComponent<UILabel>();
            districtTypeLabel = AddUIComponent<UISprite>();
            oilLabel = AddUIComponent<UILabel>();
            farmingLabel = AddUIComponent<UILabel>();
            oreLabel = AddUIComponent<UILabel>();
            forestLabel = AddUIComponent<UILabel>();
            sizeLabel = AddUIComponent<UILabel>();

            districtNameLabel.relativePosition = new Vector2(5, 0);
            oilLabel.relativePosition = new Vector2(150, 0);
            farmingLabel.relativePosition = new Vector2(220, 0);
            oreLabel.relativePosition = new Vector2(290, 0);
            forestLabel.relativePosition = new Vector2(360, 0);
            sizeLabel.relativePosition = new Vector2(430, 0);
            districtTypeLabel.relativePosition = new Vector2(508 , 8);

            districtNameLabel.textColor = new Color32(182, 221, 254, 255);
            districtTypeLabel.spriteName = GetSpriteFromType(Type);
            districtTypeLabel.size = new Vector2(25, 25);
            oilLabel.textColor = new Color32(182, 221, 254, 255);
            farmingLabel.textColor = new Color32(182, 221, 254, 255);
            oreLabel.textColor = new Color32(182, 221, 254, 255);
            forestLabel.textColor = new Color32(182, 221, 254, 255);
            sizeLabel.textColor = new Color32(182, 221, 254, 255);

            eventMouseHover += (component, param) =>
            {
                color = new Color32(180, 180, 180, 255);
            };

            eventMouseLeave += (component, param) =>
            {
                if (IsOdd)
                    color = new Color32(150, 150, 150, 255);
                else
                    color = new Color32(130, 130, 130, 255);
            };


            districtNameLabel.textScale = 0.8f;
            oilLabel.textScale = 0.8f;
            farmingLabel.textScale = 0.8f;
            oreLabel.textScale = 0.8f;
            forestLabel.textScale = 0.8f;
            sizeLabel.textScale = 0.8f;           
            
            // zebra stripes background
            backgroundSprite = "GenericPanelLight";
            if (IsOdd)
                color = new Color32(150, 150, 150, 255);
            else
                color = new Color32(130, 130, 130, 255);

            // center elements in row
            UIComponent[] children = GetComponentsInChildren<UIComponent>();
            foreach (UIComponent child in children)
            {
                if (child == this) continue;

                child.pivot = UIPivotPoint.MiddleLeft;
                child.transformPosition = new Vector3(child.transformPosition.x, GetBounds().center.y, 0);
            }
        }

        public override void Update()
        {
            base.Update();

            districtNameLabel.text = Name.ToString();
            //districtTypeLabel.text = Type.ToString();
            oilLabel.text = Oil.ToString("P");
            farmingLabel.text = Farming.ToString("P");
            oreLabel.text = Ore.ToString("P");
            forestLabel.text = Forest.ToString("P");
            sizeLabel.text = Size.ToString();
        }
    }
}
