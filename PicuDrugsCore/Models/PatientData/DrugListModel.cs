using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PicuDrugsCore.Models.PatientData
{
    public class DrugListModel
    {
        public DrugListRequired ListRequested { get; set; }
        public int? WardId { get; set; }
        public string Name { get; set; }
        [RegularExpression("[A-Za-z]3[0-9]{4}")]
        public string NHI { get; set; }
        public bool? RememberSelection { get; set; }
        [ Range(1e-12, 100 - 1e-7)]
        public float? Centile { get; set; }
        public bool? MaleGender { get; set; }
        [Range(0,130*365)]
        public int? AgeDays { get; set; }
        [Required, Range(0.3,400)]
        public int Weight { get; set; }
        [Range(23, 43)]
        public int GestationAtBirth { get; set; } = 40;
        public DrugListOutput Output { get; set; }
        
        public List<SelectListItem> DepartmentLists { get; set; }
    }
    public enum DrugListRequired { SingleInfusion, BolusDrugs, BolusPlusInfusions }
    public enum DrugListOutput { Pdf, Html }
}
