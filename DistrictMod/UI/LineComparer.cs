using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ResourceIndustryDistrict
{
    public class LineComparer : Comparer<DistrictResourceData>
    {
        private string _sortFieldName;
        private bool _ascending;
        private bool _totals;

        public LineComparer(string sortFieldName = "Name", bool ascending = true, bool totals = false) : base()
        {
            _sortFieldName = sortFieldName;
            _ascending = ascending;
            _totals = totals;
        }

        public override int Compare(DistrictResourceData x, DistrictResourceData y)
        {
            int compare;
            PropertyInfo xproperty = x.GetType().GetProperty(_sortFieldName);
            PropertyInfo yproperty = y.GetType().GetProperty(_sortFieldName);
            var lhs = xproperty.GetValue(x, null);
            var rhs = yproperty.GetValue(y, null);

            Type lhsType = lhs.GetType();
            Type rhsType = rhs.GetType();
            if (lhsType.IsPrimitive && rhsType.IsPrimitive)
            {
                if (!_totals)
                {
                    lhs = x.GetPrecentage((int)lhs);
                    rhs = y.GetPrecentage((int)rhs);
                }
                compare = ((IComparable)Convert.ToDouble(lhs)).CompareTo(Convert.ToDouble(rhs));
                return _ascending ? compare : compare * -1;
            }

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