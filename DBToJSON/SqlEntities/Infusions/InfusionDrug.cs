namespace DBToJSON.SqlEntities.Infusions
{
    using DBToJSON.SqlEntities.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class InfusionDrug : INosqlTable
    {
        [Key]
        public int InfusionDrugId { get; set; }
        public string Note { get; set; }
        public bool IsTitratable { get; set; }
        public InfusionRoute? RouteId { get; set; }

        [ForeignKey("Drug")]
        public int DrugId { get; set; }
        [ForeignKey("SpecificWard")]
        public int? SpecificWardId { get; set; }
        [ForeignKey("DrugReferenceSource")]
        public int? DrugReferenceId { get; set; }

        [ForeignKey("InfusionDiluent")]
        public int InfusionDiluentId { get; set; }

        public virtual Ward SpecificWard { get; set; }
        public virtual DrugReferenceSource DrugReferenceSource { get; set; }
        public virtual InfusionDiluent InfusionDiluent { get; set; }
        public virtual Drug Drug { get; set; }

        public virtual ICollection<DrugAmpuleConcentration> DrugAmpuleConcentrations { get; set; }
        public virtual ICollection<FixedTimeDilution> FixedTimeDilutions { get; set; }
        public virtual ICollection<InfusionSortOrdering> InfusionSortOrderings { get; set; }
        public virtual ICollection<VariableTimeDilution> VariableTimeDilutions { get; set; }
    }
}
