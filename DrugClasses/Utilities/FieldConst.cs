namespace DrugClasses.Utilities
{
    public static class FieldConst
    {
        public const double minVolume = 1;
        public const double maxVolume = 1000;
        public const string volErr = "Dilution Volume must be between 1 and 1000 mL";
        public const double maxWeight = 100;
        public const double minWeight = 0;
        public const string wtErr = "Weight must be between 0 and 100 Kg";
        public const double maxAge = 1200;
        public const double minAge = 0;
        public const string ageErr = "Age must be between 0 and 100yo (1200 months)";
        public const double maxRate = 10001;
        public const double minRate = 0;
        public const string rateErr = "Rate must be between 0 and 10 000";
    }
}
