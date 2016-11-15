using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CakeLauncher
{

    public class CakeParser
    {
        private static IEnumerable<CakeTask> Parse(FileInfo file)
        {
            if (!file.Exists) yield break;

            var lines = File.ReadAllLines(file.FullName);
            var tasks = lines.Select(x => x.Trim()).Where(x => x.StartsWith("Task(\""));

            var loads = lines.Select(x => x.Trim())
                .Where(x => x.StartsWith("#l"))
                .Select(x =>
                {
                    return x.Replace("#load", String.Empty)
                     .Replace("#l", String.Empty)
                     .Replace("\"", string.Empty)
                     .Trim();
                }).ToList();


            foreach (var newTask in tasks)
            {
                var name = newTask.Split('\"')[1];
                yield return new CakeTask { Name = name };
            }


            foreach (var load in loads)
            {
                var dir = file.Directory.FullName;
                var loadPath = Path.Combine(dir, load);
                var info = new FileInfo(loadPath);
                foreach (var t in Parse(info))
                {
                    yield return t;
                }
            }
        }

        public static IEnumerable<CakeTask> ParseFile(FileInfo file)
        {
            return Parse(file);
        }
    }
}
