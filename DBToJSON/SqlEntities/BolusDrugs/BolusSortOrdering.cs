namespace DBToJSON.SqlEntities.BolusDrugs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("BolusSortOrdering")]
    public class BolusSortOrdering : UpdateTrackingEntity
    {
        public const int maxBolusSubHeaderLength = 512;
        [Key]
        public int BolusSortOrderId { get; set; }
        [ForeignKey("Ward")]
        public int WardId { get; set; }
        [ForeignKey("BolusDrug")]
        public int? BolusDrugId { get; set; }
        [ForeignKey("FixedDrug")]
        public int? FixedDrugId { get; set; }
        public int SortOrder { get; set; }
        [StringLength(maxBolusSubHeaderLength)]
        public string SectionHeader { get; set; }
        

        public virtual FixedDrug FixedDrug { get; set; }
        public virtual BolusDrug BolusDrug { get; set; }
        public virtual Ward Ward { get; set; }
    }
}
