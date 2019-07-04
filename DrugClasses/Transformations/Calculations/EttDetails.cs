using DrugClasses.Utilities;

namespace DrugClasses.Transformations
{
    public class EttDetails
    {
        public double? InternalDiameter { get; set; }
        public DoublePrecisionRange InternalDiameterRange { get; set; }
        public double LengthAtLip { get; set; }
        public double LengthAtNose { get; set; }
        public int SuctionWith { get; set; }
        public string Note { get; set; }
    }
    public static partial class PatientCalculations
    {
        public static EttDetails ETT(double ageInMonths, double weight = 0.0, double gestAgeBirth = 40.0)
        {
            const double monthsPerWeek = 12.0 / 52.0;
            ageInMonths = ageInMonths - (40.0 - gestAgeBirth) * monthsPerWeek;
            var returnVal = new EttDetails();
            var rangeFactory = new RangeFactory { Rounding = RoundingMethod.FixedDecimalPlaces, Precision = 1 };
            if (ageInMonths < 1)
            {
                if (weight <= 0.0)
                {
                    returnVal.InternalDiameterRange = rangeFactory.Create(2.5, 3.5);
                    returnVal.Note = "Exact measurements will depend on gestation and weight (not specified)";
                }
                else
                {
                    if (weight < 0.7)
                    {
                        returnVal.InternalDiameter = 2.0;
                        returnVal.LengthAtLip = 5;
                        returnVal.LengthAtNose = 6;
                        returnVal.InternalDiameterRange = rangeFactory.Create(2.0, 2.5);
                    }
                    else if (weight < 1.0)
                    {
                        returnVal.InternalDiameter = 2.5;
                        returnVal.LengthAtLip = 5.5;
                        returnVal.LengthAtNose = 7;
                        returnVal.InternalDiameterRange = rangeFactory.Create(2.0, 2.5);
                    }
                    else if (weight <= 1.5)
                    {
                        returnVal.InternalDiameter = 3;
                        returnVal.LengthAtLip = 6;
                        returnVal.LengthAtNose = 7.5;
                        returnVal.InternalDiameterRange = rangeFactory.Create(2.5, 3.0);
                    }
                    else if (weight <= 2.0)
                    {
                        returnVal.InternalDiameter = 3;
                        returnVal.LengthAtLip = 7;
                        returnVal.LengthAtNose = 9;
                        returnVal.InternalDiameterRange = rangeFactory.Create(2.5, 3.0);
                    }
                    else if (weight <= 3.0)
                    {
                        returnVal.InternalDiameter = 3.0;
                        returnVal.LengthAtLip = 8.5;
                        returnVal.LengthAtNose = 10.5;
                        returnVal.InternalDiameterRange = rangeFactory.Create(2.5, 3.5);
                    }
                    else
                    {
                        returnVal.InternalDiameter = 3.5;
                        returnVal.LengthAtLip = 9;
                        returnVal.LengthAtNose = 11;
                        returnVal.InternalDiameterRange = rangeFactory.Create(3.0, 3.5);
                    }
                }
            }
            else if (ageInMonths <= 6.0)
            {
                returnVal.InternalDiameter = 3.5;
                returnVal.LengthAtLip = 10;
                returnVal.LengthAtNose = 12;
                returnVal.InternalDiameterRange = rangeFactory.Create(3.0, 4.0); //note had been 3.5 min
            }
            else if (ageInMonths <= 18.0)
            {
                returnVal.InternalDiameter = 4.0;
                returnVal.LengthAtLip = 11;
                returnVal.LengthAtNose = 14;
            }
            else if (ageInMonths <= 30.0) // 1.5 - 2.5
            {
                returnVal.InternalDiameter = 4.5;
                returnVal.LengthAtLip = 12;
                returnVal.LengthAtNose = 15;
            }
            else if (ageInMonths <= 42.0) // 2.5 - 3.5
            {
                returnVal.InternalDiameter = 4.5;
                returnVal.LengthAtLip = 13;
                returnVal.LengthAtNose = 16;
            }
            else if (ageInMonths <= 54.0) // 3.5 - 5
            {
                returnVal.InternalDiameter = 5.0;
                returnVal.LengthAtLip = 14;
                returnVal.LengthAtNose = 17;
            }
            else if (ageInMonths <= 84.0) //5 - 7
            {
                returnVal.InternalDiameter = 5.5;
                returnVal.LengthAtLip = 15;
                returnVal.LengthAtNose = 19;
            }
            else if (ageInMonths <= 108.0)// 7 - 9
            {
                returnVal.InternalDiameter = 6;
                returnVal.LengthAtLip = 16;
                returnVal.LengthAtNose = 20;
            }
            else // >9 should have a note
            {
                returnVal.Note = "routine nasal not indicated in this age";
                if (ageInMonths <= 132.0) //9 - 11
                {
                    returnVal.InternalDiameter = 6.5;
                    returnVal.LengthAtLip = 17;
                    returnVal.LengthAtNose = 21;
                }
                else if (ageInMonths <= 156.0) //11-13
                {
                    returnVal.InternalDiameter = 7;
                    returnVal.LengthAtLip = 18;
                    returnVal.LengthAtNose = 22;
                }
                else if (ageInMonths < 180.0) //13-15
                {
                    returnVal.InternalDiameter = 7.5;
                    returnVal.LengthAtLip = 19;
                    returnVal.LengthAtNose = 23;
                }
                else //15+
                {
                    returnVal.InternalDiameter = 8;
                    returnVal.LengthAtLip = 20;
                    returnVal.LengthAtNose = 24;
                }
            }
            if (returnVal.InternalDiameter <= 2.5) { returnVal.SuctionWith = 6; }
            else if (returnVal.InternalDiameter == 3.0) { returnVal.SuctionWith = 7; }
            else if (returnVal.InternalDiameter <= 4.5) { returnVal.SuctionWith = 8; }
            else if (returnVal.InternalDiameter <= 6.0) { returnVal.SuctionWith = 10; }
            else { returnVal.SuctionWith = 12; }

            if (returnVal.InternalDiameterRange == null)
            {
                returnVal.InternalDiameterRange = rangeFactory.Create(
                    returnVal.InternalDiameter.Value - 0.5,
                    returnVal.InternalDiameter.Value + 0.5);
            }
            return returnVal;
        }
    }
}
