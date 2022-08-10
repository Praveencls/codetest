using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using Sitecore.Mvc.Controllers;

namespace CodeTest.Feature.GenericCallout.Controllers
{
    public class AlternateCalloutController : SitecoreController
    {
        private readonly IMvcContext _mvcContext;

        public AlternateCalloutController(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }
        // GET: AlternateCallout
        public ActionResult RenderContent()
        {
            var datasource = _mvcContext.GetDataSourceItem<Models.GlassModels.I_Alternate_Callout>();
            return View("~/Views/CodeTest/Component/AlternateCallout.cshtml", datasource);
        }
    }

   
}