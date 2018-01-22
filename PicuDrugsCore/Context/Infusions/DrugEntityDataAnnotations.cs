using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.DynamicData;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//using Foolproof;

namespace PicuDrugsCore.DAL
{

    public class InfusionDilutionMetaData
    {
        [Range(FieldConst.minWeight, FieldConst.maxWeight, ErrorMessage = FieldConst.wtErr)]
        public int WeightMin { get; set; }
        [Range(FieldConst.minWeight, FieldConst.maxWeight, ErrorMessage = FieldConst.wtErr)]
        //[GreaterThan("WeightMin")]
        public int WeightMax { get; set; }
        [Range(FieldConst.minAge, FieldConst.maxAge, ErrorMessage = FieldConst.ageErr)]
        public int AgeMinMonths { get; set; }
        [Range(FieldConst.minAge, FieldConst.maxAge, ErrorMessage = FieldConst.ageErr)]
        //[GreaterThan("AgeMinMonths")]
        public int AgeMaxMonths { get; set; }
        [Required(ErrorMessage = "Must be either per minute or per hour")]
        public Boolean IsPerMin { get; set; }
    }

    public class FixedTimeDilutionMetaData : InfusionDilutionMetaData
    {
        [StringLength(100)]
        public string ReferencePage { get; set; }
    }
    public class InfusionConcentrationMetaData
    {
        [Range(0.001, 1001, ErrorMessage = "Concentration must be between 0.001 and 1000")]
        public double Concentration { get; set; }
    }
    [MetadataType(typeof(FixedTimeConcentrationMetaData))]
    public partial class FixedTimeConcentration: IInfusionConcentration
    {
    }
    public class FixedTimeConcentrationMetaData : InfusionConcentrationMetaData
    {
        [Range(FieldConst.minVolume, FieldConst.maxVolume, ErrorMessage = FieldConst.volErr)]
        public int Volume { get; set; }
        [Range(FieldConst.minStop, FieldConst.maxStop, ErrorMessage = FieldConst.stopErr)]
        public int StopMinutes { get; set; }
        [Range(FieldConst.minRate, FieldConst.maxRate, ErrorMessage = FieldConst.rateErr)]
        public double Rate { get; set; }
    }
    [MetadataType(typeof(InfusionConcentrationMetaData))]
    public partial class VariableTimeConcentration : IInfusionConcentration
    {
    }
    [MetadataType(typeof(InfusionDrugMetaData))]
    public partial class InfusionDrug
    {
    }
    public class InfusionDrugMetaData
    {
        [StringLength(50,MinimumLength=3,ErrorMessage="Drug name must be between 3 and 50 characters long")]
        [Required(ErrorMessage = "Drug name must be provided")]
        public string Fullname { get; set; }
        [StringLength(24, MinimumLength = 3, ErrorMessage = "Abbreviation must be between 3 and 24 characters long")]
        public string Abbrev { get; set; }
        [Required(ErrorMessage="Must be either IsTitratable or loading type of Infusion")]
        public Boolean IsTitratable {get; set;}
    }
    [MetadataType(typeof(DrugReferenceMetaData))]
    public partial class DrugReferenceSource
    {
    }
    public class DrugReferenceMetaData
    {
        [StringLength(15,MinimumLength=3, ErrorMessage = "Reference abbreviation must be between 3 and 15 characters long")]
        [Required(ErrorMessage = "Reference abbreviation must be provided")]
        public string Abbrev { get; set; }
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Reference description must be between 5 and 50 characters long")]
        [Required(ErrorMessage = "Reference description must be provided")]
        public string ReferenceDescription { get; set; }
//        [RegularExpression(FieldConst.hyperlinkRegEx,
//            ErrorMessage = @"Hyperlink must be either a valid web address or a network file link which is valid in a web browser (beginning with file:\\ahsl?\). drive letters are not allowed.")]
        [StringLength(100, ErrorMessage = "Hyperlink must be <100 characters long")]
        public string Hyperlink { get; set; }
    }
    [MetadataType(typeof(DrugAmpuleConcMetaData))]
    public partial class DrugAmpuleConcentration
    {
    }
    public class DrugAmpuleConcMetaData
    {
        [Range(1e-3, 250, ErrorMessage = "Drug ampule Volume must be between 0.001 and 1000 mL")]
        public double Volume { get; set; }
        [Range(1e-3, 10000, ErrorMessage = "Drug ampule Concentration must be between 0.001 and 10 000 [drug units]/mL")]
        public double Concentration { get; set; }
    }
    [MetadataType(typeof(DefibModelMetaData))]
    public partial class DefibModel
    {
    }
    public class DefibModelMetaData
    {
        [StringLength(50, MinimumLength = 2)]
        [Required]
        [Display(Name = "Model Name")]
        public string Name { get; set; }
    }
    [MetadataType(typeof(DefibJouleMetaData))]
    public partial class DefibJoule
    {
    }
    public class DefibJouleMetaData
    {
        [Required]
        [Range(0, FieldConst.maxJoules)]
        public string Joules { get; set; }
    }
}


