using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeTest.Feature.Container.Repositories
{
    using CodeTest.Feature.Container.Models.GlassModels;
    using CodeTest.Feature.Container.Models.ViewModels;

    public interface IContainerRepository
    {
        ContainerViewModel CastFromTDSModelToViewModel(I_BackgroundImage ibackgroundImage);

    }
}