using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace ResourceIndustryDistrict
{
    public class ResourceIndustryDistrictLoading : LoadingExtensionBase
    {
        GameObject buildingWindowGameObject;
        GameObject buttonObject;
        GameObject buttonObject2;
        UIButton menuButton;

        ResourceIndustryDistrictWindow buildingWindow;
        private LoadMode _mode;
        
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame && mode != LoadMode.NewMap && mode != LoadMode.LoadMap)
                return;
            _mode = mode;

            buildingWindowGameObject = new GameObject("buildingWindowObject");

            var view = UIView.GetAView();
            this.buildingWindow = buildingWindowGameObject.AddComponent<ResourceIndustryDistrictWindow>();
            this.buildingWindow.transform.parent = view.transform;
            this.buildingWindow.position = new Vector3(300, 122);
            this.buildingWindow.Hide();


            UITabstrip strip = null;
            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame)
            {
                strip = ToolsModifierControl.mainToolbar.component as UITabstrip;
            }
            else
            {
                strip = UIView.Find<UITabstrip>("MainToolstrip");
            }

            buttonObject = UITemplateManager.GetAsGameObject("MainToolbarButtonTemplate");
            buttonObject2 = UITemplateManager.GetAsGameObject("ScrollablePanelTemplate");
            menuButton = strip.AddTab("ResourceIndustryDistrict", buttonObject, buttonObject2, new Type[] { }) as UIButton;

            string sprite = "ToolbarIconDistrictPressed";

            menuButton.normalFgSprite = sprite;
            menuButton.focusedFgSprite = sprite;
            menuButton.hoveredFgSprite = sprite;
            menuButton.pressedFgSprite = sprite;
            menuButton.disabledFgSprite = sprite;
            menuButton.tooltip = "RDI";

            menuButton.eventClick += uiButton_eventClick;
        }

        private void uiButton_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {

            if (!this.buildingWindow.isVisible)
            {
                this.buildingWindow.isVisible = true;
                this.buildingWindow.BringToFront();
                this.buildingWindow.Show();
                this.buildingWindow.SortTransportLinesMethod();
            }
            else
            {
                this.buildingWindow.isVisible = false;
                this.buildingWindow.Hide();
            }
        }

        public override void OnLevelUnloading()
        {
            if (_mode != LoadMode.LoadGame && _mode != LoadMode.NewGame && _mode != LoadMode.NewMap && _mode != LoadMode.LoadMap)
                return;


            if (buildingWindowGameObject != null)
            {
                GameObject.Destroy(buildingWindowGameObject);
            }

            if (buttonObject != null)
            {
                GameObject.Destroy(buttonObject);
                GameObject.Destroy(buttonObject2);
                UIComponent.Destroy(menuButton);
            }
        }
    }
}
