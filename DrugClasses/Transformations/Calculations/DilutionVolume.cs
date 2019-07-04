using System;
using System.Collections.Generic;
using System.Text;

namespace DrugClasses.Transformations
{
    public static partial class PatientCalculations
    {
        public const double MinVariableWeight = 30;
        public const double MaxVolume = 60;
        public static double DilutionVolumeMls(double ptWeight)
        {
            const double volConvFact = 1e4 / 3;
            if (ptWeight < MinVariableWeight)
            {
                throw new Exception("ptWeight < " + MinVariableWeight.ToString());
            }

            //calculate Volume
            double returnVal = (volConvFact / ptWeight);
            if (returnVal > MaxVolume) returnVal = returnVal / 2;
            return returnVal;
        }
    }
}
