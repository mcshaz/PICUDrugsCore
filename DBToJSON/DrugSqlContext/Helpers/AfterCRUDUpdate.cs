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
    internal static class CRUDHelper
    {
        public static CRUDUpdateBase Create<T>(DateTime now, Func<T> localResult,IQueryable<T> preUpdateQuery) where T : INosqlTable
        {
            return new AfterCRUDUpdateSingle<T>(now, localResult, preUpdateQuery);
        }
        public static CRUDUpdateBase Create<T>(DateTime now, Func<IEnumerable<T>> localResult, IQueryable<T> preUpdateQuery) where T : INosqlTable
        {
            return new AfterCRUDUpdateMany<T>(now, localResult, preUpdateQuery);
        }
        public static CRUDUpdateBase Create<T>(DateTime now, Func<IEnumerable<T>> localResult, IQueryable<IEnumerable<T>> preUpdateQuery) where T : INosqlTable
        {
            return new AfterCRUDUpdateMany<T>(now, localResult, preUpdateQuery);
        }
        public static CRUDUpdateBase Create<T>(DateTime now, Func<ICollection<T>> localResult, IQueryable<ICollection<T>> preUpdateQuery) where T : INosqlTable
        {
            return new AfterCRUDUpdateMany<T>(now, localResult, preUpdateQuery);
        }
    }
    internal class AfterCRUDUpdateMany<T> : CRUDUpdateBase where T : INosqlTable
    {
        public AfterCRUDUpdateMany(DateTime now, Func<IEnumerable<T>> localResult, IQueryable<IEnumerable<T>> preUpdateQuery)
            : this(now, localResult)
        {
            _preUpdateEnumerableQuery = preUpdateQuery ?? throw new ArgumentNullException(nameof(preUpdateQuery));
        }
        public AfterCRUDUpdateMany(DateTime now, Func<IEnumerable<T>> localResult , IQueryable<T> preUpdateQuery)
            : this(now, localResult)
        {
            _preUpdateSingleQuery = preUpdateQuery ?? throw new ArgumentNullException(nameof(preUpdateQuery));
        }
        private AfterCRUDUpdateMany(DateTime now, Func<IEnumerable<T>> localResult) : base(now)
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
        public override void UpdateAfterSave()
        {
            foreach (T r in _queryResult)
            {
                r.DateModified = _now;
            }
        }
    }

    internal class AfterCRUDUpdateSingle<T> : CRUDUpdateBase where T : INosqlTable
    {
        public AfterCRUDUpdateSingle(DateTime now, Func<T> localResult,IQueryable<T> preUpdateQuery) :base(now)
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
        public override void UpdateAfterSave()
        {
            _queryResult.DateModified = _now;
        }
    }
    /*
    internal class EmptyCRUDUpdate : CRUDUpdateBase
    {
        public EmptyCRUDUpdate(DateTime now) : base(now)
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
    */
    internal abstract class CRUDUpdateBase 
    {
        public CRUDUpdateBase(DateTime now)
        {
            _now = now;
        }
        protected readonly DateTime _now;
        /// <summary>
        /// must only be called once!
        /// </summary>
        public abstract void ExecuteQuery();
        /// <summary>
        /// must only be called once!
        /// </summary>
        public abstract Task ExecuteQueryAsync();

        public abstract void UpdateAfterSave();
    }
}
