namespace DBToJSON.SqlEntities.Infusions
{
    using DBToJSON.SqlEntities.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class InfusionDrug : UpdateTrackingEntity, INosqlTable
    {
        [Key]
        public int InfusionDrugId { get; set; }
        public string Fullname { get; set; }
        public string Abbrev { get; set; }
        public int SiPrefix { get; set; }
        public SiUnit SiUnitId { get; set; }
        public string Note { get; set; }
        public bool IsTitratable { get; set; }
        [ForeignKey("SpecificWard")]
        public int? SpecificWardId { get; set; }
        [ForeignKey("DrugReferenceSource")]
        public int? DrugReferenceId { get; set; }
        [ForeignKey("DrugRoute")]
        public int? RouteId { get; set; }
        [ForeignKey("InfusionDiluent")]
        public int InfusionDiluentId { get; set; }

        public virtual Ward SpecificWard { get; set; }
        public virtual DrugReferenceSource DrugReferenceSource { get; set; }
        public virtual DrugRoute DrugRoute { get; set; }
        public virtual InfusionDiluent InfusionDiluent { get; set; }

        public virtual ICollection<DrugAmpuleConcentration> DrugAmpuleConcentrations { get; set; }
        public virtual ICollection<FixedTimeDilution> FixedTimeDilutions { get; set; }
        public virtual ICollection<InfusionSortOrdering> InfusionSortOrderings { get; set; }
        public virtual ICollection<VariableTimeDilution> VariableTimeDilutions { get; set; }
    }
}
