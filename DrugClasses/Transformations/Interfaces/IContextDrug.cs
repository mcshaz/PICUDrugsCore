using DBToJSON.RepositoryClasses.Enums;
using System.Collections.Generic;

namespace DrugClasses.Transformations.Interfaces
{
    internal interface IContextDrug
    {
        int InfusionDrugId { get; }
        int AmpulePrefix { get; }
        Unit SiUnitId { get; }
        DilutionMethod DilutionMethod { get; }
        int InfusionPrefix { get; }
        bool IsPerMin { get; }
        IEnumerable<IContextConcentration> Concentrations { get; set; }
    }
    internal interface IContextConcentration
    {
        int? Volume { get; }
        double Concentration { get; }
    }
    internal interface IContextFixedConc : IContextConcentration
    {
        int StopMins { get; }
        double Rate { get; }
    }
        
}
