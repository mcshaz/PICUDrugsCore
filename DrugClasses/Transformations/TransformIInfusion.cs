using DrugClasses.PresentationClasses;
using DrugClasses.PresentationClasses.Dosing;
using DrugClasses.RepositoryClasses;
using DrugClasses.Transformations.Interfaces;
using DrugClasses.Utilities;
using System;

namespace DrugClasses.Transformations
{
    public static class TranformIInfusion
    {
        internal static void DrugInfusion(
            double weight,
            IContextDrug contextDrug, 
            IInfusionDrugVM newDrug,
            Func<IContextConcentration, IConcentrationDetailVM> makeAndAddNewConcentration)
        {
            if (weight < FieldConst.minWeight || weight > FieldConst.maxWeight)
            {
                throw new Exception(FieldConst.wtErr);
            }

            newDrug.DrawingUpUnits = new SiUnitMeasure(contextDrug.AmpulePrefix, contextDrug.SiUnitId);
            MethodLogic method = DilutionLogic.GetMethod(contextDrug.DilutionMethod);
            newDrug.RateUnit = new InfusionRateUnit(contextDrug.InfusionPrefix, contextDrug.SiUnitId, method.IsPerKg, contextDrug.IsPerMin);
            double ampConvFact = Math.Pow(10, contextDrug.InfusionPrefix - contextDrug.AmpulePrefix);

            double workingWt = default(double);
            double dilVol=default(double);
            if (method.IsVaryConcentration)
            {
                if (method.IsVaryVolume)
                {
                    workingWt = Math.Truncate(weight / 2) * 2; //this decrease in acuracy to round to the weight to the nearest even integer is so that the volumes follow the PICU chart
                    dilVol = PatientCalculations.DilutionVolumeMls(workingWt);
                }
                else
                {
                    workingWt = weight;
                }
            }

            foreach (var contextConcentration in contextDrug.Concentrations)
            {
                var newConc = makeAndAddNewConcentration(contextConcentration);
                newConc.IsNeat = method.IsNeat;
                if (method.IsVaryConcentration)
                {
                    if (!method.IsVaryVolume)
                    {
                        dilVol = contextConcentration.Volume.Value;
                    }
                    newConc.FinalVolume = (int)(dilVol + 0.5);
                    //uses conc
                    newConc.DrawingUpDose = contextConcentration.Concentration * dilVol * workingWt * (contextDrug.IsPerMin ? 60 : 1) * ampConvFact;
                    newConc.OneMlHrDose = contextConcentration.Concentration;
                }
                else
                {
                    if (method.IsVaryVolume)
                    {
                        //vary Volume && !vary Concentration = Volume in mL per kg
                        newConc.FinalVolume = (int)(contextConcentration.Volume.Value * weight);
                        newConc.DrawingUpDose = contextConcentration.Concentration * (contextDrug.IsPerMin ? 60 : 1) * newConc.FinalVolume;
                        newConc.OneMlHrDose = contextConcentration.Concentration / weight;
                    }
                    else //method.IsNeat or a fixedConcentration
                    {
                        newConc.OneMlHrDose = method.IsPerKg 
                            ? (contextConcentration.Concentration / weight) 
                            : contextConcentration.Concentration;
                        if (contextConcentration.Volume.HasValue)
                        {
                            newConc.FinalVolume = contextConcentration.Volume.Value;
                            newConc.DrawingUpDose = newConc.FinalVolume * contextConcentration.Concentration * ampConvFact * (contextDrug.IsPerMin ? 60 : 1);
                        }
                        else if (contextConcentration is IContextFixedConc fixConc)
                        {
                            //??final concentration rather than concentration
                            newConc.DrawingUpDose = fixConc.Rate * ampConvFact * fixConc.StopMins * (method.IsPerKg ? weight : 1) / (contextDrug.IsPerMin ? 1 : 60);
                            newConc.FinalVolume = (int)(newConc.DrawingUpDose / (fixConc.Concentration * ampConvFact));
                        }
                        else
                        {
                            throw new Exception("either a volume or a duration and dose must be supplied");
                        }
                    }
                }
            }
        }
    }
}
