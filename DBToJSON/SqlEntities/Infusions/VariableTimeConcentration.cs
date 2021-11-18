//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBToJSON.SqlEntities.Infusions
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enums;
    public partial class VariableTimeConcentration
    {
        [Key]
        public int InfusionConcentrationId { get; set; }
        public DoseCat DoseCatId { get; set; }
        [ForeignKey("VariableTimeDilution")]
        public int InfusionDilutionId { get; set; }
        public double Concentration { get; set; }
        
        public virtual VariableTimeDilution VariableTimeDilution { get; set; }
    }
}