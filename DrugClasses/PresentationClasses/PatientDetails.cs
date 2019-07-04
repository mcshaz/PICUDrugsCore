using DrugClasses.PresentationClasses.Patient;
using DrugClasses.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DrugClasses.PresentationClasses
{
    public class PatientDetails
    {
        public ChildAge Age { get; set; }
        public readonly string Date = DateTime.Today.ToLongDateString();
        public string Name { get; set; }
        public string NHI { get; set; }
        [Range(FieldConst.minWeight, FieldConst.maxWeight)]
        public double ActualWeight { get; set; }
        public double WorkingWeight { get { return (ActualWeight > FieldConst.maxWeight ? FieldConst.maxWeight : ActualWeight); } }
        public string Centile { get; set; }
        public int WardId { get; set; }
        public bool? IsMale { get; set; }
        public bool WeightEstimate { get; set; }
        public double GestationAtBirth { get; set; }
    }
}
