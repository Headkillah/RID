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
        private UILabel districtTypeLabel;
        private UILabel oilLabel;
        private UILabel fertilityLabel;
        private UILabel oreLabel;
        private UILabel forestLabel;
        private UILabel sizeLabel;

        //private InstanceID _instanceID;

        public string Name { get; set; }
        public int Type { get; set; }
        public double Oil { get; set; }
        public double Fertility { get; set; }
        public double Ore { get; set; }
        public double Forest { get; set; }
        public int Size { get; set; }

        public override void Awake()
        {
            base.Awake();

            height = 32;
            width = 550;
        }

        public override void Start()
        {
            base.Start();

            districtNameLabel = AddUIComponent<UILabel>();
            districtTypeLabel = AddUIComponent<UILabel>();
            oilLabel = AddUIComponent<UILabel>();
            fertilityLabel = AddUIComponent<UILabel>();
            oreLabel = AddUIComponent<UILabel>();
            forestLabel = AddUIComponent<UILabel>();
            sizeLabel = AddUIComponent<UILabel>();

            districtNameLabel.relativePosition = new Vector2(5, 0);
            oilLabel.relativePosition = new Vector2(150, 0);
            fertilityLabel.relativePosition = new Vector2(200, 0);
            oreLabel.relativePosition = new Vector2(250, 0);
            forestLabel.relativePosition = new Vector2(300, 0);
            sizeLabel.relativePosition = new Vector2(350, 0);
            districtTypeLabel.relativePosition = new Vector2(400, 0);

            districtNameLabel.textColor = new Color32(182, 221, 254, 255);
            districtTypeLabel.textColor = new Color32(182, 221, 254, 255);
            oilLabel.textColor = new Color32(182, 221, 254, 255);
            fertilityLabel.textColor = new Color32(182, 221, 254, 255);
            oreLabel.textColor = new Color32(182, 221, 254, 255);
            forestLabel.textColor = new Color32(182, 221, 254, 255);
            sizeLabel.textColor = new Color32(182, 221, 254, 255);

            districtNameLabel.textScale = 0.8f;
            districtTypeLabel.textScale = 0.8f;
            oilLabel.textScale = 0.8f;
            fertilityLabel.textScale = 0.8f;
            oreLabel.textScale = 0.8f;
            forestLabel.textScale = 0.8f;
            sizeLabel.textScale = 0.8f;

            backgroundSprite = "GenericPanelLight";
            color = new Color32(150, 150, 150, 255);

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
            districtTypeLabel.text = Type.ToString();
            oilLabel.text = Oil.ToString("P");
            fertilityLabel.text = Fertility.ToString("P");
            oreLabel.text = Ore.ToString("P");
            forestLabel.text = Forest.ToString("P");
            sizeLabel.text = Size.ToString();
        }
    }
}
