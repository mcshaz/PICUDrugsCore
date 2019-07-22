namespace DBToJSON.SqlEntities
{
    using Enums;
    using System.Collections.Generic;
    using Infusions;
    using BolusDrugs;
    using System.ComponentModel.DataAnnotations;

    public class Drug
    {
        [Key]
        public int DrugId { get; set; }
        public string Fullname { get; set; }
        public string Abbrev { get; set; }
        public int SiPrefix { get; set; }
        public SiUnit SiUnitId { get; set; }

        public virtual ICollection<InfusionDrug> InfusionDrugs { get; set; }
        public virtual ICollection<BolusDrug> BolusDrugs { get; set; }
        public virtual ICollection<DrugAmpuleConcentration> Ampules { get; set; }
    }
}
