
namespace DBToJSON.SqlEntities.BolusDrugs
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class BolusDrug : UpdateTrackingEntity, INosqlTable
    {
        [Key]
        public int BolusDrugId { get; set; }
        internal const int DrugNameLength = 256;
        //[StringLength(256, MinimumLength = 3, ErrorMessage = "Drug name (incl. format directives) must be between 3 and 256 characters long")]
        [MinLength(3)]
        [Required(ErrorMessage = "Drug name must be provided")]
        public string DrugName { get; set; }
        [Range(0, 100000, ErrorMessage = "Concentration must be between 0 and 100 000 [units of measure] per mL")]
        public double? Conc_ml { get; set; }
        [StringLength(15, ErrorMessage = "Units must be less than 16 characters long")]
        [Required(ErrorMessage = "units must be provided")]
        public string Units { get; set; }
        [Range(0, 1000000, ErrorMessage = "Maximum adult dose must be between 0 and 1 000 000 [units of measure]")]
        public double AdultMax { get; set; }
        [Range(0, 1000000, ErrorMessage = "Minimum dose must be between 0 and 1 000 000 [units of measure]")]
        public double Min { get; set; }
        [ForeignKey("SpecificWard")]
        public int? SpecificWardId { get; set; }
        

        public virtual Ward SpecificWard { get; set; }
        public virtual ICollection<BolusDose> BolusDoses { get; set; }
        public virtual ICollection<BolusSortOrdering> BolusSortOrderings { get; set; }
    }
}
