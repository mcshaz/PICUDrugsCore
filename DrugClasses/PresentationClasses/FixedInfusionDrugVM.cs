using DrugClasses.PresentationClasses.Dosing;
using DrugClasses.PresentationClasses.Duration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrugClasses.PresentationClasses
{
    public class FixedInfusionDrugVM : IInfusionDrugVM
    {
        public string DrugName { get; set; }
        public string SourceDescription { get; set; }
        public string SourceHref { get; set; }
        public string Route { get; set; }
        public string Note { get; set; }
        public double AmpuleConcentration { get; set; }
        public SiConcentration AmpuleUnits { get; private set; }
        public string DiluentFluid { get; set; }
        public DrugDoseUnit CalculatedDoseUnit { get => RateUnit; }
        public InfusionRateUnit RateUnit { get; set; }
        public SiUnitMeasure DrawingUpUnits { get => AmpuleUnits; set => AmpuleUnits = new SiConcentration(value); }
        public IList<InfusionPeriodVM> InfusionPeriods {get; set;}
    }
    public class InfusionPeriodVM : IConcentrationDetailVM
    {
        public double CalculatedDose { get; set; }
        public double InfusionRate { get; set; }
        public double DrawingUpDose { get; set; }
        public MinutesDuration Duration { get; set; }
        public MinutesDuration CumulativeStartTime { get; set; }
        public double AmpuleMl { get; set; }
        public double DiluentVolume { get; set; }
        public int FinalVolume { get; set; }
        public bool IsNeat { get; set; }
        public double OneMlHrDose { get; set; }
    }
}
