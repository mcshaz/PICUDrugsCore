using System.Linq;

namespace DBToJSON
{
    using DBToJSON.Helpers;
    using DBToJSON.SqlEntities;
    using DBToJSON.SqlEntities.BolusDrugs;
    using DBToJSON.SqlEntities.Enums;
    using DBToJSON.SqlEntities.Infusions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class DrugSqlContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("server=(local);Database=PicuDrugData;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecordDeletionTime>().HasKey(rd => new { rd.TableId, rd.IdOfDeletedRecord });
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<BolusDose> BolusDoses { get; set; }
        public virtual DbSet<BolusDrug> BolusDrugs { get; set; }
        public virtual DbSet<FixedDrug> FixedDrugs { get; set; }
        public virtual DbSet<FixedDose> FixedDoses { get; set; }
        public virtual DbSet<BolusSortOrdering> BolusSortOrdering { get; set; }
        public virtual DbSet<DefibJoule> DefibJoules { get; set; }
        public virtual DbSet<DefibModel> DefibModels { get; set; }
        public virtual DbSet<DoseCat> DoseCats { get; set; }
        public virtual DbSet<DrugAmpuleConcentration> DrugAmpuleConcentrations { get; set; }
        public virtual DbSet<DrugReferenceSource> DrugReferenceSources { get; set; }
        public virtual DbSet<DrugRoute> DrugRoutes { get; set; }
        public virtual DbSet<FixedTimeConcentration> FixedTimeConcentrations { get; set; }
        public virtual DbSet<FixedTimeDilution> FixedTimeDilutions { get; set; }
        public virtual DbSet<InfusionDiluent> InfusionDiluents { get; set; }
        public virtual DbSet<InfusionDrug> InfusionDrugs { get; set; }
        public virtual DbSet<InfusionSortOrdering> InfusionSortOrderings { get; set; }
        public virtual DbSet<VariableTimeConcentration> VariableTimeConcentrations { get; set; }
        public virtual DbSet<VariableTimeDilution> VariableTimeDilutions { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }
        public virtual DbSet<RecordDeletionTime> RecordDeletions { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var delUpdates = BeforeSave();
            delUpdates.ForEach(d => d.ExecuteQuery());
            var returnVar = base.SaveChanges(acceptAllChangesOnSuccess);
            RecordDeletions.AddRange(from d in delUpdates
                                     let a = d.UpdateAfterSave()
                                     where a != null
                                     select a);
            base.SaveChanges(acceptAllChangesOnSuccess);
            return returnVar;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var delUpdates = BeforeSave();
            delUpdates.ForEach(async d => await d.ExecuteQueryAsync());
            var returnVar = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            RecordDeletions.AddRange(from d in delUpdates
                                     let a = d.UpdateAfterSave()
                                     where a != null
                                     select a);
            await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            return returnVar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dels"></param>
        /// <returns>whether new deletions added</returns>

        private IEnumerable<DeletionUpdateBase> BeforeSave()
        {
            List<DeletionUpdateBase> returnVar = null;
            if (ChangeTracker.HasChanges()) {
                var now = DateTime.UtcNow;
                foreach (var e in ChangeTracker.Entries())
                {
                    switch (e.State)
                    {
                        case EntityState.Added:
                        case EntityState.Modified:
                            if (e.Entity is UpdateTrackingEntity d)
                            {
                                d.DateModified = now;
                            }
                            break;
                        case EntityState.Deleted:
                            var res = HandleDeletion(e, now);
                            if (res != null)
                            {
                                (returnVar ?? (returnVar = new List<DeletionUpdateBase>())).Add(res);
                            }
                            break;
                    }
                }
            }
            return returnVar ?? Enumerable.Empty<DeletionUpdateBase>();
        }

        private DeletionUpdateBase HandleDeletion(EntityEntry e, DateTime now)
        {
            switch (e.Entity)
            {
                case BolusDose bDose:
                    return DeleteHelper.Create(now, () => bDose.BolusDrug, from d in BolusDoses
                                                                     where d.BolusDoseId == bDose.BolusDoseId
                                                                     select d.BolusDrug);
                case BolusDrug bDrug:
                    var returnVar = DeleteHelper.Create(now, () => bDrug.BolusSortOrderings?.ToListIfAllNonNull(bso => bso.Ward),
                                                            (from bso in BolusSortOrdering
                                                             where bso.BolusDrugId == bDrug.BolusDrugId
                                                             select bso.Ward).Distinct());
                    returnVar.AddRecord(bDrug.BolusDrugId, NosqlTable.BolusDrugs);
                    return returnVar;
                case FixedDrug fDrug:
                    var returnVar2 = DeleteHelper.Create(now, () => fDrug.BolusSortOrderings?.ToListIfAllNonNull(fbso => fbso.Ward),
                                                            (from fbso in BolusSortOrdering
                                                             where fbso.FixedDrugId == fDrug.FixedDrugId
                                                             select fbso.Ward).Distinct());
                    returnVar2.AddRecord(fDrug.FixedDrugId, NosqlTable.FixedDrugs);
                    return returnVar2;
                case FixedDose fDose:
                    return DeleteHelper.Create(now, () => fDose.Drug,
                                                        from fd in FixedDoses
                                                        where fd.FixedDoseId == fDose.FixedDoseId
                                                        select fd.Drug);
                case BolusSortOrdering bso:
                    //must get this info pre deletion
                    return DeleteHelper.Create(now, () => bso.Ward, from b in BolusSortOrdering
                                                              where b.BolusSortOrderId == bso.BolusSortOrderId
                                                              select b.Ward);
                case DefibJoule dj:
                   return DeleteHelper.Create(now, () => dj.DefibModel, from d in DefibJoules
                                                                  where d.Id == dj.Id
                                                                  select d.DefibModel);
                case DefibModel dm:
                    var returnVar3 = DeleteHelper.Create(now, () => dm.Wards, from d in DefibModels
                                                                        where d.Id == dm.Id
                                                                        select dm.Wards);
                    returnVar3.AddRecord(dm.Id, NosqlTable.DefibModels);
                    return returnVar3;
                case DoseCat dCat:
                    return DeleteHelper.Create(now, () =>
                                     dCat.VariableTimeConcentrations?.ToListIfAllNonNull(c => c.VariableTimeDilution?.InfusionDrug),
                                        from vtc in VariableTimeConcentrations
                                        where vtc.DoseCat.DoseCatId == dCat.DoseCatId
                                        select vtc.VariableTimeDilution.InfusionDrug);
                case DrugAmpuleConcentration ampConc:
                    return DeleteHelper.Create(now, () => ampConc.InfusionDrug,
                                                      from ac in DrugAmpuleConcentrations
                                                      where ac.AmpuleConcentrationId == ampConc.AmpuleConcentrationId
                                                      select ac.InfusionDrug);
                case DrugReferenceSource refSource:
                    return DeleteHelper.Create(now, () => refSource.InfusionDrugs, 
                                                           from rs in DrugReferenceSources
                                                           where rs.DrugReferenceId == refSource.DrugReferenceId
                                                           select rs.InfusionDrugs);
                case DrugRoute dRoute:
                    return DeleteHelper.Create(now, () => dRoute.InfusionDrugs, 
                                                       from dr in DrugRoutes
                                                       where dr.RouteId == dRoute.RouteId
                                                       select dr.InfusionDrugs);
                case FixedTimeConcentration ftc:
                    return DeleteHelper.Create(now, () => ftc.FixedTimeDilution?.InfusionDrug, 
                                                        from c in FixedTimeConcentrations
                                                        where c.InfusionConcentrationId == ftc.InfusionConcentrationId
                                                        select c.FixedTimeDilution.InfusionDrug);
                case FixedTimeDilution ftd:
                    return DeleteHelper.Create(now, () => ftd.InfusionDrug,
                                                    from d in FixedTimeDilutions
                                                    where d.InfusionDilutionId == ftd.InfusionDilutionId
                                                    select d.InfusionDrug);
                case InfusionDiluent diluent:
                    return DeleteHelper.Create(now, () => diluent.InfusionDrugs,
                                                    from idil in InfusionDiluents
                                                    where idil.DiluentId == diluent.DiluentId
                                                    select idil.InfusionDrugs);
                case InfusionDrug iDrug:
                    var returnVar4 = DeleteHelper.Create(now, () => iDrug.InfusionSortOrderings?.ToListIfAllNonNull(iso => iso.Ward),
                                                   (from iso in InfusionSortOrderings
                                                    where iso.InfusionDrugId == iDrug.InfusionDrugId
                                                    select iso.Ward).Distinct());
                    returnVar4.AddRecord(iDrug.InfusionDrugId, NosqlTable.InfusionDrugs);
                    return returnVar4;
                case InfusionSortOrdering iso:
                    return DeleteHelper.Create(now, () => iso.Ward,
                                            from i in InfusionSortOrderings
                                            where i.InfusionSortOrderId == iso.InfusionSortOrderId
                                            select i.Ward);
                case VariableTimeConcentration vtc:
                    return DeleteHelper.Create(now, () => vtc.VariableTimeDilution?.InfusionDrug, 
                                        from c in VariableTimeConcentrations
                                        where c.InfusionConcentrationId == vtc.InfusionConcentrationId
                                        select c.VariableTimeDilution.InfusionDrug);
                case VariableTimeDilution vtd:
                    return DeleteHelper.Create(now, () => vtd.InfusionDrug,
                                                    from d in VariableTimeDilutions
                                                    where d.InfusionDilutionId == vtd.InfusionDilutionId
                                                    select d.InfusionDrug);
                case Ward w:
                    var returnVar5 = DeleteHelper.Create(now);
                    returnVar5.AddRecord(w.WardId,NosqlTable.Wards);
                    return returnVar5;
                default:
                    return null;
            }
        }
    }
}