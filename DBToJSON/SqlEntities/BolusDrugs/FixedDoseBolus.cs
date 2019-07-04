namespace DBToJSON.SqlEntities.BolusDrugs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class FixedDrug : UpdateTrackingEntity, INosqlTable
    {
        [Key]
        public int FixedDrugId { get; set; }
        [StringLength(256, MinimumLength = 3, ErrorMessage = "Drug name must be between 3 and 256 characters long")]
        [Required(ErrorMessage = "Drug name must be provided")]
        public string DrugName { get; set; }
        

        public virtual ICollection<BolusSortOrdering> BolusSortOrderings { get; set; }
        public virtual ICollection<FixedDose> FixedDoses { get; set; }
    }

    public class FixedDose : UpdateTrackingEntity
    {
        [Key]
        public int FixedDoseId { get; set; }
        [StringLength(15, ErrorMessage = "Dose must be less than 16 characters long")]
        [Required(ErrorMessage = "Dose must be provided")]
        public string Dose { get; set; }
        [Range(0, 216, ErrorMessage = "Minimum age (in months) must be between 0 and 216")]
        public int MinAgeMonths { get; set; }
        [Range(0, 1200, ErrorMessage = "Maximum age (in months) must be between 0 and 1200")]
        public int MaxAgeMonths { get; set; }
        public int FixedDrugId { get; set; }
        [ForeignKey("FixedDrugId")]
        public virtual FixedDrug Drug { get; set; }
        
    }
}
