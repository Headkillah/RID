using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ResourceIndustryDistrict;
using UnityEngine;

namespace ResourceIndustryDistrict
{
    public class UIOptionsContainer : UIPanel
    {
        public UILabel _totalsLabel;
        public UICustomCheckbox _totals;
        
        public override void Start()
        {
            base.Start();

            _totals = AddUIComponent<UICustomCheckbox>();
            _totalsLabel = AddUIComponent<UILabel>();

            _totals.relativePosition = new Vector3(5, 0);
            _totalsLabel.relativePosition = new Vector3(20, 0);

            _totalsLabel.textScale = 0.85f;

            _totalsLabel.text = "Totals";

            _totals.size = new Vector2(12, 12);

            // event handler
            _totals.eventClick += (component, param) =>
            {
                _totals.IsChecked = !_totals.IsChecked;

                if (!_totals.IsChecked)
                {
                    parent.GetComponent<ResourceIndustryDistrictWindow>().SortDistrictLinesMethod();
                }
                else
                {
                    parent.GetComponent<ResourceIndustryDistrictWindow>().SortDistrictLinesMethod();
                }
            };

            width = 550;
            height = 20;
        }
    }
}
