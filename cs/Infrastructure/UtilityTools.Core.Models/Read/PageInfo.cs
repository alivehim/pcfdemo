using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Read
{
    public class PageInfo
    {
        string _text = string.Empty;
        public PageInfo()
        {
            this.Row = new List<string>();
        }
        /// <summary>
        /// 当前字数
        /// </summary>
        public int CharNum { get; set; }

        /// <summary>
        /// 行
        /// </summary>
        public List<string> Row { get; set; }

        public string Text
        {
            get
            {
                if (_text.Length == 0)
                {
                    StringBuilder text = new StringBuilder();
                    for (int i = 0; i < Row.Count; i++)
                    {
                        if (Regex.IsMatch(Row[i], @"[\r\n]"))
                        {
                            text.AppendFormat(Row[i]);
                        }
                        else
                        {
                            text.Append(Row[i] + "\r\n");
                            //Text = Text + Row[i] + "\r\n";
                        }
                    }
                    _text = text.ToString();
                }


                return _text;
            }
            set
            {
                _text = value; 
            }
        }

        public PageInfo Clone()
        {
            PageInfo page = new PageInfo();
            page.Row = new List<string> { "巴掌" };
            //foreach(var item in Row)
            //{
            //    page.Row.Add(item);

            //}
            return page;
        }

        public void Refresh()
        {
            Text = "\t";
            Text = string.Empty;
        }
    }
}
