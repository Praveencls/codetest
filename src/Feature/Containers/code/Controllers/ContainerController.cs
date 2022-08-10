using CodeTest.Feature.Container.Repositories;
using Glass.Mapper.Sc.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeTest.Feature.Container.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IMvcContext _mvcContext;
        private readonly IContainerRepository _icontainerRepository;
        public ContainerController(IMvcContext mvcContext, IContainerRepository igenericContentRepository)
        {
            _mvcContext = mvcContext;
            _icontainerRepository = igenericContentRepository;
        }

        // GET: Sublayouts
        public ActionResult RenderOneColumn()
        {
          
            var datasource = _mvcContext.DataSourceItem != null ? _mvcContext.GetDataSourceItem<Models.GlassModels.I_BackgroundImage>() : null;
            var containerViewModel = _icontainerRepository.CastFromTDSModelToViewModel(datasource);

            return View("~/Views/CodeTest/Container/onecolumn.cshtml", containerViewModel);
        }

        public ActionResult RenderTwoColumn()
        {
            var datasource = _mvcContext.GetDataSourceItem<Models.GlassModels.I_BackgroundImage>();
            var containerViewModel = _icontainerRepository.CastFromTDSModelToViewModel(datasource);

            return View("~/Views/CodeTest/Container/twocolumn.cshtml", containerViewModel);
        }
    }
}