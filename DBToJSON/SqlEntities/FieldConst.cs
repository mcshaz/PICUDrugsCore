namespace DBToJSON.SqlEntities
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
        public const double maxStop = 10080;
        public const double minStop = 1;
        public const string stopErr = "Time to stop Infusion must be 1 and 7 days (10 080 minutes)";
        public const string fileTypeRegEx = @"(\.pdf|\.html?|\.docx?)[\w#=?]*$";
        public const string remoteHostsRegEx = @"ahsl\d\d?";
        //define regular expressions for input - acceptable input = '\\'|('file://' + '\\' | +/-localhost + '///' then ahsl? 
        public const string inputFileRegEx = @"^(file:[/\\]+(localhost)?)?[/\\]*(" + remoteHostsRegEx + @"[\w \./\\%+]+)$";
        public const string filePathRegEx = @"^\\{2}" + remoteHostsRegEx + @"[\w \\\.#=+]+?$"; //currently allowing space at the end & trimming this off in BLL - technically could potentially have different regex server & client
        public const string urlRegEx = @"^(https?)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        public const string hyperlinkRegEx = "(" + urlRegEx + ")|(" + filePathRegEx + ")";
        public const string urlOrPathRegEx = "(" + urlRegEx + ")|(" + inputFileRegEx + ")";
        public const double maxJoules = 360;
    }
}