using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPlaceholderRenderings;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTest.Foundation.DynamicPlaceholder.Pipelines.Renderings
{
    public class GetDynamicKeyAllowedRenderings
    {
        private GetAllowedRenderings getRenderings = new GetAllowedRenderings();
        public void Process(GetPlaceholderRenderingsArgs args)
        {
           
            Assert.IsNotNull((object)args, nameof(args));
            string setting = Settings.GetSetting("DynamicPlaceholderPattern");
            if (!string.IsNullOrEmpty(setting))
            {
                string str = args.PlaceholderKey;
                if (str.Contains("/"))
                    str = str.Substring(args.PlaceholderKey.LastIndexOf('/'));
                if (str.ToLower().Contains(setting.ToLower()))
                {
                    string placeholderKey = args.PlaceholderKey.ToLower().Substring(0, args.PlaceholderKey.ToLower().LastIndexOf(setting.ToLower()));
                    GetPlaceholderRenderingsArgs placeholderRenderingsArgs = args.DeviceId.IsNull ? new GetPlaceholderRenderingsArgs(placeholderKey, args.LayoutDefinition, args.ContentDatabase) : new GetPlaceholderRenderingsArgs(placeholderKey, args.LayoutDefinition, args.ContentDatabase, args.DeviceId);
                    this.getRenderings.Process(placeholderRenderingsArgs);
                    this.AssimilateArgumentResults(ref args, placeholderRenderingsArgs);
                    return;
                }
            }
            this.getRenderings.Process(args);
        }

        private void AssimilateArgumentResults(ref GetPlaceholderRenderingsArgs args, GetPlaceholderRenderingsArgs basePlaceholderArgs)
        {
            args.Options.ShowTree = basePlaceholderArgs.Options.ShowTree;

            ((SelectItemOptions)args.Options).ShowRoot = (((SelectItemOptions)basePlaceholderArgs.Options).ShowRoot);

            ((SelectItemOptions)args.Options).SetRootAsSearchRoot = (((SelectItemOptions)basePlaceholderArgs.Options).SetRootAsSearchRoot);

            args.HasPlaceholderSettings = (basePlaceholderArgs.HasPlaceholderSettings);
            args.OmitNonEditableRenderings = (basePlaceholderArgs.OmitNonEditableRenderings);
            args.PlaceholderRenderings = (basePlaceholderArgs.PlaceholderRenderings);
        }
    }
}
