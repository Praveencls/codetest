using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CodeTest.Foundation.ORM.Models.GlassModels
{
    public partial interface ISiteRootItem : IGlassBase
    {
        /// <summary>
        /// The Setting field.
        /// <para></para>
        /// <para>Field Type: Droptree</para>		
        /// <para>Field ID: 0b2e4528-2606-4f07-b31d-c89f4629b228</para>
        /// <para>Custom Data: </para>
        /// </summary>
        [SitecoreField(ISiteRootItemConstants.SettingFieldName)]
        ISettings SiteSetting { get; set; }

    }
}
