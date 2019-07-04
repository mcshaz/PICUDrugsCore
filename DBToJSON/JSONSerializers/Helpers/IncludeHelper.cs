using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DBToJSON.JSONSerializers.Helpers
{
    public class IncludeHelper<TEntity> where TEntity : class
    {
        public IncludeHelper()
        {
            _includes = new List<string[]>();
        }
        public IncludeHelper(IEnumerable<string[]> includes)
        {
            _includes = new List<string[]>(includes);
        }
        private readonly List<string[]> _includes;
        public IReadOnlyList<string[]> Includes { get => _includes.AsReadOnly(); }
        public IncludeHelper<TEntity> Add(string include)
        {
            _includes.Add(include.Split('.'));
            return this;
        }
        public IncludeHelper<TEntity> Add<U>(Expression<Func<TEntity,U>> include)
        {
            _includes.Add((include.ToString().Split('.').Skip(1).ToArray()));
            return this;
        }
        public IQueryable<TEntity> AddIncludes(IQueryable<TEntity> db)
        {
            IQueryable<TEntity> returnVar = db;
            _includes.ForEach(i => returnVar = returnVar.Include(string.Join('.', i)));
            return returnVar;
        }

        public override string ToString()
        {
            return "['" + string.Join("','",_includes.Select(i => String.Join('.', i))) + "']";
        }
    }

    public static class IncludeHelperStatic
    {
        public static IQueryable<TEntity> AddIncludes<TEntity>(this DbSet<TEntity> dbSet, IncludeHelper<TEntity> helper) where TEntity : class
        {
            return helper.AddIncludes(dbSet);
        }
    }
}
