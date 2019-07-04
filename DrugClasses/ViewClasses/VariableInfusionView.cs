using System.Collections.Generic;
using DBToJSON.RepositoryClasses.Enums;
using DrugClasses.Transformations.Interfaces;

namespace DrugClasses.RepositoryClasses
{
    public class VariableInfusionView : IContextDrug, IContextConcentration
    //maps to [dbo].[sp_GetVariableInfusions2]
    {
        public int InfusionDrugId { get; set; }
        public string Fullname { get; set; }
        public string Abbrev { get; set; }
        public int AmpulePrefix { get; set; }
        public string Note { get; set; }
        public Unit SiUnitId { get; set; }
        public string Category { get; set; }
        public DilutionMethod DilutionMethod { get; set; }
        public int InfusionPrefix { get; set; }
        public int? Volume { get; set; }
        public double RateMin { get; set; }
        public double RateMax { get; set; }
        public bool IsPerMin { get; set; }
        public double Concentration { get; set; }
        public string HrefBase { get; set; }
        public string HrefPage { get; set; }

        IEnumerable<IContextConcentration> IContextDrug.Concentrations { get; set; }
    }
}
