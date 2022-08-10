namespace CodeTest.Feature.HeroSlider.Controllers
{
    using Glass.Mapper.Sc.Web.Mvc;
    using Sitecore.Mvc.Controllers;
    using System.Web.Mvc;

    public class HeroSliderController : SitecoreController
    {
        private readonly IMvcContext _mvcContext;

        public HeroSliderController(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        // GET: Hero Slider Content
        public ActionResult RenderContent()
        {
            var datasource = _mvcContext.GetDataSourceItem<Models.GlassModels.I_Herosliderfolder>();
            return View("~/Views/CodeTest/Component/HeroSlider.cshtml", datasource);
        }
    }
}