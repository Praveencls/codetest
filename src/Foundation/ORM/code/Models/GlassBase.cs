using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System;

namespace CodeTest.Foundation.ORM.Models
{
    public abstract partial class GlassBase : IGlassBase
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        public string ItemName { get; set; }

        [SitecoreInfo(SitecoreInfoType.Language)]
        public virtual Sitecore.Globalization.Language Language { get; set; }

        [SitecoreInfo(SitecoreInfoType.Version)]
        public virtual int Version { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public string Url { get; set; }

        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        public virtual Guid TemplateId { get; set; }

        [SitecoreInfo(SitecoreInfoType.TemplateName)]
        public virtual string TemplateName { get; set; }

        [SitecoreInfo(SitecoreInfoType.BaseTemplateIds)]
        public virtual IEnumerable<Guid> BaseTemplateIds { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        public virtual string Name { get; set; }

        [SitecoreInfo(SitecoreInfoType.ContentPath)]
        public virtual string ContentPath { get; set; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string DisplayName { get; set; }

        [SitecoreInfo(SitecoreInfoType.FullPath)]
        public virtual string FullPath { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

        [SitecoreParent(InferType = true)]
        public virtual IGlassBase Parent { get; set; }

        [SitecoreField("__Sortorder")]
        public virtual int Sortorder { get; set; }

        [SitecoreChildren(InferType = true)]
        public virtual IEnumerable<IGlassBase> Children { get; set; }

        [SitecoreItem]
        public virtual Item SitecoreItem { get; set; }

    }
}