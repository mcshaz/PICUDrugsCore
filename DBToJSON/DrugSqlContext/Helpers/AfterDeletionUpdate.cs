using DBToJSON.Helpers;
using DBToJSON.SqlEntities;
using DBToJSON.SqlEntities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBToJSON.Helpers
{
    internal static class DeleteHelper
    {
        public static DeletionUpdateBase Create<T>(DateTime now, Func<T> localResult,IQueryable<T> preUpdateQuery) where T : UpdateTrackingEntity, INosqlTable
        {
            return new AfterDeletionUpdateSingle<T>(now, localResult, preUpdateQuery);
        }
        public static DeletionUpdateBase Create<T>(DateTime now, Func<IEnumerable<T>> localResult, IQueryable<T> preUpdateQuery) where T : UpdateTrackingEntity, INosqlTable
        {
            return new AfterDeletionUpdateMany<T>(now, localResult, preUpdateQuery);
        }
        public static DeletionUpdateBase Create<T>(DateTime now, Func<IEnumerable<T>> localResult, IQueryable<IEnumerable<T>> preUpdateQuery) where T : UpdateTrackingEntity, INosqlTable
        {
            return new AfterDeletionUpdateMany<T>(now, localResult, preUpdateQuery);
        }
        public static DeletionUpdateBase Create<T>(DateTime now, Func<ICollection<T>> localResult, IQueryable<ICollection<T>> preUpdateQuery) where T : UpdateTrackingEntity, INosqlTable
        {
            return new AfterDeletionUpdateMany<T>(now, localResult, preUpdateQuery);
        }
        public static DeletionUpdateBase Create(DateTime now)
        {
            return new EmptyDeletionUpdate(now);
        }
    }
    internal class AfterDeletionUpdateMany<T> : DeletionUpdateBase where T : UpdateTrackingEntity,INosqlTable
    {
        public AfterDeletionUpdateMany(DateTime now, Func<IEnumerable<T>> localResult, IQueryable<IEnumerable<T>> preUpdateQuery)
            : this(now, localResult)
        {
            _preUpdateEnumerableQuery = preUpdateQuery ?? throw new ArgumentNullException(nameof(preUpdateQuery));
        }
        public AfterDeletionUpdateMany(DateTime now, Func<IEnumerable<T>> localResult , IQueryable<T> preUpdateQuery)
            : this(now, localResult)
        {
            _preUpdateSingleQuery = preUpdateQuery ?? throw new ArgumentNullException(nameof(preUpdateQuery));
        }
        private AfterDeletionUpdateMany(DateTime now, Func<IEnumerable<T>> localResult) : base(now)
        {
            _localResult = localResult;
        }
        private readonly IQueryable<IEnumerable<T>> _preUpdateEnumerableQuery;
        private readonly IQueryable<T> _preUpdateSingleQuery;
        private IEnumerable<T> _queryResult;
        private Func<IEnumerable<T>> _localResult;
        private bool ShouldExecuteDb()
        {
            _queryResult = _localResult();
            _localResult = null;
            if (_queryResult == null)
            {
                return false;
            }
            if (!(_queryResult is IList))
            {
                _queryResult = _queryResult.ToList();
            }
            return true;
        }
        public override void ExecuteQuery()
        {
            if (ShouldExecuteDb())
            {
                if (_preUpdateSingleQuery != null)
                {
                    _queryResult = _preUpdateSingleQuery.ToList();
                }
                else
                {
                    _queryResult = _preUpdateEnumerableQuery.Single();
                }
            }
        }
        public override async Task ExecuteQueryAsync()
        {
            if (ShouldExecuteDb())
            {
                if (_preUpdateSingleQuery != null)
                {
                    _queryResult = await _preUpdateSingleQuery.ToListAsync();
                }
                else
                {
                    _queryResult = await _preUpdateEnumerableQuery.SingleAsync();
                }
            }
        }
        public override RecordDeletionTime UpdateAfterSave()
        {
            foreach (T r in _queryResult)
            {
                r.DateModified = _now;
            }
            return base.UpdateAfterSave();
        }
    }

    internal class AfterDeletionUpdateSingle<T> : DeletionUpdateBase where T : UpdateTrackingEntity, INosqlTable
    {
        public AfterDeletionUpdateSingle(DateTime now, Func<T> localResult,IQueryable<T> preUpdateQuery) :base(now)
        {
            _localResult = localResult ?? throw new ArgumentNullException(nameof(localResult)); ;
            _preUpdateQuery = preUpdateQuery ?? throw new ArgumentNullException(nameof(preUpdateQuery));
        }
        private readonly IQueryable<T> _preUpdateQuery;
        private T _queryResult;
        private Func<T> _localResult;
        private bool ShouldExecuteDb()
        {
            _queryResult = _localResult();
            _localResult = null;
            return _queryResult == null;
        }

        public override void ExecuteQuery()
        {
            if (ShouldExecuteDb())
            {
                _queryResult = _preUpdateQuery.Single();
            }
        }

        public override async Task ExecuteQueryAsync()
        {
            if (ShouldExecuteDb())
            {
                _queryResult = await _preUpdateQuery.SingleAsync();
            }
        }
        public override RecordDeletionTime UpdateAfterSave()
        {
            _queryResult.DateModified = _now;
            return base.UpdateAfterSave();
        }
    }

    internal class EmptyDeletionUpdate : DeletionUpdateBase
    {
        public EmptyDeletionUpdate(DateTime now) : base(now)
        {
        }

        public override void ExecuteQuery()
        {
        }

        public override Task ExecuteQueryAsync()
        {
            return Task.CompletedTask;
        }
    }

    internal abstract class DeletionUpdateBase 
    {
        public DeletionUpdateBase(DateTime now)
        {
            _now = now;
        }
        protected readonly DateTime _now;
        protected RecordDeletionTime _record = null;
        /// <summary>
        /// must only be called once!
        /// </summary>
        public abstract void ExecuteQuery();
        /// <summary>
        /// must only be called once!
        /// </summary>
        public abstract Task ExecuteQueryAsync();
        public virtual RecordDeletionTime UpdateAfterSave()
        {
            return _record;
        }
        public void AddRecord(int id, NosqlTable table)
        {
            _record = new RecordDeletionTime
            {
                Deleted = _now,
                IdOfDeletedRecord = id,
                TableId = table
            };
        }
    }
}
