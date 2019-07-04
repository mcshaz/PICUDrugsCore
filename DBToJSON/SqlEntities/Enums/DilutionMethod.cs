using System;
using System.Collections.Generic;
using System.Text;

namespace DBToJSON.SqlEntities.Enums
{
    public enum DilutionMethod
    {
        NeatFixedFlow = 1,
        NeatVaryFlowByWeight = 2,
        FixedDilutionFixedFlow = 3,
        FixedDilutionVaryFlowByWeight = 4,
        VaryDilutionVolumeFixedFlow = 5,
        VaryDrugFixedFlow = 6,
        VaryDrugDilutionVolFlowByWeight = 7,
        FluidAllowance
    }
}
