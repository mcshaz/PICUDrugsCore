using PicuDrugsCore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PicuDrugsCore.Context.Infusions
{
    public interface IInfusionDilution
    {
        int InfusionDilutionId { get; set; }
        int InfusionDrugId { get; set; }
        int DilutionMethodId { get; set; }
        int SiPrefixVal { get; set; }
        double WeightMin { get; set; }
        double WeightMax { get; set; }
        int AgeMinMonths { get; set; }
        int AgeMaxMonths { get; set; }
        bool IsPerMin { get; set; }
        //int roundSigFigs {get; set;}
        string InfusionUnits { get; set; }
        string ReferencePage { get; set; }

        //ICollection<IInfusionConcentration> InfusionConcentrations {get;}
        InfusionDrug InfusionDrug { get; set; }
        DilutionMethod DilutionMethod { get; set; }
    }

}
