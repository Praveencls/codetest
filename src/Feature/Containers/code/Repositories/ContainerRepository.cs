using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Pipelines.LoadHtml;

namespace CodeTest.Feature.Container.Repositories
{
    
    using CodeTest.Feature.Container.Models.GlassModels;
    using CodeTest.Feature.Container.Models.ViewModels;
    using Sitecore.Data.Items;
    using Sitecore.Mvc.Presentation;

    public class ContainerRepository : IContainerRepository
    {
        /// <summary>
        /// Cast the backgroundtheme and I_Content properties value to a new viewmodel
        /// </summary>
        /// <param name="backgroundtheme">backgroundtheme</param>
        /// <param name="icontent">I_Data</param>
        /// <returns>GenericContentViewModel</returns>
        public ContainerViewModel CastFromTDSModelToViewModel(I_BackgroundImage iBackgroundImage)
        {
            string positionStyle = "";
            var positionItemId = RenderingContext.Current.Rendering.Parameters["Position"];
            if (positionItemId != null && !string.IsNullOrEmpty(positionItemId))
            {
                var positionItem = Sitecore.Context.Database.GetItem(positionItemId);
                if (positionItem != null && positionItem.Fields["Position Style"] != null && !string.IsNullOrEmpty(positionItem.Fields["Position Style"].Value))
                {
                    positionStyle = positionItem.Fields["Position Style"].Value;
                }
            }
            ContainerViewModel containerViewModel = new ContainerViewModel()
            {
                BackgroundPosition = positionStyle,
                BackgroundImage = iBackgroundImage
            };

            return containerViewModel;
        }

        
    }
}