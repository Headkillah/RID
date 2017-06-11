﻿using ColossalFramework.Plugins;
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

        private bool ascending = false;
        private string lastCol = "";

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
            _fertility.relativePosition = new Vector3(220, 0);
            _ore.relativePosition = new Vector3(290, 0);
            _forest.relativePosition = new Vector3(360, 0);
            _size.relativePosition = new Vector2(430, 0);
            _type.relativePosition = new Vector2(500, 0);

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
            _name.eventClick += (component, param) => SortAndChangeAscending("Name");
            _oil.eventClick += (component, eventParam) => SortAndChangeAscending("Oil");
            _ore.eventClick += (component, param) => SortAndChangeAscending("Ore");
            _fertility.eventClick += (component, param) => SortAndChangeAscending("Fertility");
            _forest.eventClick += (component, param) => SortAndChangeAscending("Forest");
            _size.eventClick += (component, param) => SortAndChangeAscending("Size");
            _type.eventClick += (component, param) => SortAndChangeAscending("Type");


            width = 550;
            height = 20;
        }
        public void SortAndChangeAscending(string colName)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"colName: {colName}");
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"lastCol: {lastCol}");
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"ascending: {ascending}");

            if (lastCol == colName)
            {
                
                ascending = !ascending;
            }
            else
            {
                ascending = false;
            }
            SortDelegate(colName, ascending);
            
            
            lastCol = colName;

            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"colName: {colName}");
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"lastCol: {lastCol}");
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"ascending: {ascending}");
        }
    }
}
