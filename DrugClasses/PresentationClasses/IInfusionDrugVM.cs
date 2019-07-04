using DrugClasses.PresentationClasses.Dosing;
using System.Collections.Generic;

namespace DrugClasses.PresentationClasses
{
    internal interface IInfusionDrugVM
    {
        InfusionRateUnit RateUnit { set; }
        SiUnitMeasure DrawingUpUnits { set; }
        // IEnumerable<IConcentrationDetail> ConcentrationDetails { get; }
    }
    internal interface IConcentrationDetailVM
    {
        bool IsNeat { set; }
        double DrawingUpDose { get; set; }
        double OneMlHrDose { set; }
        int FinalVolume { get; set; }
    }
}
