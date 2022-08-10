using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeTest.Feature.Container.Models.ViewModels
{
    using CodeTest.Feature.Container.Models.GlassModels;
    using Glass.Mapper.Sc.Fields;

    public class ContainerViewModel
    {
        public string BackgroundPosition { get; set; }
      //  public string BackgroundImageUrl { get; set; }

        public I_BackgroundImage BackgroundImage { get; set; }
    }
}

 