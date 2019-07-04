using DrugClasses.PresentationClasses;
using DrugClasses.PresentationClasses.Dosing;
using DrugClasses.RepositoryClasses;
using DrugClasses.Transformations;
using DrugClasses.Utilities;
using System;
using System.Collections.Generic;
using Xunit;
using DeepEqual.Syntax;
using System.Linq;
using DBToJSON.RepositoryClasses.Enums;

namespace XUnitTestDrugs
{
    public class VariableInfusionVMConversionTests
    {
        /*
        private readonly ITestOutputHelper _output;
        public InfusionVMConversionTests(ITestOutputHelper output)
        {
            _output = output;
        }
        */
        [Theory]
        [ClassData(typeof(InfusionVMTestData))]
        public void TestVariableConversion(double weight, VariableInfusionView[] viewRows, VariableInfusionDrugVM[] expected)
        {
            var testOut = VariableInfusions.DrugInfusion(weight, viewRows);
            testOut.ShouldDeepEqual(expected);
        }
    }

    public class InfusionVMTestData : TheoryData<double,VariableInfusionView[], VariableInfusionDrugVM[]>
    {
        private void Add(double weight, VariableInfusionView[] views, VariableInfusionDrugVM expected)
            => Add(weight, views, new[] { expected });

        private void Add(double weight, VariableInfusionView view, VariableInfusionDrugVM expected)
            => Add(weight, new[] { view }, new[] { expected });

        private readonly HashSet<DilutionMethod> _methods = new HashSet<DilutionMethod>(Enum.GetValues(typeof(DilutionMethod)).Cast<DilutionMethod>());

        public new void Add(double weight, VariableInfusionView[] views, VariableInfusionDrugVM[] expected)
        {
            foreach (var v in views) { _methods.Remove(v.DilutionMethod); }
            base.Add(weight, views, expected);
        }

        public InfusionVMTestData()
        {
            Add(2.8, new[] { new VariableInfusionView {
                    InfusionDrugId = 24,
                    Fullname = "doPamine/doBUTamine",
                    Abbrev = "doP/doBUT",
                    AmpulePrefix = -3,
                    SiUnitId = Unit.gram,
                    DilutionMethod = DilutionMethod.VaryDrugFixedFlow,
                    InfusionPrefix = -6,
                    Volume = 50,
                    RateMin = 2.5,
                    RateMax = 10,
                    Concentration = 5,
                    HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
                    HrefPage = "Paediatric%20Drug%20Infusion%20Chart.pdf",
                    IsPerMin = true
                },  new VariableInfusionView {
                    InfusionDrugId = 22,
                    Fullname = "Propofol",
                    Abbrev = "Propofol",
                    AmpulePrefix = -3,
                    Note = "NEVER in shock or compensated shock",
                    SiUnitId = Unit.gram,
                    DilutionMethod = DilutionMethod.NeatVaryFlowByWeight,
                    InfusionPrefix = -3,
                    Volume = 50,
                    RateMin = 0,
                    RateMax = 3,
                    IsPerMin = false,
                    Concentration = 10,
                    HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
                    HrefPage = "Paediatric%20Drug%20Infusion%20Chart.pdf",
                } },
                new[] { new VariableInfusionDrugVM
                {
                    DrugName = "doPamine/doBUTamine",
                    Link = "http://www.adhb.govt.nz/picu/Protocols/Paediatric%20Drug%20Infusion%20Chart.pdf",
                    DoseRange = new DoublePrecisionRange(2.5,10),
                    RateUnit = new InfusionRateUnit(-6,Unit.gram,true,true),
                    DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
                    InfusionDetails = new List<VariableConcentrationDetailVM>
                    {
                        new VariableConcentrationDetailVM
                        {
                            DetailName = "doP/doBUT",
                            FinalVolume = 50,
                            DrawingUpDose = 42,
                            OneMlHrDose =5,
                            FlowRange = new DoublePrecisionRange(0.5,2),
                            IsNeat = false
                        }
                    }
                },
                new VariableInfusionDrugVM
                {
                    DrugName = "Propofol",
                    Link = "http://www.adhb.govt.nz/picu/Protocols/Paediatric%20Drug%20Infusion%20Chart.pdf",
                    DoseRange = new DoublePrecisionRange(0,3),
                    RateUnit = new InfusionRateUnit(-3,Unit.gram,true,false),
                    DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
                    Note = "NEVER in shock or compensated shock",
                    InfusionDetails = new List<VariableConcentrationDetailVM>
                    {
                        new VariableConcentrationDetailVM
                        {
                            DetailName = "Propofol",
                            FinalVolume = 50,
                            DrawingUpDose = 500,
                            OneMlHrDose = 10.0/2.8,
                            FlowRange = new DoublePrecisionRange(0,0.84),
                            IsNeat = true
                        }
                    }
                }
                });
            Add(5.7, new[] {
                new VariableInfusionView {
                    InfusionDrugId = 25,
                    Fullname = "Adrenaline/Noradrenaline",
                    Abbrev = "Adrenaline/Norad",
                    AmpulePrefix = -3,
                    SiUnitId = Unit.gram,
                    Category = "Low",
                    DilutionMethod = DilutionMethod.VaryDrugFixedFlow,
                    InfusionPrefix = -6,
                    Volume = 50,
                    RateMin = 0.01,
                    RateMax = 1,
                    IsPerMin = true,
                    Concentration = 0.05,
                    HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
                    HrefPage = "Paediatric%20Drug%20Infusion%20Chart.pdf",
                },
                new VariableInfusionView {
                    InfusionDrugId = 25,
                    Fullname = "Adrenaline/Noradrenaline",
                    Abbrev = "Adrenaline/Norad",
                    AmpulePrefix = -3,
                    SiUnitId = Unit.gram,
                    Category = "High",
                    DilutionMethod = DilutionMethod.VaryDrugFixedFlow,
                    InfusionPrefix = -6,
                    Volume = 50,
                    RateMin = 0.01,
                    RateMax = 1,
                    IsPerMin = true,
                    Concentration = 0.1,
                    HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
                    HrefPage = "Paediatric%20Drug%20Infusion%20Chart.pdf",
                },
             }, new VariableInfusionDrugVM
             {
                 DrugName = "Adrenaline/Noradrenaline",
                 Link = "http://www.adhb.govt.nz/picu/Protocols/Paediatric%20Drug%20Infusion%20Chart.pdf",
                 DoseRange = new DoublePrecisionRange(0.01, 1),
                 RateUnit = new InfusionRateUnit(-6, Unit.gram, true, true),
                 DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
                 InfusionDetails = new List<VariableConcentrationDetailVM>
                    {
                        new VariableConcentrationDetailVM
                        {
                            DetailName = "Low",
                            FinalVolume = 50,
                            DrawingUpDose = 0.855,
                            OneMlHrDose =0.05,
                            FlowRange = new DoublePrecisionRange(0.2,20),
                            IsNeat = false
                        },
                        new VariableConcentrationDetailVM
                        {
                            DetailName = "High",
                            FinalVolume = 50,
                            DrawingUpDose = 1.71,
                            OneMlHrDose =0.1,
                            FlowRange = new DoublePrecisionRange(0.1,10),
                            IsNeat = false
                        }
                    }
             });
            Add(73, new[] {
    new VariableInfusionView {
        InfusionDrugId = 25,
        Fullname = "Adrenaline/Noradrenaline",
        Abbrev = "Adrenaline/Norad",
        AmpulePrefix = -3,
        SiUnitId = Unit.gram,
        Category = "Low",
        DilutionMethod = DilutionMethod.VaryDilutionVolumeFixedFlow,
        InfusionPrefix = -6,
        RateMin = 0.01,
        RateMax = 1,
        IsPerMin = true,
        Concentration = 0.01,
        HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
        HrefPage = "Adult%20Drug%20Infusion%20Chart.pdf",
    },
    new VariableInfusionView {
        InfusionDrugId = 25,
        Fullname = "Adrenaline/Noradrenaline",
        Abbrev = "Adrenaline/Norad",
        AmpulePrefix = -3,
        SiUnitId = Unit.gram,
        Category = "Medium",
        DilutionMethod = DilutionMethod.VaryDilutionVolumeFixedFlow,
        InfusionPrefix = -6,
        RateMin = 0.01,
        RateMax = 1,
        IsPerMin = true,
        Concentration = 0.02,
        HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
        HrefPage = "Adult%20Drug%20Infusion%20Chart.pdf",
    },
    new VariableInfusionView {
        InfusionDrugId = 25,
        Fullname = "Adrenaline/Noradrenaline",
        Abbrev = "Adrenaline/Norad",
        AmpulePrefix = -3,
        SiUnitId = Unit.gram,
        Category = "High",
        DilutionMethod = DilutionMethod.VaryDilutionVolumeFixedFlow,
        InfusionPrefix = -6,
        RateMin = 0.01,
        RateMax = 1,
        IsPerMin = true,
        Concentration = 0.05,
        HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
        HrefPage = "Adult%20Drug%20Infusion%20Chart.pdf",
    },
}, 
    new VariableInfusionDrugVM
        {
            DrugName = "Adrenaline/Noradrenaline",
            Link = "http://www.adhb.govt.nz/picu/Protocols/Adult%20Drug%20Infusion%20Chart.pdf",
            DoseRange = new DoublePrecisionRange(0.01, 1),
            RateUnit = new InfusionRateUnit(-6, Unit.gram, true, true),
            DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
            InfusionDetails = new List<VariableConcentrationDetailVM>
            {
                new VariableConcentrationDetailVM
                {
                    DetailName = "Low",
                    FinalVolume = 46,
                    DrawingUpDose = 2,
                    OneMlHrDose =0.01,
                    FlowRange = new DoublePrecisionRange(1,100),
                    IsNeat = false
                },
                new VariableConcentrationDetailVM
                {
                    DetailName = "Medium",
                    FinalVolume = 46,
                    DrawingUpDose = 4,
                    OneMlHrDose =0.02,
                    FlowRange = new DoublePrecisionRange(0.5,50),
                    IsNeat = false
                },
                new VariableConcentrationDetailVM
                {
                    DetailName = "High",
                    FinalVolume = 46,
                    DrawingUpDose = 10,
                    OneMlHrDose =0.05,
                    FlowRange = new DoublePrecisionRange(0.2,20),
                    IsNeat = false
                }
            }
        });
            Add(3.6, new VariableInfusionView
            {
                InfusionDrugId = 16,
                Fullname = "Actrapid Insulin",
                Abbrev = "Actrapid",
                AmpulePrefix = 0,
                SiUnitId = Unit.unit,
                DilutionMethod = DilutionMethod.FixedDilutionVaryFlowByWeight,
                InfusionPrefix = 0,
                Volume = 50,
                RateMin = 0.05,
                RateMax = 0.1,
                IsPerMin = false,
                Concentration = 1,
                HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
                HrefPage = "Paediatric%20Drug%20Infusion%20Chart.pdf",
            }, new VariableInfusionDrugVM
            {
                DrugName = "Actrapid Insulin",
                Link = "http://www.adhb.govt.nz/picu/Protocols/Paediatric%20Drug%20Infusion%20Chart.pdf",
                DoseRange = new DoublePrecisionRange(0.05, 0.1),
                RateUnit = new InfusionRateUnit(0, Unit.unit, true, false),
                DrawingUpUnits = new SiUnitMeasure(0, Unit.unit),
                InfusionDetails = new List<VariableConcentrationDetailVM>
                    {
                        new VariableConcentrationDetailVM
                        {
                            DetailName = "Actrapid",
                            FinalVolume = 50,
                            DrawingUpDose = 50,
                            OneMlHrDose = 1.0/3.6,
                            FlowRange = new DoublePrecisionRange(0.18,0.36),
                            IsNeat = false
                        }
                    }
            });
            Add(73, new VariableInfusionView
            {
                InfusionDrugId = 12,
                Fullname = "Morphine",
                Abbrev = "Morphine",
                AmpulePrefix = -3,
                SiUnitId = Unit.gram,
                DilutionMethod = DilutionMethod.FixedDilutionFixedFlow,
                InfusionPrefix = -3,
                Volume = 60,
                RateMin = 1,
                RateMax = 4,
                IsPerMin = false,
                Concentration = 1,
                HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
                HrefPage = "Paediatric%20Drug%20Infusion%20Chart.pdf",
            }, new VariableInfusionDrugVM
            {
                DrugName = "Morphine",
                Link = "http://www.adhb.govt.nz/picu/Protocols/Paediatric%20Drug%20Infusion%20Chart.pdf",
                DoseRange = new DoublePrecisionRange(1, 4),
                RateUnit = new InfusionRateUnit(-3, Unit.gram, false, false),
                DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
                InfusionDetails = new List<VariableConcentrationDetailVM>
                    {
                        new VariableConcentrationDetailVM
                        {
                            DetailName = "Morphine",
                            FinalVolume = 60,
                            DrawingUpDose = 60,
                            OneMlHrDose = 1,
                            FlowRange = new DoublePrecisionRange(1,4),
                            IsNeat = false
                        }
                    }
            });
            Add(73, new VariableInfusionView
            {
                InfusionDrugId = 15,
                Fullname = "Frusemide",
                Abbrev = "Frusemide",
                AmpulePrefix = -3,
                SiUnitId = Unit.gram,
                DilutionMethod = DilutionMethod.NeatFixedFlow,
                InfusionPrefix = -3,
                Volume = 25,
                RateMin = 2,
                RateMax = 20,
                IsPerMin = false,
                Concentration = 10,
                HrefBase = "http://www.adhb.govt.nz/picu/Protocols/",
                HrefPage = "Paediatric%20Drug%20Infusion%20Chart.pdf",
            }, new VariableInfusionDrugVM
            {
                DrugName = "Frusemide",
                Link = "http://www.adhb.govt.nz/picu/Protocols/Paediatric%20Drug%20Infusion%20Chart.pdf",
                DoseRange = new DoublePrecisionRange(2, 20),
                RateUnit = new InfusionRateUnit(-3, Unit.gram, false, false),
                DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
                InfusionDetails = new List<VariableConcentrationDetailVM>
                    {
                        new VariableConcentrationDetailVM
                        {
                            DetailName = "Frusemide",
                            FinalVolume = 25,
                            DrawingUpDose = 250,
                            OneMlHrDose = 10,
                            FlowRange = new DoublePrecisionRange(0.2,2),
                            IsNeat = true
                        }
                    }
            });
        }
        ~InfusionVMTestData()
        {
            Console.WriteLine("The following methods remain untested: " + string.Join(",",_methods
                .Select(m=>$"{m}={(int)m}")));
        }
    }
}