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
    
    public partial class DefibJoule
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("DefibModel")]
        public int DefibId { get; set; }
        [Required]
        [Range(0, FieldConst.maxJoules)]
        public int Joules { get; set; }
    
        public virtual DefibModel DefibModel { get; set; }
    }
}
