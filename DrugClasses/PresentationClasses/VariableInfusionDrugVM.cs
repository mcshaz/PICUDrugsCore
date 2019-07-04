using DrugClasses.PresentationClasses.Dosing;
using DrugClasses.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DrugClasses.PresentationClasses
{
    public class VariableInfusionDrugVM : IInfusionDrugVM
    {
        public string DrugName { get; set; }
        public DoublePrecisionRange DoseRange { get; set; }
        public InfusionRateUnit RateUnit { get; set; }
        public string Link { get; set; }
        public string Note { get; set; }
        public SiUnitMeasure DrawingUpUnits { get; set; }
        public List<VariableConcentrationDetailVM> InfusionDetails { get; set; }
        // IEnumerable<IConcentrationDetail> IInfusionDrug.ConcentrationDetails { get => InfusionDetails.Cast<IConcentrationDetail>(); }
    }

    public class VariableConcentrationDetailVM : IConcentrationDetailVM
    {
        public string DetailName { get; set; }
        public double DrawingUpDose { get; set; }
        public bool IsNeat { get; set; }
        public int FinalVolume { get; set; }
        public double OneMlHrDose { get; set; }
        public DoublePrecisionRange FlowRange { get; set; }
    }
}
