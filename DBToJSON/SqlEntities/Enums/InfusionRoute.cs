namespace DBToJSON.SqlEntities.Enums
{
    /*
    [Flags]
    public enum DrugRoute
    {
        CVLOnly = 1,
        CVLPref = 2,
        LargePIV = 4, // was 3
        PIV = 8, // was 4
        IMI = 16,
        SC = 32,
        Bucal = 64,
        Rectal = 128,
        ETT = 256,
        Nebulised = 512,
        IO = 1024,
    }
    */
    public enum InfusionRoute
    {
        CVLOnly = 1, // 1	Central Line Only CVL only  
        CVLPref = 2, // 2	Central Line Preferred CVL pref.
        LargePIV = 3, // 3	Large Peripheral or Central Line Large PIV 
        PIV = 4, // 4	Peripheral or Central Line PIV
    }
}

