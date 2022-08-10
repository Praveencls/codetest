using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Maps;
using CodeTest.Foundation.ORM.Models;

namespace CodeTest.Foundation.ORM.Mappings
{
    public class GlassMappings : SitecoreGlassMap<IGlassBase>
    {
        public override void Configure()
        {
            Map(config =>
            {
                config.AutoMap();
                config.Info(f => f.BaseTemplateIds).InfoType(SitecoreInfoType.BaseTemplateIds);
            });
        }
    }
}