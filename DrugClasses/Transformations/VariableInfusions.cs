using DrugClasses.PresentationClasses;
using DrugClasses.RepositoryClasses;
using DrugClasses.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DrugClasses.Transformations
{
    public static class VariableInfusions
    {
        public static IList<VariableInfusionDrugVM> DrugInfusion(
            double weight,
            ICollection<VariableInfusionView> infusions)
        {
            var returnVar = new List<VariableInfusionDrugVM>(infusions.Count);
            foreach (var i in ViewToEnumerable.Transform((ICollection<Interfaces.IContextDrug>)infusions))
            {
                var Viv = (VariableInfusionView)i;

                var d = new VariableInfusionDrugVM {
                    DrugName = Viv.Fullname,
                    Link = Viv.HrefBase + Viv.HrefPage,
                    DoseRange = new DoublePrecisionRange(Viv.RateMin, Viv.RateMax),
                    Note = Viv.Note,
                    InfusionDetails = new List<VariableConcentrationDetailVM>()
                };
                returnVar.Add(d);
                TranformIInfusion.DrugInfusion(weight,i,d,icc=> {
                    var ic = (VariableInfusionView)icc;
                    var cd = new VariableConcentrationDetailVM
                    {
                        DetailName = ic.Category ?? ic.Abbrev
                    };
                    d.InfusionDetails.Add(cd);
                    return cd;
                });
                foreach (var c in d.InfusionDetails)
                {
                    c.FlowRange = d.DoseRange / c.OneMlHrDose;
                }
            }
            return returnVar;
        }
    }
}
