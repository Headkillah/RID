using ColossalFramework.Plugins;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceIndustryDistrict
{
    public delegate void SortDistrictLinesDelegate(string sortFieldName = "Name", bool ascending = false);

    public class ResourceIndustryDistrictWindow : UIPanel
    {
        private UITitleContainer _title;
        private UIOptionsContainer _options;
        private UICaptionContainer _captions;
        private List<GameObject> districtLineLabels;
        private UIScrollablePanel _scrollablePanel;
        private UIPanel _panelForScrollPanel;

    
        public override void Awake()
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"Awake()");

            base.Awake();
        }

        //this seems to be called initially
        public override void Start()
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"Start()");
            base.Start();
            
            relativePosition = new Vector3(396, 58);
            backgroundSprite = "MenuPanel2";

            isInteractive = true;
            width = 550;
            height = 700;

            enabled = true;

            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            autoLayoutPadding = new RectOffset(0, 0, 1, 1);
            autoLayoutStart = LayoutStart.TopLeft;

            SetupControls();
            SetupScrollPanel();
            PopulateDistrictLineLabels();
        }

        //this gets called every frame
        public override void Update()
        {
            //DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"Update()");
        }

        public void SetupControls()
        {
            _title = AddUIComponent<UITitleContainer>();
            _title.Parent = this;

            _title.IconSprite = "FeatureDistricts";
            _title.TitleText = "Resource Industry Districts";

            _options = AddUIComponent<UIOptionsContainer>();
            
            _captions = AddUIComponent<UICaptionContainer>();
            _captions.SortDelegate = SortDistrictLinesMethod;

            districtLineLabels = new List<GameObject>();
        }

        public void SetupScrollPanel()
        {
            //this probably needs to exist, otherwise the autoLayout of this UITransportPanel places the scrollbar weird
            _panelForScrollPanel = AddUIComponent<UIPanel>();
            // needed so that the colorpicker finds the right parent
            _panelForScrollPanel.gameObject.AddComponent<UICustomControl>();

            _panelForScrollPanel.width = width - 6;
            //_captions reporting 450 height? fixed value of 20
            _panelForScrollPanel.height = height - _title.height - 20 * 2 - autoLayoutPadding.bottom * 4 - autoLayoutPadding.top * 4;
            
            // taken from http://www.reddit.com/r/CitiesSkylinesModding/comments/2zrz0k/extended_public_transport_ui_provides_addtional/cpnet5q
            _scrollablePanel = _panelForScrollPanel.AddUIComponent<UIScrollablePanel>();
            _scrollablePanel.width = _scrollablePanel.parent.width - 5f;
            _scrollablePanel.height = _scrollablePanel.parent.height;

            _scrollablePanel.autoLayout = true;
            _scrollablePanel.autoLayoutDirection = LayoutDirection.Vertical;
            _scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;
            _scrollablePanel.autoLayoutPadding = new RectOffset(2, 5, 0, 0);
            _scrollablePanel.clipChildren = true;

            _scrollablePanel.pivot = UIPivotPoint.TopLeft;
            _scrollablePanel.AlignTo(_scrollablePanel.parent, UIAlignAnchor.TopLeft);

            UIScrollbar scrollbar = _panelForScrollPanel.AddUIComponent<UIScrollbar>();
            scrollbar.width = scrollbar.parent.width - _scrollablePanel.width;
            scrollbar.height = _scrollablePanel.height;
            scrollbar.orientation = UIOrientation.Vertical;
            scrollbar.pivot = UIPivotPoint.BottomLeft;
            scrollbar.AlignTo(scrollbar.parent, UIAlignAnchor.TopRight);
            scrollbar.minValue = 0;
            scrollbar.value = 0;
            scrollbar.incrementAmount = 50;

            UISlicedSprite tracSprite = scrollbar.AddUIComponent<UISlicedSprite>();
            tracSprite.relativePosition = Vector2.zero;
            tracSprite.autoSize = true;
            tracSprite.size = tracSprite.parent.size;
            tracSprite.fillDirection = UIFillDirection.Vertical;
            tracSprite.spriteName = "ScrollbarTrack";

            scrollbar.trackObject = tracSprite;

            UISlicedSprite thumbSprite = tracSprite.AddUIComponent<UISlicedSprite>();
            thumbSprite.relativePosition = Vector2.zero;
            thumbSprite.fillDirection = UIFillDirection.Vertical;
            thumbSprite.autoSize = true;
            thumbSprite.width = thumbSprite.parent.width;
            thumbSprite.spriteName = "ScrollbarThumb";

            scrollbar.thumbObject = thumbSprite;

            _scrollablePanel.verticalScrollbar = scrollbar;
            _scrollablePanel.eventMouseWheel += (component, param) =>
            {
                var sign = Math.Sign(param.wheelDelta);
                _scrollablePanel.scrollPosition += new Vector2(0, sign * (-1) * 20);
            };
        }

        public void UpdateDistrictLineLabels(string sortFieldName = "Name", bool ascending = true, bool totals = true)
        {
            SortAndAdd(sortFieldName, ascending, totals);

            int counter = 0;
            foreach (var index in DistrictResource.districtResourceList)
            {
                var go = new GameObject();
                var uic = districtLineLabels[counter].GetComponent<ResourceIndustryDistrictLineRow>();
                SetFields(uic, index, totals);
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"counter ${counter}");
                counter++;
            }

            foreach (var go in districtLineLabels)
            {
                _scrollablePanel.AttachUIComponent(go);
            }

        }

        public void PopulateDistrictLineLabels(string sortFieldName = "Name", bool ascending = true, bool totals = true)
        {
            SortAndAdd(sortFieldName, ascending, totals);

            foreach (var index in DistrictResource.districtResourceList)
            {
                var go = new GameObject();
                var uic = go.AddComponent<ResourceIndustryDistrictLineRow>();
                SetFields(uic, index, totals);

                districtLineLabels.Add(go);
            }

            bool odd = false;
            foreach (var go in districtLineLabels)
            {
                _scrollablePanel.AttachUIComponent(go);
                go.GetComponent<ResourceIndustryDistrictLineRow>().IsOdd = odd;
                odd = !odd;

            }
        }

        public void ClearDistrictLineLabels()
        {
            // the obvious approach using RemoveUIComponent doesn't work -
            // probably because it only removes the Component in the PoolList
            // but doesn't remove the Unity GameObject containing the Component itself
            foreach (var go in districtLineLabels)
                Destroy(go);
            districtLineLabels.Clear();
        }

        public void CreateDistrictLinesMethod(string sortFieldName = "Name", bool ascending = true)
        {
            ClearDistrictLineLabels();
            PopulateDistrictLineLabels(sortFieldName, ascending, _options._totals.IsChecked);
        }

        public void SortDistrictLinesMethod(string sortFieldName = "Name", bool ascending = true)
        {
            UpdateDistrictLineLabels(sortFieldName, ascending, _options._totals.IsChecked);
        }

        void SortAndAdd(string sortFieldName, bool ascending, bool totals)
        {
            DistrictResource.districtResourceList.Sort(new LineComparer(sortFieldName, ascending, totals));
        }

        void SetFields(ResourceIndustryDistrictLineRow uic, DistrictResourceData index, bool totals)
        {
            uic.Oil = totals ? index.Oil : index.GetPrecentage(index.Oil);
            uic.Farming = totals ? index.Farming : index.GetPrecentage(index.Farming);
            uic.Ore = totals ? index.Ore : index.GetPrecentage(index.Ore);
            uic.Forest = totals ? index.Forest : index.GetPrecentage(index.Forest);
            uic.Name = index.Name;
            uic.Size = index.Size;
            uic.Type = index.Type;
            uic.totals = totals;
            uic.OilDecline = index.OilDecline;
            uic.OreDecline = index.OreDecline;
        }

    }
}
