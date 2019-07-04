using DBToJSON.RepositoryClasses.Enums;
using DrugClasses.Transformations.Interfaces;
using System.Collections.Generic;

namespace DrugClasses.RepositoryClasses
{
    public class FixedInfusionView : IContextDrug, IContextFixedConc
    {
        public int InfusionDrugId { get; set; }
        public string Fullname { get; set; }
        public string Abbrev { get; set; }
        public int AmpulePrefix { get; set; }
        public Unit SiUnitId { get; set; }
        public string Note { get; set; }
        public string ReferenceDescription { get; set; }
        public string RefAbbrev { get; set; }
        public string Hyperlink { get; set; }
        public string RouteDescription { get; set; }
        public string RouteAbbrev { get; set; }
        public DilutionMethod DilutionMethod { get; set; }
        public int InfusionPrefix { get; set; }
        public bool IsPerMin { get; set; }
        public string ReferencePage { get; set; }
        public double Concentration { get; set; }
        public int? Volume { get; set; }
        public int StopMins { get; set; }
        public double Rate { get; set; }
        public string DiluentType { get; set; }
        public string DiluentAbbrev { get; set; }
        public double AmpuleConcentration { get; set; }

        IEnumerable<IContextConcentration> IContextDrug.Concentrations { get; set; }
    }
}
