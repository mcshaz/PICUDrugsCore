using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PicuDrugsCore.Context.Infusions
{
    public interface IInfusionConcentration
    {
        int InfusionConcentrationId { get; set; }
        int InfusionDilutionId { get; set; }
        double Concentration { get; set; }

        //IInfusionDilution InfusionDilution {get;}
    }
}
