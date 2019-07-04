using DrugClasses.RepositoryClasses;
using DrugClasses.Transformations.Interfaces;
using System.Collections.Generic;

namespace DrugClasses.Transformations
{

    internal static class ViewToEnumerable
    {
        public static IEnumerable<IContextDrug> Transform(ICollection<IContextDrug> view)
        {
            var returnVar = new List<IContextDrug>(view.Count);
            IContextDrug previousRow = null;
            List<IContextConcentration> conc=null;
            foreach (var r in view)
            {
                if (previousRow == null)
                {
                    conc = new List<IContextConcentration>();
                }
                else if (previousRow.InfusionDrugId != r.InfusionDrugId)
                {
                    previousRow.Concentrations = conc;
                    returnVar.Add(previousRow);
                    conc = new List<IContextConcentration>();
                }
                conc.Add((IContextConcentration)r);
                previousRow = r;
            }
            if (previousRow != null)
            {
                previousRow.Concentrations = conc;
                returnVar.Add(previousRow);
            }
            return returnVar;
        }
    }
}
