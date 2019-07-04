namespace DrugClasses.Transformations
{
    public static partial class PatientCalculations
    {
        /// <summary>
        /// Return daily fluid requirement in mls calculated according to Holliday and Segar (max 2400 ml/day)
        /// </summary>
        /// <param name="weightKg">Weight in Kilograms</param>
        /// <returns></returns>
        public static int DailyMaintenanceFluid(double weightKg)
        {
            if (weightKg >= 65)
            {
                return 2400;
            }
            if (weightKg >= 20.0)
            {
                return (int)(1500.0 + 20.0 * (weightKg - 20.0));
            }
            if (weightKg >= 10.0)
            {
                return (int)(1000 + 50.0 * (weightKg - 10.0));
            }
            return (int)(100 * weightKg);
        }
    }
}
