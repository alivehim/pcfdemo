using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UtilityTools.Core.Models
{
    public class SubtitleBlock
    {
        public int Index { get; private set; }
        public double Length { get; private set; }
        public double From { get; private set; }
        public double To { get; private set; }
        public string Text { get; private set; }

        public SubtitleBlock(int index, double from, double to, string text)
        {
            this.Index = index;
            this.From = from;
            this.To = to;
            this.Length = to - from;
            this.Text = text;
        }
        public override string ToString()
        {
            return "Index: " + Index + " From: " + From + " To: " + To + " Text: " + Text;
        }
        public  static List<SubtitleBlock> ParseSubtitles(string content)
        {
            var subtitles = new List<SubtitleBlock>();
            var regex = new Regex($@"(?<index>\d*\s*)\n(?<start>\d*:\d*:\d*,\d*)\s*-->\s*(?<end>\d*:\d*:\d*,\d*)\s*\n(?<content>.*)\n(?<content2>.*)\n");
            var matches = regex.Matches(content);
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                int ind = int.Parse(groups["index"].Value);
                TimeSpan fromtime, totime;
                TimeSpan.TryParse(groups["start"].Value.Replace(',', '.'), out fromtime);
                TimeSpan.TryParse(groups["end"].Value.Replace(',', '.'), out totime);
                string contenttext = $"{ groups["content"].Value}\n{ groups["content2"].Value}";
                subtitles.Add(new SubtitleBlock(ind, fromtime.TotalMilliseconds, totime.TotalMilliseconds, contenttext));
            }
            return subtitles;
        }
    }

}
