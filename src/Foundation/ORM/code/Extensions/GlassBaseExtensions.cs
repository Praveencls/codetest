using CodeTest.Foundation.ORM.Models;
using Glass.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeTest.Foundation.ORM.Extensions
{
    public static class GlassBaseExtensions
    {
        /// <summary>
        /// Returns true if the item's template matches the templateId parameter or if the item's template inherits the templateId parameter
        /// </summary>
        /// <param name="item">The instance item</param>
        /// <param name="templateId">The template ID to compare</param>
        /// <returns>bool</returns>
        public static bool IsOrInherits(this IGlassBase item, Guid templateId)
        {
            return item.TemplateId == templateId || item.BaseTemplateIds.Contains(templateId);
        }

        public static bool HasValue(this Glass.Mapper.Sc.Fields.Image image)
        {
            return image != null && image.Src.HasValue();
        }
        public static bool HasValue(this Glass.Mapper.Sc.Fields.Link link)
        {
            return link != null && link.Url.HasValue();
        }
    }
}