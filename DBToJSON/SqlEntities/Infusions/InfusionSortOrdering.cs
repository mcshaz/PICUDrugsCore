//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBToJSON.SqlEntities.Infusions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("InfusionSortOrdering")]
    public partial class InfusionSortOrdering
    {
        [Key]
        public int InfusionSortOrderId { get; set; }
        [ForeignKey("Ward")]
        public int WardId { get; set; }
        [ForeignKey("InfusionDrug")]
        public int InfusionDrugId { get; set; }
        public int SortOrder { get; set; }
        

        public virtual InfusionDrug InfusionDrug { get; set; }
        public virtual Ward Ward { get; set; }
    }
}
