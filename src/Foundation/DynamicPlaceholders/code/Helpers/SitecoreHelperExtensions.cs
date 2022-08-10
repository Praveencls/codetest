using Sitecore.Configuration;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Helpers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CodeTest.Foundation.DynamicPlaceholder.Helpers
{

    public static class SitecoreHelperExtension
    {
        //public static HtmlString DynamicPlaceholder(this SitecoreHelper sitecoreHelper, string placeholderName)
        //{
        //    Rendering currentRendering = RenderingContext.Current.Rendering;
        //    return sitecoreHelper.Placeholder(string.Format("{0}|{1}", GetDynamicKey(placeholderName), currentRendering.UniqueId.ToString()));
        //}


        //private static string GetDynamicKey(string placeHolderName)
        //{
        //    bool needIncrement = false;
        //    int incrementStep = 0;
        //    IEnumerable<PlaceholderContext> myPlaceholders = ContextService.Get().GetInstances<PlaceholderContext>();

        //    foreach (PlaceholderContext myPHContext in myPlaceholders)
        //    {
        //        if (myPHContext.PlaceholderName == placeHolderName || myPHContext.PlaceholderName.StartsWith(placeHolderName + "_"))
        //        {
        //            needIncrement = true;
        //            incrementStep++;
        //        }
        //    }

        //    if (needIncrement)
        //        placeHolderName += "_" + incrementStep.ToString();

        //    return placeHolderName;
        //}

        private static string placeholderPattern;

        private static List<string> DynamicPlaceholderList
        {
            get
            {
                return (List<string>)HttpContext.Current.Items[(object)"DynamicPlaceholders"];
            }
            set
            {
                HttpContext.Current.Items[(object)"DynamicPlaceholders"] = (object)value;
            }
        }

        public static HtmlString DynamicPlaceholder(this SitecoreHelper myScHelper, string placeholderName)
        {
            if (SitecoreHelperExtension.DynamicPlaceholderList == null)
                SitecoreHelperExtension.DynamicPlaceholderList = new List<string>();
            string dynamicPlaceholderKey = SitecoreHelperExtension.GetDynamicPlaceholderKey(placeholderName);
            if (!string.IsNullOrWhiteSpace(dynamicPlaceholderKey))
                placeholderName = dynamicPlaceholderKey;
            SitecoreHelperExtension.DynamicPlaceholderList.Add(placeholderName);
            return myScHelper.Placeholder(placeholderName);
        }

        private static string GetDynamicPlaceholderKey(string placeholderName)
        {
            bool flag = false;
            int num = 0;
            foreach (string dynamicPlaceholder in SitecoreHelperExtension.DynamicPlaceholderList)
            {
                if (placeholderName == dynamicPlaceholder || dynamicPlaceholder.StartsWith(placeholderName + SitecoreHelperExtension.placeholderPattern))
                {
                    flag = true;
                    ++num;
                }
            }
            if (flag)
                placeholderName = placeholderName + SitecoreHelperExtension.placeholderPattern + num.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            return placeholderName;
        }

        static SitecoreHelperExtension()
        {
            SitecoreHelperExtension.placeholderPattern = Settings.GetSetting("DynamicPlaceholderPattern");
        }

    }
}