using DrugClasses.PresentationClasses;
using DrugClasses.RepositoryClasses;
using DrugClasses.Transformations;
using System;
using System.Collections.Generic;
using Xunit;
using DeepEqual.Syntax;
using System.Linq;
using DrugClasses.PresentationClasses.Dosing;
using DrugClasses.PresentationClasses.Duration;
using DBToJSON.RepositoryClasses.Enums;

namespace XUnitTestDrugs
{
    public class FixedInfusionVMConversionTests
    {
        /*
        private readonly ITestOutputHelper _output;
        public InfusionVMConversionTests(ITestOutputHelper output)
        {
            _output = output;
        }
        */
        [Theory]
        [ClassData(typeof(FixedInfusionVMTestData))]
        public void TestFixedConversion(double weight, FixedInfusionView[] viewRows, FixedInfusionDrugVM expected)
        {
            var testOut = FixedInfusions.DrugInfusion(weight, viewRows);
            testOut.ShouldDeepEqual(expected);
        }
    }

    public class FixedInfusionVMTestData : TheoryData<double,FixedInfusionView[], FixedInfusionDrugVM>
    {

        private void Add(double weight, FixedInfusionView view, FixedInfusionDrugVM expected)
            => Add(weight, new[] { view }, expected);

        private readonly HashSet<DilutionMethod> _methods = new HashSet<DilutionMethod>(Enum.GetValues(typeof(DilutionMethod)).Cast<DilutionMethod>());

        public new void Add(double weight, FixedInfusionView[] views, FixedInfusionDrugVM expected)
        {
            foreach (var v in views) { _methods.Remove(v.DilutionMethod); }
            base.Add(weight, views, expected);
        }

        public FixedInfusionVMTestData()
        {
            Add(10, // Dilution Method Id: 7 wt: 10 ageMth: 71
                new[] {
                    new FixedInfusionView {
                        InfusionDrugId = 32,
                        Fullname = "Acetylcysteine",
                        Abbrev = "NAC",
                        AmpulePrefix = -3,
                        SiUnitId = Unit.gram,
                        Note = "watch for hypotension.",
                        ReferenceDescription = "Notes on Injectable Drugs 6th Ed",
                        RefAbbrev = "Notes on Inj",
                        Hyperlink = "file://ahsl6/main/Groups/INTRANET/Pharmacy/eNoids6/eNOIDs6Mongraphs/",
                        RouteDescription = "Peripheral or Central Line",
                        RouteAbbrev = "PIV",
                        DilutionMethod = DilutionMethod.VaryDrugDilutionVolFlowByWeight,
                        InfusionPrefix = -3,
                        IsPerMin = false,
                        ReferencePage = "ACETYLCYSTEINE.pdf",
                        Concentration = 50,
                        Volume = 3,
                        StopMins = 15,
                        Rate = 600,
                        DiluentType = "5% Dextrose",
                        DiluentAbbrev = "5% dex",
                        AmpuleConcentration = 200,
                    },
                    new FixedInfusionView {
                        InfusionDrugId = 32,
                        Fullname = "Acetylcysteine",
                        Abbrev = "NAC",
                        AmpulePrefix = -3,
                        SiUnitId = Unit.gram,
                        Note = "watch for hypotension.",
                        ReferenceDescription = "Notes on Injectable Drugs 6th Ed",
                        RefAbbrev = "Notes on Inj",
                        Hyperlink = "file://ahsl6/main/Groups/INTRANET/Pharmacy/eNoids6/eNOIDs6Mongraphs/",
                        RouteDescription = "Peripheral or Central Line",
                        RouteAbbrev = "PIV",
                        DilutionMethod = DilutionMethod.VaryDrugDilutionVolFlowByWeight,
                        InfusionPrefix = -3,
                        IsPerMin = false,
                        ReferencePage = "ACETYLCYSTEINE.pdf",
                        Concentration = 7.14285714285714,
                        Volume = 7,
                        StopMins = 255,
                        Rate = 12.5,
                        DiluentType = "5% Dextrose",
                        DiluentAbbrev = "5% dex",
                        AmpuleConcentration = 200,
                    },
                    new FixedInfusionView {
                        InfusionDrugId = 32,
                        Fullname = "Acetylcysteine",
                        Abbrev = "NAC",
                        AmpulePrefix = -3,
                        SiUnitId = Unit.gram,
                        Note = "watch for hypotension.",
                        ReferenceDescription = "Notes on Injectable Drugs 6th Ed",
                        RefAbbrev = "Notes on Inj",
                        Hyperlink = "file://ahsl6/main/Groups/INTRANET/Pharmacy/eNoids6/eNOIDs6Mongraphs/",
                        RouteDescription = "Peripheral or Central Line",
                        RouteAbbrev = "PIV",
                        DilutionMethod = DilutionMethod.VaryDrugDilutionVolFlowByWeight,
                        InfusionPrefix = -3,
                        IsPerMin = false,
                        ReferencePage = "ACETYLCYSTEINE.pdf",
                        Concentration = 7.14285714285714,
                        Volume = 14,
                        StopMins = 1215,
                        Rate = 6.25,
                        DiluentType = "5% Dextrose",
                        DiluentAbbrev = "5% dex",
                        AmpuleConcentration = 200,
                    },
                }
                , new FixedInfusionDrugVM
                {
                    DrugName = "Acetylcysteine",
                    SourceDescription = "Notes on Injectable Drugs 6th Ed",
                    SourceHref = "file://ahsl6/main/Groups/INTRANET/Pharmacy/eNoids6/eNOIDs6Mongraphs/ACETYLCYSTEINE.pdf",
                    AmpuleConcentration = 200,
                    DiluentFluid = "5% Dextrose",
                    Note = "watch for hypotension.",
                    Route = "Peripheral or Central Line",
                    DrawingUpUnits = new SiUnitMeasure(-3,Unit.gram),
                    RateUnit = new InfusionRateUnit(-3, Unit.gram,true,false),
                    InfusionPeriods = new[]
                    {
                        new InfusionPeriodVM
                        {
                            AmpuleMl = 7.5,
                            CalculatedDose = 150,
                            CumulativeStartTime = new MinutesDuration(),
                            Duration = new MinutesDuration(15),
                            DiluentVolume = 22.5,
                            DrawingUpDose = 1500,
                            FinalVolume = 30,
                            InfusionRate = 120,
                            OneMlHrDose = 5
                        },
                        new InfusionPeriodVM
                        {
                            AmpuleMl = 2.5,
                            CalculatedDose = 50,
                            CumulativeStartTime = new MinutesDuration(15),
                            Duration = new MinutesDuration(240),
                            DiluentVolume = 67.5,
                            DrawingUpDose = 500,
                            FinalVolume = 70,
                            InfusionRate = 17.5,
                            OneMlHrDose = 0.714285714285714
                        },
                        new InfusionPeriodVM
                        {
                            AmpuleMl = 5,
                            CalculatedDose = 100,
                            CumulativeStartTime = new MinutesDuration(255),
                            Duration = new MinutesDuration(960),
                            DiluentVolume = 135,
                            DrawingUpDose = 1000,
                            FinalVolume = 140,
                            InfusionRate = 8.75,
                            OneMlHrDose = 0.714285714285714
                        }
                    }
                });
            // Dilution Method Id: 3 wt: 70 ageMth: 600
            Add(70,
                new FixedInfusionView {
                    InfusionDrugId = 33,
                    Fullname = "Levosimendan",
                    Abbrev = "Levosimendan",
                    AmpulePrefix = -3,
                    SiUnitId = Unit.gram,
                    ReferenceDescription = "Starship Pharmacy Guidelines (paediatric)",
                    RefAbbrev = "SS pharm",
                    Hyperlink = "file://ahsl6/main/Groups/Everyone/POLICY/Master%20file%20of%20Intranet/Medication%20Admin/Paed/IV/",
                    RouteDescription = "Peripheral or Central Line",
                    RouteAbbrev = "PIV",
                    DilutionMethod = DilutionMethod.FixedDilutionFixedFlow,
                    InfusionPrefix = -6,
                    IsPerMin = true,
                    ReferencePage = "Levosimendan_Paed.pdf",
                    Concentration = 0.833333333333333,
                    Volume = 250,
                    StopMins = 1440,
                    Rate = 8.68055555555556,
                    DiluentType = "5% Dextrose",
                    DiluentAbbrev = "5% dex",
                    AmpuleConcentration = 2.5,
                }, new FixedInfusionDrugVM
                {
                    DrugName = "Levosimendan",
                    SourceDescription = "Starship Pharmacy Guidelines (paediatric)",
                    SourceHref = "file://ahsl6/main/Groups/Everyone/POLICY/Master%20file%20of%20Intranet/Medication%20Admin/Paed/IV/Levosimendan_Paed.pdf",
                    AmpuleConcentration = 2.5,
                    DiluentFluid = "5% Dextrose",
                    Route = "Peripheral or Central Line",
                    DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
                    RateUnit = new InfusionRateUnit(-6, Unit.gram, false, true),
                    InfusionPeriods = new[]
                    {
                        new InfusionPeriodVM
                        {
                            AmpuleMl = 5,
                            CalculatedDose = 12.5,
                            CumulativeStartTime = new MinutesDuration(),
                            Duration = new MinutesDuration(1440),
                            DiluentVolume = 245,
                            DrawingUpDose = 12.5,
                            FinalVolume = 250,
                            InfusionRate = 10.416666666666677,
                            OneMlHrDose = 0.833333333333333
                        }
                    }
                });

                Add(28,
                // Dilution Method Id: 6 wt: 28 ageMth: 606
                    new FixedInfusionView {
                        InfusionDrugId = 35,
                        Fullname = "Magnesium Sulphate (asthma)",
                        Abbrev = "Mg",
                        AmpulePrefix = -3,
                        SiUnitId = Unit.gram,
                        Note = "Watch for hypotension. Keep serum Mg 1.5-2.5 mmol/L. May be repeated.",
                        ReferenceDescription = "Starship PICU Protocols",
                        RefAbbrev = "PICU RBPs",
                        Hyperlink = "http://www.adhb.govt.nz/picu/Protocols/",
                        RouteDescription = "Peripheral or Central Line",
                        RouteAbbrev = "PIV",
                        DilutionMethod = DilutionMethod.VaryDrugFixedFlow,
                        InfusionPrefix = -3,
                        IsPerMin = false,
                        ReferencePage = "Asthma.pdf",
                        Concentration = 2.5,
                        Volume = 20,
                        StopMins = 20,
                        Rate = 150,
                        DiluentType = "5% Dextrose",
                        DiluentAbbrev = "5% dex",
                        AmpuleConcentration = 493,
                    }, new FixedInfusionDrugVM
                    {
                        DrugName = "Magnesium Sulphate (asthma)",
                        SourceDescription = "Starship PICU Protocols",
                        SourceHref = "http://www.adhb.govt.nz/picu/Protocols/Asthma.pdf",
                        AmpuleConcentration = 493,
                        DiluentFluid = "5% Dextrose",
                        Note = "Watch for hypotension. Keep serum Mg 1.5-2.5 mmol/L. May be repeated.",
                        Route = "Peripheral or Central Line",
                        DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
                        RateUnit = new InfusionRateUnit(-3, Unit.gram, true, false),
                        InfusionPeriods = new[]
                        {
                            new InfusionPeriodVM
                            {
                                AmpuleMl = 2.83975659229209 ,
                                CalculatedDose = 50,
                                CumulativeStartTime = new MinutesDuration(),
                                Duration = new MinutesDuration(20),
                                DiluentVolume = 17.1602434077079 ,
                                DrawingUpDose = 1400,
                                FinalVolume = 20,
                                InfusionRate = 60,
                                OneMlHrDose = 2.5
                            }
                        }
                    });
                Add(50,
                // Dilution Method Id: 4 wt: 50 ageMth: 600
                    new FixedInfusionView {
                        InfusionDrugId = 37,
                        Fullname = "Phenytoin - Peripheral IV",
                        Abbrev = "Phenytoin PIV",
                        AmpulePrefix = -3,
                        SiUnitId = Unit.gram,
                        ReferenceDescription = "Starship Clinical Guidelines",
                        RefAbbrev = "SS Guidelines",
                        Hyperlink = "http://www.adhb.govt.nz/StarShipClinicalGuidelines/",
                        RouteDescription = "Large Peripheral Line",
                        RouteAbbrev = "Large PIV",
                        DilutionMethod = DilutionMethod.FixedDilutionVaryFlowByWeight,
                        InfusionPrefix = -3,
                        IsPerMin = false,
                        ReferencePage = "Convulsions%20Status%20Epilepticus.htm#Manage_in_Resuscitation_Area",
                        Concentration = 5,
                        StopMins = 20,
                        Rate = 60,
                        DiluentType = "0.9% Saline",
                        DiluentAbbrev = "Saline",
                        AmpuleConcentration = 50,
                    }, new FixedInfusionDrugVM
                    {
                        DrugName = "Phenytoin - Peripheral IV",
                        SourceDescription = "Starship Clinical Guidelines",
                        SourceHref = "http://www.adhb.govt.nz/StarShipClinicalGuidelines/Convulsions%20Status%20Epilepticus.htm#Manage_in_Resuscitation_Area",
                        AmpuleConcentration = 50,
                        DiluentFluid = "0.9% Saline",
                        Route = "Large Peripheral Line",
                        DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
                        RateUnit = new InfusionRateUnit(-3, Unit.gram, true, false),
                        InfusionPeriods = new[]
                        {
                            new InfusionPeriodVM
                            {
                                AmpuleMl = 20,
                                CalculatedDose = 20,
                                CumulativeStartTime = new MinutesDuration(),
                                Duration = new MinutesDuration(20),
                                DiluentVolume = 180,
                                DrawingUpDose = 1000,
                                FinalVolume = 200,
                                InfusionRate = 600,
                                OneMlHrDose = 0.1
                            }
                        }
                    });

                Add(50,
                // Dilution Method Id: 2 wt: 50 ageMth: 600
                    new FixedInfusionView {
                        InfusionDrugId = 36,
                        Fullname = "Phenytoin - Central Access",
                        Abbrev = "Phenytoin CVL",
                        AmpulePrefix = -3,
                        SiUnitId = Unit.gram,
                        ReferenceDescription = "Starship Clinical Guidelines",
                        RefAbbrev = "SS Guidelines",
                        Hyperlink = "http://www.adhb.govt.nz/StarShipClinicalGuidelines/",
                        RouteDescription = "Central Line Only",
                        RouteAbbrev = "CVL only",
                        DilutionMethod = DilutionMethod.NeatVaryFlowByWeight,
                        InfusionPrefix = -3,
                        IsPerMin = false,
                        ReferencePage = "Convulsions%20Status%20Epilepticus.htm#Manage_in_Resuscitation_Area",
                        Concentration = 50,
                        StopMins = 20,
                        Rate = 60,
                        DiluentType = "Undiluted",
                        DiluentAbbrev = "Neat",
                        AmpuleConcentration = 50,
                    }, new FixedInfusionDrugVM
                    {
                        DrugName = "Phenytoin - Central Access",
                        SourceDescription = "Starship Clinical Guidelines",
                        SourceHref = "http://www.adhb.govt.nz/StarShipClinicalGuidelines/Convulsions%20Status%20Epilepticus.htm#Manage_in_Resuscitation_Area",
                        AmpuleConcentration = 50,
                        DiluentFluid = "Undiluted",
                        Route = "Central Line Only",
                        DrawingUpUnits = new SiUnitMeasure(-3, Unit.gram),
                        RateUnit = new InfusionRateUnit(-3, Unit.gram, true, false),
                        InfusionPeriods = new[]
                        {
                            new InfusionPeriodVM
                            {
                                AmpuleMl = 20,
                                CalculatedDose = 20,
                                CumulativeStartTime = new MinutesDuration(),
                                Duration = new MinutesDuration(20),
                                DiluentVolume = 0,
                                DrawingUpDose = 1000,
                                FinalVolume = 20,
                                InfusionRate = 60,
                                IsNeat = true,
                                OneMlHrDose = 1
                            }
                        }
                    });
        }
        ~FixedInfusionVMTestData()
        {
            Console.WriteLine("The following methods remain untested: " + string.Join(",",_methods
                .Select(m=>$"{m}={(int)m}")));
        }
    }
}