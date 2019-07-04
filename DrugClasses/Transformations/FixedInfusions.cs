using DrugClasses.PresentationClasses;
using DrugClasses.PresentationClasses.Duration;
using DrugClasses.RepositoryClasses;
using DrugClasses.Transformations.Interfaces;
using System;
using System.Collections.Generic;

namespace DrugClasses.Transformations
{
    public static class FixedInfusions
    {
        public static FixedInfusionDrugVM DrugInfusion(
            double weight,
            IList<FixedInfusionView> views)
        {
            var v1 = views[0];
            ((IContextDrug)v1).Concentrations = views;
            var d = new FixedInfusionDrugVM
            {
                DrugName = v1.Fullname,
                Note = v1.Note,
                SourceHref = v1.Hyperlink + v1.ReferencePage,
                SourceDescription = v1.ReferenceDescription,
                Route = v1.RouteDescription,
                DiluentFluid = v1.DiluentType,
                InfusionPeriods = new List<InfusionPeriodVM>(),
                AmpuleConcentration = v1.AmpuleConcentration
            };
            MinutesDuration cumulativeDuration = new MinutesDuration();
            TranformIInfusion.DrugInfusion(weight,v1,d,icc=> {
                var fiv = (FixedInfusionView)icc;
                var cd = new InfusionPeriodVM
                {
                    Duration = fiv.StopMins - cumulativeDuration,
                    CumulativeStartTime = cumulativeDuration
                };
                d.InfusionPeriods.Add(cd);
                cumulativeDuration = new MinutesDuration(fiv.StopMins);
                return cd;
            });
            double ampConversion = Math.Pow(10,v1.AmpulePrefix-v1.InfusionPrefix);
            for (int i=0; i<views.Count; i++)
            {
                var v = views[i];
                var p = d.InfusionPeriods[i];
                p.InfusionRate = v.Rate / p.OneMlHrDose;
                var unitsPerMin = v.Rate / (d.RateUnit.IsPerMin?1:60);
                p.CalculatedDose = unitsPerMin * p.Duration.TotalMins / ampConversion;
                p.AmpuleMl = p.DrawingUpDose / v1.AmpuleConcentration;
                p.DiluentVolume = p.FinalVolume - p.AmpuleMl;
            }
            return d;
        }
    }
}
