//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PicuDrugsCore.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("CdcWeightForAge")]
    public partial class CdcWeightForAge
    {
        [Key]
        public int LookupVal { get; set; }
        public string GenderText { get; set; }
        public double AgeMonths { get; set; }
        public double L { get; set; }
        public double M { get; set; }
        public double S { get; set; }
        public int Gender { get; set; }
    }
}
