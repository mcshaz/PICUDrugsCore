using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DrugClasses.RepositoryClasses;
using System.Linq;

namespace XUnitTestDrugs.Utilities
{
    class MyContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("server=(local);Database=PicuDrugData;Integrated Security=True;");
        }
        public DbQuery<VariableInfusionView> VariableInfusions { private get; set; }
        public ICollection<VariableInfusionView> GetVariableInfusionView (double weight, int wardId, int ageMonths)
        {
            return VariableInfusions
                .FromSql("EXEC sp_GetVariableInfusions2 @WardId={0}, @AgeMonths={1}, @WeightKg={2}", wardId, ageMonths, weight)
                .ToList();
        }
        public DbQuery<FixedInfusionView> FixedInfusions { private get; set; }
        public ICollection<FixedInfusionView> GetFixedInfusionView(double weight, int ampuleConcentrationId, int ageMonths)
        {
            return FixedInfusions
                .FromSql("EXEC sp_GetFixedInfusions @AmpuleConcentrationId={0}, @AgeMonths={1}, @WeightKg={2}", ampuleConcentrationId, ageMonths, weight)
                .ToList();
        }
    }
}
