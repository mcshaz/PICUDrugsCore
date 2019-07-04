using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace XUnitTestDrugs.Utilities
{
    public class CreateCSTextInstances<T>
    {
        public CreateCSTextInstances(TextWriter writer)
        {
            _textWriter = writer;
            _delegates = EnumerateProperties(typeof(T));
        }
        private TextWriter _textWriter;
        private readonly List<Action<T>> _delegates;
       
        public void AddObjects(IEnumerable<T> ts)
        {
            _textWriter.WriteLine("new [] {");
            foreach (var t in ts)
            {
                _textWriter.WriteLine($"\tnew {typeof(T).Name} {{");
                foreach(var a in _delegates)
                {
                    a(t);
                }
                _textWriter.WriteLine("\t},");
            }
            _textWriter.WriteLine("}");
        }
        //todo - proper ignore if any nullable type == null - create pair canbenull, action(or format string)
        private List<Action<T>> EnumerateProperties(Type t)
        {
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            var returnVar = new List<Action<T>>(props.Length);
            foreach (var p in props)
            {
                Action<T> action;
                if (p.PropertyType.IsEnum)
                {
                    action = (obj) => _textWriter.WriteLine($"\t\t{p.Name} = {p.PropertyType.Name}.{p.GetValue(obj)},");
                }
                else
                {
                    switch (Nullable.GetUnderlyingType(p.PropertyType)?.Name ?? p.PropertyType.Name)
                    {
                        case nameof(String):
                            action = (obj) => {
                                object val = p.GetValue(obj);
                                if (val != null) {
                                    _textWriter.WriteLine($"\t\t{p.Name} = \"{val}\",");
                                } };
                            break;
                        case nameof(Char):
                            action = (obj) => _textWriter.WriteLine($"\t\t{p.Name} = '{p.GetValue(obj)}',");
                            break;
                        case nameof(Byte):
                        case nameof(Int16):
                        case nameof(Int32):
                        case nameof(Int64):
                            action = (obj) => _textWriter.WriteLine($"\t\t{p.Name} = {p.GetValue(obj)},");
                            break;
                        case nameof(Single):
                        case nameof(Double):
                        case nameof(Decimal):
                            action = (obj) => _textWriter.WriteLine($"\t\t{p.Name} = {p.GetValue(obj)},");
                            break;
                        case nameof(Boolean):
                            action = (obj) => _textWriter.WriteLine($"\t\t{p.Name} = {p.GetValue(obj).ToString().ToLowerInvariant()},");
                            break;
                        default:
                            action = null;
                            break;

                    }
                }
                if (action != null)
                {
                    returnVar.Add(action);
                }
            }
            return returnVar;
        }
    }
}
