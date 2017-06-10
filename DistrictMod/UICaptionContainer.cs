using ColossalFramework.UI;
using ResourceIndustryDistrict;
using UnityEngine;

namespace ResourceIndustryDistrict
{
    public class UICaptionContainer : UIPanel
    {
        public SortTransportLinesDelegate SortDelegate { get; set; }

        private UILabel _name;
        private UILabel _oil;
        private UILabel _ore;
        private UILabel _fertility;
        private UILabel _forest;
        private UILabel _type;
        private UILabel _size;

        public override void Start()
        {
            base.Start();

            _name = AddUIComponent<UILabel>();
            _type = AddUIComponent<UILabel>();
            _size = AddUIComponent<UILabel>();
            _oil = AddUIComponent<UILabel>();
            _ore = AddUIComponent<UILabel>();
            _fertility = AddUIComponent<UILabel>();
            _forest = AddUIComponent<UILabel>();

            _name.relativePosition = new Vector3(5, 0);
            _oil.relativePosition = new Vector3(150, 0);
            _fertility.relativePosition = new Vector3(200, 0);
            _ore.relativePosition = new Vector3(250, 0);
            _forest.relativePosition = new Vector3(300, 0);
            _size.relativePosition = new Vector2(350, 0);
            _type.relativePosition = new Vector2(400, 0);

            _name.textScale = 0.85f;
            _oil.textScale = 0.85f;
            _ore.textScale = 0.85f;
            _fertility.textScale = 0.85f;
            _forest.textScale = 0.85f;
            _size.textScale = 0.85f;
            _type.textScale = 0.85f;

            _name.text = "Name";
            _oil.text = "Oil";
            _fertility.text = "Fertility";
            _ore.text = "Ore";
            _forest.text = "Forest";
            _size.text = "Size";
            _type.text = "Type";
 
            // sort by each column
            // ultimately based on value from UITransportLineRow (via LineComparer)
            _name.eventClick += (component, param) => SortDelegate("Name");
            _oil.eventClick += (component, eventParam) => SortDelegate("Oil");
            _ore.eventClick += (component, param) => SortDelegate("Ore");
            _fertility.eventClick += (component, param) => SortDelegate("Fertility");
            _forest.eventClick += (component, param) => SortDelegate("Forest");
            _size.eventClick += (component, param) => SortDelegate("Size");
            _type.eventClick += (component, param) => SortDelegate("Type");


            width = 550;
            height = 20;
        }
    }
}
