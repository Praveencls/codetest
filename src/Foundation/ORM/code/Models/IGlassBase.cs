using System;
using System.Collections;
using Sitecore.Globalization;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Glass.Mapper.Sc.Configuration;

namespace CodeTest.Foundation.ORM.Models
{
    public interface IGlassBase
    {
        [SitecoreId]
        Guid Id { get; set; }

        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        Guid TemplateId { get; set; }

        [SitecoreInfo(SitecoreInfoType.TemplateName)]
        string TemplateName { get; set; }

        [SitecoreInfo(SitecoreInfoType.BaseTemplateIds)]
        IEnumerable<Guid> BaseTemplateIds { get; set; }

        [SitecoreInfo(SitecoreInfoType.Language)]
        Language Language { get; set; }

        [SitecoreInfo(SitecoreInfoType.Version)]
        int Version { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        string Name { get; set; }

        [SitecoreInfo(SitecoreInfoType.ContentPath)]
        string ContentPath { get; set; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        string DisplayName { get; set; }

        [SitecoreInfo(SitecoreInfoType.FullPath)]
        string FullPath { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        string Path { get; set; }

        [SitecoreParent(InferType = true)]
        IGlassBase Parent { get; set; }

        [SitecoreField("__Sortorder")]
        int Sortorder { get; set; }

        [SitecoreChildren(InferType = true)]
        IEnumerable<IGlassBase> Children { get; set; }

        [SitecoreItem]
        Item SitecoreItem { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        string Url { get; set; }
    }
}