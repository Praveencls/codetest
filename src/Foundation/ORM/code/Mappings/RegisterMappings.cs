using Glass.Mapper.Sc.Pipelines.AddMaps;
using CodeTest.Foundation.ORM.Extensions;

namespace CodeTest.Foundation.ORM.Mappings
{
    public class RegisterMappings : AddMapsPipeline
    {
        public void Process(AddMapsPipelineArgs args)
        {
            args.MapsConfigFactory.AddFluentMaps("CodeTest.Foundation.ORM");
        }
    }
}