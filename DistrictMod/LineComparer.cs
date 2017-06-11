﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ResourceIndustryDistrict
{
    public class LineComparer : Comparer<GameObject>
    {
        private string _sortFieldName;
        private bool _ascending;

        public LineComparer(string sortFieldName = "Name", bool ascending = false) : base()
        {
            _sortFieldName = sortFieldName;
            _ascending = ascending;
        }

        public override int Compare(GameObject x, GameObject y)
        {
            int compare;
            PropertyInfo xproperty = x.GetComponent<ResourceIndustryDistrictLineRow>().GetType().GetProperty(_sortFieldName);
            PropertyInfo yproperty = y.GetComponent<ResourceIndustryDistrictLineRow>().GetType().GetProperty(_sortFieldName);
            var lhs = xproperty.GetValue(x.GetComponent<ResourceIndustryDistrictLineRow>(), null);
            var rhs = yproperty.GetValue(y.GetComponent<ResourceIndustryDistrictLineRow>(), null);

            if (lhs == rhs)
                return 0;

            var tokensLhs = Regex.Split(lhs.ToString().Replace(" ", ""), "([0-9]+)");
            var tokensRhs = Regex.Split(rhs.ToString().Replace(" ", ""), "([0-9]+)");

            for (var i = 0; i < tokensLhs.Length && i < tokensRhs.Length; ++i)
            {
                if (tokensLhs[i] == tokensRhs[i])
                    continue;

                int valueLhs;
                if (!int.TryParse(tokensLhs[i], out valueLhs))
                {
                    compare = String.Compare(tokensLhs[i], tokensRhs[i], StringComparison.OrdinalIgnoreCase);
                    return _ascending ? compare : compare * -1;
                }

                int valueRhs;
                if (!int.TryParse(tokensRhs[i], out valueRhs))
                {
                    compare = String.Compare(tokensLhs[i], tokensRhs[i], StringComparison.OrdinalIgnoreCase);
                    return _ascending ? compare : compare * -1; 
                }

                compare = valueLhs.CompareTo(valueRhs);
                return _ascending ? compare : compare * 1;
            }

            if (tokensLhs.Length < tokensRhs.Length)
                return -1;
            if (tokensLhs.Length > tokensRhs.Length)
                return 1;

            return 0;
        }
    }
}