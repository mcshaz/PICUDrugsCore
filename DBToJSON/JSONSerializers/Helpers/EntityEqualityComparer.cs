using DBToJSON.SqlEntities;
using DBToJSON.SqlEntities.BolusDrugs;
using DBToJSON.SqlEntities.Infusions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DBToJSON.JSONSerializers.Helpers
{
    class EntityEqualityComparer : IEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            if (x == null) { return y == null; }
            if (x.GetType() == y.GetType())
            {
                int? k = GetEntityKey(x);
                if (k.HasValue)
                {
                    return k == GetEntityKey(y);
                }
                return x.Equals(y);
            }
            return false;
        }

        public int GetHashCode(object obj)
        {
            int? k = GetEntityKey(obj);
            if (k.HasValue)
            {
                unchecked
                {
                    int hash = 23;
                    hash = hash * 31 + k.GetType().GetHashCode();
                    hash = hash * 31 + k.Value;
                    return hash;
                }
            }
            return obj.GetHashCode();
        }

        private int? GetEntityKey(object obj)
        {
            var type = obj.GetType();
            if (_typesKeys.TryGetValue(type, out Func<object, object> getValue))
            {
                if (getValue == null)
                {
                    getValue = _typesKeys[type] = GetKeyFunc(type);
                }
                return (int)getValue(obj);
            }
            return null;
        }

        private static Func<object, object> GetKeyFunc(Type t)
        {
            var pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField).Single(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute)));
            return pi.GetValue;
        }

        private Dictionary<Type, Func<object, object>> _typesKeys = new Dictionary<Type, Func<object, object>>
        {
            { typeof(BolusDose), null },
            { typeof(BolusDrug), null },
            { typeof(FixedDrug), null },
            { typeof(FixedDose), null },
            { typeof(BolusSortOrdering), null },
            { typeof(DefibJoule), null },
            { typeof(DefibModel), null },
            { typeof(DoseCat), null },
            { typeof(DrugAmpuleConcentration), null },
            { typeof(DrugReferenceSource), null },
            { typeof(DrugRoute), null },
            { typeof(FixedTimeConcentration), null },
            { typeof(FixedTimeDilution), null },
            { typeof(InfusionDiluent), null },
            { typeof(InfusionDrug), null },
            { typeof(InfusionSortOrdering), null },
            { typeof(VariableTimeConcentration), null },
            { typeof(VariableTimeDilution), null },
            { typeof(Ward), null }
        };
    }
}
