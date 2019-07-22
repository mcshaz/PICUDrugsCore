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
        public virtual DbSet<BolusSortOrdering> BolusSortOrdering { get; set; }
        public virtual DbSet<DefibJoule> DefibJoules { get; set; }
        public virtual DbSet<DefibModel> DefibModels { get; set; }
        public virtual DbSet<Drug> Drugs { get; set; }
        public virtual DbSet<DrugAmpuleConcentration> DrugAmpuleConcentrations { get; set; }
        public virtual DbSet<DrugReferenceSource> DrugReferenceSources { get; set; }
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
            var cruds = BeforeSave();
            cruds.Mods.ForEach(m => m.ExecuteQuery());
            var returnVar = base.SaveChanges(acceptAllChangesOnSuccess);
            cruds.Mods.ForEach(m => m.UpdateAfterSave());
            RecordDeletions.AddRange(cruds.Dels);
            base.SaveChanges(true);
            return returnVar;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var cruds = BeforeSave();
            cruds.Mods.ForEach(async m => await m.ExecuteQueryAsync());
            var returnVar = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            cruds.Mods.ForEach(m => m.UpdateAfterSave());
            RecordDeletions.AddRange(cruds.Dels);
            await base.SaveChangesAsync(true);
            return returnVar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dels"></param>
        /// <returns>whether new deletions added</returns>

        private UpdateTracked BeforeSave()
        {
            var returnVar = new UpdateTracked();
            if (ChangeTracker.HasChanges()) {
                var now = DateTime.UtcNow;
                foreach (var e in ChangeTracker.Entries())
                {
                    switch (e.State)
                    {
                        case EntityState.Added:
                        case EntityState.Modified:
                            if (e.Entity is INosqlTable d)
                            {
                                d.DateModified = now;
                            }
                            break;
                        case EntityState.Deleted:
                            returnVar.Dels.Add(CreateDeletionRecord(e, now));
                            break;
                    }
                    var res = HandleCRUD(e, now);
                    if (res != null)
                    {
                        returnVar.Mods.AddRange(res);
                    }
                }
            }
            return returnVar;
        }

        private class UpdateTracked
        {
            public List<CRUDUpdateBase> Mods { get; } = new List<CRUDUpdateBase>();
            public List<RecordDeletionTime> Dels { get; } = new List<RecordDeletionTime>();
        }

        private CRUDUpdateBase[] HandleCRUD(EntityEntry e, DateTime now)
        {
            switch (e.Entity)
            {
                case BolusDose bDose:
                    return new[] { CRUDHelper.Create(now, () => bDose.BolusDrug, from d in BolusDoses
                                                                     where d.BolusDoseId == bDose.BolusDoseId
                                                                     select d.BolusDrug) };
                case BolusDrug bDrug:
                    return new[] { CRUDHelper.Create(now, () => bDrug.BolusSortOrderings?.ToListIfAllNonNull(bso => bso.Ward),
                                                            (from bso in BolusSortOrdering
                                                             where bso.BolusDrugId == bDrug.BolusDrugId
                                                             select bso.Ward).Distinct()) };
                case BolusSortOrdering bso:
                    //must get this info pre CRUD
                    return new[] { CRUDHelper.Create(now, () => bso.Ward, from b in BolusSortOrdering
                                                              where b.BolusSortOrderId == bso.BolusSortOrderId
                                                              select b.Ward) };
                case DefibJoule dj:
                    return new[] { CRUDHelper.Create(now, () => dj.DefibModel, from d in DefibJoules
                                                                  where d.Id == dj.Id
                                                                  select d.DefibModel) };
                case DefibModel dm:
                    return new[] { CRUDHelper.Create(now, () => dm.Wards, from d in DefibModels
                                                                        where d.Id == dm.Id
                                                                        select dm.Wards) };
                case Drug dr:
                    return new[] { CRUDHelper.Create(now, () => dr.InfusionDrugs,
                                                      from d in Drugs
                                                      where d.DrugId == dr.DrugId
                                                      select d.InfusionDrugs),
                                    CRUDHelper.Create(now, () => dr.BolusDrugs,
                                                      from d in Drugs
                                                      where d.DrugId == dr.DrugId
                                                      select d.BolusDrugs)};
                case DrugAmpuleConcentration ampConc:
                    return new[] { CRUDHelper.Create(now, () => ampConc.Drug?.InfusionDrugs,
                                                      from ac in DrugAmpuleConcentrations
                                                      where ac.AmpuleConcentrationId == ampConc.AmpuleConcentrationId
                                                      select ac.Drug.InfusionDrugs),
                                   CRUDHelper.Create(now, () => ampConc.Drug?.BolusDrugs,
                                                      from ac in DrugAmpuleConcentrations
                                                      where ac.AmpuleConcentrationId == ampConc.AmpuleConcentrationId
                                                      select ac.Drug.BolusDrugs)};
                case DrugReferenceSource refSource:
                    return new[] { CRUDHelper.Create(now, () => refSource.InfusionDrugs,
                                                           from rs in DrugReferenceSources
                                                           where rs.DrugReferenceId == refSource.DrugReferenceId
                                                           select rs.InfusionDrugs) };
                case FixedTimeConcentration ftc:
                    return new[] { CRUDHelper.Create(now, () => ftc.FixedTimeDilution?.InfusionDrug,
                                                        from c in FixedTimeConcentrations
                                                        where c.InfusionConcentrationId == ftc.InfusionConcentrationId
                                                        select c.FixedTimeDilution.InfusionDrug) };
                case FixedTimeDilution ftd:
                    return new[] { CRUDHelper.Create(now, () => ftd.InfusionDrug,
                                                    from d in FixedTimeDilutions
                                                    where d.InfusionDilutionId == ftd.InfusionDilutionId
                                                    select d.InfusionDrug) };
                case InfusionDiluent diluent:
                    return new[] { CRUDHelper.Create(now, () => diluent.InfusionDrugs,
                                                    from idil in InfusionDiluents
                                                    where idil.DiluentId == diluent.DiluentId
                                                    select idil.InfusionDrugs) };
                case InfusionDrug iDrug:
                    return new[] { CRUDHelper.Create(now, () => iDrug.InfusionSortOrderings?.ToListIfAllNonNull(iso => iso.Ward),
                                                   (from iso in InfusionSortOrderings
                                                    where iso.InfusionDrugId == iDrug.InfusionDrugId
                                                    select iso.Ward).Distinct()) };
                case InfusionSortOrdering iso:
                    return new[] { CRUDHelper.Create(now, () => iso.Ward,
                                            from i in InfusionSortOrderings
                                            where i.InfusionSortOrderId == iso.InfusionSortOrderId
                                            select i.Ward) };
                case VariableTimeConcentration vtc:
                    return new[] { CRUDHelper.Create(now, () => vtc.VariableTimeDilution?.InfusionDrug,
                                        from c in VariableTimeConcentrations
                                        where c.InfusionConcentrationId == vtc.InfusionConcentrationId
                                        select c.VariableTimeDilution.InfusionDrug) };
                case VariableTimeDilution vtd:
                    return new[] { CRUDHelper.Create(now, () => vtd.InfusionDrug,
                                                    from d in VariableTimeDilutions
                                                    where d.InfusionDilutionId == vtd.InfusionDilutionId
                                                    select d.InfusionDrug) };
                default:
                    return null;
            }
        }

        private RecordDeletionTime CreateDeletionRecord(EntityEntry e, DateTime now)
        {
            var deleted = new RecordDeletionTime
            {
                Deleted = now
            };
            switch (e.Entity)
            {
                case BolusDrug bDrug:
                    deleted.IdOfDeletedRecord = bDrug.BolusDrugId;
                    deleted.TableId = NosqlTable.BolusDrugs;
                    break;
                case DefibModel dm:
                    deleted.IdOfDeletedRecord = dm.Id;
                    deleted.TableId = NosqlTable.DefibModels;
                    break;
                case InfusionDrug iDrug:
                    deleted.IdOfDeletedRecord = iDrug.InfusionDrugId;
                    deleted.TableId = NosqlTable.InfusionDrugs;
                    break;
                case Ward w:
                    deleted.IdOfDeletedRecord = w.WardId;
                    deleted.TableId = NosqlTable.Wards;
                    break;
                default:
                    return null;
            }
            return deleted;
        }
    }
}