using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PicuDrugsCore.DAL
{
    
    public class BolusDose
    {
        [Key]
        public int BolusDoseId { get; set; }
        [ForeignKey("BolusDrug")]
        public int BolusDrugId { get; set; }
        [Range(0, 100000, ErrorMessage = "Dose per kg must be between 0 and 100 000 [units of measure]" )]
        public double MinDosePerKg { get; set; }
        [Range(0, 100000, ErrorMessage = "Dose per kg must be between 0 and 100 000 [units of measure]")]
        public double MaxDosePerKg { get; set; }
        [Range(FieldConst.minWeight, FieldConst.maxWeight, ErrorMessage = FieldConst.wtErr)]
        public double WeightMin { get; set; }
        [Range(FieldConst.minWeight, FieldConst.maxWeight, ErrorMessage = FieldConst.wtErr)]
        public double WeightMax { get; set; }
    
        public virtual BolusDrug BolusDrug { get; set; }
    }
}