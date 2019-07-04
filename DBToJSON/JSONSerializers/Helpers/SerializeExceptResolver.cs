using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DBToJSON.JSONSerializers.Helpers;

namespace DBToJSON.JsonSerializers.Helpers
{
    public class SerializeExceptResolver : DefaultContractResolver
    {
        //lest assume this called 1st for object at top of tree
        //prob wrong place, as are serializable - maybe look at source code for how notmapped handled
        private readonly Dictionary<Type, string[]> _excludeProperties = new Dictionary<Type, string[]>();
        private readonly Dictionary<Type, Func<MemberInfo, bool>> _excludeWithAttributes = new Dictionary<Type, Func<MemberInfo, bool>>();
        private readonly Dictionary<Type, Func<object, MemberInfo, string>> _excludePropertyOnAttribute = new Dictionary<Type, Func<object, MemberInfo,string>>(); 

        public void ExcludeProperties<T>(params Expression<Func<T, object>>[] expressions) where T : class
        {
            var propertyNames = expressions.Select(e => ((MemberExpression)e.Body).Member.Name).ToArray();
            _excludeProperties.Add(typeof(T), propertyNames);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">an attribute type</typeparam>
        /// <param name="excludePropertyName">returning null or empty will not exclude the property</param>
        public void ExcludeOtherPropByAttribute<T>(Func<T, MemberInfo,string> excludePropertyName) where T : Attribute
        {
            _excludePropertyOnAttribute.Add(typeof(T), (Func<object, MemberInfo, string>)excludePropertyName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">an attribute type</typeparam>
        /// <param name="excludePropertyName">returning null or empty will not exclude the property</param>
        public void ExcludeOtherPropByAttribute<T>(Func<T,string> excludePropertyName) where T: Attribute
        {
            _excludePropertyOnAttribute.Add(typeof(T), (o, mi) => excludePropertyName((T)o));
        }

        private Dictionary<Type, IEnumerable<string[]>> _helpers;
        public void UseIncludeHelper<T>(IncludeHelper<T> helper) where T : class
        {
            (_helpers ?? (_helpers = new Dictionary<Type, IEnumerable<string[]>>()))
                .Add(typeof(T), helper.Includes);
        } 

        /// <summary>
        /// Will pass the memberinfo for the property decorated by the attribute
        /// </summary>
        /// <typeparam name="T">Attribute</typeparam>
        /// <param name="exclude">Whether to exclude the member from serialization (true = exclude, false = ignore exclusion)</param>
        public void ExcludeAttribute<T>(Func<MemberInfo,bool> exclude) where T : Attribute
        {
            _excludeWithAttributes.Add(typeof(T), exclude);
        }

        public void ExcludeAttribute<T>() where T : Attribute
        {
            _excludeWithAttributes.Add(typeof(T), DummyFunc);
        }

        private bool DummyFunc(MemberInfo mi)
        {
            return true;
        }
        //watch out - lets objects through as nulls are excluded & include will determine if we want the object
        //however, if entities were tracked (currently AsNoTracking) this could create unintended serialization
        /*
        private static bool IsObject(MemberInfo mi)
        {
            var pi = mi as PropertyInfo;
            if (pi == null) { return false; }
            Type t = pi.PropertyType;
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                return false;
            }
            return Type.GetTypeCode(t) == TypeCode.Object;
        }
        */
        private List<MemberInfo> PassThru(Type objectType, List<MemberInfo> allMembers)
        {
            List<MemberInfo> returnVar = new List<MemberInfo>();
            if (_helpers != null && _helpers.TryGetValue(objectType, out IEnumerable<string[]> values))
            {
                var propNames = values.ToLookup(k => k[0], v => v.Skip(1).ToArray());
                var passThru = allMembers.ToLookup(mi => propNames.Contains(mi.Name));
                returnVar.AddRange(passThru[true]);
                allMembers.Clear();
                allMembers.AddRange(passThru[false]);
                foreach (var mi in returnVar)
                {
                    var nextProps = propNames[mi.Name].Where(pn => pn.Length > 0).ToList();
                    if (nextProps.Any())
                    {
                        Type propType = ((PropertyInfo)mi).PropertyType;
                        if (!IsNotICollectionProp(propType))// double not = isCollection!! method really made for different context but we might as well use ut
                        {
                            propType = propType.GetGenericArguments()[0];
                        }
                        if (_helpers.TryGetValue(propType, out IEnumerable<string[]> value))
                        {
                            nextProps.AddRange(value);
                            _helpers[propType] = nextProps;
                        }
                        else
                        {
                            _helpers.Add(propType, nextProps);
                        }
                    }
                }
            }
            return returnVar;
        }

        private static bool IsNotICollectionProp(MemberInfo mi)
        {
            var pi = mi as PropertyInfo;
            if (pi == null) { return false; }
            return !pi.PropertyType.IsGenericType || pi.PropertyType.GetGenericTypeDefinition() != typeof(ICollection<>);
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var mis = base.GetSerializableMembers(objectType);
            var passThru = PassThru(objectType, mis);
            mis = mis.Where(IsNotICollectionProp).ToList();
            if (_excludeProperties.TryGetValue(objectType, out string[] properties))
            {
                //for our use scenario, we only want simple types - include & check for null will determmine if it stays
                mis = mis.Where(mi => !properties.Contains(mi.Name)).ToList();
            }
            if (_excludeWithAttributes.Any() || _excludePropertyOnAttribute.Any())
            {
                var atrExclProps = new List<string>();
                //note checking for exact matches rather than inheritance isAssignableFrom
                mis = mis.Where(mi =>
                {
                    //some attributes have a multiple property, and below could fail with duplicate keys but that is beyond my use case
                    var ats = mi.GetCustomAttributes(true).ToDictionary(a => a.GetType());
                    var intersectTypes = ats.Keys.Intersect(_excludePropertyOnAttribute.Keys);
                    atrExclProps.AddRange(intersectTypes.Select(t => _excludePropertyOnAttribute[t](ats[t],mi)));
                    intersectTypes = ats.Keys.Intersect(_excludeWithAttributes.Keys);
                    //return true = exclude property
                    //currently in a where clause, so need boolean not
                    return intersectTypes.All(t => !_excludeWithAttributes[t](mi));
                }).ToList();
                mis = mis.Where(mi => !atrExclProps.Contains(mi.Name)).ToList();
           }
            mis.AddRange(passThru);
            return mis;
        }
    }
}