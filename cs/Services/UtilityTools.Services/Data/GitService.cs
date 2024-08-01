using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Data
{
    public class GitService : IGitService
    {
        public IList<string> GetChangedFiles(string dir)
        {
            var result = new List<string>();
            var git = new CommandRunner("git", dir);
            var status = git.Run("status");

            //Console.WriteLine(status);

            //string pattern = @"modified:   (?<key>.*?).(html|js|css)";
            string pattern = @"modified:   (?<key>.*?)$";
            var rows = status.Split('\n');

            foreach (var row in rows)
            {
                if (row.Contains("modified:"))
                {
                    var text = row.TrimStart().TrimEnd().Trim();
                    var mc = Regex.Match(text, pattern);
                    if (mc.Success)
                    {
                        var key = mc.Groups["key"].Value.ToString();

                        if (key.Contains("/"))
                        {
                            var index = key.LastIndexOf('/');

                            key = key.Substring(index + 1, key.Length - index - 1);
                        }
                        result.Add(key);
                    }
                }
                else if (row.EndsWith(".js"))
                {
                    if (row.Contains("/"))
                    {
                        var index = row.LastIndexOf('/');

                        var fileName = row.Substring(index + 1, row.Length - index - 1);
                        result.Add(fileName);
                    }
                    else
                    {

                        result.Add(row);
                    }
                }
            }
            return result;
        }
    }
}
