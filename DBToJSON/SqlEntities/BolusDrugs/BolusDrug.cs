
namespace DBToJSON.SqlEntities.BolusDrugs
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class BolusDrug : INosqlTable
    {
        [Key]
        public int BolusDrugId { get; set; }
        internal const int DrugNameLength = 256;
        //[StringLength(256, MinimumLength = 3, ErrorMessage = "Drug name (incl. format directives) must be between 3 and 256 characters long")]
        [ForeignKey("Drug")]
        public int DrugId { get; set;  }
        public string Note { get; set; }
        public string RouteDescription { get; set; }
        [Range(0, 1000000, ErrorMessage = "Maximum adult dose must be between 0 and 1 000 000 [units of measure]")]
        public double AdultMax { get; set; }
        [Range(0, 1000000, ErrorMessage = "Minimum dose must be between 0 and 1 000 000 [units of measure]")]
        public double Min { get; set; }
        [ForeignKey("SpecificWard")]
        public int? SpecificWardId { get; set; }
        
        public virtual Drug Drug { get; set; }
        public virtual Ward SpecificWard { get; set; }
        public virtual ICollection<BolusDose> BolusDoses { get; set; }
        public virtual ICollection<BolusSortOrdering> BolusSortOrderings { get; set; }
    }
}
