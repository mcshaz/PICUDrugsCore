using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using System.Linq;
using System.Text.RegularExpressions;
using DrugClasses.Utilities;

namespace XUnitTestDrugs.Utilities
{
    public class CSToTStool
    {
        //[Fact]
        public void AddIndexTs()
        {
            var folder = Path.Combine(TsDestPath, "anthropometry");
            var tsFiles = MakeClassToFolder(folder, "*.ts");
            var vals = new List<string>(tsFiles.Count);
            foreach (var kv in tsFiles)
            {
                if (kv.Key != "index")
                {
                    var val = kv.Value.StartsWith('\\')
                        ? kv.Value.Substring(1)
                        : kv.Value;
                    var f = File.ReadAllText(Path.Combine(folder, val, kv.Key + ".ts"));
                    var exports = Regex.Matches(f, @"^\s*export (class|enum|interface|{|(async )?function) (\w+)",RegexOptions.Multiline)
                        .Select(m => m.Groups.Last().Value);
                    if (exports.Any())
                    {
                        var rel = MakeEcmaPath("",kv.Value);
                        vals.Add($"export {{{string.Join(',', exports)}}} from '{rel + kv.Key}'");
                    }
                }
            }
            File.WriteAllLines(Path.Combine(folder, "index.ts"),
                vals);
        }
        public void SplitTS()
        {
            var csFiles = MakeClassToFolder(CsPath, "*.cs");
            foreach (var p in csFiles.Values.Distinct())
            {
                Directory.CreateDirectory(Path.Combine(TsDestPath, p));
            }
            var f = File.ReadAllText(TsSourcePath);
            var classDef = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            var enumNames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            Match prev = null;
            foreach (Match next in Regex.Matches(f, @"^(class|enum|interface) (\w+)", RegexOptions.Multiline))
            {
                if (prev != null)
                {
                    string def = f.Substring(prev.Index, next.Index - prev.Index);
                    classDef.Add(prev.Groups[2].Value, def);
                    if (prev.Groups[1].Value == "enum")
                    {
                        enumNames.Add(prev.Groups[2].Value);
                    }
                    
                }
                prev = next;
            }
            classDef.Add(prev.Groups[1].Value, f.Substring(prev.Index));
            Regex enumRegex = new Regex($"\\b({ string.Join('|', enumNames) })\\b");
            var allKeys = csFiles.Keys.ToSets(classDef.Keys,StringComparer.InvariantCultureIgnoreCase);
            foreach (var k in allKeys[SetResult.Intersect].Concat(allKeys[SetResult.TrailingSetOnly]))
            {
                string classText = classDef[k];
                if (!csFiles.TryGetValue(k, out string path))
                {
                    path = string.Empty;
                }
                var destFile = Path.Combine(TsDestPath, path, k + ".ts");
                var refs = Regex.Matches(classText, @"\b(new|extends) (\w+)")
                    .Select(m => m.Groups[2].Value)
                    .Distinct();
                if (!enumRegex.IsMatch(k))
                {
                    refs = refs.Concat(
                        enumRegex.Matches(classText)
                        .Select(m => m.Groups[1].Value)
                        .Distinct());
                }
                refs = refs.Where(r => !(new[] { k, "Date", "Error", "RangeError" }).Contains(r));
                var prepend = string.Join("\r\n",
                    refs
                    .Select(className=> {
                        string relPath;
                        if (csFiles.TryGetValue(className, out string refPath))
                        {
                            relPath = MakeEcmaPath(path, refPath);
                        }
                        else
                        {
                            relPath = MakeEcmaPath(path,string.Empty);
                        }
                        return $"import {{ {className} }} from '{relPath}{className}'"; ;
                    })
                    .Concat(new[] { "export " }));
                File.WriteAllText(destFile, prepend + classText);
            }
        }
        const string CsPath = @"C:\Users\OEM\Documents\Visual Studio 2017\Projects\DrugClasses\DrugClasses\";
        const string TsDestPath = @"C:\Users\OEM\Documents\PicuDrugsClient\src\";
        const string TsSourcePath = @"C:\Users\OEM\Documents\Visual Studio 2017\Projects\DrugClasses\DrugClasses\bin\Debug\netcoreapp2.0\DrugClasses.ts";
        public static Dictionary<string, string> MakeClassToFolder(string path, string fileSelector)
        {

            var returnVar = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var f in Directory.GetFiles(path, fileSelector, SearchOption.AllDirectories))
            {
                var relPath = (Path.GetDirectoryName(f) + '\\').Substring(path.Length);
                var filename = Path.GetFileNameWithoutExtension(f);
                returnVar.Add(filename, relPath);
            }
            return returnVar;
        }

        public static string MakeEcmaPath(string from, string to)
        {
            const string drive = @"C:";
            if (!from.StartsWith(drive)) { from = drive + (from.StartsWith('\\') ? from :('\\' + from)); }
            if (!to.StartsWith(drive)) { to = drive + (to.StartsWith('\\') ? to : ('\\' + to)); }
            if (!from.EndsWith("\\")) { from += "\\"; }
            if (!to.EndsWith("\\")) { to += "\\"; }
            Uri uriTo = new Uri(to);
            Uri uriFrom = new Uri(from);
            Uri relativeUri = uriFrom.MakeRelativeUri(uriTo);
            return "./" + relativeUri.ToString();
        }
    }
}
